Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class APIService
    Enum SortSequenceCandelsValue As Integer
        ASC = 0
        DES = 1
    End Enum

    Structure SymbolInformation
        Dim symbolTrade As String
        Dim tradeCoin As String
        Dim stableCoin As String
        Dim decimalPlace As Double
    End Structure

    Structure WalletInformation
        Dim coinName As String
        Dim coinVolume As Double
    End Structure

    Public Shared bitkubKey As String = ""
    Public Shared bitkubSecret As String = ""
    Public Shared lineToken As String = ""

    Public Shared Function HashString(ByVal dataString As String, ByVal secretKey As String) As String
        Dim keyhash() As Byte = Encoding.UTF8.GetBytes(secretKey)
        Dim dataHash() As Byte = Encoding.UTF8.GetBytes(dataString)
        Dim hashConvert = New HMACSHA256(keyhash)

        Dim dataByte() As Byte = hashConvert.ComputeHash(dataHash)

        Return BitConverter.ToString(dataByte).Replace("-", "").ToLower()
    End Function

    Public Shared Function SendLineNotification(ByVal lineMessage As String, ByRef errorMessage As String) As Boolean
        Dim result As Boolean = False
        Dim bodyBytes() As Byte = Nothing
        Dim webRequest As HttpWebRequest = Nothing
        Dim webResponse As HttpWebResponse = Nothing
        Dim streamReader As StreamReader = Nothing

        errorMessage = ""

        'Check line token
        If lineToken = "" Then
            Return result
        End If

        Try
            'Setup message
            Dim sendMessage = String.Format("message={0}", lineMessage)
            Dim sendData = Encoding.UTF8.GetBytes(sendMessage)

            'URL API
            webRequest = HttpWebRequest.Create("https://notify-api.line.me/api/notify")

            webRequest.Method = "POST"
            webRequest.ContentType = "application/x-www-form-urlencoded"
            webRequest.Headers.Add("Authorization", "Bearer " & lineToken)
            webRequest.ContentLength = sendData.Length
            webRequest.AllowWriteStreamBuffering = True
            webRequest.KeepAlive = False
            webRequest.Credentials = CredentialCache.DefaultCredentials

            Using Stream = webRequest.GetRequestStream()
                Stream.Write(sendData, 0, sendData.Length)
            End Using

            webResponse = webRequest.GetResponse()
            streamReader = New StreamReader(webResponse.GetResponseStream())
            Dim jsonOutput As String = streamReader.ReadToEnd()
            streamReader.Close()

            webRequest.Abort()

            result = True

        Catch ex As Exception
            errorMessage = ex.Message()

        End Try

        Return result
    End Function

    Public Shared Function ConsumeAPI(ByVal urlAPI As String,
                                      ByVal consumeMethod As String,
                                      ByVal jsonInput As String,
                                      ByRef jsonOutput As String,
                                      ByRef errorMessage As String) As Boolean

        Dim result As Boolean = False
        Dim encode As UTF8Encoding = Nothing
        Dim bodyBytes() As Byte = Nothing
        Dim webRequest As HttpWebRequest = Nothing
        Dim webResponse As HttpWebResponse = Nothing
        Dim streamReader As StreamReader = Nothing

        jsonOutput = ""
        errorMessage = ""

        Try
            'URL API
            webRequest = HttpWebRequest.Create(urlAPI)

            webRequest.Method = consumeMethod
            webRequest.Timeout = 5000
            webRequest.Accept = "application/json"
            webRequest.Headers("x-btk-apikey") = bitkubKey

            If consumeMethod = "POST" Then
                webRequest.Headers("x-btk-apikey") = bitkubKey
                webRequest.ContentType = "application/json"

                Dim byteArray() As Byte = Encoding.UTF8.GetBytes(jsonInput)
                Dim dataStream As Stream = webRequest.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
                dataStream.Close()
            End If

            webResponse = webRequest.GetResponse()
            streamReader = New StreamReader(webResponse.GetResponseStream())
            jsonOutput = streamReader.ReadToEnd()
            streamReader.Close()

            webRequest.Abort()

            If webResponse.StatusCode = HttpStatusCode.OK Then
                result = True

            Else
                errorMessage = webResponse.StatusCode

            End If

        Catch ex As Exception
            jsonOutput = ""
            errorMessage = ex.Message()

        End Try

        Return result
    End Function

    Public Shared Function GetServerTime() As Long
        Dim urlAPI As String = "https://api.bitkub.com/api/servertime"
        Dim timestamp As Long = 0
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        'Call API service get server time
        If ConsumeAPI(urlAPI, "GET", jsonInput, jsonOutput, errorMessage) Then
            timestamp = jsonOutput
        End If

        Return timestamp
    End Function

    Public Shared Function GetApplicationActive(ByVal expireApplication As DateTime) As Boolean
        Dim applicationExpire As Boolean = True

        'Get Binance Server Time
        Dim binanceServerTimestamp As Long = GetServerTime()

        While (binanceServerTimestamp = 0)
            Threading.Thread.Sleep(2000)
            binanceServerTimestamp = GetServerTime()
        End While

        'Convert date
        Dim binanceDateTime As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)
        binanceDateTime = binanceDateTime.AddMilliseconds(binanceServerTimestamp)
        binanceDateTime = binanceDateTime.ToLocalTime()

        Dim appDateInt As Integer = Integer.Parse(expireApplication.ToString("yyyyMMdd"))
        Dim binanceDateInt As Integer = Integer.Parse(binanceDateTime.ToString("yyyyMMdd"))

        'Check expire date with binance server time
        If appDateInt >= binanceDateInt Then
            applicationExpire = False
        End If

        Return applicationExpire
    End Function

    Public Shared Function GetPreviousDate() As DateTime
        Dim binanceServerTimestamp As Long = GetServerTime()

        While (binanceServerTimestamp = 0)
            Threading.Thread.Sleep(2000)
            binanceServerTimestamp = GetServerTime()
        End While

        Dim binanceDateTime As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)
        binanceDateTime = binanceDateTime.AddMilliseconds(binanceServerTimestamp)
        binanceDateTime = binanceDateTime.ToLocalTime().AddDays(-1)

        Return binanceDateTime
    End Function

    Public Shared Function GetSymbolTradeList() As List(Of SymbolInformation)
        Dim symbolList As List(Of SymbolInformation) = New List(Of SymbolInformation)
        Dim symbolRecord As SymbolInformation = Nothing

        Dim urlAPI As String = "https://api.bitkub.com/api/market/symbols"
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        'Get crypto symbols
        If ConsumeAPI(urlAPI, "GET", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing
                Dim filterJson As JToken = Nothing
                Dim optionJson As JToken = Nothing

                resultJson = json.Item("result")

                For i As Integer = 0 To resultJson.Count - 1
                    recordJson = resultJson.Item(i)
                    Dim symbolTrade As String = recordJson.Item("symbol").ToString()
                    Dim arrayString() As String = symbolTrade.Split("_")

                    If arrayString(1) <> "THB" Then
                        'Create symbol record
                        symbolRecord = New SymbolInformation()
                        symbolRecord.symbolTrade = symbolTrade
                        symbolRecord.stableCoin = arrayString(0)
                        symbolRecord.tradeCoin = arrayString(1)
                        symbolRecord.decimalPlace = 0.000000001

                        'Add crypto list
                        symbolList.Add(symbolRecord)
                    End If
                Next

            Catch ex As Exception

            End Try
        End If

        'Sort crypto list
        symbolList = symbolList.OrderBy(Function(x) x.symbolTrade).ToList()

        Return symbolList
    End Function

    Public Shared Function GetWalletList(symbolList As List(Of APIService.SymbolInformation)) As List(Of WalletInformation)
        Dim walletList As List(Of WalletInformation) = New List(Of WalletInformation)
        Dim walletRecord As WalletInformation = Nothing

        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        Dim jsonBody As JObject = New JObject()

        Dim timestamp As Long = GetServerTime()
        jsonBody.Add("ts", timestamp)
        jsonInput = jsonBody.ToString()
        jsonInput = jsonInput.Replace(vbCrLf, "")
        jsonInput = jsonInput.Replace(" ", "")

        Dim signature As String = HashString(jsonInput, bitkubSecret)
        jsonBody.Add("sig", signature)
        jsonInput = jsonBody.ToString()
        jsonInput = jsonInput.Replace(vbCrLf, "")
        jsonInput = jsonInput.Replace(" ", "")

        Dim urlAPI As String = "https://api.bitkub.com/api/market/balances"

        'Get wallet coin
        If ConsumeAPI(urlAPI, "POST", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                'Get buy or sale result
                resultJson = json.Item("result")

                For Each symbolRecord As SymbolInformation In symbolList
                    recordJson = resultJson.Item(symbolRecord.tradeCoin)

                    walletRecord = New WalletInformation()
                    walletRecord.coinName = symbolRecord.tradeCoin
                    walletRecord.coinVolume = Double.Parse(recordJson.Item("available").ToString())
                    walletList.Add(walletRecord)
                Next

            Catch ex As Exception

            End Try
        End If

        Return walletList
    End Function

    Public Shared Function GetWalletCoinBalance(ByVal cryptoName As String,
                                                ByVal decimalsPlace As Double,
                                                ByRef coinVolume As Double) As Boolean
        Dim result As Boolean = False

        coinVolume = 0

        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        Dim jsonBody As JObject = New JObject()

        Dim timestamp As Long = GetServerTime()
        jsonBody.Add("ts", timestamp)
        jsonInput = jsonBody.ToString()
        jsonInput = jsonInput.Replace(vbCrLf, "")
        jsonInput = jsonInput.Replace(" ", "")

        Dim signature As String = HashString(jsonInput, bitkubSecret)
        jsonBody.Add("sig", signature)
        jsonInput = jsonBody.ToString()
        jsonInput = jsonInput.Replace(vbCrLf, "")
        jsonInput = jsonInput.Replace(" ", "")

        Dim urlAPI As String = "https://api.bitkub.com/api/market/balances"

        'Get wallet coin
        If ConsumeAPI(urlAPI, "POST", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                'Get buy or sale result
                resultJson = json.Item("result")

                recordJson = resultJson.Item(cryptoName)
                coinVolume = Double.Parse(recordJson.Item("available").ToString())

                'Round decimal
                coinVolume = coinVolume - (coinVolume Mod decimalsPlace)
                result = True

            Catch ex As Exception

            End Try
        End If

        Return result
    End Function

    Public Shared Function GetCandlesTrade(ByVal stableCoin As String,
                                           ByVal tradeCoin As String,
                                           ByVal tradeInterval As String,
                                           ByVal sortingCandles As SortSequenceCandelsValue) As List(Of Crypto.CandlesInformation)

        'Call API service get trend caldles
        Dim result As Boolean = False
        Dim candlesList As List(Of Crypto.CandlesInformation) = New List(Of Crypto.CandlesInformation)
        Dim candlesRecord As Crypto.CandlesInformation = Nothing

        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""
        Dim dateJan1st1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)

        Dim timestamp As Long = GetServerTime()
        Dim fromMilliSecond As Long = timestamp - (60 * 100 * 1000)

        Dim toDateTime As DateTime = dateJan1st1970.AddMilliseconds(timestamp)
        Dim fromDateTime As DateTime = toDateTime.AddMinutes(-100)
        'Dim fromMilliSecond As Long = fromDateTime.ToUnixTimeMilliseconds()

        Dim urlAPI As String = "https://api.bitkub.com/api/market/tradingview?sym=" + stableCoin + "_" + tradeCoin + "&int=900&frm=" + fromMilliSecond.ToString() + "&to=" + timestamp.ToString()
        MsgBox(urlAPI)
        If ConsumeAPI(urlAPI, "GET", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                jsonOutput = "{""result"":" + jsonOutput + "}"

                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                resultJson = json.Item("result")

                For j As Integer = 0 To resultJson.Count - 1
                    recordJson = resultJson.Item(j)

                    candlesRecord = New Crypto.CandlesInformation()
                    candlesRecord.openTime = Double.Parse(recordJson.Item(0).ToString())
                    candlesRecord.closeTime = Double.Parse(recordJson.Item(6).ToString())
                    candlesRecord.openPrice = Double.Parse(recordJson.Item(1).ToString())
                    candlesRecord.closePrice = Double.Parse(recordJson.Item(4).ToString())
                    candlesRecord.maxHigh = Double.Parse(recordJson.Item(2).ToString())
                    candlesRecord.maxLow = Double.Parse(recordJson.Item(3).ToString())

                    candlesRecord.avgPrice = (candlesRecord.maxHigh + candlesRecord.maxLow) / 2

                    'Check high, low price between open and close price
                    If candlesRecord.openPrice >= candlesRecord.closePrice Then
                        candlesRecord.highPrice = candlesRecord.openPrice
                        candlesRecord.lowPrice = candlesRecord.closePrice
                    Else
                        candlesRecord.highPrice = candlesRecord.closePrice
                        candlesRecord.lowPrice = candlesRecord.openPrice
                    End If

                    candlesRecord.localCloseDateTime = dateJan1st1970.AddMilliseconds(candlesRecord.openTime).ToLocalTime()

                    'Add Candles
                    If sortingCandles = SortSequenceCandelsValue.ASC Then
                        candlesList.Add(candlesRecord)
                    ElseIf sortingCandles = SortSequenceCandelsValue.DES Then
                        candlesList.Insert(0, candlesRecord)
                    End If

                Next

            Catch ex As Exception

            End Try
        End If

        Return candlesList
    End Function

    Public Shared Function GetActualBidAsk(ByVal stableCoin As String,
                                           ByVal tradeCoin As String,
                                           ByRef actualBidPriceValue As Double,
                                           ByRef actualAskPriceValue As Double) As Boolean

        Dim result As Boolean = False
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        actualBidPriceValue = 0
        actualAskPriceValue = 0

        ' Adjust the URL according to Bitkub's API documentation for market books
        Dim urlAPI As String = "https://api.bitkub.com/api/market/books" _
                         + "?sym=" + tradeCoin.ToLower() + "_" + stableCoin.ToLower() _
                         + "&lmt=1" ' Assuming "lmt=1" fetches the top bid and ask; adjust if necessary

        ' Call API to get bids and asks
        If ConsumeAPI(urlAPI, "GET", jsonInput, jsonOutput, errorMessage) Then
            Try
                ' Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)

                ' Assuming the JSON structure contains "result" object with "bids" and "asks" arrays
                Dim bids As JArray = json("result")("bids")
                If bids.Count > 0 Then
                    actualBidPriceValue = Double.Parse(bids(0)(3).ToString()) ' The 4th element in the array is the rate
                End If

                Dim asks As JArray = json("result")("asks")
                If asks.Count > 0 Then
                    actualAskPriceValue = Double.Parse(asks(0)(3).ToString()) ' The 4th element in the array is the rate
                End If

                result = True

            Catch ex As Exception
                ' Handle exception
                errorMessage = ex.Message
            End Try
        End If

        Return result
    End Function


    Public Shared Function GetHistoryPrice(ByVal symbolName As String,
                                       ByVal tradeCoinBuyCommission As Double,
                                       ByVal decimalPlace As Double,
                                       ByVal coinVolume As Double,
                                       ByRef priceHistory As Double) As Boolean
        Dim result As Boolean = False

        priceHistory = 0

        Dim remainCoin As Double = coinVolume

        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        Dim timestamp As Long = GetServerTime()
        Dim urlParameter As String = "symbol=" + symbolName _
                                   + "&limit=20" _
                                   + "&recvWindow=50000" _
                                   + "&timestamp=" + timestamp.ToString()

        Dim signature As String = HashString(urlParameter, bitkubSecret)
        urlParameter = urlParameter + "&signature=" + signature

        Dim urlAPI As String = "https://api.binance.com/api/v3/myTrades" + "?" + urlParameter

        'Get wallet coin
        If ConsumeAPI(urlAPI, "GET", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json
                jsonOutput = "{""result"":" + jsonOutput + "}"

                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                Dim buySide As Boolean = False
                Dim coinOrderVolume As Double = 0
                Dim coinDeductCommission As Double = 0
                Dim coinOrderPrice As Double = 0
                Dim coinOrderCommission As Double = 0

                Dim coinOrderTotalVolume As Double = 0
                Dim coinOrderTotalAmount As Double = 0

                resultJson = json.Item("result")

                For indexCount As Integer = resultJson.Count - 1 To 0 Step -1
                    recordJson = resultJson.Item(indexCount)

                    'Check buy transaction
                    buySide = Boolean.Parse(recordJson.Item("isBuyer").ToString())

                    If buySide Then
                        coinOrderVolume = Double.Parse(recordJson.Item("qty").ToString())
                        coinOrderPrice = Double.Parse(recordJson.Item("price").ToString())

                        If remainCoin < coinOrderVolume Then
                            coinOrderVolume = remainCoin
                        End If

                        coinOrderTotalAmount = coinOrderTotalAmount + (coinOrderVolume * coinOrderPrice)
                        remainCoin = remainCoin - coinOrderVolume

                        'Deduct Commission Volume
                        coinOrderVolume = coinOrderVolume - (coinOrderVolume * (tradeCoinBuyCommission / 100))
                        coinOrderTotalVolume = coinOrderTotalVolume + coinOrderVolume

                        If remainCoin <= decimalPlace Then
                            Exit For
                        End If
                    End If
                Next

                priceHistory = coinOrderTotalAmount / coinOrderTotalVolume
                result = True

            Catch ex As Exception

            End Try
        End If

        Return result
    End Function

    Public Shared Sub OpenOrderCoin(ByRef cryptoObject As Crypto)
        Dim listItem As ListViewItem = Nothing
        Dim datetimeTransaction As DateTime

        Dim urlParameter As String = ""
        Dim timestamp As Long = 0

        'Set parameter url api
        timestamp = GetServerTime()

        urlParameter = urlParameter + "symbol=" + cryptoObject.tradeCoin + cryptoObject.stableCoin _
                                    + "&side=BUY" _
                                    + "&type=MARKET" _
                                    + "&quoteOrderQty=" + cryptoObject.fillupAmount.ToString("#####0.00") _
                                    + "&timestamp=" + timestamp.ToString() _
                                    + "&recvWindow=5000"

        Dim signature As String = HashString(urlParameter, bitkubSecret)
        urlParameter = urlParameter + "&signature=" + signature

        Dim urlAPI As String = "https://api.binance.com/api/v3/order" + "?" + urlParameter
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        'Call API get bids and ask
        If ConsumeAPI(urlAPI, "POST", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                Dim coinItemVolume As Double = 0
                Dim coinItemPrice As Double = 0
                Dim coinOrderVolume As Double = 0
                Dim coinOrderPrice As Double = 0
                Dim coinOrderAmount As Double = 0

                'Get date time transaction
                Dim dateJan1st1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)
                Dim millionSecond As Long = Long.Parse(json.Item("transactTime").ToString())
                datetimeTransaction = dateJan1st1970.AddMilliseconds(millionSecond)
                datetimeTransaction = datetimeTransaction.ToLocalTime()

                'Get buy result
                resultJson = json.Item("fills")

                For j As Integer = 0 To resultJson.Count - 1
                    recordJson = resultJson.Item(j)

                    coinItemVolume = coinOrderVolume + Double.Parse(recordJson.Item("qty").ToString())
                    coinItemPrice = coinOrderPrice + Double.Parse(recordJson.Item("price").ToString())

                    coinOrderVolume = coinOrderVolume + coinItemVolume
                    coinOrderAmount = coinOrderAmount + (coinItemVolume * coinItemPrice)
                Next

                coinOrderPrice = coinOrderAmount / coinOrderVolume

                'Add data into list view order transaction
                listItem = New ListViewItem(datetimeTransaction.ToString("dd MMM yy / HH:mm:ss"))   'Date-time
                listItem.SubItems.Add(cryptoObject.tradeCoin)                                       'Crypto name
                listItem.SubItems.Add("Buy Coin")                                                   'Process name
                listItem.SubItems.Add("0")                                                          'Profit amount
                listItem.SubItems.Add("0.00%")                                                      'Profit percent
                listItem.SubItems.Add(coinOrderVolume.ToString("###,##0.00###"))                    'Coin Volume
                listItem.SubItems.Add(coinOrderPrice.ToString("###,##0.00###"))                     'Coin price
                listItem.SubItems.Add(coinOrderAmount.ToString("###,##0.00###"))                    'Coin amount
                listItem.BackColor = Color.Salmon

                'Insert item trade
                FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

                'Waiting binance database update 2 second
                Threading.Thread.Sleep(2000)

                'Update balance of trade coin
                GetWalletCoinBalance(cryptoObject.tradeCoin,
                                     cryptoObject.decimalPlace,
                                     cryptoObject.tradeCoinVolume)

                While (cryptoObject.tradeCoinVolume = 0)
                    Threading.Thread.Sleep(2000)

                    GetWalletCoinBalance(cryptoObject.tradeCoin,
                                         cryptoObject.decimalPlace,
                                         cryptoObject.tradeCoinVolume)
                End While

                'Update price history
                cryptoObject.tradeCoinPrice = 0

                GetHistoryPrice(cryptoObject.tradeCoin + cryptoObject.stableCoin,
                                cryptoObject.tradeCoinBuyCommission,
                                cryptoObject.decimalPlace,
                                cryptoObject.tradeCoinVolume,
                                cryptoObject.tradeCoinPrice)

                While (cryptoObject.tradeCoinPrice = 0)
                    Threading.Thread.Sleep(2000)

                    GetHistoryPrice(cryptoObject.tradeCoin + cryptoObject.stableCoin,
                                    cryptoObject.tradeCoinBuyCommission,
                                    cryptoObject.decimalPlace,
                                    cryptoObject.tradeCoinVolume,
                                    cryptoObject.tradeCoinPrice)
                End While

                cryptoObject.tradeCoinAmount = cryptoObject.tradeCoinVolume * cryptoObject.tradeCoinPrice
                cryptoObject.tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
                cryptoObject.tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()
                cryptoObject.fillupVolume = 0
                cryptoObject.fillupAmount = 0

                'Calculate take profit price and fillup price
                cryptoObject.CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount)

                'Update balance stable coin
                Dim balanceStableCoin As Double = FrmSpotTrade.GetBalanceStableCoin()

                'Check active send line buy order
                If FrmSpotTrade.GetSendLineBuyCoin() Then
                    'Send notification
                    Dim lineMessage As String = vbCrLf _
                                              + "Buy Coin (" + cryptoObject.tradeCoin + ")" + vbCrLf _
                                              + "On " + datetimeTransaction.ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                                              + "============================" + vbCrLf + vbCrLf _
                                              + "Coin volume: " + cryptoObject.tradeCoinVolume.ToString("###,##0.00###") + vbCrLf _
                                              + "Coin price: " + cryptoObject.tradeCoinPrice.ToString("###,##0.00###") + vbCrLf _
                                              + "Coin amount: " + cryptoObject.tradeCoinAmount.ToString("###,##0.00###") + vbCrLf _
                                              + "============================" + vbCrLf + vbCrLf _
                                              + "Take profit on: " + cryptoObject.tradeCoinTakeProfitPrice.ToString("###,##0.00###") + vbCrLf _
                                              + "Bid price: " + cryptoObject.actualBidPrice.ToString("###,##0.00###") + vbCrLf + vbCrLf _
                                              + "Fillup on: " + cryptoObject.tradeCoinFillupPrice.ToString("###,##0.00###") + vbCrLf _
                                              + "Ask price: " + cryptoObject.actualAskPrice.ToString("###,##0.00###") + vbCrLf _
                                              + "============================" + vbCrLf

                    SendLineNotification(lineMessage, errorMessage)
                End If

            Catch ex As Exception

            End Try

        Else
            'Error process buy trade coin (Send notification)
            Dim lineMessage = "Error process" + vbCrLf _
                            + "On " + Date.Now().ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                            + "Process Buy Coin ( " + cryptoObject.tradeCoin + " )" + " is error transaction." + vbCrLf _
                            + "Buy amount: " + cryptoObject.fillupAmount.ToString("###,##0.00###") + vbCrLf _
                            + "Please check a balance of stable coin ( " + cryptoObject.stableCoin + " )" + vbCrLf

            SendLineNotification(lineMessage, errorMessage)

        End If
    End Sub

    Public Shared Sub FillupOrderCoin(ByRef cryptoObject As Crypto)
        Dim listItem As ListViewItem = Nothing
        Dim datetimeTransaction As DateTime
        Dim urlParameter As String = ""
        Dim timestamp As Long = 0

        'Set parameter url api
        timestamp = GetServerTime()

        urlParameter = urlParameter + "symbol=" + cryptoObject.tradeCoin + cryptoObject.stableCoin _
                                    + "&side=BUY" _
                                    + "&type=MARKET" _
                                    + "&quoteOrderQty=" + cryptoObject.fillupAmount.ToString("#####0.00") _
                                    + "&timestamp=" + timestamp.ToString() _
                                    + "&recvWindow=5000"

        Dim signature As String = HashString(urlParameter, bitkubSecret)
        urlParameter = urlParameter + "&signature=" + signature

        Dim urlAPI As String = "https://api.binance.com/api/v3/order" + "?" + urlParameter
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        'Call API get bids and ask
        If ConsumeAPI(urlAPI, "POST", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                Dim coinItemVolume As Double = 0
                Dim coinItemPrice As Double = 0
                Dim coinOrderVolume As Double = 0
                Dim coinOrderPrice As Double = 0
                Dim coinOrderAmount As Double = 0

                'Get date time transaction
                Dim dateJan1st1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)
                Dim millionSecond As Long = Long.Parse(json.Item("transactTime").ToString())
                datetimeTransaction = dateJan1st1970.AddMilliseconds(millionSecond)
                datetimeTransaction = datetimeTransaction.ToLocalTime()

                'Get buy result
                resultJson = json.Item("fills")

                For j As Integer = 0 To resultJson.Count - 1
                    recordJson = resultJson.Item(j)

                    coinItemVolume = coinOrderVolume + Double.Parse(recordJson.Item("qty").ToString())
                    coinItemPrice = coinOrderPrice + Double.Parse(recordJson.Item("price").ToString())

                    coinOrderVolume = coinOrderVolume + coinItemVolume
                    coinOrderAmount = coinOrderAmount + (coinItemVolume * coinItemPrice)
                Next

                coinOrderPrice = coinOrderAmount / coinOrderVolume

                'Add data into list view order transaction
                listItem = New ListViewItem(datetimeTransaction.ToString("dd MMM yy / HH:mm:ss"))   'Date-time
                listItem.SubItems.Add(cryptoObject.tradeCoin)                                       'Crypto name
                listItem.SubItems.Add("Fillup")                                                     'Process name
                listItem.SubItems.Add("0")                                                          'Profit amount
                listItem.SubItems.Add("0%")                                                         'Profit percent
                listItem.SubItems.Add(coinOrderVolume.ToString("###,##0.00###"))                    'Coin Volume
                listItem.SubItems.Add(coinOrderPrice.ToString("###,##0.00###"))                     'Coin price
                listItem.SubItems.Add(coinOrderAmount.ToString("###,##0.00###"))                    'Coin amount
                listItem.BackColor = Color.Salmon

                'Insert item trade
                FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

                'Waiting binance database update 2 second
                Threading.Thread.Sleep(2000)

                'Update balance of trade coin
                GetWalletCoinBalance(cryptoObject.tradeCoin,
                                     cryptoObject.decimalPlace,
                                     cryptoObject.tradeCoinVolume)

                While (cryptoObject.tradeCoinVolume = 0)
                    Threading.Thread.Sleep(2000)

                    GetWalletCoinBalance(cryptoObject.tradeCoin,
                                         cryptoObject.decimalPlace,
                                         cryptoObject.tradeCoinVolume)
                End While

                'Update price history
                cryptoObject.tradeCoinPrice = 0

                GetHistoryPrice(cryptoObject.tradeCoin + cryptoObject.stableCoin,
                                cryptoObject.tradeCoinBuyCommission,
                                cryptoObject.decimalPlace,
                                cryptoObject.tradeCoinVolume,
                                cryptoObject.tradeCoinPrice)

                While (cryptoObject.tradeCoinPrice = 0)
                    Threading.Thread.Sleep(2000)

                    GetHistoryPrice(cryptoObject.tradeCoin + cryptoObject.stableCoin,
                                    cryptoObject.tradeCoinBuyCommission,
                                    cryptoObject.decimalPlace,
                                    cryptoObject.tradeCoinVolume,
                                    cryptoObject.tradeCoinPrice)
                End While

                cryptoObject.tradeCoinAmount = cryptoObject.tradeCoinVolume * cryptoObject.tradeCoinPrice
                cryptoObject.tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
                cryptoObject.fillupVolume = 0
                cryptoObject.fillupAmount = 0

                If cryptoObject.tradeStyle = "High Risk" Then
                    cryptoObject.tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()

                ElseIf cryptoObject.tradeStyle = "Play Safe" Then
                    cryptoObject.tradeCoinFillupPercent = cryptoObject.tradeCoinFillupPercent + 0.5
                End If

                'Calculate take profit price and fillup price
                cryptoObject.CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount)

                'Update balance stable coin
                Dim balanceStableCoin As Double = FrmSpotTrade.GetBalanceStableCoin()

                'Check active send line buy order
                If FrmSpotTrade.GetSendLineBuyCoin() Then
                    'Send notification
                    Dim lineMessage = vbCrLf _
                                    + "Fillup Coin (" + cryptoObject.tradeCoin + ")" + vbCrLf _
                                    + "On " + datetimeTransaction.ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                                    + "============================" + vbCrLf + vbCrLf _
                                    + "Coin volume: " + cryptoObject.fillupVolume.ToString("###,##0.00###") + vbCrLf _
                                    + "Coin price: " + cryptoObject.tradeCoinPrice.ToString("###,##0.00###") + vbCrLf _
                                    + "Coin amount: " + cryptoObject.fillupAmount.ToString("###,##0.00###") + vbCrLf _
                                    + "============================" + vbCrLf + vbCrLf _
                                    + "Take profit on: " + cryptoObject.tradeCoinTakeProfitPrice.ToString("###,##0.00###") + vbCrLf _
                                    + "Bid price: " + cryptoObject.actualBidPrice.ToString("###,##0.00###") + vbCrLf + vbCrLf _
                                    + "Fillup on: " + cryptoObject.tradeCoinFillupPrice.ToString("###,##0.00###") + vbCrLf _
                                    + "Ask price: " + cryptoObject.actualAskPrice.ToString("###,##0.00###") + vbCrLf _
                                    + "============================" + vbCrLf

                    SendLineNotification(lineMessage, errorMessage)
                End If

            Catch ex As Exception

            End Try

        Else
            'Error process buy trade coin (Send notification)
            Dim lineMessage = "Error process" + vbCrLf _
                            + "On " + Date.Now().ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                            + "Process Fillup Coin ( " + cryptoObject.tradeCoin + " )" + " is error transaction." + vbCrLf _
                            + "Buy amount: " + cryptoObject.fillupAmount.ToString("###,##0.00###") + vbCrLf _
                            + "Please check a balance of stable coin ( " + cryptoObject.stableCoin + " )" + vbCrLf

            SendLineNotification(lineMessage, errorMessage)

        End If
    End Sub

    Public Shared Sub CloseOrderCoin(ByRef cryptoObject As Crypto)
        Dim listItem As ListViewItem = Nothing
        Dim datetimeTransaction As DateTime
        Dim urlParameter As String = ""
        Dim timestamp As Long = 0
        Dim tradeCoinSaleVolume As Double = 0

        'Get actual trade coin volume
        While (GetWalletCoinBalance(cryptoObject.tradeCoin, cryptoObject.decimalPlace, tradeCoinSaleVolume) = False)
            Threading.Thread.Sleep(2000)
        End While

        'Check limit sale amount
        Dim checkTotalSaleAmount = tradeCoinSaleVolume * cryptoObject.actualBidPrice
        If checkTotalSaleAmount <= FrmSpotTrade.GetMinCoinAmount() Then
            cryptoObject.tradeCoinVolume = tradeCoinSaleVolume
            cryptoObject.tradeCoinPrice = 0
            cryptoObject.tradeCoinAmount = 0
            cryptoObject.tradeCoinTakeProfitPrice = 0
            cryptoObject.tradeCoinFillupPrice = 0

            Exit Sub
        End If

        'Set parameter url api
        timestamp = GetServerTime()

        urlParameter = urlParameter + "symbol=" + cryptoObject.tradeCoin + cryptoObject.stableCoin _
                                    + "&side=SELL" _
                                    + "&type=MARKET" _
                                    + "&quantity=" + tradeCoinSaleVolume.ToString() _
                                    + "&timestamp=" + timestamp.ToString() _
                                    + "&recvWindow=5000"

        Dim signature As String = HashString(urlParameter, bitkubSecret)
        urlParameter = urlParameter + "&signature=" + signature

        Dim urlAPI As String = "https://api.binance.com/api/v3/order" + "?" + urlParameter
        Dim jsonInput As String = ""
        Dim jsonOutput As String = ""
        Dim errorMessage As String = ""

        'Call API get bids and ask
        If ConsumeAPI(urlAPI, "POST", jsonInput, jsonOutput, errorMessage) Then
            Try
                'Update Json Value
                Dim json As JObject = JObject.Parse(jsonOutput)
                Dim resultJson As JToken = Nothing
                Dim recordJson As JToken = Nothing

                Dim coinOrderVolume As Double = 0
                Dim coinOrderPrice As Double = 0
                Dim coinOrderAmount As Double = 0
                Dim profitAmount As Double = 0
                Dim profitPercent As Double = 0

                'Get date time transaction
                Dim dateJan1st1970 As DateTime = New DateTime(1970, 1, 1, 0, 0, 0)
                Dim millionSecond As Long = Long.Parse(json.Item("transactTime").ToString())
                datetimeTransaction = dateJan1st1970.AddMilliseconds(millionSecond)
                datetimeTransaction = datetimeTransaction.ToLocalTime()

                'Waiting binance database update 2 second
                Threading.Thread.Sleep(2000)

                'Get sale result
                resultJson = json.Item("fills")

                For j As Integer = 0 To resultJson.Count - 1
                    recordJson = resultJson.Item(j)

                    coinOrderVolume = coinOrderVolume + Math.Abs(Double.Parse(recordJson.Item("qty").ToString()))
                    coinOrderPrice = coinOrderPrice + Math.Abs(Double.Parse(recordJson.Item("price").ToString()))
                    coinOrderAmount = coinOrderAmount + (Math.Abs(Double.Parse(recordJson.Item("qty").ToString())) * Math.Abs(Double.Parse(recordJson.Item("price").ToString())))
                Next

                coinOrderPrice = coinOrderAmount / coinOrderVolume

                'Calculate profit
                cryptoObject.tradeCoinAmount = coinOrderVolume * cryptoObject.tradeCoinPrice
                profitAmount = coinOrderAmount - cryptoObject.tradeCoinAmount
                profitAmount = profitAmount - ((cryptoObject.tradeCoinSaleCommission / 100) * profitAmount)
                profitPercent = (profitAmount * 100) / cryptoObject.tradeCoinAmount

                'Add total profit
                cryptoObject.totalProfit = cryptoObject.totalProfit + profitAmount

                'Add data into list view order transaction
                listItem = New ListViewItem(datetimeTransaction.ToString("dd MMM yy / HH:mm:ss"))   'Date-time
                listItem.SubItems.Add(cryptoObject.tradeCoin)                                       'Crypto name
                listItem.SubItems.Add("Take Profit")                                                'Process name
                listItem.SubItems.Add(profitAmount.ToString("###,##0.00"))                          'Profit amount
                listItem.SubItems.Add(profitPercent.ToString("##0.00"))                             'Profit percent
                listItem.SubItems.Add(coinOrderVolume.ToString("###,##0.00###"))                    'Coin Volume
                listItem.SubItems.Add(coinOrderPrice.ToString("###,##0.00###"))                     'Coin price
                listItem.SubItems.Add(coinOrderAmount.ToString("###,##0.00###"))                    'Coin amount
                listItem.BackColor = Color.LightGreen

                'Insert item trade
                FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

                'Update balance of trade coin
                cryptoObject.tradeCoinVolume = 0
                cryptoObject.tradeCoinAmount = 0
                cryptoObject.tradeCoinPrice = 0
                cryptoObject.tradeCoinTakeProfitPrice = 0
                cryptoObject.tradeCoinFillupPrice = 0
                cryptoObject.tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
                cryptoObject.tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()
                cryptoObject.fillupVolume = 0
                cryptoObject.fillupAmount = 0

                'Calculate take profit price and fillup price
                cryptoObject.CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount)

                'Update balance stable coin
                Dim balanceStableCoin As Double = FrmSpotTrade.GetBalanceStableCoin()

                'Check active send line sell coin
                If FrmSpotTrade.GetSendLineSellCoin() Then
                    'Send notification
                    Dim lineMessage = "Take Profit (" + cryptoObject.tradeCoin + ")" + vbCrLf _
                                    + "On " + datetimeTransaction.ToString("dd MMM yy / HH:mm") + vbCrLf _
                                    + "============================" + vbCrLf + vbCrLf _
                                    + "Coin volume: " + coinOrderVolume.ToString("###,##0.00###") + vbCrLf _
                                    + "Coin price: " + coinOrderPrice.ToString("###,##0.00###") + vbCrLf _
                                    + "Coin amount: " + coinOrderAmount.ToString("###,##0.00###") + vbCrLf _
                                    + "============================" + vbCrLf + vbCrLf _
                                    + "Profit amount: " + profitAmount.ToString("###,##0.00") + vbCrLf _
                                    + "Profit (%25): " + profitPercent.ToString("###,##0.00") + "%25" + vbCrLf _
                                    + "============================" + vbCrLf

                    SendLineNotification(lineMessage, errorMessage)
                End If

                'Update profit amount
                FrmSpotTrade.UpdateProfitAmount(datetimeTransaction, cryptoObject.stableCoin, cryptoObject.tradeCoin, profitAmount)

            Catch ex As Exception

            End Try

        Else
            'Error process buy trade coin (Send notification)
            Dim lineMessage = "Error process" + vbCrLf _
                            + "On " + Date.Now().ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                            + "Process Take Profit ( " + cryptoObject.tradeCoin + " )" + " is error transaction." + vbCrLf _
                            + "Sale volume: " + tradeCoinSaleVolume.ToString("###,##0.00###") + vbCrLf _
                            + "Please check a balance of tradeing coin ( " + cryptoObject.tradeCoin + " )" + vbCrLf

            SendLineNotification(lineMessage, errorMessage)

        End If
    End Sub
End Class
