Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class Crypto
    Enum ContactTradingSideValue As Integer
        TradeFree = 0
        TradeLong = 1
        TradeShort = 2
    End Enum

    Structure CandlesInformation
        Dim localCloseDateTime As DateTime
        Dim openTime As Long
        Dim closeTime As Long
        Dim openPrice As Double
        Dim closePrice As Double
        Dim highPrice As Double
        Dim lowPrice As Double
        Dim maxHigh As Double
        Dim maxLow As Double
        Dim volume As Double
        Dim avgPrice As Double
    End Structure

    Public botTestMode As Boolean = True
    Public tradeInterval As String = ""
    Public tradeUpdateCount As Integer = 3

    Public balanceAmount As Double = 0
    Public symbolTrade As String = ""
    Public stableCoin As String = ""
    Public tradeCoin As String = ""
    Public decimalPlace As Double = 0
    Public totalProfit As Double = 0

    Public stepupAmount As Double = 0
    Public tradeStyle As String = ""
    Public fillupVolume As Double = 0
    Public fillupAmount As Double = 0

    Public trendChangePercent As Double = 0.001
    Public wma725TrendPercent As Double = 0
    Public wma760TrendPercent As Double = 0
    Public wma2560TrendPercent As Double = 0
    Public wma7TrendPercent As Double = 0
    Public wmaTrendSide As ContactTradingSideValue = ContactTradingSideValue.TradeFree

    Public tradeCoinTakeProfitPercent As Double = 0
    Public tradeCoinFillupPercent As Double = 0
    Public tradeCoinBuyCommission As Double = 0
    Public tradeCoinSaleCommission As Double = 0

    Public tradeCoinTradingSide As ContactTradingSideValue = ContactTradingSideValue.TradeFree
    Public tradeCoinVolume As Double = 0
    Public tradeCoinPrice As Double = 0
    Public tradeCoinAmount As Double = 0
    Public tradeCoinTakeProfitPrice As Double = 0
    Public tradeCoinFillupPrice As Double = 0
    Public tradeCoinStatus As String = ""

    Public diffPrice As Double = 0
    Public diffPricePercent As Double = 0
    Public diffProfitPrice As Double = 0
    Public diffProfitPercent As Double = 0
    Public diffFillupPrice As Double = 0
    Public diffFillupPercent As Double = 0

    Public buyTrendChange = False
    Public saleTakeProfit = False
    Public buyFillup = False

    Public candlesActualRecord As CandlesInformation = Nothing
    Public tradeActualPrice As Double = 0
    Public actualBidPrice As Double = 0
    Public actualAskPrice As Double = 0

    Public candlesSimulateList As List(Of CandlesInformation) = New List(Of CandlesInformation)
    Public candlesProcessList As List(Of CandlesInformation) = New List(Of CandlesInformation)

    Public Sub StartTrade(ByVal historyTest As Boolean)
        Dim candlesList As List(Of CandlesInformation) = New List(Of CandlesInformation)
        Dim wma7Price As Double = 0
        Dim wma25Price As Double = 0
        Dim wma60Price As Double = 0
        Dim wma7PriceHistory As Double = 0
        Dim wma25PriceHistory As Double = 0
        Dim wma60PriceHistory As Double = 0

        Dim wma7CheckNewWave01 As Double = 0
        Dim wma7CheckNewWave02 As Double = 0
        Dim wma7CheckNewWave03 As Double = 0

        'Reset process from candles change
        buyTrendChange = False
        saleTakeProfit = False
        buyFillup = False

        'Get candles list
        If historyTest Then
            candlesList = candlesProcessList

            'Set Actual Candles
            If candlesActualRecord.openTime < candlesList.Item(0).openTime Then
                candlesActualRecord = candlesList.Item(0)
            End If

            'Set Bid and Ask Price
            actualAskPrice = candlesActualRecord.openPrice
            actualBidPrice = candlesActualRecord.openPrice

        Else
            candlesList = APIService.GetCandlesTrade(stableCoin,
                                                     tradeCoin,
                                                     100,
                                                     APIService.SortSequenceCandelsValue.DES)
            If candlesList.Count > 0 Then
                'Set Actual Candles
                If candlesActualRecord.openTime < candlesList.Item(0).openTime Then
                    candlesActualRecord = candlesList.Item(0)
                End If

                'Actual Bid and Ask Price
                While (APIService.GetActualBidAsk(stableCoin, tradeCoin, actualBidPrice, actualAskPrice) = False)
                    'Wait 2 second on case cannot get data
                    Threading.Thread.Sleep(2000)
                End While
            End If
        End If

        If candlesList.Count > 0 Then
            'Calculate WMA
            CalculateWMA(wma7Price, 7, candlesList, 0)
            CalculateWMA(wma25Price, 25, candlesList, 0)
            CalculateWMA(wma60Price, 60, candlesList, 0)
            CalculateWMA(wma7PriceHistory, 7, candlesList, 1)
            CalculateWMA(wma25PriceHistory, 25, candlesList, 1)
            CalculateWMA(wma60PriceHistory, 60, candlesList, 1)

            CalculateWMA(wma7CheckNewWave01, 7, candlesList, 3)
            CalculateWMA(wma7CheckNewWave02, 7, candlesList, 2)
            wma7CheckNewWave03 = wma7PriceHistory

            'Calculate WMA Information
            wma725TrendPercent = (wma7Price - wma25Price) / wma7Price
            wma760TrendPercent = (wma7Price - wma60Price) / wma7Price
            wma2560TrendPercent = (wma25Price - wma60Price) / wma25Price
            wma7TrendPercent = (wma7Price - wma7PriceHistory) / wma7Price

            If wma725TrendPercent > trendChangePercent And wma760TrendPercent > trendChangePercent Then
                wmaTrendSide = ContactTradingSideValue.TradeLong
            ElseIf wma725TrendPercent < (trendChangePercent * -1) And wma760TrendPercent < (trendChangePercent * -1) Then
                wmaTrendSide = ContactTradingSideValue.TradeShort
            Else
                wmaTrendSide = ContactTradingSideValue.TradeFree
            End If

            'Process candles graph
            If tradeCoinVolume = 0 Then
                'Set Price and Volume
                tradeActualPrice = actualAskPrice

                'Check Point to Buy New Coin
                If FrmSpotTrade.StopBuyNewCoin() = False Then
                    fillupVolume = stepupAmount / tradeActualPrice
                    fillupVolume = fillupVolume - (fillupVolume Mod decimalPlace)
                    fillupAmount = fillupVolume * tradeActualPrice

                    'Open Contract (Buy Coin)
                    If tradeStyle = "High Risk" Then
                        If FrmSpotTrade.GetBuyNewOnNextWave() Then
                            If (wma7CheckNewWave01 > wma7CheckNewWave02) _
                                And (wma7CheckNewWave02 < wma7CheckNewWave03) Then

                                'Check Application Mode
                                If botTestMode Then
                                    OpenOrderCoinSimulate()
                                Else
                                    APIService.OpenOrderCoin(Me)
                                End If
                            End If

                        ElseIf wma725TrendPercent > 0 Then
                            'Check Application Mode
                            If botTestMode Then
                                OpenOrderCoinSimulate()
                            Else
                                APIService.OpenOrderCoin(Me)
                            End If

                        End If

                    ElseIf tradeStyle = "Play Safe" Then
                        If FrmSpotTrade.GetBuyNewOnNextWave() Then
                            If (wma7CheckNewWave01 > wma7CheckNewWave02) _
                                And (wma7CheckNewWave02 < wma7CheckNewWave03) _
                                And wmaTrendSide = ContactTradingSideValue.TradeLong Then

                                'Check Application Mode
                                If botTestMode Then
                                    OpenOrderCoinSimulate()
                                Else
                                    APIService.OpenOrderCoin(Me)
                                End If
                            End If

                        ElseIf wma725TrendPercent > 0 _
                            And wmaTrendSide = ContactTradingSideValue.TradeLong Then
                            'Check Application Mode
                            If botTestMode Then
                                OpenOrderCoinSimulate()
                            Else
                                APIService.OpenOrderCoin(Me)
                            End If

                        End If

                    End If

                End If


            ElseIf tradeCoinVolume > 0 Then
                'Check Point Take Profit and Fillup

                'Set Price for Close Contract
                tradeActualPrice = actualBidPrice

                'Set Profit and Cutloss Information
                diffPrice = tradeActualPrice - tradeCoinPrice
                diffPricePercent = (diffPrice * 100) / tradeCoinPrice

                diffProfitPrice = tradeActualPrice - tradeCoinTakeProfitPrice
                diffProfitPercent = (diffProfitPrice * 100) / tradeCoinTakeProfitPrice

                diffFillupPrice = tradeActualPrice - tradeCoinFillupPrice
                diffFillupPercent = (diffFillupPrice * 100) / tradeCoinFillupPrice

                'Check Take Profit and Cut Loss
                If tradeActualPrice >= tradeCoinTakeProfitPrice Then
                    'Take Profit Process
                    If botTestMode Then
                        CloseOrderCoinSimulate()
                    Else
                        APIService.CloseOrderCoin(Me)
                    End If

                ElseIf tradeActualPrice <= tradeCoinFillupPrice And wma725TrendPercent > 0 Then
                    'Set Price
                    tradeActualPrice = actualAskPrice

                    If CalculateFillupInformation() Then
                        'Fillup Process
                        If botTestMode Then
                            FillupOrderCoinSimulate()
                        Else
                            APIService.FillupOrderCoin(Me)
                        End If

                    End If

                End If
            End If
        End If

        'Clear Volume and Amount Present
        If tradeCoinAmount = 0 Then
            tradeCoinTakeProfitPrice = 0
            tradeCoinFillupPercent = 0
            diffPrice = 0
            diffPricePercent = 0
            diffProfitPrice = 0
            diffProfitPercent = 0
            diffFillupPrice = 0
            diffFillupPercent = 0
            fillupVolume = 0
            fillupAmount = 0

        ElseIf tradeCoinAmount > 0 And wma725TrendPercent < 0 Then
            fillupVolume = 0
            fillupAmount = 0

        End If

        'Update Crypto Trade
        If historyTest Then
            FrmSpotTrade.UpdateCryptoTrade(Me, False)
        Else
            FrmSpotTrade.UpdateCryptoTrade(Me, True)
        End If
    End Sub

    Public Sub CalculateWMA(ByRef wmaPrice As Double,
                            ByRef weigthAvg As Double,
                            ByVal candlesList As List(Of CandlesInformation),
                            ByVal indexStart As Integer)

        Dim indexWeigth As Integer = weigthAvg
        Dim summaryWMA As Double = 0
        Dim candlesRecord As CandlesInformation = Nothing

        For indexCount As Integer = indexStart To candlesList.Count - 1
            candlesRecord = candlesList.Item(indexCount)
            summaryWMA = summaryWMA + (candlesRecord.closePrice * indexWeigth)
            indexWeigth = indexWeigth - 1

            If indexWeigth = 0 Then
                Exit For
            End If
        Next

        wmaPrice = summaryWMA / ((weigthAvg * (weigthAvg + 1)) / 2)
    End Sub

    Public Function CalculateFillupInformation() As Boolean
        Dim result As Boolean = False
        Dim minContractAmount As Double = 15
        Dim differencePricePercent As Double = 0.4
        Dim needAvgPrice As Double = 0
        Dim newAvgPrice As Double = tradeCoinPrice
        Dim newAmountFillup As Double = 0
        Dim newVolumeFillup As Double = 0

        If wma7TrendPercent > 0 Then
            needAvgPrice = tradeActualPrice + ((differencePricePercent / 100) * tradeActualPrice)

            'Get Fillup Amount 
            While (newAvgPrice > needAvgPrice)
                newAmountFillup = newAmountFillup + 10
                newVolumeFillup = newAmountFillup / tradeActualPrice

                newAvgPrice = (tradeCoinAmount + newAmountFillup) / (tradeCoinVolume + newVolumeFillup)
            End While

            'Calculate min decimal place
            newVolumeFillup = newVolumeFillup - (newVolumeFillup Mod decimalPlace)
            newAmountFillup = newVolumeFillup * tradeActualPrice

            If newVolumeFillup < decimalPlace Then
                newVolumeFillup = decimalPlace
                newAmountFillup = newVolumeFillup * tradeActualPrice
            End If

            fillupVolume = newVolumeFillup
            fillupAmount = newAmountFillup

            'Check minimun fillup, max amount, balance amount
            Dim checkMaxAmount As Double = tradeCoinAmount + fillupAmount
            If fillupAmount > minContractAmount _
                And checkMaxAmount <= FrmSpotTrade.GetLimitCoinAmount() _
                And fillupAmount < balanceAmount Then

                result = True
            End If
        End If

        Return result
    End Function

    Public Sub CalculateTakeProfitAndFillupInformation(ByVal minCoinAmount As Double)
        'Check error double value
        If tradeCoinVolume = Double.NaN Then
            tradeCoinAmount = 0
        End If

        If tradeCoinAmount <= minCoinAmount Then
            'Clear value
            tradeCoinVolume = 0
            tradeCoinPrice = 0
            tradeCoinAmount = 0
            tradeCoinTakeProfitPrice = 0
            tradeCoinFillupPrice = 0

        Else
            tradeCoinPrice = tradeCoinAmount / tradeCoinVolume

            'Calculate take profit
            tradeCoinTakeProfitPrice = tradeCoinPrice + (tradeCoinPrice * (tradeCoinTakeProfitPercent + tradeCoinSaleCommission) / 100)
            tradeCoinFillupPrice = tradeCoinPrice - (tradeCoinPrice * (tradeCoinFillupPercent / 100))
        End If
    End Sub

    Public Sub OpenOrderCoinSimulate()
        Dim listItem As ListViewItem = Nothing

        'Date-time
        listItem = New ListViewItem(candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm:ss"))
        listItem.SubItems.Add(tradeCoin)                                          'Trade coin name
        listItem.SubItems.Add("Buy Coin")                                         'Process type
        listItem.SubItems.Add("0")                                                'Profit amount
        listItem.SubItems.Add("0")                                                'Profit percent
        listItem.SubItems.Add(fillupVolume.ToString("###,##0.00###"))             'Buy volume
        listItem.SubItems.Add(tradeActualPrice.ToString("###,##0.00###"))         'Buy price
        listItem.SubItems.Add(fillupAmount.ToString("###,##0.00###"))             'Total amount
        listItem.BackColor = Color.Salmon

        'Insert item trade
        FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

        'Update balance of trade coin
        tradeCoinVolume = fillupVolume
        tradeCoinPrice = tradeActualPrice
        tradeCoinAmount = fillupAmount
        tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
        tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()
        fillupVolume = 0
        fillupAmount = 0

        'Calculate Profit Priceand Fillup Price
        CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount())

        'Update Balance Amount
        Dim notificationBalanceStableCoin = FrmSpotTrade.AdjustAmountBuyCoinSimulate(fillupAmount)

        'Send line notification
        If FrmSpotTrade.GetSendLineBuyCoin() Then
            'Send notification
            Dim lineMessage = vbCrLf _
                            + "Buy Coin (" + tradeCoin + ")" + vbCrLf _
                            + "On " + candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Coin volume: " + fillupVolume.ToString("###,##0.00###") + vbCrLf _
                            + "Coin price: " + tradeActualPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Coin amount: " + fillupAmount.ToString("###,##0.00###") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Take profit on: " + tradeCoinTakeProfitPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Bid price: " + actualBidPrice.ToString("###,##0.00###") + vbCrLf + vbCrLf _
                            + "Fillup on: " + tradeCoinFillupPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Ask price: " + actualAskPrice.ToString("###,##0.00###") + vbCrLf _
                            + "============================" + vbCrLf

            Dim errorMessage As String = ""
            APIService.SendLineNotification(lineMessage, errorMessage)
        End If
    End Sub

    Public Sub FillupOrderCoinSimulate()
        Dim listItem As ListViewItem = Nothing

        'Date-time
        listItem = New ListViewItem(candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm:ss"))
        listItem.SubItems.Add(tradeCoin)                                          'Trade coin name
        listItem.SubItems.Add("Fillup")                                           'Process type
        listItem.SubItems.Add("0")                                                'Profit amount
        listItem.SubItems.Add("0")                                                'Profit percent
        listItem.SubItems.Add(fillupVolume.ToString("###,##0.00###"))             'Buy volume
        listItem.SubItems.Add(tradeActualPrice.ToString("###,##0.00###"))         'Buy price
        listItem.SubItems.Add(fillupAmount.ToString("###,##0.00###"))             'Total amount
        listItem.BackColor = Color.Salmon

        'Insert item trade
        FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

        'Update balance of trade coin
        tradeCoinVolume = tradeCoinVolume + fillupVolume
        tradeCoinAmount = tradeCoinAmount + fillupAmount
        tradeCoinPrice = tradeCoinAmount / tradeCoinVolume
        tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
        fillupVolume = 0
        fillupAmount = 0

        If tradeStyle = "High Risk" Then
            tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()

        ElseIf tradeStyle = "Play Safe" Then
            tradeCoinFillupPercent = tradeCoinFillupPercent + 0.5
        End If

        'Calculate Profit Priceand Fillup Price
        CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount())

        'Update Balance Amount
        Dim notificationBalanceStableCoin = FrmSpotTrade.AdjustAmountBuyCoinSimulate(fillupAmount)

        'Send line notification
        If FrmSpotTrade.GetSendLineBuyCoin() Then
            'Send notification
            Dim lineMessage = vbCrLf _
                            + "Fillup Coin (" + tradeCoin + ")" + vbCrLf _
                            + "On " + candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm:ss") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Coin volume: " + fillupVolume.ToString("###,##0.00###") + vbCrLf _
                            + "Coin price: " + tradeActualPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Coin amount: " + fillupAmount.ToString("###,##0.00###") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Take profit on: " + tradeCoinTakeProfitPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Bid price: " + actualBidPrice.ToString("###,##0.00###") + vbCrLf + vbCrLf _
                            + "Fillup on: " + tradeCoinFillupPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Ask price: " + actualAskPrice.ToString("###,##0.00###") + vbCrLf _
                            + "============================" + vbCrLf

            Dim errorMessage As String = ""
            APIService.SendLineNotification(lineMessage, errorMessage)
        End If
    End Sub

    Public Sub CloseOrderCoinSimulate()
        Dim sellAmount As Double = tradeCoinVolume * tradeActualPrice
        Dim profitAmount As Double = sellAmount - tradeCoinAmount
        profitAmount = profitAmount - ((tradeCoinSaleCommission / 100) * profitAmount)

        Dim profitPercent As Double = (profitAmount * 100) / tradeCoinAmount
        Dim listItem As ListViewItem = Nothing

        'Date-Time
        listItem = New ListViewItem(candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm:ss"))
        listItem.SubItems.Add(tradeCoin)                                          'Trade coin name
        listItem.SubItems.Add("Take Profit")                                      'Process type
        listItem.SubItems.Add(profitAmount.ToString("###,##0.00"))                'Profit amount
        listItem.SubItems.Add(profitPercent.ToString("###,##0.00") + "%")         'Profit percent
        listItem.SubItems.Add(tradeCoinVolume.ToString("###,##0.00###"))          'Volume
        listItem.SubItems.Add(tradeActualPrice.ToString("###,##0.00###"))         'Price
        listItem.SubItems.Add(sellAmount.ToString("###,##0.00###"))               'Amount
        listItem.BackColor = Color.LightGreen

        'Insert item trade
        FrmSpotTrade.Invoke(Sub() FrmSpotTrade.ListViewTrade.Items.Insert(0, listItem))

        'Update Balance Amount
        Dim notificationBalanceStableCoin = FrmSpotTrade.AdjustAmountSaleCoinSimulate(sellAmount)

        'Send Line Notificatoin
        If FrmSpotTrade.GetSendLineSellCoin() Then
            Dim lineMessage = "Take Profit (" + tradeCoin + ")" + vbCrLf _
                            + "On " + candlesActualRecord.localCloseDateTime.ToString("dd MMM yy / HH:mm") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Coin volume: " + tradeCoinVolume.ToString("###,##0.00###") + vbCrLf _
                            + "Coin price: " + tradeActualPrice.ToString("###,##0.00###") + vbCrLf _
                            + "Coin amount: " + sellAmount.ToString("###,##0.00###") + vbCrLf _
                            + "============================" + vbCrLf + vbCrLf _
                            + "Profit amount: " + profitAmount.ToString("###,##0.00") + vbCrLf _
                            + "Profit percent: " + profitPercent.ToString("###,##0.00") + vbCrLf _
                            + "============================" + vbCrLf

            Dim errorMessage As String = ""
            APIService.SendLineNotification(lineMessage, errorMessage)
        End If

        'Update balance of trade coin
        tradeCoinVolume = 0
        tradeCoinPrice = 0
        tradeCoinAmount = 0
        tradeCoinTakeProfitPercent = FrmSpotTrade.GetTakeProfitPercent()
        tradeCoinFillupPercent = FrmSpotTrade.GetFillupPercent()
        fillupVolume = 0
        fillupAmount = 0

        'Calculate Profit Priceand Fillup Price
        CalculateTakeProfitAndFillupInformation(FrmSpotTrade.GetMinCoinAmount())

        'Update summary profit amount
        FrmSpotTrade.UpdateProfitAmount(candlesActualRecord.localCloseDateTime, stableCoin, tradeCoin, profitAmount)
    End Sub
End Class
