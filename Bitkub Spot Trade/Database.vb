Imports System.Data.SQLite
Imports System.IO

Public Class Database
    Private Shared connectionString As String = ""
    Private Shared sqlConnnetion As SQLiteConnection = Nothing

    Public Shared Sub ConnectDatabase(ByVal databaseFilePath As String)
        connectionString = String.Format("Data Source = {0}", databaseFilePath)
        sqlConnnetion = New SQLiteConnection(connectionString)
        sqlConnnetion.Open()

        CreateInitialDatabase(databaseFilePath)
        InitialMasterConfiguration()
    End Sub

    Public Shared Sub DisConnectDatabase()
        sqlConnnetion.Close()
    End Sub

    Private Shared Function CheckTableExist(ByVal tableName As String) As Boolean
        Dim result As Boolean = False
        Dim querySQL As String = "SELECT name FROM sqlite_master WHERE type ='table' AND name='" + tableName + "';"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()

        result = dataReader.HasRows()
        Return result
    End Function

    Private Shared Sub CreateInitialDatabase(ByVal databaseFilePath As String)
        Dim querySQL As String = ""
        Dim sqlExecute As SQLiteCommand = Nothing

        'Create Table Configuration
        If CheckTableExist("Configuration") = False Then
            querySQL = "CREATE TABLE 'Configuration' ( " _
                     + "'id' TEXT, " _
                     + "'autoRun' INTEGER, " _
                     + "'licenseKey' TEXT, " _
                     + "'bitkubKey' TEXT, " _
                     + "'bitkubSecret' TEXT, " _
                     + "'lineToken' TEXT, " _
                     + "'serviceKey' TEXT, " _
                     + "'sendSummaryHour' INTEGER, " _
                     + "'sendLineSummary' INTEGER, " _
                     + "'sendLineBuyCoin' INTEGER, " _
                     + "'sendLineSellCoin' INTEGER, " _
                     + "'botTestMode' INTEGER, " _
                     + "'stableCoin' TEXT, " _
                     + "'stepupAmount' REAL, " _
                     + "'buyNewOnNextWave' INTEGER, " _
                     + "'tradeCoinBuyCommission' REAL, " _
                     + "'tradeCoinSaleCommission' REAL, " _
                     + "'tradeCoinTakeProfitPercent' REAL, " _
                     + "'tradeCoinFillupPercent' REAL, " _
                     + "'tradeInterval' TEXT, " _
                     + "'limitCoinAmount' REAL, " _
                     + "'tradeStyle' TEXT, " _
                     + " PRIMARY KEY('id') " _
                     + " );"

            sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)
            sqlExecute.ExecuteNonQuery()
        End If

        If CheckTableExist("CoinTrade") = False Then
            querySQL = "CREATE TABLE 'CoinTrade' ( " _
                     + "'symbol' TEXT, " _
                     + "'stableCoin' TEXT, " _
                     + "'tradeCoin' TEXT, " _
                     + " PRIMARY KEY('tradeCoin') " _
                     + " );"

            sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)
            sqlExecute.ExecuteNonQuery()
        End If

        If CheckTableExist("SummaryProfit") = False Then
            querySQL = "CREATE TABLE 'SummaryProfit' ( " _
                     + "'summaryDate' INTEGER, " _
                     + "'symbol' TEXT, " _
                     + "'stableCoin' TEXT, " _
                     + "'tradeCoin' TEXT, " _
                     + "'takeProfitCount' REAL, " _
                     + "'takeProfitAmount' REAL, " _
                     + " PRIMARY KEY('summaryDate', 'symbol') " _
                     + " );"

            sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)
            sqlExecute.ExecuteNonQuery()
        End If
    End Sub

    Private Shared Sub InitialMasterConfiguration()
        Dim configuration As FrmSpotTrade.ConfigurationInformation = New FrmSpotTrade.ConfigurationInformation()

        If ReadConfiguration(configuration) = False Then
            'Add initial Configuration
            configuration = New FrmSpotTrade.ConfigurationInformation()

            configuration.autoRun = False
            configuration.licenseKey = ""
            configuration.bitkubKey = ""
            configuration.bitkubSecret = ""
            configuration.lineToken = ""
            configuration.serviceKey = ""
            configuration.sendSummaryHour = 4
            configuration.sendLineSummary = False
            configuration.sendLineBuyCoin = False
            configuration.sendLineSellCoin = False
            configuration.botTestMode = True
            configuration.stableCoin = ""
            configuration.stepupAmount = 20
            configuration.buyNewOnNextWave = True
            configuration.tradeCoinBuyCommission = 0.1
            configuration.tradeCoinSaleCommission = 0.1
            configuration.tradeCoinTakeProfitPercent = 0.3
            configuration.tradeCoinFillupPercent = 1.0
            configuration.limitCoinAmount = 800
            configuration.tradeInterval = "1m"
            configuration.tradeStyle = "High Risk"

            UpdateConfiguration(configuration)
        End If
    End Sub

    Public Shared Function ReadConfiguration(ByRef configuration As FrmSpotTrade.ConfigurationInformation) As Boolean
        Dim result As Boolean = False
        Dim querySQL As String = "SELECT * FROM Configuration WHERE id = '1' ;"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()

        'Get Found record
        If dataReader.HasRows() Then
            'Get value configuration record
            While (dataReader.Read())
                Dim configurationMaster As FrmSpotTrade.ConfigurationInformation = New FrmSpotTrade.ConfigurationInformation()

                For i As Integer = 0 To dataReader.FieldCount - 1
                    Select Case dataReader.GetName(i)
                        Case "autoRun"
                            configurationMaster.autoRun = dataReader.GetValue(i)
                        Case "licenseKey"
                            configurationMaster.licenseKey = dataReader.GetValue(i)
                        Case "bitkubKey"
                            configurationMaster.bitkubKey = dataReader.GetValue(i)
                        Case "bitkubSecret"
                            configurationMaster.bitkubSecret = dataReader.GetValue(i)
                        Case "lineToken"
                            configurationMaster.lineToken = dataReader.GetValue(i)
                        Case "serviceKey"
                            configurationMaster.serviceKey = dataReader.GetValue(i)
                        Case "sendSummaryHour"
                            configurationMaster.sendSummaryHour = dataReader.GetValue(i)
                        Case "sendLineSummary"
                            configurationMaster.sendLineSummary = dataReader.GetValue(i)
                        Case "sendLineBuyCoin"
                            configurationMaster.sendLineBuyCoin = dataReader.GetValue(i)
                        Case "sendLineSellCoin"
                            configurationMaster.sendLineSellCoin = dataReader.GetValue(i)
                        Case "botTestMode"
                            configurationMaster.botTestMode = dataReader.GetValue(i)
                        Case "stableCoin"
                            configurationMaster.stableCoin = dataReader.GetValue(i)
                        Case "stepupAmount"
                            configurationMaster.stepupAmount = Double.Parse(dataReader.GetValue(i))
                        Case "buyNewOnNextWave"
                            configurationMaster.buyNewOnNextWave = dataReader.GetValue(i)
                        Case "tradeCoinBuyCommission"
                            configurationMaster.tradeCoinBuyCommission = Double.Parse(dataReader.GetValue(i))
                        Case "tradeCoinSaleCommission"
                            configurationMaster.tradeCoinSaleCommission = Double.Parse(dataReader.GetValue(i))
                        Case "tradeCoinTakeProfitPercent"
                            configurationMaster.tradeCoinTakeProfitPercent = Double.Parse(dataReader.GetValue(i))
                        Case "tradeCoinFillupPercent"
                            configurationMaster.tradeCoinFillupPercent = Double.Parse(dataReader.GetValue(i))
                        Case "limitCoinAmount"
                            configurationMaster.limitCoinAmount = Double.Parse(dataReader.GetValue(i))
                        Case "tradeInterval"
                            configurationMaster.tradeInterval = dataReader.GetValue(i)
                        Case "tradeStyle"
                            configurationMaster.tradeStyle = dataReader.GetValue(i)
                    End Select

                Next

                configuration = configurationMaster
                result = True
            End While
        End If

        Return result
    End Function

    Public Shared Sub UpdateConfiguration(ByVal configuration As FrmSpotTrade.ConfigurationInformation)
        Dim querySQL As String = "SELECT * FROM Configuration WHERE id = '1' ;"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()

        'Get Found record
        If dataReader.HasRows() Then
            'Found record (Update value)
            querySQL = "UPDATE Configuration " _
                     + "SET autoRun = @autoRun, " _
                     + "licenseKey = @licenseKey, " _
                     + "bitkubKey = @bitkubKey, " _
                     + "bitkubSecret = @bitkubSecret, " _
                     + "lineToken = @lineToken, " _
                     + "serviceKey = @serviceKey, " _
                     + "sendSummaryHour = @sendSummaryHour, " _
                     + "sendLineSummary = @sendLineSummary, " _
                     + "sendLineBuyCoin = @sendLineBuyCoin, " _
                     + "sendLineSellCoin = @sendLineSellCoin, " _
                     + "botTestMode = @botTestMode, " _
                     + "stableCoin = @stableCoin, " _
                     + "stepupAmount = @stepupAmount, " _
                     + "buyNewOnNextWave = @buyNewOnNextWave, " _
                     + "tradeCoinBuyCommission = @tradeCoinBuyCommission, " _
                     + "tradeCoinSaleCommission = @tradeCoinSaleCommission, " _
                     + "tradeCoinTakeProfitPercent = @tradeCoinTakeProfitPercent, " _
                     + "tradeCoinFillupPercent = @tradeCoinFillupPercent, " _
                     + "limitCoinAmount = @limitCoinAmount, " _
                     + "tradeInterval = @tradeInterval, " _
                     + "tradeStyle = @tradeStyle " _
                     + "WHERE id = '1' ;"

        Else
            'New record (Insert value)
            querySQL = "INSERT INTO Configuration ( " _
                     + " id, " _
                     + " autoRun, " _
                     + " licenseKey, " _
                     + " bitkubKey, " _
                     + " bitkubSecret, " _
                     + " lineToken, " _
                     + " serviceKey, " _
                     + " sendSummaryHour, " _
                     + " sendLineSummary, " _
                     + " sendLineBuyCoin, " _
                     + " sendLineSellCoin, " _
                     + " botTestMode, " _
                     + " stableCoin, " _
                     + " stepupAmount, " _
                     + " buyNewOnNextWave, " _
                     + " tradeCoinBuyCommission, " _
                     + " tradeCoinSaleCommission, " _
                     + " tradeCoinTakeProfitPercent, " _
                     + " tradeCoinFillupPercent, " _
                     + " limitCoinAmount, " _
                     + " tradeInterval, " _
                     + " tradeStyle )" _
                     + " VALUES( " _
                     + " '1', " _
                     + " @autoRun, " _
                     + " @licenseKey, " _
                     + " @bitkubKey, " _
                     + " @bitkubSecret, " _
                     + " @lineToken, " _
                     + " @serviceKey, " _
                     + " @sendSummaryHour, " _
                     + " @sendLineSummary, " _
                     + " @sendLineBuyCoin, " _
                     + " @sendLineSellCoin, " _
                     + " @botTestMode, " _
                     + " @stableCoin, " _
                     + " @stepupAmount, " _
                     + " @buyNewOnNextWave, " _
                     + " @tradeCoinBuyCommission, " _
                     + " @tradeCoinSaleCommission, " _
                     + " @tradeCoinTakeProfitPercent, " _
                     + " @tradeCoinFillupPercent, " _
                     + " @limitCoinAmount, " _
                     + " @tradeInterval, " _
                     + " @tradeStyle );"

        End If

        'Set execute command
        sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)

        'Set value
        sqlExecute.Parameters.AddWithValue("@autoRun", configuration.autoRun)
        sqlExecute.Parameters.AddWithValue("@licenseKey", configuration.licenseKey)
        sqlExecute.Parameters.AddWithValue("@bitkubKey", configuration.bitkubKey)
        sqlExecute.Parameters.AddWithValue("@bitkubSecret", configuration.bitkubSecret)
        sqlExecute.Parameters.AddWithValue("@lineToken", configuration.lineToken)
        sqlExecute.Parameters.AddWithValue("@serviceKey", configuration.serviceKey)
        sqlExecute.Parameters.AddWithValue("@sendSummaryHour", configuration.sendSummaryHour)
        sqlExecute.Parameters.AddWithValue("@sendLineSummary", configuration.sendLineSummary)
        sqlExecute.Parameters.AddWithValue("@sendLineBuyCoin", configuration.sendLineBuyCoin)
        sqlExecute.Parameters.AddWithValue("@sendLineSellCoin", configuration.sendLineSellCoin)
        sqlExecute.Parameters.AddWithValue("@botTestMode", configuration.botTestMode)
        sqlExecute.Parameters.AddWithValue("@stableCoin", configuration.stableCoin)
        sqlExecute.Parameters.AddWithValue("@stepupAmount", configuration.stepupAmount)
        sqlExecute.Parameters.AddWithValue("@buyNewOnNextWave", configuration.buyNewOnNextWave)
        sqlExecute.Parameters.AddWithValue("@tradeCoinBuyCommission", configuration.tradeCoinBuyCommission)
        sqlExecute.Parameters.AddWithValue("@tradeCoinSaleCommission", configuration.tradeCoinSaleCommission)
        sqlExecute.Parameters.AddWithValue("@tradeCoinTakeProfitPercent", configuration.tradeCoinTakeProfitPercent)
        sqlExecute.Parameters.AddWithValue("@tradeCoinFillupPercent", configuration.tradeCoinFillupPercent)
        sqlExecute.Parameters.AddWithValue("@limitCoinAmount", configuration.limitCoinAmount)
        sqlExecute.Parameters.AddWithValue("@tradeInterval", configuration.tradeInterval)
        sqlExecute.Parameters.AddWithValue("@tradeStyle", configuration.tradeStyle)

        'Execute command
        sqlExecute.ExecuteNonQuery()
    End Sub

    Public Shared Function ReadTradeCoinList(ByVal stableCoin As String) As List(Of FrmSpotTrade.TradeCoinInformation)
        Dim tradeCoinList As List(Of FrmSpotTrade.TradeCoinInformation) = New List(Of FrmSpotTrade.TradeCoinInformation)
        Dim querySQL As String = "SELECT * FROM CoinTrade WHERE stableCoin = '" + stableCoin + "' ORDER BY tradeCoin ;"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()

        If dataReader.HasRows() Then
            While (dataReader.Read())
                Dim tradeCoinRecord As FrmSpotTrade.TradeCoinInformation = New FrmSpotTrade.TradeCoinInformation()

                For i As Integer = 0 To dataReader.FieldCount - 1
                    Select Case dataReader.GetName(i)
                        Case "symbol"
                            tradeCoinRecord.symbol = dataReader.GetValue(i)
                        Case "stableCoin"
                            tradeCoinRecord.stableCoin = dataReader.GetValue(i)
                        Case "tradeCoin"
                            tradeCoinRecord.tradeCoin = dataReader.GetValue(i)
                    End Select
                Next

                tradeCoinList.Add(tradeCoinRecord)
            End While
        End If

        Return tradeCoinList
    End Function

    Public Shared Sub UpdateTradeCoinList(ByVal stableCoin As String, ByVal tradeCoinList As List(Of FrmSpotTrade.TradeCoinInformation))
        Dim sqlExecute As SQLiteCommand = Nothing
        Dim dataReader As SQLiteDataReader = Nothing
        Dim querySQL As String = ""

        'Delete trade coin record
        querySQL = "DELETE FROM CoinTrade ;"

        sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)
        sqlExecute.ExecuteNonQuery()

        'Add new update trade coin list
        Dim tradeCoinRecord As FrmSpotTrade.TradeCoinInformation = Nothing
        For Each tradeCoinRecord In tradeCoinList
            'Insert value
            querySQL = "INSERT INTO CoinTrade ( " _
                     + "symbol, " _
                     + "stableCoin, " _
                     + "tradeCoin )" _
                     + " VALUES( " _
                     + "@symbol, " _
                     + "@stableCoin, " _
                     + "@tradeCoin );"

            'Set execute command
            sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)

            'Set value
            sqlExecute.Parameters.AddWithValue("@symbol", tradeCoinRecord.symbol)
            sqlExecute.Parameters.AddWithValue("@stableCoin", tradeCoinRecord.stableCoin)
            sqlExecute.Parameters.AddWithValue("@tradeCoin", tradeCoinRecord.tradeCoin)

            'Execute command
            sqlExecute.ExecuteNonQuery()
        Next
    End Sub

    Public Shared Function ReadSummaryProfitList(ByVal stableCoin As String) As List(Of FrmSpotTrade.SummaryProfitInformation)
        Dim summaryProfitList As List(Of FrmSpotTrade.SummaryProfitInformation) = New List(Of FrmSpotTrade.SummaryProfitInformation)
        Dim querySQL As String = "SELECT * FROM SummaryProfit WHERE stableCoin = '" + stableCoin + "' ORDER BY summaryDate DESC, symbol ASC ;"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()

        If dataReader.HasRows() Then
            While (dataReader.Read())
                Dim summaryProfitRecord As FrmSpotTrade.SummaryProfitInformation = New FrmSpotTrade.SummaryProfitInformation()

                For i As Integer = 0 To dataReader.FieldCount - 1
                    Select Case dataReader.GetName(i)
                        Case "summaryDate"
                            summaryProfitRecord.summaryDate = Integer.Parse(dataReader.GetValue(i))
                        Case "symbol"
                            summaryProfitRecord.symbol = dataReader.GetValue(i)
                        Case "stableCoin"
                            summaryProfitRecord.stableCoin = dataReader.GetValue(i)
                        Case "tradeCoin"
                            summaryProfitRecord.tradeCoin = dataReader.GetValue(i)
                        Case "takeProfitCount"
                            summaryProfitRecord.takeProfitCount = Double.Parse(dataReader.GetValue(i))
                        Case "takeProfitAmount"
                            summaryProfitRecord.takeProfitAmount = Double.Parse(dataReader.GetValue(i))
                    End Select
                Next

                summaryProfitList.Add(summaryProfitRecord)
            End While
        End If

        Return summaryProfitList
    End Function

    Public Shared Sub AddProfitOnDate(ByVal orderDate As DateTime,
                                      ByVal stableCoin As String,
                                      ByVal tradeCoin As String,
                                      ByVal profitAmount As Double)

        Dim summaryProfitList As List(Of FrmSpotTrade.SummaryProfitInformation) = New List(Of FrmSpotTrade.SummaryProfitInformation)
        Dim querySQL As String = "SELECT * FROM SummaryProfit WHERE summaryDate = " + orderDate.ToString("yyyyMMdd") + " AND symbol = '" + tradeCoin + stableCoin + "' ;"
        Dim sqlExecute As SQLiteCommand = New SQLiteCommand(querySQL, sqlConnnetion)
        Dim dataReader As SQLiteDataReader = sqlExecute.ExecuteReader()
        Dim takeProfitCount As Integer = 0
        Dim takeProfitAmount As Double = 0

        If dataReader.HasRows() Then
            'Get count a amount
            While (dataReader.Read())
                For i As Integer = 0 To dataReader.FieldCount - 1
                    Select Case dataReader.GetName(i)
                        Case "takeProfitCount"
                            takeProfitCount = Double.Parse(dataReader.GetValue(i))
                        Case "takeProfitAmount"
                            takeProfitAmount = Double.Parse(dataReader.GetValue(i))
                    End Select
                Next
            End While

            takeProfitCount = takeProfitCount + 1
            takeProfitAmount = takeProfitAmount + profitAmount

            'Update profit
            querySQL = "UPDATE SummaryProfit " _
                     + "SET takeProfitCount = @takeProfitCount, " _
                     + "takeProfitAmount = @takeProfitAmount " _
                     + "WHERE summaryDate = @summaryDate " _
                     + "AND symbol = @symbol ;"

        Else
            'Add new profit
            takeProfitCount = 1
            takeProfitAmount = profitAmount

            querySQL = "INSERT INTO SummaryProfit ( " _
                     + "summaryDate, " _
                     + "symbol, " _
                     + "stableCoin, " _
                     + "tradeCoin, " _
                     + "takeProfitCount, " _
                     + "takeProfitAmount )" _
                     + " VALUES( " _
                     + "@summaryDate, " _
                     + "@symbol, " _
                     + "@stableCoin, " _
                     + "@tradeCoin, " _
                     + "@takeProfitCount, " _
                     + "@takeProfitAmount );"
        End If

        'Set execute command
        sqlExecute = New SQLiteCommand(querySQL, sqlConnnetion)

        'Set value
        sqlExecute.Parameters.AddWithValue("@summaryDate", orderDate.ToString("yyyyMMdd"))
        sqlExecute.Parameters.AddWithValue("@symbol", tradeCoin + stableCoin)
        sqlExecute.Parameters.AddWithValue("@stableCoin", stableCoin)
        sqlExecute.Parameters.AddWithValue("@tradeCoin", tradeCoin)
        sqlExecute.Parameters.AddWithValue("@takeProfitCount", takeProfitCount)
        sqlExecute.Parameters.AddWithValue("@takeProfitAmount", takeProfitAmount)

        'Execute command
        sqlExecute.ExecuteNonQuery()
    End Sub
End Class