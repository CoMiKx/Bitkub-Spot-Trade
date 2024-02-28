Imports System.ComponentModel
Imports System.IO
Imports System.Management
Imports System.Security.Cryptography
Imports System.Xml
Imports Newtonsoft.Json.Linq

Public Class FrmSpotTrade
    Structure ConfigurationInformation
        Dim licenseKey As String
        Dim autoRun As Boolean
        Dim bitkubKey As String
        Dim bitkubSecret As String
        Dim lineToken As String
        Dim serviceKey As String
        Dim sendSummaryHour As Integer
        Dim sendLineSummary As Boolean
        Dim sendLineBuyCoin As Boolean
        Dim sendLineSellCoin As Boolean
        Dim botTestMode As Boolean
        Dim stableCoin As String
        Dim stepupAmount As Double
        Dim buyNewOnNextWave As Boolean
        Dim tradeCoinBuyCommission As Double
        Dim tradeCoinSaleCommission As Double
        Dim tradeCoinTakeProfitPercent As Double
        Dim tradeCoinFillupPercent As Double
        Dim limitCoinAmount As Double
        Dim tradeInterval As String
        Dim tradeStyle As String
        Dim stopBuyNew As Boolean
    End Structure

    Structure TradeCoinInformation
        Dim symbol As String
        Dim stableCoin As String
        Dim tradeCoin As String
    End Structure

    Structure SummaryProfitInformation
        Dim summaryDate As Integer
        Dim symbol As String
        Dim stableCoin As String
        Dim tradeCoin As String
        Dim takeProfitCount As Double
        Dim takeProfitAmount As Double
    End Structure

    Structure WalletCoin
        Dim coinName As String
        Dim coinVolume As Double
        Dim coinPrice As Double
        Dim coinAmount As Double
        Dim decimalPlace As Double
    End Structure

    Private botActive As Boolean = False
    Private minCoinAmount As Double = 10.5
    Private applicationExpire As Boolean = True
    Private expireLocalDateTime As DateTime = Date.Now().AddDays(-1)
    Private runingRowactive As Integer = 0
    Private summaryTime As Date = DateTime.Now.AddHours(1)
    Private updateDashBroadBalanceTime As Date = DateTime.Now.AddMinutes(30)
    Private balanceAmount As Double = 0
    Private maxBudgetUse As Double = 0

    Private configuration As ConfigurationInformation = New ConfigurationInformation()
    Private tradeCoinList As List(Of TradeCoinInformation)
    Private summaryProfitList As List(Of SummaryProfitInformation) = New List(Of SummaryProfitInformation)

    Private cryptoWalletList As List(Of WalletCoin) = New List(Of WalletCoin)
    Private cryptoTradeList As List(Of Crypto) = New List(Of Crypto)

    Private pathFileDatabaseConfig As String = ""
    Private needExit As Boolean = False
    Private serialNumber As String = ""

    Private intervalMapping As New Dictionary(Of String, String) From {
        {"1m", "1"},
        {"5m", "5"},
        {"15m", "15"},
        {"1H", "60"},
        {"4H", "240"},
        {"1D", "1D"}
    }

    Private Sub FrmSpotTrade_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check duplicate runing program
        If CheckDuplicateProcess() Then
            Me.Close()
            Exit Sub
        End If

        'Lock Screen
        Me.Enabled = False

        'Set tab control mode
        TabControlTrade.DrawMode = TabDrawMode.OwnerDrawFixed

        'Get serial number
        serialNumber = GetSerialNumber()

        'Setup master list value
        SetupMasterListValue()

        'Build database file path
        pathFileDatabaseConfig = BuildDatabaseConfigFilePath(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))

        'Connect datbase
        Database.ConnectDatabase(pathFileDatabaseConfig)

        'Get configuration
        Database.ReadConfiguration(configuration)
        configuration.stopBuyNew = False

        'Set binance service information
        APIService.bitkubKey = configuration.bitkubKey
        APIService.bitkubSecret = configuration.bitkubSecret
        APIService.lineToken = configuration.lineToken

        'Get active trade coin
        tradeCoinList = Database.ReadTradeCoinList(configuration.stableCoin)

        'Get summary profit list
        If configuration.botTestMode = False Then
            summaryProfitList = Database.ReadSummaryProfitList(configuration.stableCoin)
        End If

        'Get previous Date Time
        expireLocalDateTime = APIService.GetPreviousDate()

        'Active timer setup environment
        ToolStripLabelMessageLog.Text = "Initial environment...."
        ToolStripLabelMessageLog.BackColor = SystemColors.Info

        TimerSetupEnvironment.Enabled = True
    End Sub

    'Class HashLicenseString
    Public Class HashLicenseString
        Private TripleDes As New TripleDESCryptoServiceProvider
        Private hashLicenseKey As String = ""

        Sub New(ByVal hashLicenseKey As String)
            'Update Key
            Me.hashLicenseKey = hashLicenseKey

            ' Initialize the crypto provider.
            TripleDes.Key = TruncateHash(hashLicenseKey, TripleDes.KeySize \ 8)
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
        End Sub

        Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()
            Dim sha1 As New SHA1CryptoServiceProvider

            ' Hash the key.
            Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
            Dim hash() As Byte = sha1.ComputeHash(keyBytes)

            ' Truncate or pad the hash.
            ReDim Preserve hash(length - 1)
            Return hash
        End Function

        Public Function EncryptData(ByVal dataString As String) As String
            ' Convert the plaintext string to a byte array.
            Dim textBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(dataString)

            ' Create the stream.
            Dim memoryStream As New System.IO.MemoryStream

            ' Create the encoder to write to the stream.
            Dim encStream As New CryptoStream(memoryStream, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            encStream.Write(textBytes, 0, textBytes.Length)
            encStream.FlushFinalBlock()

            ' Convert the encrypted stream to a printable string.
            Return Convert.ToBase64String(memoryStream.ToArray())
        End Function

        Public Function DecryptData(ByVal encryptedtext As String) As String
            ' Convert the encrypted text string to a byte array.
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream.
            Dim memoryStream As New System.IO.MemoryStream
            ' Create the decoder to write to the stream.
            Dim decStream As New CryptoStream(memoryStream, TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string.
            Return System.Text.Encoding.Unicode.GetString(memoryStream.ToArray())
        End Function
    End Class

    Private Function CheckDuplicateProcess() As Boolean
        Dim result As Boolean = False
        Dim processList() As Process

        processList = Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)
        If processList.Count > 1 Then
            MsgBox("Application is already runing." + vbCrLf + "Please check a application on task bar.", MsgBoxStyle.Critical, "Application Check")
            result = True
        End If

        Return result
    End Function

    Public Function GeAapplicationExpire() As Boolean
        Return applicationExpire
    End Function

    Public Function GetMinCoinAmount() As String
        Return minCoinAmount
    End Function

    Public Function GetSendLineSummary() As Boolean
        Return configuration.sendLineSummary
    End Function

    Public Function GetSendLineBuyCoin() As Boolean
        Return configuration.sendLineBuyCoin
    End Function

    Public Function GetSendLineSellCoin() As Boolean
        Return configuration.sendLineSellCoin
    End Function

    Public Function GetTradeStyle() As String
        Return configuration.tradeStyle
    End Function

    Public Function GetLimitCoinAmount() As Double
        Return configuration.limitCoinAmount
    End Function

    Public Function StopBuyNewCoin() As Boolean
        Return configuration.stopBuyNew
    End Function

    Public Function GetTakeProfitPercent() As Boolean
        Return configuration.tradeCoinTakeProfitPercent
    End Function

    Public Function GetFillupPercent() As Boolean
        Return configuration.tradeCoinFillupPercent
    End Function

    Public Function GetBuyNewOnNextWave() As Boolean
        Return configuration.buyNewOnNextWave
    End Function

    Private Function GetSerialNumber() As String
        Dim processerID As String = ""
        Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("select * from Win32_Processor")

        For Each oReturn As ManagementObject In searcher.Get()
            processerID = oReturn("ProcessorId").ToString
        Next oReturn

        Return processerID
    End Function

    Private Function BuildDatabaseConfigFilePath(ByVal defaultPath As String) As String
        Dim jfApplicationPath As String = defaultPath + "\" + Process.GetCurrentProcess.ProcessName

        If Not Directory.Exists(jfApplicationPath) Then
            Directory.CreateDirectory(jfApplicationPath)
        End If

        Return jfApplicationPath + "\DatabaseSpotTrade.db"
    End Function

    Public Function GetExpirDateFromLicenseKey(ByVal licenseKey As String,
                                           ByRef expireLocalDateTimeValue As DateTime,
                                           ByRef errorMessage As String) As Boolean

        Dim resultProcess As Boolean = False
        Dim expireLocalDateTimeLicense As DateTime = Nothing

        If licenseKey <> "" Then
            Try
                'Convert license key to JSON
                Dim keyCode As String = APIService.HashString("serialNumber=" + serialNumber + "&application=Bitkubspot&developby=carotcap", serialNumber)
                Dim hashLicenseString As HashLicenseString = New HashLicenseString(keyCode)
                Dim xmlString As String = hashLicenseString.DecryptData(licenseKey)

                Dim xmlDocument As XmlDocument = New XmlDocument()
                xmlDocument.LoadXml(xmlString)

                Dim checkSerial As String = xmlDocument.Item("licenseKey").Item("serialNumber").InnerText()
                expireLocalDateTimeLicense = DateTime.ParseExact(xmlDocument.Item("licenseKey").Item("expireDateLocal").InnerText(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

                If checkSerial = serialNumber Then
                    expireLocalDateTimeValue = expireLocalDateTimeLicense
                    resultProcess = True

                Else
                    errorMessage = "License key is not match with hardware"
                End If

            Catch ex As Exception
                errorMessage = "License key is invalid"

            End Try
        End If

        Return resultProcess
    End Function

    Public Function GetBalanceStableCoin() As Double
        While (APIService.GetWalletCoinBalance(configuration.stableCoin, 0.001, balanceAmount) = False)
            Threading.Thread.Sleep(2000)
        End While

        Return balanceAmount
    End Function

    Public Function GetBalanceStableCoinSimulate() As Double
        Return balanceAmount
    End Function

    Public Sub UpdateProfitAmount(ByVal orderDate As DateTime, ByVal stableCoin As String, ByVal tradeCoin As String, ByVal profitAmount As Double)
        Dim summaryProfitRecord As SummaryProfitInformation = Nothing
        Dim summaryDate As Integer = orderDate.ToString("yyyyMMdd")
        Dim summaryDateString As String = orderDate.ToString("dd MMM yyyy") + " / " + tradeCoin
        Dim listItem As ListViewItem = Nothing
        Dim newRecord As Boolean = True
        Dim indexViewCount As Integer = 0

        For indexCount As Integer = 0 To summaryProfitList.Count - 1
            summaryProfitRecord = summaryProfitList.Item(indexCount)

            If summaryProfitRecord.summaryDate = summaryDate _
                And summaryProfitRecord.stableCoin = stableCoin _
                And summaryProfitRecord.tradeCoin = tradeCoin Then

                newRecord = False

                'Update summary profit list
                summaryProfitRecord.takeProfitCount = summaryProfitRecord.takeProfitCount + 1
                summaryProfitRecord.takeProfitAmount = summaryProfitRecord.takeProfitAmount + profitAmount
                summaryProfitList.Item(indexCount) = summaryProfitRecord

                'Update summary profit listview
                For indexViewCount = 0 To ListViewSummaryProfit.Items.Count - 1
                    listItem = ListViewSummaryProfit.Items(indexViewCount)

                    If listItem.SubItems.Item(0).Text = summaryDateString Then
                        listItem.SubItems.Item(1).Text = summaryProfitRecord.takeProfitCount.ToString("###,##0")
                        listItem.SubItems.Item(2).Text = summaryProfitRecord.takeProfitAmount.ToString("###,###,##0.00")

                        Me.Invoke(Sub() Me.ListViewSummaryProfit.Items(indexViewCount) = listItem)
                    End If
                Next

                Exit For
            End If
        Next

        'Check new record
        If newRecord Then
            'Add summary profit list
            summaryProfitRecord = New SummaryProfitInformation()
            summaryProfitRecord.summaryDate = summaryDate
            summaryProfitRecord.stableCoin = stableCoin
            summaryProfitRecord.tradeCoin = tradeCoin
            summaryProfitRecord.takeProfitCount = 1
            summaryProfitRecord.takeProfitAmount = profitAmount
            summaryProfitList.Insert(0, summaryProfitRecord)

            'Add summary profit listview
            listItem = New ListViewItem(summaryDateString)
            listItem.SubItems.Add(summaryProfitRecord.takeProfitCount.ToString("###,##0"))
            listItem.SubItems.Add(summaryProfitRecord.takeProfitAmount.ToString("###,###,##0.00"))
            Me.Invoke(Sub() Me.ListViewSummaryProfit.Items.Add(listItem))
        End If

        'Add profit amount
        If configuration.botTestMode = False Then
            Database.AddProfitOnDate(orderDate, stableCoin, tradeCoin, profitAmount)
        End If
    End Sub

    Public Sub UpdateCryptoTrade(ByVal cryptoTradetRecordUpdate As Crypto, ByVal processNextTimer As Boolean)
        Dim cryptoTradetRecord As Crypto = New Crypto()

        If cryptoTradetRecordUpdate.tradeCoinVolume = Double.NaN Or cryptoTradetRecordUpdate.tradeCoinVolume = 0 Then
            cryptoTradetRecordUpdate.tradeCoinVolume = 0
            cryptoTradetRecordUpdate.tradeCoinPrice = 0
            cryptoTradetRecordUpdate.tradeCoinAmount = 0
            cryptoTradetRecordUpdate.tradeCoinTakeProfitPrice = 0
            cryptoTradetRecordUpdate.tradeCoinFillupPrice = 0
        End If

        For indexCount As Integer = 0 To cryptoTradeList.Count - 1
            cryptoTradetRecord = cryptoTradeList.Item(indexCount)

            If cryptoTradetRecord.stableCoin = cryptoTradetRecordUpdate.stableCoin _
                And cryptoTradetRecord.tradeCoin = cryptoTradetRecordUpdate.tradeCoin Then
                cryptoTradeList.Item(indexCount) = cryptoTradetRecordUpdate

                'Update wallet coin
                Dim cryptoWalletRecord As WalletCoin = GetCryptoWallet(cryptoTradetRecordUpdate.tradeCoin)
                cryptoWalletRecord.coinVolume = cryptoTradetRecordUpdate.tradeCoinVolume
                cryptoWalletRecord.coinPrice = cryptoTradetRecordUpdate.tradeCoinPrice
                cryptoWalletRecord.coinAmount = cryptoTradetRecordUpdate.tradeCoinAmount
                UpdateWalletInformation(cryptoWalletRecord)

                'Update listview
                Dim listItem As ListViewItem = Nothing
                For indexlist As Integer = 0 To ListViewCrypto.Items.Count - 1
                    listItem = ListViewCrypto.Items(indexlist)

                    If listItem.SubItems.Item(0).Text = cryptoTradetRecordUpdate.tradeCoin Then
                        Dim indexUpdate As Integer = indexlist

                        'Date-Time
                        listItem.SubItems.Item(1).Text = Date.Now().ToString("dd MMM yy / HH:mm:ss")
                        'Short Trend
                        listItem.SubItems.Item(2).Text = cryptoTradetRecordUpdate.wma725TrendPercent.ToString("###,##0.0000") + "%"
                        'Long Trend
                        listItem.SubItems.Item(3).Text = cryptoTradetRecordUpdate.wma760TrendPercent.ToString("###,##0.0000") + "%"
                        'Diff Profit Percent
                        listItem.SubItems.Item(4).Text = cryptoTradetRecordUpdate.diffProfitPercent.ToString("###,##0.0000") + "%"
                        'Diff Fillup Percent
                        listItem.SubItems.Item(5).Text = cryptoTradetRecordUpdate.diffFillupPercent.ToString("###,##0.0000") + "%"
                        'Coin Amount
                        listItem.SubItems.Item(6).Text = cryptoTradetRecordUpdate.tradeCoinAmount.ToString("###,##0.00")
                        'Coin Volume
                        listItem.SubItems.Item(7).Text = cryptoTradetRecordUpdate.tradeCoinVolume.ToString("###,##0.00###")
                        'Coin Price
                        listItem.SubItems.Item(8).Text = cryptoTradetRecordUpdate.tradeCoinPrice.ToString("###,##0.00###")
                        'Sell Price
                        listItem.SubItems.Item(9).Text = cryptoTradetRecordUpdate.tradeCoinTakeProfitPrice.ToString("###,##0.00###")
                        'Market Bid Price
                        listItem.SubItems.Item(10).Text = cryptoTradetRecordUpdate.actualBidPrice.ToString("###,##0.00###")
                        'Fillup Price
                        listItem.SubItems.Item(11).Text = cryptoTradetRecordUpdate.tradeCoinFillupPrice.ToString("###,##0.00###")
                        'Market Ask Price
                        listItem.SubItems.Item(12).Text = cryptoTradetRecordUpdate.actualAskPrice.ToString("###,##0.00###")
                        'Fillup Amount
                        listItem.SubItems.Item(13).Text = cryptoTradetRecordUpdate.fillupAmount.ToString("###,##0.00###")

                        'Set color list item
                        If cryptoTradetRecordUpdate.wma725TrendPercent > 0 _
                            And cryptoTradetRecordUpdate.wma760TrendPercent > 0 Then

                            'Trend high
                            listItem.BackColor = Color.LightGreen
                        Else
                            'Trend Low
                            listItem.BackColor = SystemColors.Info
                        End If

                        Me.Invoke(Sub() Me.ListViewCrypto.Items(indexUpdate) = listItem)

                        Exit For
                    End If
                Next

                Exit For
            End If
        Next

        SetScreenValue()

        'Process next crypto trade
        If processNextTimer Then
            Me.Invoke(Sub() Me.TimerAutoTrade.Enabled = botActive)
        End If
    End Sub

    Public Function AdjustAmountBuyCoinSimulate(ByVal amountUpdate As Double) As Double
        balanceAmount = balanceAmount - amountUpdate
        Return balanceAmount
    End Function

    Public Function AdjustAmountSaleCoinSimulate(ByVal amountUpdate As Double) As Double
        balanceAmount = balanceAmount + amountUpdate
        Return balanceAmount
    End Function

    Private Function GetCryptoWallet(ByVal coinNameCondition As String) As WalletCoin
        Dim cryptoWalletRecord As WalletCoin = New WalletCoin()
        Dim resultFound As Boolean = False

        For Each cryptoWalletRecord In cryptoWalletList
            If cryptoWalletRecord.coinName = coinNameCondition Then
                resultFound = True
                Exit For
            End If
        Next

        If resultFound = False Then
            cryptoWalletRecord = New WalletCoin()
            cryptoWalletRecord.coinName = coinNameCondition
            cryptoWalletList.Add(cryptoWalletRecord)
        End If

        Return cryptoWalletRecord
    End Function

    Private Function GetCryptoTrade(ByVal stableCoinName As String, ByVal tradeCoinName As String) As Crypto
        Dim cryptoTradetRecord As Crypto = New Crypto()

        For Each cryptoTradetRecord In cryptoTradeList
            If cryptoTradetRecord.stableCoin = stableCoinName And cryptoTradetRecord.tradeCoin = tradeCoinName Then
                Exit For
            End If
        Next

        Return cryptoTradetRecord
    End Function

    Public Sub UpdateWalletInformation(ByVal cryptoWalletRecordUpdate As WalletCoin)
        Dim cryptoWalletRecord As WalletCoin = Nothing

        For indexCount As Integer = 0 To cryptoWalletList.Count - 1
            cryptoWalletRecord = cryptoWalletList.Item(indexCount)

            If cryptoWalletRecord.coinName = cryptoWalletRecordUpdate.coinName Then
                'Check min coin volume
                If cryptoWalletRecordUpdate.coinAmount < minCoinAmount Then
                    cryptoWalletRecord.coinVolume = 0
                    cryptoWalletRecord.coinPrice = 0
                    cryptoWalletRecord.coinAmount = 0
                Else
                    cryptoWalletRecord.coinVolume = cryptoWalletRecordUpdate.coinVolume
                    cryptoWalletRecord.coinPrice = cryptoWalletRecordUpdate.coinPrice
                    cryptoWalletRecord.coinAmount = cryptoWalletRecordUpdate.coinAmount
                End If

                cryptoWalletList.Item(indexCount) = cryptoWalletRecord
                Exit For
            End If
        Next
    End Sub

    Private Sub SetupMasterListValue()
        ComboBoxStepAmount.Items.Clear()
        For indexCount As Integer = 1 To 100
            If indexCount >= 4 Then
                Dim valueDouble As Double = indexCount * 5
                ComboBoxStepAmount.Items.Add(valueDouble.ToString("##,##0.00"))
            End If
        Next

        ComboBoxIntervalTime.Items.Clear()
        ComboBoxIntervalTime.Items.Add("1m")
        ComboBoxIntervalTime.Items.Add("5m")
        ComboBoxIntervalTime.Items.Add("15m")
        ComboBoxIntervalTime.Items.Add("1H")
        ComboBoxIntervalTime.Items.Add("4H")
        ComboBoxIntervalTime.Items.Add("1D")

        ComboBoxBuyCommission.Items.Clear()
        ComboBoxSaleCommission.Items.Clear()
        ComboBoxTakeProfitPercent.Items.Clear()
        ComboBoxFillupPercent.Items.Clear()
        For indexCount As Integer = 1 To 100
            Dim valueDouble As Double = indexCount * 0.05
            ComboBoxBuyCommission.Items.Add(valueDouble.ToString("##0.00"))
            ComboBoxSaleCommission.Items.Add(valueDouble.ToString("##0.00"))
            ComboBoxTakeProfitPercent.Items.Add(valueDouble.ToString("##0.00"))
            ComboBoxFillupPercent.Items.Add(valueDouble.ToString("##0.00"))
        Next

        ComboBoxLimitCoinAmount.Items.Clear()
        For indexCount As Integer = 1 To 100
            Dim valueDouble As Double = indexCount * 50
            ComboBoxLimitCoinAmount.Items.Add(valueDouble.ToString("###,##0.00"))
        Next

        ComboBoxSummaryHour.Items.Clear()
        For indexCount As Integer = 1 To 24
            ComboBoxSummaryHour.Items.Add(indexCount.ToString())
        Next
    End Sub

    Private Sub SetScreenValueMaster()
        TextBoxAPIKey.Text = APIService.bitkubKey
        TextBoxAPISecret.Text = APIService.bitkubSecret
        TextBoxLineToken.Text = APIService.lineToken

        If configuration.botTestMode Then
            RadioButtonTradeModeSimulate.Checked = True
        Else
            RadioButtonTradeModeReal.Checked = True
        End If

        ComboBoxSummaryHour.Text = configuration.sendSummaryHour.ToString()
        CheckBoxSendLineSummary.Checked = configuration.sendLineSummary
        CheckBoxSendLineBuyCoin.Checked = configuration.sendLineBuyCoin
        CheckBoxSendLineSellCoin.Checked = configuration.sendLineSellCoin
        ComboBoxTradeStyle.Text = configuration.tradeStyle

        ComboBoxStableCoin.Text = configuration.stableCoin
        ComboBoxStepAmount.Text = configuration.stepupAmount.ToString("##0.00")
        ComboBoxIntervalTime.Text = configuration.tradeInterval
        ComboBoxBuyCommission.Text = configuration.tradeCoinBuyCommission.ToString("##0.00")
        ComboBoxSaleCommission.Text = configuration.tradeCoinSaleCommission.ToString("##0.00")
        ComboBoxTakeProfitPercent.Text = configuration.tradeCoinTakeProfitPercent.ToString("##0.00")
        ComboBoxFillupPercent.Text = configuration.tradeCoinFillupPercent.ToString("##0.00")
        ComboBoxTradeStyle.Text = configuration.tradeStyle
        ComboBoxLimitCoinAmount.Text = configuration.limitCoinAmount.ToString("###,##0.00")
        CheckBoxBuyNewOnNextWave.Checked = configuration.buyNewOnNextWave

        Dim listItem As ListViewItem = Nothing
        Dim convertDate As DateTime = Nothing

        ListViewSummaryProfit.Items.Clear()
        For Each summaryProfitRecord As SummaryProfitInformation In summaryProfitList
            convertDate = DateTime.ParseExact(summaryProfitRecord.summaryDate, "yyyyMMdd", Nothing)

            listItem = New ListViewItem(convertDate.ToString("dd MMM yyyy") + " / " + summaryProfitRecord.tradeCoin)
            listItem.SubItems.Add(summaryProfitRecord.takeProfitCount.ToString("###,##0"))
            listItem.SubItems.Add(summaryProfitRecord.takeProfitAmount.ToString("###,##0.00"))

            listItem.BackColor = SystemColors.Info
            ListViewSummaryProfit.Items.Add(listItem)
        Next

        'Check application active
        If applicationExpire And configuration.botTestMode = False Then
            CheckBoxStopBuyNew.Checked = False
            CheckBoxStopBuyNew.Enabled = False
        End If

        CheckBoxAutoRun.Checked = configuration.autoRun
    End Sub

    Private Sub SetScreenValue()
        TextBoxBalanceAmount.Text = balanceAmount.ToString("###,###,##0.00")

        Dim coinTotalAmount As Double = 0
        For Each cryptoRecord As Crypto In cryptoTradeList
            If cryptoRecord.tradeCoinAmount > 0 Then
                coinTotalAmount = coinTotalAmount + cryptoRecord.tradeCoinAmount
            End If
        Next

        Dim totalProfitAmount As Double = 0
        Dim tradeDateInteger As Integer = Date.Now().ToString("yyyyMMdd")
        For Each summaryProfit As SummaryProfitInformation In summaryProfitList
            If summaryProfit.summaryDate = tradeDateInteger Then
                totalProfitAmount = totalProfitAmount + summaryProfit.takeProfitAmount
            End If
        Next

        TextBoxCoinAmount.Text = coinTotalAmount.ToString("###,###,##0.00")
        TextBoxProfitAmount.Text = totalProfitAmount.ToString("###,###,##0.00")

        Dim grandTotal As Double = balanceAmount + coinTotalAmount
        TextBoxTotalAmount.Text = grandTotal.ToString("###,###,##0.00")

        'Check new max budget use
        If maxBudgetUse < coinTotalAmount Then
            maxBudgetUse = coinTotalAmount
        End If
        TextBoxMaxBudgetUse.Text = maxBudgetUse.ToString("###,###,##0.00")

        ToolStripLabelRemainTakeProfitTime.Text = "Expire Date: " + expireLocalDateTime.ToString("dd MMM yyyy")
        If applicationExpire = False Then
            ToolStripLabelRemainTakeProfitTime.BackColor = SystemColors.Info
        Else
            ToolStripLabelRemainTakeProfitTime.BackColor = Color.Salmon
        End If

        'Check application active
        If applicationExpire And configuration.botTestMode = False Then
            CheckBoxStopBuyNew.Checked = False
            CheckBoxStopBuyNew.Enabled = False
        End If
    End Sub

    Private Function CheckRequireStart() As Boolean
        Dim result As Boolean = True

        If APIService.bitkubKey = "" And configuration.botTestMode = False Then
            MsgBox("Please update an binance api key", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf APIService.bitkubSecret = "" And configuration.botTestMode = False Then
            MsgBox("Please update an binance api secret", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf applicationExpire And configuration.botTestMode = False Then
            MsgBox("Application is inactive", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeInterval = "" Then
            MsgBox("Please choose a trade interval", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.stableCoin = "" Then
            MsgBox("Please choose an stable coin", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.stepupAmount = 0 Then
            MsgBox("Please choose a step up amount", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeCoinBuyCommission = 0 Then
            MsgBox("Please choose a buy commission", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeCoinSaleCommission = 0 Then
            MsgBox("Please choose a sale commission", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeCoinTakeProfitPercent = 0 Then
            MsgBox("Please choose a take profit percent", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeCoinFillupPercent = 0 Then
            MsgBox("Please choose a fillup percent", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf configuration.tradeStyle = "" Then
            MsgBox("Please choose a trade style", MsgBoxStyle.Critical, "Start Trade")
            result = False
        ElseIf ListViewCrypto.Items.Count = 0 Then
            MsgBox("Please select a trade coin", MsgBoxStyle.Critical, "Start Trade")
            result = False
        End If

        Return result
    End Function

    Private Sub CheckBoxAutoRun_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAutoRun.CheckedChanged
        configuration.autoRun = CheckBoxAutoRun.Checked

        If CheckBoxAutoRun.Checked Then
            If CheckRequireStart() = False Then
                configuration.autoRun = False
                CheckBoxAutoRun.Checked = False
            End If
        End If
    End Sub

    Private Sub ComboBoxStableCoin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxStableCoin.SelectedIndexChanged
        Dim indexCount As Integer = 0

        configuration.stableCoin = ComboBoxStableCoin.Text

        'Get wallet stable coin 
        If RadioButtonTradeModeReal.Checked Then
            While (APIService.GetWalletCoinBalance(configuration.stableCoin, 0.001, balanceAmount) = False)
                Threading.Thread.Sleep(2000)
            End While
        End If

        ListViewCrypto.Items.Clear()
        CheckedListBoxTradeCoin.Items.Clear()

        'Assign crypto list
        For Each cryptoRecord As Crypto In cryptoTradeList
            If cryptoRecord.stableCoin = configuration.stableCoin Then
                CheckedListBoxTradeCoin.Items.Add(cryptoRecord.tradeCoin)
            End If
        Next

        'Check select crypto on configuration
        For Each tradeCoinMasterRecord As TradeCoinInformation In tradeCoinList
            indexCount = 0

            For Each cryptoCheckString In CheckedListBoxTradeCoin.Items
                If cryptoCheckString = tradeCoinMasterRecord.tradeCoin Then
                    CheckedListBoxTradeCoin.SetItemChecked(indexCount, True)
                    Exit For
                End If

                indexCount = indexCount + 1
            Next
        Next

        'Clear select list
        tradeCoinList = New List(Of TradeCoinInformation)
        SetScreenValue()
    End Sub

    Private Sub CheckedListBoxTradeCoin_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBoxTradeCoin.ItemCheck
        If e.CurrentValue = CheckState.Checked Then
            'Remove crypto item
            For Each listItem As ListViewItem In ListViewCrypto.Items
                If listItem.SubItems.Item(0).Text = CheckedListBoxTradeCoin.Items(e.Index).ToString() Then
                    'Check have remain trade coin volume
                    Dim cryptoTradeRecord As Crypto = GetCryptoTrade(configuration.stableCoin, listItem.SubItems.Item(0).Text)

                    If cryptoTradeRecord.tradeCoinVolume <= 0 Then
                        'Remove list view item
                        ListViewCrypto.Items.Remove(listItem)
                    Else
                        'Cancle process
                        MsgBox("Cannot remove trade coin have a remain of volume!", MsgBoxStyle.Critical, "Remove Trade Coin")
                        e.NewValue = CheckState.Checked
                    End If

                    Exit For
                End If
            Next

        Else
            'Add crypto item
            Dim listItem As ListViewItem = Nothing
            Dim cryptoWalletRecord As WalletCoin = GetCryptoWallet(CheckedListBoxTradeCoin.Items(e.Index).ToString())
            Dim cryptoTradeRecord As Crypto = GetCryptoTrade(configuration.stableCoin, CheckedListBoxTradeCoin.Items(e.Index).ToString())
            Dim remainCoinVolume = cryptoWalletRecord.coinVolume - (cryptoWalletRecord.coinVolume Mod cryptoTradeRecord.decimalPlace)

            'Set value of remain coin
            If remainCoinVolume > 0 Then
                cryptoTradeRecord.tradeCoinVolume = remainCoinVolume

                APIService.GetHistoryPrice(cryptoTradeRecord.symbolTrade,
                                            configuration.tradeCoinBuyCommission,
                                            cryptoTradeRecord.decimalPlace,
                                            cryptoTradeRecord.tradeCoinVolume,
                                            cryptoTradeRecord.tradeCoinPrice)

                While (cryptoTradeRecord.tradeCoinPrice = 0)
                    Threading.Thread.Sleep(2000)

                    APIService.GetHistoryPrice(cryptoTradeRecord.symbolTrade,
                                                configuration.tradeCoinBuyCommission,
                                                cryptoTradeRecord.decimalPlace,
                                                cryptoTradeRecord.tradeCoinVolume,
                                                cryptoTradeRecord.tradeCoinPrice)
                End While

                cryptoTradeRecord.tradeCoinAmount = cryptoTradeRecord.tradeCoinVolume * cryptoTradeRecord.tradeCoinPrice
                cryptoTradeRecord.stepupAmount = configuration.stepupAmount
                cryptoTradeRecord.tradeCoinBuyCommission = configuration.tradeCoinBuyCommission
                cryptoTradeRecord.tradeCoinSaleCommission = configuration.tradeCoinSaleCommission
                cryptoTradeRecord.tradeCoinTakeProfitPercent = configuration.tradeCoinTakeProfitPercent

                If cryptoTradeRecord.tradeCoinVolume < minCoinAmount Then
                    cryptoTradeRecord.tradeCoinFillupPercent = configuration.tradeCoinFillupPercent
                End If

                cryptoTradeRecord.CalculateTakeProfitAndFillupInformation(minCoinAmount)

            Else
                cryptoTradeRecord.tradeCoinVolume = 0
                cryptoTradeRecord.tradeCoinPrice = 0
                cryptoTradeRecord.tradeCoinAmount = 0
                cryptoTradeRecord.stepupAmount = configuration.stepupAmount
                cryptoTradeRecord.tradeCoinBuyCommission = configuration.tradeCoinBuyCommission
                cryptoTradeRecord.tradeCoinSaleCommission = configuration.tradeCoinSaleCommission
                cryptoTradeRecord.tradeCoinTakeProfitPercent = configuration.tradeCoinTakeProfitPercent

                If cryptoTradeRecord.tradeCoinVolume < minCoinAmount Then
                    cryptoTradeRecord.tradeCoinFillupPercent = configuration.tradeCoinFillupPercent
                End If

                cryptoTradeRecord.CalculateTakeProfitAndFillupInformation(minCoinAmount)

            End If

            'Update wallet coin information
            cryptoWalletRecord.coinVolume = cryptoTradeRecord.tradeCoinVolume
            cryptoWalletRecord.coinPrice = cryptoTradeRecord.tradeCoinPrice
            cryptoWalletRecord.coinAmount = cryptoTradeRecord.tradeCoinAmount
            UpdateWalletInformation(cryptoWalletRecord)

            listItem = New ListViewItem(cryptoTradeRecord.tradeCoin)                                        'Trade Coin
            listItem.SubItems.Add("")                                                                       'Date-Time
            listItem.SubItems.Add("0.0000%")                                                                'Short Trend
            listItem.SubItems.Add("0.0000%")                                                                'Long Trend
            listItem.SubItems.Add("0.0000%")                                                                'Difference Take Profit Percent
            listItem.SubItems.Add("0.0000%")                                                                'Difference Fillup Percent
            listItem.SubItems.Add(cryptoTradeRecord.tradeCoinAmount.ToString("###,###,##0.00###"))          'Amount
            listItem.SubItems.Add(cryptoTradeRecord.tradeCoinVolume.ToString("###,###,##0.00###"))          'Volume
            listItem.SubItems.Add(cryptoTradeRecord.tradeCoinPrice.ToString("###,###,##0.00###"))           'Price
            listItem.SubItems.Add(cryptoTradeRecord.tradeCoinTakeProfitPrice.ToString("###,###,##0.00###")) 'Take profit price
            listItem.SubItems.Add(cryptoTradeRecord.actualBidPrice.ToString("###,###,##0.00###"))           'Bid price
            listItem.SubItems.Add(cryptoTradeRecord.tradeCoinFillupPrice.ToString("###,###,##0.00###"))     'Fillup price
            listItem.SubItems.Add(cryptoTradeRecord.actualAskPrice.ToString("###,###,##0.00###"))           'Ask price
            listItem.SubItems.Add("0.00")                                                                   'Fillup Amount

            listItem.BackColor = SystemColors.Info
            ListViewCrypto.Items.Add(listItem)
        End If
    End Sub

    Private Sub ButtonUpdateBinanceAPI_Click(sender As Object, e As EventArgs) Handles ButtonUpdateBinanceAPI.Click
        If MsgBox("Do you want to update a binance API?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Save Binance API") = MsgBoxResult.Yes Then
            APIService.bitkubKey = Trim(TextBoxAPIKey.Text)
            APIService.bitkubSecret = Trim(TextBoxAPISecret.Text)

            configuration.bitkubKey = Trim(TextBoxAPIKey.Text)
            configuration.bitkubSecret = Trim(TextBoxAPISecret.Text)
            Database.UpdateConfiguration(configuration)
        End If
    End Sub

    Private Sub ButtonUpdateLineToken_Click(sender As Object, e As EventArgs) Handles ButtonUpdateLineToken.Click
        If MsgBox("Do you want to update a line token?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Save Line Taken") = MsgBoxResult.Yes Then
            APIService.lineToken = Trim(TextBoxLineToken.Text)

            configuration.lineToken = Trim(TextBoxLineToken.Text)
            configuration.sendLineSummary = CheckBoxSendLineSummary.Checked
            configuration.sendLineBuyCoin = CheckBoxSendLineBuyCoin.Checked
            configuration.sendLineSellCoin = CheckBoxSendLineSellCoin.Checked

            Dim integerString As String = Trim(ComboBoxSummaryHour.Text)
            integerString = integerString.Replace(",", "")

            Try
                configuration.sendSummaryHour = Integer.Parse(integerString)
                summaryTime = Date.Now().AddHours(configuration.sendSummaryHour)

            Catch ex As Exception

            End Try
            Database.UpdateConfiguration(configuration)
        End If
    End Sub

    Private Sub ComboBoxStepAmount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxStepAmount.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxStepAmount.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.stepupAmount = Double.Parse(doubleString)
            SetScreenValue()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxTakeProfitPercent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxTakeProfitPercent.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxTakeProfitPercent.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.tradeCoinTakeProfitPercent = Double.Parse(doubleString)
            SetScreenValue()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxFillupPercent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxFillupPercent.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxFillupPercent.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.tradeCoinFillupPercent = Double.Parse(doubleString)
            SetScreenValue()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxBuyCommission_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxBuyCommission.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxBuyCommission.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.tradeCoinBuyCommission = Double.Parse(doubleString)
            SetScreenValue()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxSaleCommission_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSaleCommission.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxSaleCommission.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.tradeCoinSaleCommission = Double.Parse(doubleString)
            SetScreenValue()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxLimitCoinAmount_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxLimitCoinAmount.SelectedIndexChanged
        Dim doubleString As String = Trim(ComboBoxLimitCoinAmount.Text)
        doubleString = doubleString.Replace(",", "")

        Try
            configuration.limitCoinAmount = Double.Parse(doubleString)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadioButtonTradeModeSimulate_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonTradeModeSimulate.CheckedChanged
        If RadioButtonTradeModeSimulate.Checked Then
            configuration.botTestMode = True
            maxBudgetUse = 0
            balanceAmount = 10000

            Dim walletCoinRecord As WalletCoin = Nothing
            For indexCount As Integer = 0 To cryptoWalletList.Count - 1
                walletCoinRecord = cryptoWalletList.Item(indexCount)
                walletCoinRecord.coinVolume = 0
                walletCoinRecord.coinPrice = 0
                walletCoinRecord.coinAmount = 0
                cryptoWalletList.Item(indexCount) = walletCoinRecord
            Next

            Dim cryptoTradeRecord As Crypto = Nothing
            For indexCount As Integer = 0 To cryptoTradeList.Count - 1
                cryptoTradeRecord = cryptoTradeList.Item(indexCount)
                cryptoTradeRecord.tradeCoinVolume = 0
                cryptoTradeRecord.tradeCoinPrice = 0
                cryptoTradeRecord.tradeCoinAmount = 0

                cryptoTradeList.Item(indexCount) = cryptoTradeRecord
            Next

            Dim listItem As ListViewItem = Nothing
            For indexCount As Integer = 0 To ListViewCrypto.Items.Count - 1
                listItem = ListViewCrypto.Items(indexCount)
                listItem.SubItems.Item(1).Text = ""           'Update Date-Time
                listItem.SubItems.Item(2).Text = "0.0000%"    'Short Trend
                listItem.SubItems.Item(3).Text = "0.0000%"    'Long Trend
                listItem.SubItems.Item(4).Text = "0.0000%"    'Difference Take Profit Percent
                listItem.SubItems.Item(5).Text = "0.0000%"    'Difference Fillup Percent
                listItem.SubItems.Item(6).Text = "0.0000"     'Amount
                listItem.SubItems.Item(7).Text = "0.0000"     'Volume
                listItem.SubItems.Item(8).Text = "0.0000"     'Price
                listItem.SubItems.Item(9).Text = "0.0000"     'Sell Price
                listItem.SubItems.Item(10).Text = "0.0000"    'Bid Price
                listItem.SubItems.Item(11).Text = "0.0000"    'Fillup Price
                listItem.SubItems.Item(12).Text = "0.0000"    'Ask Price
                listItem.SubItems.Item(13).Text = "0.0000"    'Fillup Amount

                listItem.BackColor = SystemColors.Info
                ListViewCrypto.Items(indexCount) = listItem
            Next

            ListViewTrade.Items.Clear()
            ListViewSummaryProfit.Items.Clear()
            ListViewErrorLog.Items.Clear()

            'Set screen value
            SetScreenValue()

            TextBoxBalanceAmount.ReadOnly = False
            TextBoxBalanceAmount.BackColor = SystemColors.Info
            ButtonHistoryTest.Visible = True

            Me.Text = "Binance Spot Trade [ Simulate Trade ]" + " [Version: " + My.Application.Info.Version.ToString() + "]"

        Else
            TextBoxBalanceAmount.ReadOnly = True
            TextBoxBalanceAmount.BackColor = Color.Aquamarine
            ButtonHistoryTest.Visible = False

        End If
    End Sub

    Private Sub RadioButtonTradeModeReal_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonTradeModeReal.CheckedChanged
        If RadioButtonTradeModeReal.Checked Then
            'Check API Binance
            If configuration.bitkubKey = "" Or configuration.bitkubSecret = "" Then
                MsgBox("Please input API Binance key and secret!", MsgBoxStyle.Critical, "Change Trade Mode")
                RadioButtonTradeModeSimulate.Checked = True
                Exit Sub
            End If

            If applicationExpire Then
                MsgBox("Application is not active!", MsgBoxStyle.Critical, "Change Trade Mode")
                RadioButtonTradeModeSimulate.Checked = True
                Exit Sub
            End If

            configuration.botTestMode = False

            maxBudgetUse = 0

            While (APIService.GetWalletCoinBalance(configuration.stableCoin, 0.001, balanceAmount) = False)
                Threading.Thread.Sleep(2000)
            End While

            'Set screen value
            SetScreenValue()

            TextBoxBalanceAmount.ReadOnly = True
            TextBoxBalanceAmount.BackColor = Color.Aquamarine
            ButtonHistoryTest.Visible = False

            Me.Text = "Binance Spot Trade [ Real Trade ]" + " [Version: " + My.Application.Info.Version.ToString() + "]"

        Else
            TextBoxBalanceAmount.ReadOnly = False
            TextBoxBalanceAmount.BackColor = SystemColors.Info
            ButtonHistoryTest.Visible = True

        End If
    End Sub

    Private Sub ComboBoxIntervalTime_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxIntervalTime.SelectedIndexChanged
        Dim selectedInterval As String = ComboBoxIntervalTime.SelectedItem.ToString()
        If intervalMapping.ContainsKey(selectedInterval) Then
            configuration.tradeInterval = intervalMapping(selectedInterval)
        Else
            configuration.tradeInterval = "5"
        End If
        SetScreenValue()
    End Sub

    Private Sub ComboBoxTradeStyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxTradeStyle.SelectedIndexChanged
        If ComboBoxTradeStyle.Text <> "" Then
            configuration.tradeStyle = ComboBoxTradeStyle.Text
        End If
    End Sub

    Private Sub CheckBoxStopBuyNew_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxStopBuyNew.CheckedChanged
        configuration.stopBuyNew = CheckBoxStopBuyNew.Checked
    End Sub

    Private Sub CheckBoxBuyNewOnNextWave_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxBuyNewOnNextWave.CheckedChanged
        configuration.buyNewOnNextWave = CheckBoxBuyNewOnNextWave.Checked
    End Sub

    Private Sub ButtonStartTrade_Click(sender As Object, e As EventArgs) Handles ButtonStartTrade.Click
        If CheckRequireStart() Then
            If MsgBox("Do you want to start an auto trade?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Start Auto Trade") = MsgBoxResult.Yes Then
                botActive = True
                TimerAutoTrade.Enabled = botActive

                RadioButtonTradeModeSimulate.Enabled = False
                RadioButtonTradeModeReal.Enabled = False
                ComboBoxStableCoin.Enabled = False

                ButtonStartTrade.Visible = False
                ButtonStopTrade.Visible = True

                Me.BackColor = SystemColors.GradientActiveCaption

                'Set message log
                ToolStripLabelMessageLog.Text = "Application is started...."
                ToolStripLabelMessageLog.BackColor = SystemColors.Info

                Dim lineMessage = vbCrLf _
                                + "Start Bot" + vbCrLf _
                                + "On device: " + Environment.MachineName + vbCrLf _
                                + "At " + Date.Now().ToString("dd MMM yyyy / HH:mm") + vbCrLf + vbCrLf _
                                + "With condition" + vbCrLf _
                                + "Interval time: " + configuration.tradeInterval + vbCrLf _
                                + "First buy amount: " + configuration.stepupAmount.ToString("###,##0.00") + vbCrLf _
                                + "Take profit on (%25): " + configuration.tradeCoinTakeProfitPercent.ToString("##0.00") + "%25" + vbCrLf _
                                + "Fillup on (%25): " + configuration.tradeCoinFillupPercent.ToString("##0.00") + "%25" + vbCrLf _
                                + "Buy commission (%25): " + configuration.tradeCoinBuyCommission.ToString("##0.00") + "%25" + vbCrLf _
                                + "Sale commission (%25): " + configuration.tradeCoinSaleCommission.ToString("##0.00") + "%25" + vbCrLf _
                                + "Limit Coin Amount: " + configuration.limitCoinAmount.ToString("###,##0.00") + vbCrLf _
                                + "Trade style: " + configuration.tradeStyle + vbCrLf

                Dim errorMessage As String = ""
                APIService.SendLineNotification(lineMessage, errorMessage)
            End If
        End If
    End Sub

    Private Sub ButtonStopTrade_Click(sender As Object, e As EventArgs) Handles ButtonStopTrade.Click
        If MsgBox("Do you want to stop an auto trade?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Stop Auto Trade") = MsgBoxResult.Yes Then
            botActive = False
            TimerAutoTrade.Enabled = botActive

            RadioButtonTradeModeSimulate.Enabled = True
            RadioButtonTradeModeReal.Enabled = True
            ComboBoxStableCoin.Enabled = True

            ButtonStartTrade.Visible = True
            ButtonStopTrade.Visible = False

            Me.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub ButtonHistoryTest_Click(sender As Object, e As EventArgs) Handles ButtonHistoryTest.Click
        If MsgBox("Do you want to test a history data?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "History Test") = MsgBoxResult.No Then
            Exit Sub
        End If

        'Test Function
        ListViewTrade.Items.Clear()
        ListViewSummaryProfit.Items.Clear()

        summaryProfitList = New List(Of SummaryProfitInformation)
        Dim intervalLimit As Integer = 1000

        'Get and set default candles process
        For Each litItem As ListViewItem In ListViewCrypto.Items
            Dim cryptTradeRecord As Crypto = GetCryptoTrade(configuration.stableCoin, litItem.SubItems.Item(0).Text)

            cryptTradeRecord.balanceAmount = balanceAmount
            cryptTradeRecord.stepupAmount = configuration.stepupAmount
            cryptTradeRecord.tradeCoinVolume = 0
            cryptTradeRecord.tradeCoinPrice = 0
            cryptTradeRecord.tradeCoinAmount = 0

            cryptTradeRecord.candlesSimulateList = APIService.GetCandlesTrade(cryptTradeRecord.stableCoin,
                                                                              cryptTradeRecord.tradeCoin,
                                                                              intervalLimit,
                                                                              APIService.SortSequenceCandelsValue.ASC)

            cryptTradeRecord.candlesProcessList = New List(Of Crypto.CandlesInformation)
            'Set initial Candles
            For indexCount As Integer = 0 To 99
                cryptTradeRecord.candlesProcessList.Insert(0, cryptTradeRecord.candlesSimulateList.Item(indexCount))
            Next

            'Process trade
            cryptTradeRecord.StartTrade(True)

            'Update Crypto Information
            UpdateCryptoTrade(cryptTradeRecord, True)
        Next

        'Process Candles
        Dim candlesRecord As Crypto.CandlesInformation = Nothing
        For indexCount As Integer = 100 To intervalLimit - 1
            For Each litItem As ListViewItem In ListViewCrypto.Items
                Dim cryptTradeRecord As Crypto = GetCryptoTrade(configuration.stableCoin, litItem.SubItems.Item(0).Text)

                cryptTradeRecord.balanceAmount = balanceAmount
                cryptTradeRecord.stepupAmount = configuration.stepupAmount
                cryptTradeRecord.tradeStyle = configuration.tradeStyle
                cryptTradeRecord.tradeCoinTakeProfitPercent = configuration.tradeCoinBuyCommission

                If cryptTradeRecord.tradeCoinAmount = minCoinAmount Then
                    cryptTradeRecord.tradeCoinFillupPercent = configuration.tradeCoinFillupPercent
                End If

                cryptTradeRecord.CalculateTakeProfitAndFillupInformation(minCoinAmount)

                'Assign Candle
                candlesRecord = cryptTradeRecord.candlesSimulateList.Item(indexCount)
                cryptTradeRecord.candlesProcessList.Insert(0, candlesRecord)
                cryptTradeRecord.candlesProcessList.RemoveAt(100)

                'Process trade
                cryptTradeRecord.StartTrade(True)

                'Update Crypto Information
                UpdateCryptoTrade(cryptTradeRecord, False)
                SetScreenValue()
            Next
        Next
    End Sub

    Private Sub DisplayProgramMenu_Click(sender As Object, e As EventArgs) Handles DisplayProgramMenu.Click
        Me.ShowInTaskbar = True
        Me.Visible = True
    End Sub

    Private Sub ExitProgramMenu_Click(sender As Object, e As EventArgs) Handles ExitProgramMenu.Click
        needExit = True
        Me.Close()
    End Sub

    Private Sub FrmSpotTrade_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If needExit Then
            'Save configuration
            Database.UpdateConfiguration(configuration)

            'Update Coin trade list
            tradeCoinList = New List(Of TradeCoinInformation)
            For Each listItem As ListViewItem In ListViewCrypto.Items
                Dim tradeCoinRecord As TradeCoinInformation = New TradeCoinInformation()
                tradeCoinRecord.symbol = listItem.SubItems.Item(0).Text + configuration.stableCoin
                tradeCoinRecord.stableCoin = configuration.stableCoin
                tradeCoinRecord.tradeCoin = listItem.SubItems.Item(0).Text

                tradeCoinList.Add(tradeCoinRecord)
            Next
            Database.UpdateTradeCoinList(configuration.stableCoin, tradeCoinList)

            'Send line notification
            Dim lineMessage = "Stop Bot" + vbCrLf _
                            + "On device: " + Environment.MachineName + vbCrLf _
                            + "At " + Date.Now().ToString("dd MMM yyyy / HH:mm")

            Dim errorMessage As String = ""
            APIService.SendLineNotification(lineMessage, errorMessage)

            'Waiting binance database update 2 second
            Threading.Thread.Sleep(2000)

        Else
            e.Cancel = True
            Me.ShowInTaskbar = False
            Me.Visible = False

        End If
    End Sub

    Private Sub DownloadLicenseMenu_Click(sender As Object, e As EventArgs) Handles DownloadLicenseMenu.Click
        SaveFileDialogLicense.Title = "Save License File"
        SaveFileDialogLicense.FileName = "BitkubSpotTradeLicense.key"
        SaveFileDialogLicense.Filter = "License files (*.key)|*.key"
        SaveFileDialogLicense.RestoreDirectory = True

        If SaveFileDialogLicense.ShowDialog() = DialogResult.OK Then
            SaveLicenseKey(SaveFileDialogLicense.FileName)
        End If
    End Sub

    Private Sub UploadLicenseKeyToolMenu_Click(sender As Object, e As EventArgs) Handles UploadLicenseKeyToolMenu.Click
        OpenFileDialogLicense.Title = "Save License File"
        OpenFileDialogLicense.FileName = "BitkubSpotTradeLicense.key"
        OpenFileDialogLicense.Filter = "License files (*.key)|*.key"
        OpenFileDialogLicense.RestoreDirectory = True

        If OpenFileDialogLicense.ShowDialog() = DialogResult.OK Then
            Dim dataLicense As String = File.ReadAllText(OpenFileDialogLicense.FileName)
            UpdateLicenseKey(dataLicense)
        End If
    End Sub

    Private Sub SaveLicenseKey(ByVal filePath As String)
        Try
            Dim keyCode As String = "BitkubSpotTradeByCarotcap"
            Dim hashLicenseString As HashLicenseString = New HashLicenseString(keyCode)

            'Setup Json Data
            Dim stringWriter = New StringWriter()
            Dim writer As New XmlTextWriter(stringWriter)
            writer.WriteStartDocument(True)
            writer.Formatting = Formatting.Indented
            writer.Indentation = 2
            writer.WriteStartElement("licenseKey")

            writer.WriteElementString("serialNumber", serialNumber)

            If applicationExpire Then
                writer.WriteElementString("expireDateLocal", APIService.GetPreviousDate().ToString("yyyyMMdd"))
            Else
                writer.WriteElementString("expireDateLocal", expireLocalDateTime.ToString("yyyyMMdd"))
            End If

            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Close()

            'Covert to xml string
            Dim xmlString As String = stringWriter.ToString()

            'Encrypt Data
            Dim encryptData As String = hashLicenseString.EncryptData(xmlString)
            File.WriteAllText(filePath, encryptData)

            MsgBox("Save license file completed on path " + filePath, MsgBoxStyle.Information, "Save License File")

        Catch ex As Exception
            MsgBox(ex.Message(), MsgBoxStyle.Critical, "Save License File")

        End Try
    End Sub

    Private Sub UpdateLicenseKey(ByVal licenseKey As String)
        Dim expireLocalDateTimeValue As DateTime = Nothing
        Dim errorMessage As String = ""

        If GetExpirDateFromLicenseKey(licenseKey, expireLocalDateTimeValue, errorMessage) Then
            If MsgBox("New license key is expire on " + expireLocalDateTimeValue.ToString("dd MMM yyyy") + vbCrLf + "Do you want to update a license?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                configuration.licenseKey = licenseKey
                expireLocalDateTime = expireLocalDateTimeValue

                If expireLocalDateTime >= Date.Now() Then
                    applicationExpire = False
                End If

                SetScreenValue()
            End If
        Else
            MsgBox(errorMessage, MsgBoxStyle.Critical, "Load License Key")
        End If

    End Sub

    Private Sub TabControlTrade_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControlTrade.DrawItem
        If e.Index = TabControlTrade.SelectedTab.TabIndex Then
            e.Graphics.FillRectangle(New SolidBrush(Color.GreenYellow), e.Bounds)
        Else
            e.Graphics.FillRectangle(New SolidBrush(SystemColors.Control), e.Bounds)
        End If

        Dim stringFlags As StringFormat = New StringFormat()
        stringFlags.Alignment = StringAlignment.Center
        stringFlags.LineAlignment = StringAlignment.Center

        Dim paddedBounds As Rectangle = e.Bounds
        paddedBounds.Inflate(-2, -2)
        e.Graphics.DrawString(TabControlTrade.TabPages(e.Index).Text, Me.Font, SystemBrushes.ControlText, paddedBounds, stringFlags)

    End Sub

    Private Sub TimerSetupEnvironment_Tick(sender As Object, e As EventArgs) Handles TimerSetupEnvironment.Tick
        Dim walletList As List(Of APIService.WalletInformation) = New List(Of APIService.WalletInformation)
        Dim symbolList As List(Of APIService.SymbolInformation) = New List(Of APIService.SymbolInformation)

        TimerSetupEnvironment.Enabled = False

        'Check Expire Application
        Dim errorMessage As String = ""
        GetExpirDateFromLicenseKey(configuration.licenseKey, expireLocalDateTime, errorMessage)

        'Get Active Application
        applicationExpire = APIService.GetApplicationActive(expireLocalDateTime)

        'Get crypto wallet list and crypto trade list 
        If configuration.botTestMode Then
            'Get crypto trade list
            symbolList = APIService.GetSymbolTradeList()

            Dim cryptoTradeRecord As Crypto = Nothing
            For Each symbolRecode As APIService.SymbolInformation In symbolList
                cryptoTradeRecord = New Crypto()
                cryptoTradeRecord.symbolTrade = symbolRecode.symbolTrade
                cryptoTradeRecord.tradeCoin = symbolRecode.tradeCoin
                cryptoTradeRecord.stableCoin = symbolRecode.stableCoin
                cryptoTradeRecord.decimalPlace = symbolRecode.decimalPlace
                cryptoTradeList.Add(cryptoTradeRecord)
            Next

            Me.Text = "Biikub Spot Trade [ Simulate Trade ]" + " [Version: " + My.Application.Info.Version.ToString() + "]"

        Else
            'Get crypto trade list
            symbolList = APIService.GetSymbolTradeList()

            'Get crypto wallet list
            walletList = APIService.GetWalletList(symbolList)

            Dim walletCoinRecord As WalletCoin = Nothing
            For Each walletRecord As APIService.WalletInformation In walletList
                walletCoinRecord = New WalletCoin()
                walletCoinRecord.coinName = walletRecord.coinName
                walletCoinRecord.coinVolume = walletRecord.coinVolume
                walletCoinRecord.decimalPlace = 0
                walletCoinRecord.coinPrice = 0
                walletCoinRecord.coinAmount = 0

                cryptoWalletList.Add(walletCoinRecord)
            Next

            Dim cryptoTradeRecord As Crypto = Nothing
            For Each symbolRecode As APIService.SymbolInformation In symbolList
                cryptoTradeRecord = New Crypto()
                cryptoTradeRecord.symbolTrade = symbolRecode.symbolTrade
                cryptoTradeRecord.tradeCoin = symbolRecode.tradeCoin
                cryptoTradeRecord.stableCoin = symbolRecode.stableCoin
                cryptoTradeRecord.decimalPlace = symbolRecode.decimalPlace

                'Get actual balance
                For Each coinWalletRecord As WalletCoin In cryptoWalletList
                    If coinWalletRecord.coinName = cryptoTradeRecord.tradeCoin Then
                        cryptoTradeRecord.tradeCoinVolume = coinWalletRecord.coinVolume
                        cryptoTradeRecord.tradeCoinVolume = cryptoTradeRecord.tradeCoinVolume _
                                                          - (cryptoTradeRecord.tradeCoinVolume Mod cryptoTradeRecord.decimalPlace)
                        Exit For
                    End If
                Next

                'Get coin price
                If cryptoTradeRecord.tradeCoinVolume > 0 Then
                    While (APIService.GetHistoryPrice(cryptoTradeRecord.tradeCoin + cryptoTradeRecord.stableCoin,
                                                      cryptoTradeRecord.tradeCoinBuyCommission,
                                                      cryptoTradeRecord.decimalPlace,
                                                      cryptoTradeRecord.tradeCoinVolume,
                                                      cryptoTradeRecord.tradeCoinPrice) = False)
                        Threading.Thread.Sleep(2000)
                    End While
                End If

                cryptoTradeRecord.tradeCoinAmount = cryptoTradeRecord.tradeCoinVolume * cryptoTradeRecord.tradeCoinPrice
                cryptoTradeList.Add(cryptoTradeRecord)
            Next

            Me.Text = "Biikub Spot Trade [ Real Trade ]" + " [Version: " + My.Application.Info.Version.ToString() + "]"

        End If

        Dim aa As List(Of Crypto.CandlesInformation) = APIService.GetCandlesTrade("THB",
                                                                                  "ETH",
                                                                                  100,
                                                                                  APIService.SortSequenceCandelsValue.ASC)


        'Set summary hour
        summaryTime = Date.Now().AddHours(configuration.sendSummaryHour)

        'Set message log
        ToolStripLabelMessageLog.Text = "Application is ready...."
        ToolStripLabelMessageLog.BackColor = SystemColors.Info

        'Enable start trade button
        ButtonStartTrade.Visible = True

        'Display value master
        SetScreenValueMaster()

        'Display value
        SetScreenValue()

        'Uplock Screen
        Me.Enabled = True

        'Check auto start
        If configuration.autoRun Then
            botActive = True
            TimerAutoTrade.Enabled = botActive

            RadioButtonTradeModeSimulate.Enabled = False
            RadioButtonTradeModeReal.Enabled = False

            ButtonStartTrade.Visible = False
            ButtonStopTrade.Visible = True

            Me.BackColor = SystemColors.GradientActiveCaption

            'Set message log
            ToolStripLabelMessageLog.Text = "Application auto run is started...."
            ToolStripLabelMessageLog.BackColor = SystemColors.Info

            Dim lineMessage = vbCrLf _
                            + "Start Bot" + vbCrLf _
                            + "On device: " + Environment.MachineName + vbCrLf _
                            + "At " + Date.Now().ToString("dd MMM yyyy / HH:mm") + vbCrLf + vbCrLf _
                            + "With condition" + vbCrLf _
                            + "Interval time: " + configuration.tradeInterval + vbCrLf _
                            + "First buy amount: " + configuration.stepupAmount.ToString("###,##0.00") + vbCrLf _
                            + "Take profit on (%25): " + configuration.tradeCoinTakeProfitPercent.ToString("##0.00") + "%25" + vbCrLf _
                            + "Fillup on (%25): " + configuration.tradeCoinFillupPercent.ToString("##0.00") + "%25" + vbCrLf _
                            + "Buy commission (%25): " + configuration.tradeCoinBuyCommission.ToString("##0.00") + "%25" + vbCrLf _
                            + "Sale commission (%25):" + configuration.tradeCoinSaleCommission.ToString("##0.00") + "%25" + vbCrLf _
                            + "Limit Coin Amount: " + configuration.limitCoinAmount.ToString("###,##0.00") + vbCrLf _
                            + "Trade style: " + configuration.tradeStyle + vbCrLf

            APIService.SendLineNotification(lineMessage, errorMessage)
        End If
    End Sub
End Class
