<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSpotTrade
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSpotTrade))
        Me.CheckBoxBuyNewOnNextWave = New System.Windows.Forms.CheckBox()
        Me.ComboBoxLimitCoinAmount = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CheckBoxStopBuyNew = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ComboBoxTradeStyle = New System.Windows.Forms.ComboBox()
        Me.TextBoxProfitAmount = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBoxMaxBudgetUse = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TextBoxTotalAmount = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TextBoxCoinAmount = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RadioButtonTradeModeReal = New System.Windows.Forms.RadioButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DisplayProgramMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuNotification = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitProgramMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotifyIconAutoTrade = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckedListBoxTradeCoin = New System.Windows.Forms.CheckedListBox()
        Me.TabSummaryProfit = New System.Windows.Forms.TabPage()
        Me.ListViewSummaryProfit = New System.Windows.Forms.ListView()
        Me.ColSummaryDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColSummaryCount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColSummaryAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabControlTrade = New System.Windows.Forms.TabControl()
        Me.TabCryptoTrade = New System.Windows.Forms.TabPage()
        Me.ListViewCrypto = New System.Windows.Forms.ListView()
        Me.ColCryptoName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoDateTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoShortTrend = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoLongTrend = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoDiffProfitPercent = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoDiffFillupPercent = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoPrice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoSellPrice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoBidPrice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoFillup = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoAskPrice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColCryptoFillupAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabOrder = New System.Windows.Forms.TabPage()
        Me.ListViewTrade = New System.Windows.Forms.ListView()
        Me.ColTradeDateTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColTradeCoin = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColTradeProcess = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColOrderProfitAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColOrderProfitPercent = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColTradeVolume = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColTradeCoinPrice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColTradeAmount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabErrorLog = New System.Windows.Forms.TabPage()
        Me.ListViewErrorLog = New System.Windows.Forms.ListView()
        Me.ColErrorDateTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColErrorLogMessage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TimerAutoTrade = New System.Windows.Forms.Timer(Me.components)
        Me.RadioButtonTradeModeSimulate = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ButtonUpdateBinanceAPI = New System.Windows.Forms.Button()
        Me.TextBoxAPISecret = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.TextBoxAPIKey = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.ButtonStopTrade = New System.Windows.Forms.Button()
        Me.TimerSetupEnvironment = New System.Windows.Forms.Timer(Me.components)
        Me.ButtonStartTrade = New System.Windows.Forms.Button()
        Me.TextBoxBalanceAmount = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ComboBoxStableCoin = New System.Windows.Forms.ComboBox()
        Me.ComboBoxStepAmount = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ComboBoxIntervalTime = New System.Windows.Forms.ComboBox()
        Me.ComboBoxSaleCommission = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ComboBoxTakeProfitPercent = New System.Windows.Forms.ComboBox()
        Me.ComboBoxFillupPercent = New System.Windows.Forms.ComboBox()
        Me.ComboBoxBuyCommission = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ToolStripLabelMessageLog = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabelRemainTakeProfitTime = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripProgressAutoTrade = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMain = New System.Windows.Forms.ToolStrip()
        Me.ToolStripDropDownMenu = New System.Windows.Forms.ToolStripDropDownButton()
        Me.DownloadLicenseMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.UploadLicenseKeyToolMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.OpenFileDialogLicense = New System.Windows.Forms.OpenFileDialog()
        Me.CheckBoxAutoRun = New System.Windows.Forms.CheckBox()
        Me.SaveFileDialogLicense = New System.Windows.Forms.SaveFileDialog()
        Me.ButtonHistoryTest = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ComboBoxSummaryHour = New System.Windows.Forms.ComboBox()
        Me.CheckBoxSendLineSellCoin = New System.Windows.Forms.CheckBox()
        Me.CheckBoxSendLineBuyCoin = New System.Windows.Forms.CheckBox()
        Me.CheckBoxSendLineSummary = New System.Windows.Forms.CheckBox()
        Me.ButtonUpdateLineToken = New System.Windows.Forms.Button()
        Me.TextBoxLineToken = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ContextMenuNotification.SuspendLayout()
        Me.TabSummaryProfit.SuspendLayout()
        Me.TabControlTrade.SuspendLayout()
        Me.TabCryptoTrade.SuspendLayout()
        Me.TabOrder.SuspendLayout()
        Me.TabErrorLog.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ToolStripMain.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'CheckBoxBuyNewOnNextWave
        '
        Me.CheckBoxBuyNewOnNextWave.AutoSize = True
        Me.CheckBoxBuyNewOnNextWave.Location = New System.Drawing.Point(246, 100)
        Me.CheckBoxBuyNewOnNextWave.Name = "CheckBoxBuyNewOnNextWave"
        Me.CheckBoxBuyNewOnNextWave.Size = New System.Drawing.Size(157, 17)
        Me.CheckBoxBuyNewOnNextWave.TabIndex = 39
        Me.CheckBoxBuyNewOnNextWave.Text = "Buy new coin on next wave"
        Me.CheckBoxBuyNewOnNextWave.UseVisualStyleBackColor = True
        '
        'ComboBoxLimitCoinAmount
        '
        Me.ComboBoxLimitCoinAmount.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxLimitCoinAmount.FormattingEnabled = True
        Me.ComboBoxLimitCoinAmount.Location = New System.Drawing.Point(338, 74)
        Me.ComboBoxLimitCoinAmount.Name = "ComboBoxLimitCoinAmount"
        Me.ComboBoxLimitCoinAmount.Size = New System.Drawing.Size(111, 21)
        Me.ComboBoxLimitCoinAmount.TabIndex = 37
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(243, 77)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(91, 13)
        Me.Label14.TabIndex = 38
        Me.Label14.Text = "Limit Coin Amount"
        '
        'CheckBoxStopBuyNew
        '
        Me.CheckBoxStopBuyNew.AutoSize = True
        Me.CheckBoxStopBuyNew.Location = New System.Drawing.Point(25, 100)
        Me.CheckBoxStopBuyNew.Name = "CheckBoxStopBuyNew"
        Me.CheckBoxStopBuyNew.Size = New System.Drawing.Size(180, 17)
        Me.CheckBoxStopBuyNew.TabIndex = 36
        Me.CheckBoxStopBuyNew.Text = "Stop a new trade (Buy new coin)"
        Me.CheckBoxStopBuyNew.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(466, 77)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(61, 13)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Trade Style"
        '
        'ComboBoxTradeStyle
        '
        Me.ComboBoxTradeStyle.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxTradeStyle.FormattingEnabled = True
        Me.ComboBoxTradeStyle.Items.AddRange(New Object() {"High Risk", "Play Safe"})
        Me.ComboBoxTradeStyle.Location = New System.Drawing.Point(540, 74)
        Me.ComboBoxTradeStyle.Name = "ComboBoxTradeStyle"
        Me.ComboBoxTradeStyle.Size = New System.Drawing.Size(115, 21)
        Me.ComboBoxTradeStyle.TabIndex = 34
        '
        'TextBoxProfitAmount
        '
        Me.TextBoxProfitAmount.BackColor = System.Drawing.Color.Aquamarine
        Me.TextBoxProfitAmount.Location = New System.Drawing.Point(791, 72)
        Me.TextBoxProfitAmount.Name = "TextBoxProfitAmount"
        Me.TextBoxProfitAmount.ReadOnly = True
        Me.TextBoxProfitAmount.Size = New System.Drawing.Size(102, 20)
        Me.TextBoxProfitAmount.TabIndex = 30
        Me.TextBoxProfitAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(677, 77)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(97, 13)
        Me.Label13.TabIndex = 29
        Me.Label13.Text = "Total Profit Amount"
        '
        'TextBoxMaxBudgetUse
        '
        Me.TextBoxMaxBudgetUse.BackColor = System.Drawing.Color.Aquamarine
        Me.TextBoxMaxBudgetUse.Location = New System.Drawing.Point(791, 46)
        Me.TextBoxMaxBudgetUse.Name = "TextBoxMaxBudgetUse"
        Me.TextBoxMaxBudgetUse.ReadOnly = True
        Me.TextBoxMaxBudgetUse.Size = New System.Drawing.Size(102, 20)
        Me.TextBoxMaxBudgetUse.TabIndex = 28
        Me.TextBoxMaxBudgetUse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(677, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(108, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Maximun Budget Use"
        '
        'TextBoxTotalAmount
        '
        Me.TextBoxTotalAmount.BackColor = System.Drawing.Color.Aquamarine
        Me.TextBoxTotalAmount.Location = New System.Drawing.Point(1005, 72)
        Me.TextBoxTotalAmount.Name = "TextBoxTotalAmount"
        Me.TextBoxTotalAmount.ReadOnly = True
        Me.TextBoxTotalAmount.Size = New System.Drawing.Size(102, 20)
        Me.TextBoxTotalAmount.TabIndex = 26
        Me.TextBoxTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(914, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "Total Amount"
        '
        'TextBoxCoinAmount
        '
        Me.TextBoxCoinAmount.BackColor = System.Drawing.Color.Aquamarine
        Me.TextBoxCoinAmount.Location = New System.Drawing.Point(1005, 45)
        Me.TextBoxCoinAmount.Name = "TextBoxCoinAmount"
        Me.TextBoxCoinAmount.ReadOnly = True
        Me.TextBoxCoinAmount.Size = New System.Drawing.Size(102, 20)
        Me.TextBoxCoinAmount.TabIndex = 24
        Me.TextBoxCoinAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(914, 50)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "Coin Amount"
        '
        'RadioButtonTradeModeReal
        '
        Me.RadioButtonTradeModeReal.AutoSize = True
        Me.RadioButtonTradeModeReal.Location = New System.Drawing.Point(577, 21)
        Me.RadioButtonTradeModeReal.Name = "RadioButtonTradeModeReal"
        Me.RadioButtonTradeModeReal.Size = New System.Drawing.Size(78, 17)
        Me.RadioButtonTradeModeReal.TabIndex = 18
        Me.RadioButtonTradeModeReal.TabStop = True
        Me.RadioButtonTradeModeReal.Text = "Real Trade"
        Me.RadioButtonTradeModeReal.UseVisualStyleBackColor = True
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(164, 6)
        '
        'DisplayProgramMenu
        '
        Me.DisplayProgramMenu.Name = "DisplayProgramMenu"
        Me.DisplayProgramMenu.Size = New System.Drawing.Size(167, 22)
        Me.DisplayProgramMenu.Text = "Show Application"
        '
        'ContextMenuNotification
        '
        Me.ContextMenuNotification.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayProgramMenu, Me.ToolStripSeparator1, Me.ExitProgramMenu})
        Me.ContextMenuNotification.Name = "ContextMenuNotification"
        Me.ContextMenuNotification.Size = New System.Drawing.Size(168, 54)
        '
        'ExitProgramMenu
        '
        Me.ExitProgramMenu.Name = "ExitProgramMenu"
        Me.ExitProgramMenu.Size = New System.Drawing.Size(167, 22)
        Me.ExitProgramMenu.Text = "Exit Program"
        '
        'NotifyIconAutoTrade
        '
        Me.NotifyIconAutoTrade.BalloonTipText = "Binance Spot Trade"
        Me.NotifyIconAutoTrade.BalloonTipTitle = "Binance Spot Trade"
        Me.NotifyIconAutoTrade.ContextMenuStrip = Me.ContextMenuNotification
        Me.NotifyIconAutoTrade.Icon = CType(resources.GetObject("NotifyIconAutoTrade.Icon"), System.Drawing.Icon)
        Me.NotifyIconAutoTrade.Text = "Binance Spot Trade"
        Me.NotifyIconAutoTrade.Visible = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 64
        Me.Label1.Text = "Stable Coin"
        '
        'CheckedListBoxTradeCoin
        '
        Me.CheckedListBoxTradeCoin.CheckOnClick = True
        Me.CheckedListBoxTradeCoin.FormattingEnabled = True
        Me.CheckedListBoxTradeCoin.Location = New System.Drawing.Point(9, 118)
        Me.CheckedListBoxTradeCoin.Name = "CheckedListBoxTradeCoin"
        Me.CheckedListBoxTradeCoin.Size = New System.Drawing.Size(124, 169)
        Me.CheckedListBoxTradeCoin.TabIndex = 63
        '
        'TabSummaryProfit
        '
        Me.TabSummaryProfit.Controls.Add(Me.ListViewSummaryProfit)
        Me.TabSummaryProfit.Location = New System.Drawing.Point(4, 22)
        Me.TabSummaryProfit.Name = "TabSummaryProfit"
        Me.TabSummaryProfit.Size = New System.Drawing.Size(1252, 204)
        Me.TabSummaryProfit.TabIndex = 2
        Me.TabSummaryProfit.Text = "Summary Profit"
        Me.TabSummaryProfit.UseVisualStyleBackColor = True
        '
        'ListViewSummaryProfit
        '
        Me.ListViewSummaryProfit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColSummaryDate, Me.ColSummaryCount, Me.ColSummaryAmount})
        Me.ListViewSummaryProfit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewSummaryProfit.FullRowSelect = True
        Me.ListViewSummaryProfit.GridLines = True
        Me.ListViewSummaryProfit.HideSelection = False
        Me.ListViewSummaryProfit.Location = New System.Drawing.Point(0, 0)
        Me.ListViewSummaryProfit.MultiSelect = False
        Me.ListViewSummaryProfit.Name = "ListViewSummaryProfit"
        Me.ListViewSummaryProfit.Size = New System.Drawing.Size(1252, 204)
        Me.ListViewSummaryProfit.TabIndex = 1
        Me.ListViewSummaryProfit.UseCompatibleStateImageBehavior = False
        Me.ListViewSummaryProfit.View = System.Windows.Forms.View.Details
        '
        'ColSummaryDate
        '
        Me.ColSummaryDate.Text = "Date / Trade Coin"
        Me.ColSummaryDate.Width = 277
        '
        'ColSummaryCount
        '
        Me.ColSummaryCount.Text = "Trading Count (SELL)"
        Me.ColSummaryCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColSummaryCount.Width = 210
        '
        'ColSummaryAmount
        '
        Me.ColSummaryAmount.Text = "Profit Amount"
        Me.ColSummaryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColSummaryAmount.Width = 246
        '
        'TabControlTrade
        '
        Me.TabControlTrade.Controls.Add(Me.TabCryptoTrade)
        Me.TabControlTrade.Controls.Add(Me.TabOrder)
        Me.TabControlTrade.Controls.Add(Me.TabSummaryProfit)
        Me.TabControlTrade.Controls.Add(Me.TabErrorLog)
        Me.TabControlTrade.Location = New System.Drawing.Point(9, 297)
        Me.TabControlTrade.Name = "TabControlTrade"
        Me.TabControlTrade.Padding = New System.Drawing.Point(20, 3)
        Me.TabControlTrade.SelectedIndex = 0
        Me.TabControlTrade.Size = New System.Drawing.Size(1260, 230)
        Me.TabControlTrade.TabIndex = 65
        '
        'TabCryptoTrade
        '
        Me.TabCryptoTrade.Controls.Add(Me.ListViewCrypto)
        Me.TabCryptoTrade.Location = New System.Drawing.Point(4, 22)
        Me.TabCryptoTrade.Name = "TabCryptoTrade"
        Me.TabCryptoTrade.Padding = New System.Windows.Forms.Padding(3)
        Me.TabCryptoTrade.Size = New System.Drawing.Size(1252, 204)
        Me.TabCryptoTrade.TabIndex = 0
        Me.TabCryptoTrade.Text = "Crypto Trade"
        Me.TabCryptoTrade.UseVisualStyleBackColor = True
        '
        'ListViewCrypto
        '
        Me.ListViewCrypto.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColCryptoName, Me.ColCryptoDateTime, Me.ColCryptoShortTrend, Me.ColCryptoLongTrend, Me.ColCryptoDiffProfitPercent, Me.ColCryptoDiffFillupPercent, Me.ColCryptoAmount, Me.ColCryptoVolume, Me.ColCryptoPrice, Me.ColCryptoSellPrice, Me.ColCryptoBidPrice, Me.ColCryptoFillup, Me.ColCryptoAskPrice, Me.ColCryptoFillupAmount})
        Me.ListViewCrypto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewCrypto.FullRowSelect = True
        Me.ListViewCrypto.GridLines = True
        Me.ListViewCrypto.HideSelection = False
        Me.ListViewCrypto.Location = New System.Drawing.Point(3, 3)
        Me.ListViewCrypto.MultiSelect = False
        Me.ListViewCrypto.Name = "ListViewCrypto"
        Me.ListViewCrypto.Size = New System.Drawing.Size(1246, 198)
        Me.ListViewCrypto.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListViewCrypto.TabIndex = 34
        Me.ListViewCrypto.UseCompatibleStateImageBehavior = False
        Me.ListViewCrypto.View = System.Windows.Forms.View.Details
        '
        'ColCryptoName
        '
        Me.ColCryptoName.Text = "Crypto Coin"
        Me.ColCryptoName.Width = 67
        '
        'ColCryptoDateTime
        '
        Me.ColCryptoDateTime.Text = "Update Date-Time"
        Me.ColCryptoDateTime.Width = 125
        '
        'ColCryptoShortTrend
        '
        Me.ColCryptoShortTrend.Text = "Short Trend"
        Me.ColCryptoShortTrend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoShortTrend.Width = 85
        '
        'ColCryptoLongTrend
        '
        Me.ColCryptoLongTrend.Text = "Long Trend"
        Me.ColCryptoLongTrend.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoLongTrend.Width = 85
        '
        'ColCryptoDiffProfitPercent
        '
        Me.ColCryptoDiffProfitPercent.Text = "Diff Profit (%)"
        Me.ColCryptoDiffProfitPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoDiffProfitPercent.Width = 85
        '
        'ColCryptoDiffFillupPercent
        '
        Me.ColCryptoDiffFillupPercent.Text = "Diff Fillup (%)"
        Me.ColCryptoDiffFillupPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoDiffFillupPercent.Width = 85
        '
        'ColCryptoAmount
        '
        Me.ColCryptoAmount.Text = "Amount"
        Me.ColCryptoAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoAmount.Width = 85
        '
        'ColCryptoVolume
        '
        Me.ColCryptoVolume.Text = "Volume"
        Me.ColCryptoVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoVolume.Width = 85
        '
        'ColCryptoPrice
        '
        Me.ColCryptoPrice.Text = "Price"
        Me.ColCryptoPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoPrice.Width = 85
        '
        'ColCryptoSellPrice
        '
        Me.ColCryptoSellPrice.Text = "Sell Price"
        Me.ColCryptoSellPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoSellPrice.Width = 85
        '
        'ColCryptoBidPrice
        '
        Me.ColCryptoBidPrice.Text = "Bid Price"
        Me.ColCryptoBidPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoBidPrice.Width = 85
        '
        'ColCryptoFillup
        '
        Me.ColCryptoFillup.Text = "Fillup Price"
        Me.ColCryptoFillup.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoFillup.Width = 85
        '
        'ColCryptoAskPrice
        '
        Me.ColCryptoAskPrice.Text = "Ask Price"
        Me.ColCryptoAskPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoAskPrice.Width = 85
        '
        'ColCryptoFillupAmount
        '
        Me.ColCryptoFillupAmount.Text = "Fillup Amount"
        Me.ColCryptoFillupAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColCryptoFillupAmount.Width = 85
        '
        'TabOrder
        '
        Me.TabOrder.Controls.Add(Me.ListViewTrade)
        Me.TabOrder.Location = New System.Drawing.Point(4, 22)
        Me.TabOrder.Name = "TabOrder"
        Me.TabOrder.Padding = New System.Windows.Forms.Padding(3)
        Me.TabOrder.Size = New System.Drawing.Size(1252, 204)
        Me.TabOrder.TabIndex = 1
        Me.TabOrder.Text = "Order Transaction"
        Me.TabOrder.UseVisualStyleBackColor = True
        '
        'ListViewTrade
        '
        Me.ListViewTrade.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColTradeDateTime, Me.ColTradeCoin, Me.ColTradeProcess, Me.ColOrderProfitAmount, Me.ColOrderProfitPercent, Me.ColTradeVolume, Me.ColTradeCoinPrice, Me.ColTradeAmount})
        Me.ListViewTrade.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewTrade.FullRowSelect = True
        Me.ListViewTrade.GridLines = True
        Me.ListViewTrade.HideSelection = False
        Me.ListViewTrade.Location = New System.Drawing.Point(3, 3)
        Me.ListViewTrade.MultiSelect = False
        Me.ListViewTrade.Name = "ListViewTrade"
        Me.ListViewTrade.Size = New System.Drawing.Size(1246, 198)
        Me.ListViewTrade.TabIndex = 0
        Me.ListViewTrade.UseCompatibleStateImageBehavior = False
        Me.ListViewTrade.View = System.Windows.Forms.View.Details
        '
        'ColTradeDateTime
        '
        Me.ColTradeDateTime.Text = "Date-Time"
        Me.ColTradeDateTime.Width = 180
        '
        'ColTradeCoin
        '
        Me.ColTradeCoin.Text = "Crypto"
        Me.ColTradeCoin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColTradeCoin.Width = 105
        '
        'ColTradeProcess
        '
        Me.ColTradeProcess.Text = "Process"
        Me.ColTradeProcess.Width = 110
        '
        'ColOrderProfitAmount
        '
        Me.ColOrderProfitAmount.Text = "Profit Amount"
        Me.ColOrderProfitAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColOrderProfitAmount.Width = 110
        '
        'ColOrderProfitPercent
        '
        Me.ColOrderProfitPercent.Text = "Profit (%)"
        Me.ColOrderProfitPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColOrderProfitPercent.Width = 110
        '
        'ColTradeVolume
        '
        Me.ColTradeVolume.Text = "Coin Volume"
        Me.ColTradeVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColTradeVolume.Width = 110
        '
        'ColTradeCoinPrice
        '
        Me.ColTradeCoinPrice.Text = "Coin Price"
        Me.ColTradeCoinPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColTradeCoinPrice.Width = 110
        '
        'ColTradeAmount
        '
        Me.ColTradeAmount.Text = "Total Amount"
        Me.ColTradeAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColTradeAmount.Width = 110
        '
        'TabErrorLog
        '
        Me.TabErrorLog.Controls.Add(Me.ListViewErrorLog)
        Me.TabErrorLog.Location = New System.Drawing.Point(4, 22)
        Me.TabErrorLog.Name = "TabErrorLog"
        Me.TabErrorLog.Size = New System.Drawing.Size(1252, 204)
        Me.TabErrorLog.TabIndex = 3
        Me.TabErrorLog.Text = "Error Log"
        Me.TabErrorLog.UseVisualStyleBackColor = True
        '
        'ListViewErrorLog
        '
        Me.ListViewErrorLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColErrorDateTime, Me.ColErrorLogMessage})
        Me.ListViewErrorLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListViewErrorLog.FullRowSelect = True
        Me.ListViewErrorLog.GridLines = True
        Me.ListViewErrorLog.HideSelection = False
        Me.ListViewErrorLog.Location = New System.Drawing.Point(0, 0)
        Me.ListViewErrorLog.MultiSelect = False
        Me.ListViewErrorLog.Name = "ListViewErrorLog"
        Me.ListViewErrorLog.Size = New System.Drawing.Size(1252, 204)
        Me.ListViewErrorLog.TabIndex = 1
        Me.ListViewErrorLog.UseCompatibleStateImageBehavior = False
        Me.ListViewErrorLog.View = System.Windows.Forms.View.Details
        '
        'ColErrorDateTime
        '
        Me.ColErrorDateTime.Text = "Date-Time"
        Me.ColErrorDateTime.Width = 140
        '
        'ColErrorLogMessage
        '
        Me.ColErrorLogMessage.Text = "Error Log"
        Me.ColErrorLogMessage.Width = 1082
        '
        'TimerAutoTrade
        '
        Me.TimerAutoTrade.Interval = 2000
        '
        'RadioButtonTradeModeSimulate
        '
        Me.RadioButtonTradeModeSimulate.AutoSize = True
        Me.RadioButtonTradeModeSimulate.Checked = True
        Me.RadioButtonTradeModeSimulate.Location = New System.Drawing.Point(466, 21)
        Me.RadioButtonTradeModeSimulate.Name = "RadioButtonTradeModeSimulate"
        Me.RadioButtonTradeModeSimulate.Size = New System.Drawing.Size(96, 17)
        Me.RadioButtonTradeModeSimulate.TabIndex = 17
        Me.RadioButtonTradeModeSimulate.TabStop = True
        Me.RadioButtonTradeModeSimulate.Text = "Simulate Trade"
        Me.RadioButtonTradeModeSimulate.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ButtonUpdateBinanceAPI)
        Me.GroupBox3.Controls.Add(Me.TextBoxAPISecret)
        Me.GroupBox3.Controls.Add(Me.Label27)
        Me.GroupBox3.Controls.Add(Me.TextBoxAPIKey)
        Me.GroupBox3.Controls.Add(Me.Label26)
        Me.GroupBox3.Location = New System.Drawing.Point(141, 39)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1124, 58)
        Me.GroupBox3.TabIndex = 57
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Binance Key"
        '
        'ButtonUpdateBinanceAPI
        '
        Me.ButtonUpdateBinanceAPI.Location = New System.Drawing.Point(986, 17)
        Me.ButtonUpdateBinanceAPI.Name = "ButtonUpdateBinanceAPI"
        Me.ButtonUpdateBinanceAPI.Size = New System.Drawing.Size(121, 23)
        Me.ButtonUpdateBinanceAPI.TabIndex = 25
        Me.ButtonUpdateBinanceAPI.Text = "Update API Value"
        Me.ButtonUpdateBinanceAPI.UseVisualStyleBackColor = True
        '
        'TextBoxAPISecret
        '
        Me.TextBoxAPISecret.Location = New System.Drawing.Point(565, 19)
        Me.TextBoxAPISecret.Name = "TextBoxAPISecret"
        Me.TextBoxAPISecret.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxAPISecret.Size = New System.Drawing.Size(402, 20)
        Me.TextBoxAPISecret.TabIndex = 24
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(506, 22)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(58, 13)
        Me.Label27.TabIndex = 23
        Me.Label27.Text = "API Secret"
        '
        'TextBoxAPIKey
        '
        Me.TextBoxAPIKey.Location = New System.Drawing.Point(74, 19)
        Me.TextBoxAPIKey.Name = "TextBoxAPIKey"
        Me.TextBoxAPIKey.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxAPIKey.Size = New System.Drawing.Size(412, 20)
        Me.TextBoxAPIKey.TabIndex = 0
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(23, 22)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(45, 13)
        Me.Label26.TabIndex = 21
        Me.Label26.Text = "API Key"
        '
        'ButtonStopTrade
        '
        Me.ButtonStopTrade.BackColor = System.Drawing.Color.Salmon
        Me.ButtonStopTrade.Location = New System.Drawing.Point(1075, 533)
        Me.ButtonStopTrade.Name = "ButtonStopTrade"
        Me.ButtonStopTrade.Size = New System.Drawing.Size(190, 23)
        Me.ButtonStopTrade.TabIndex = 61
        Me.ButtonStopTrade.Text = "Stop Trade"
        Me.ButtonStopTrade.UseVisualStyleBackColor = False
        Me.ButtonStopTrade.Visible = False
        '
        'TimerSetupEnvironment
        '
        Me.TimerSetupEnvironment.Interval = 1000
        '
        'ButtonStartTrade
        '
        Me.ButtonStartTrade.BackColor = System.Drawing.Color.Aquamarine
        Me.ButtonStartTrade.Location = New System.Drawing.Point(1075, 533)
        Me.ButtonStartTrade.Name = "ButtonStartTrade"
        Me.ButtonStartTrade.Size = New System.Drawing.Size(190, 23)
        Me.ButtonStartTrade.TabIndex = 60
        Me.ButtonStartTrade.Text = "Start Trade"
        Me.ButtonStartTrade.UseVisualStyleBackColor = False
        Me.ButtonStartTrade.Visible = False
        '
        'TextBoxBalanceAmount
        '
        Me.TextBoxBalanceAmount.BackColor = System.Drawing.SystemColors.Info
        Me.TextBoxBalanceAmount.Location = New System.Drawing.Point(1005, 19)
        Me.TextBoxBalanceAmount.Name = "TextBoxBalanceAmount"
        Me.TextBoxBalanceAmount.Size = New System.Drawing.Size(102, 20)
        Me.TextBoxBalanceAmount.TabIndex = 5
        Me.TextBoxBalanceAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(243, 50)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Sale Commission"
        '
        'ComboBoxStableCoin
        '
        Me.ComboBoxStableCoin.FormattingEnabled = True
        Me.ComboBoxStableCoin.Items.AddRange(New Object() {"THB"})
        Me.ComboBoxStableCoin.Location = New System.Drawing.Point(9, 89)
        Me.ComboBoxStableCoin.Name = "ComboBoxStableCoin"
        Me.ComboBoxStableCoin.Size = New System.Drawing.Size(124, 21)
        Me.ComboBoxStableCoin.TabIndex = 55
        '
        'ComboBoxStepAmount
        '
        Me.ComboBoxStepAmount.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxStepAmount.FormattingEnabled = True
        Me.ComboBoxStepAmount.Location = New System.Drawing.Point(117, 19)
        Me.ComboBoxStepAmount.Name = "ComboBoxStepAmount"
        Me.ComboBoxStepAmount.Size = New System.Drawing.Size(109, 21)
        Me.ComboBoxStepAmount.TabIndex = 19
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "First Buy Amount"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(914, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(85, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Balance Amount"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(466, 51)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Interval Time"
        '
        'ComboBoxIntervalTime
        '
        Me.ComboBoxIntervalTime.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxIntervalTime.FormattingEnabled = True
        Me.ComboBoxIntervalTime.Location = New System.Drawing.Point(540, 45)
        Me.ComboBoxIntervalTime.Name = "ComboBoxIntervalTime"
        Me.ComboBoxIntervalTime.Size = New System.Drawing.Size(115, 21)
        Me.ComboBoxIntervalTime.TabIndex = 12
        '
        'ComboBoxSaleCommission
        '
        Me.ComboBoxSaleCommission.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxSaleCommission.FormattingEnabled = True
        Me.ComboBoxSaleCommission.Location = New System.Drawing.Point(338, 47)
        Me.ComboBoxSaleCommission.Name = "ComboBoxSaleCommission"
        Me.ComboBoxSaleCommission.Size = New System.Drawing.Size(111, 21)
        Me.ComboBoxSaleCommission.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Take Profit (%)"
        '
        'ComboBoxTakeProfitPercent
        '
        Me.ComboBoxTakeProfitPercent.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxTakeProfitPercent.FormattingEnabled = True
        Me.ComboBoxTakeProfitPercent.Location = New System.Drawing.Point(117, 46)
        Me.ComboBoxTakeProfitPercent.Name = "ComboBoxTakeProfitPercent"
        Me.ComboBoxTakeProfitPercent.Size = New System.Drawing.Size(109, 21)
        Me.ComboBoxTakeProfitPercent.TabIndex = 6
        '
        'ComboBoxFillupPercent
        '
        Me.ComboBoxFillupPercent.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxFillupPercent.FormattingEnabled = True
        Me.ComboBoxFillupPercent.Location = New System.Drawing.Point(117, 73)
        Me.ComboBoxFillupPercent.Name = "ComboBoxFillupPercent"
        Me.ComboBoxFillupPercent.Size = New System.Drawing.Size(109, 21)
        Me.ComboBoxFillupPercent.TabIndex = 8
        '
        'ComboBoxBuyCommission
        '
        Me.ComboBoxBuyCommission.BackColor = System.Drawing.SystemColors.Info
        Me.ComboBoxBuyCommission.FormattingEnabled = True
        Me.ComboBoxBuyCommission.Location = New System.Drawing.Point(338, 20)
        Me.ComboBoxBuyCommission.Name = "ComboBoxBuyCommission"
        Me.ComboBoxBuyCommission.Size = New System.Drawing.Size(111, 21)
        Me.ComboBoxBuyCommission.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(243, 23)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(83, 13)
        Me.Label10.TabIndex = 13
        Me.Label10.Text = "Buy Commission"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBoxBuyNewOnNextWave)
        Me.GroupBox1.Controls.Add(Me.ComboBoxLimitCoinAmount)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.CheckBoxStopBuyNew)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.ComboBoxTradeStyle)
        Me.GroupBox1.Controls.Add(Me.TextBoxProfitAmount)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.TextBoxMaxBudgetUse)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.TextBoxTotalAmount)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.TextBoxCoinAmount)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.RadioButtonTradeModeReal)
        Me.GroupBox1.Controls.Add(Me.RadioButtonTradeModeSimulate)
        Me.GroupBox1.Controls.Add(Me.TextBoxBalanceAmount)
        Me.GroupBox1.Controls.Add(Me.ComboBoxStepAmount)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.ComboBoxIntervalTime)
        Me.GroupBox1.Controls.Add(Me.ComboBoxSaleCommission)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.ComboBoxTakeProfitPercent)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.ComboBoxFillupPercent)
        Me.GroupBox1.Controls.Add(Me.ComboBoxBuyCommission)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Location = New System.Drawing.Point(141, 167)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1124, 124)
        Me.GroupBox1.TabIndex = 56
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Configuration Information"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 76)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Fillup (%)"
        '
        'ToolStripLabelMessageLog
        '
        Me.ToolStripLabelMessageLog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripLabelMessageLog.BackColor = System.Drawing.SystemColors.Info
        Me.ToolStripLabelMessageLog.Name = "ToolStripLabelMessageLog"
        Me.ToolStripLabelMessageLog.Size = New System.Drawing.Size(76, 22)
        Me.ToolStripLabelMessageLog.Text = "Message Log"
        '
        'ToolStripLabelRemainTakeProfitTime
        '
        Me.ToolStripLabelRemainTakeProfitTime.BackColor = System.Drawing.Color.Salmon
        Me.ToolStripLabelRemainTakeProfitTime.Name = "ToolStripLabelRemainTakeProfitTime"
        Me.ToolStripLabelRemainTakeProfitTime.Size = New System.Drawing.Size(69, 22)
        Me.ToolStripLabelRemainTakeProfitTime.Text = "Expire Date:"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripProgressAutoTrade
        '
        Me.ToolStripProgressAutoTrade.Maximum = 5
        Me.ToolStripProgressAutoTrade.Name = "ToolStripProgressAutoTrade"
        Me.ToolStripProgressAutoTrade.Size = New System.Drawing.Size(50, 22)
        Me.ToolStripProgressAutoTrade.Step = 1
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripMain
        '
        Me.ToolStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownMenu, Me.ToolStripSeparator3, Me.ToolStripProgressAutoTrade, Me.ToolStripSeparator2, Me.ToolStripLabelRemainTakeProfitTime, Me.ToolStripLabelMessageLog})
        Me.ToolStripMain.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripMain.Name = "ToolStripMain"
        Me.ToolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStripMain.Size = New System.Drawing.Size(1273, 25)
        Me.ToolStripMain.TabIndex = 58
        Me.ToolStripMain.Text = "ToolStrip1"
        '
        'ToolStripDropDownMenu
        '
        Me.ToolStripDropDownMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DownloadLicenseMenu, Me.ToolStripSeparator4, Me.UploadLicenseKeyToolMenu})
        Me.ToolStripDropDownMenu.Image = CType(resources.GetObject("ToolStripDropDownMenu.Image"), System.Drawing.Image)
        Me.ToolStripDropDownMenu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownMenu.Name = "ToolStripDropDownMenu"
        Me.ToolStripDropDownMenu.Size = New System.Drawing.Size(29, 22)
        Me.ToolStripDropDownMenu.Text = "ToolStripDropDownButton1"
        '
        'DownloadLicenseMenu
        '
        Me.DownloadLicenseMenu.Name = "DownloadLicenseMenu"
        Me.DownloadLicenseMenu.Size = New System.Drawing.Size(192, 22)
        Me.DownloadLicenseMenu.Text = "Download License Key"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(189, 6)
        '
        'UploadLicenseKeyToolMenu
        '
        Me.UploadLicenseKeyToolMenu.Name = "UploadLicenseKeyToolMenu"
        Me.UploadLicenseKeyToolMenu.Size = New System.Drawing.Size(192, 22)
        Me.UploadLicenseKeyToolMenu.Text = "Upload License Key"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(506, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Notification"
        '
        'OpenFileDialogLicense
        '
        Me.OpenFileDialogLicense.FileName = "OpenFileDialog1"
        '
        'CheckBoxAutoRun
        '
        Me.CheckBoxAutoRun.AutoSize = True
        Me.CheckBoxAutoRun.Location = New System.Drawing.Point(12, 39)
        Me.CheckBoxAutoRun.Name = "CheckBoxAutoRun"
        Me.CheckBoxAutoRun.Size = New System.Drawing.Size(104, 17)
        Me.CheckBoxAutoRun.TabIndex = 59
        Me.CheckBoxAutoRun.Text = "Auto Start Trade"
        Me.CheckBoxAutoRun.UseVisualStyleBackColor = True
        '
        'ButtonHistoryTest
        '
        Me.ButtonHistoryTest.BackColor = System.Drawing.SystemColors.Info
        Me.ButtonHistoryTest.Location = New System.Drawing.Point(9, 533)
        Me.ButtonHistoryTest.Name = "ButtonHistoryTest"
        Me.ButtonHistoryTest.Size = New System.Drawing.Size(173, 23)
        Me.ButtonHistoryTest.TabIndex = 66
        Me.ButtonHistoryTest.Text = "History Test"
        Me.ButtonHistoryTest.UseVisualStyleBackColor = False
        Me.ButtonHistoryTest.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(732, 22)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(30, 13)
        Me.Label16.TabIndex = 31
        Me.Label16.Text = "Hour"
        '
        'ComboBoxSummaryHour
        '
        Me.ComboBoxSummaryHour.FormattingEnabled = True
        Me.ComboBoxSummaryHour.Location = New System.Drawing.Point(647, 19)
        Me.ComboBoxSummaryHour.Name = "ComboBoxSummaryHour"
        Me.ComboBoxSummaryHour.Size = New System.Drawing.Size(79, 21)
        Me.ComboBoxSummaryHour.TabIndex = 30
        '
        'CheckBoxSendLineSellCoin
        '
        Me.CheckBoxSendLineSellCoin.AutoSize = True
        Me.CheckBoxSendLineSellCoin.Location = New System.Drawing.Point(900, 21)
        Me.CheckBoxSendLineSellCoin.Name = "CheckBoxSendLineSellCoin"
        Me.CheckBoxSendLineSellCoin.Size = New System.Drawing.Size(67, 17)
        Me.CheckBoxSendLineSellCoin.TabIndex = 28
        Me.CheckBoxSendLineSellCoin.Text = "Sell Coin"
        Me.CheckBoxSendLineSellCoin.UseVisualStyleBackColor = True
        '
        'CheckBoxSendLineBuyCoin
        '
        Me.CheckBoxSendLineBuyCoin.AutoSize = True
        Me.CheckBoxSendLineBuyCoin.Location = New System.Drawing.Point(826, 21)
        Me.CheckBoxSendLineBuyCoin.Name = "CheckBoxSendLineBuyCoin"
        Me.CheckBoxSendLineBuyCoin.Size = New System.Drawing.Size(68, 17)
        Me.CheckBoxSendLineBuyCoin.TabIndex = 27
        Me.CheckBoxSendLineBuyCoin.Text = "Buy Coin"
        Me.CheckBoxSendLineBuyCoin.UseVisualStyleBackColor = True
        '
        'CheckBoxSendLineSummary
        '
        Me.CheckBoxSendLineSummary.AutoSize = True
        Me.CheckBoxSendLineSummary.Location = New System.Drawing.Point(572, 21)
        Me.CheckBoxSendLineSummary.Name = "CheckBoxSendLineSummary"
        Me.CheckBoxSendLineSummary.Size = New System.Drawing.Size(69, 17)
        Me.CheckBoxSendLineSummary.TabIndex = 26
        Me.CheckBoxSendLineSummary.Text = "Summary"
        Me.CheckBoxSendLineSummary.UseVisualStyleBackColor = True
        '
        'ButtonUpdateLineToken
        '
        Me.ButtonUpdateLineToken.Location = New System.Drawing.Point(986, 17)
        Me.ButtonUpdateLineToken.Name = "ButtonUpdateLineToken"
        Me.ButtonUpdateLineToken.Size = New System.Drawing.Size(121, 23)
        Me.ButtonUpdateLineToken.TabIndex = 25
        Me.ButtonUpdateLineToken.Text = "Update Link Token"
        Me.ButtonUpdateLineToken.UseVisualStyleBackColor = True
        '
        'TextBoxLineToken
        '
        Me.TextBoxLineToken.Location = New System.Drawing.Point(74, 19)
        Me.TextBoxLineToken.Name = "TextBoxLineToken"
        Me.TextBoxLineToken.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxLineToken.Size = New System.Drawing.Size(412, 20)
        Me.TextBoxLineToken.TabIndex = 0
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(23, 22)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(38, 13)
        Me.Label29.TabIndex = 21
        Me.Label29.Text = "Token"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.ComboBoxSummaryHour)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.CheckBoxSendLineSellCoin)
        Me.GroupBox2.Controls.Add(Me.CheckBoxSendLineBuyCoin)
        Me.GroupBox2.Controls.Add(Me.CheckBoxSendLineSummary)
        Me.GroupBox2.Controls.Add(Me.ButtonUpdateLineToken)
        Me.GroupBox2.Controls.Add(Me.TextBoxLineToken)
        Me.GroupBox2.Controls.Add(Me.Label29)
        Me.GroupBox2.Location = New System.Drawing.Point(141, 103)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1124, 58)
        Me.GroupBox2.TabIndex = 62
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Line Notification"
        '
        'FrmSpotTrade
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1273, 562)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CheckedListBoxTradeCoin)
        Me.Controls.Add(Me.TabControlTrade)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.ButtonStopTrade)
        Me.Controls.Add(Me.ButtonStartTrade)
        Me.Controls.Add(Me.ComboBoxStableCoin)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStripMain)
        Me.Controls.Add(Me.CheckBoxAutoRun)
        Me.Controls.Add(Me.ButtonHistoryTest)
        Me.Controls.Add(Me.GroupBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmSpotTrade"
        Me.Text = "BItkub Spot Trade"
        Me.ContextMenuNotification.ResumeLayout(False)
        Me.TabSummaryProfit.ResumeLayout(False)
        Me.TabControlTrade.ResumeLayout(False)
        Me.TabCryptoTrade.ResumeLayout(False)
        Me.TabOrder.ResumeLayout(False)
        Me.TabErrorLog.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ToolStripMain.ResumeLayout(False)
        Me.ToolStripMain.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CheckBoxBuyNewOnNextWave As CheckBox
    Friend WithEvents ComboBoxLimitCoinAmount As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents CheckBoxStopBuyNew As CheckBox
    Friend WithEvents Label15 As Label
    Friend WithEvents ComboBoxTradeStyle As ComboBox
    Friend WithEvents TextBoxProfitAmount As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents TextBoxMaxBudgetUse As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents TextBoxTotalAmount As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBoxCoinAmount As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents RadioButtonTradeModeReal As RadioButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents DisplayProgramMenu As ToolStripMenuItem
    Friend WithEvents ContextMenuNotification As ContextMenuStrip
    Friend WithEvents ExitProgramMenu As ToolStripMenuItem
    Private WithEvents NotifyIconAutoTrade As NotifyIcon
    Friend WithEvents Label1 As Label
    Friend WithEvents CheckedListBoxTradeCoin As CheckedListBox
    Friend WithEvents TabSummaryProfit As TabPage
    Friend WithEvents ListViewSummaryProfit As ListView
    Friend WithEvents ColSummaryDate As ColumnHeader
    Friend WithEvents ColSummaryCount As ColumnHeader
    Friend WithEvents ColSummaryAmount As ColumnHeader
    Friend WithEvents TabControlTrade As TabControl
    Friend WithEvents TabCryptoTrade As TabPage
    Friend WithEvents ListViewCrypto As ListView
    Friend WithEvents ColCryptoName As ColumnHeader
    Friend WithEvents ColCryptoDateTime As ColumnHeader
    Friend WithEvents ColCryptoShortTrend As ColumnHeader
    Friend WithEvents ColCryptoLongTrend As ColumnHeader
    Friend WithEvents ColCryptoDiffProfitPercent As ColumnHeader
    Friend WithEvents ColCryptoDiffFillupPercent As ColumnHeader
    Friend WithEvents ColCryptoAmount As ColumnHeader
    Friend WithEvents ColCryptoVolume As ColumnHeader
    Friend WithEvents ColCryptoPrice As ColumnHeader
    Friend WithEvents ColCryptoSellPrice As ColumnHeader
    Friend WithEvents ColCryptoBidPrice As ColumnHeader
    Friend WithEvents ColCryptoFillup As ColumnHeader
    Friend WithEvents ColCryptoAskPrice As ColumnHeader
    Friend WithEvents ColCryptoFillupAmount As ColumnHeader
    Friend WithEvents TabOrder As TabPage
    Friend WithEvents ListViewTrade As ListView
    Friend WithEvents ColTradeDateTime As ColumnHeader
    Friend WithEvents ColTradeCoin As ColumnHeader
    Friend WithEvents ColTradeProcess As ColumnHeader
    Friend WithEvents ColOrderProfitAmount As ColumnHeader
    Friend WithEvents ColOrderProfitPercent As ColumnHeader
    Friend WithEvents ColTradeVolume As ColumnHeader
    Friend WithEvents ColTradeCoinPrice As ColumnHeader
    Friend WithEvents ColTradeAmount As ColumnHeader
    Friend WithEvents TabErrorLog As TabPage
    Friend WithEvents ListViewErrorLog As ListView
    Friend WithEvents ColErrorDateTime As ColumnHeader
    Friend WithEvents ColErrorLogMessage As ColumnHeader
    Friend WithEvents TimerAutoTrade As Timer
    Friend WithEvents RadioButtonTradeModeSimulate As RadioButton
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ButtonUpdateBinanceAPI As Button
    Friend WithEvents TextBoxAPISecret As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents TextBoxAPIKey As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents ButtonStopTrade As Button
    Friend WithEvents TimerSetupEnvironment As Timer
    Friend WithEvents ButtonStartTrade As Button
    Friend WithEvents TextBoxBalanceAmount As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents ComboBoxStableCoin As ComboBox
    Friend WithEvents ComboBoxStepAmount As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents ComboBoxIntervalTime As ComboBox
    Friend WithEvents ComboBoxSaleCommission As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBoxTakeProfitPercent As ComboBox
    Friend WithEvents ComboBoxFillupPercent As ComboBox
    Friend WithEvents ComboBoxBuyCommission As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ToolStripLabelMessageLog As ToolStripLabel
    Friend WithEvents ToolStripLabelRemainTakeProfitTime As ToolStripLabel
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripProgressAutoTrade As ToolStripProgressBar
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripMain As ToolStrip
    Friend WithEvents ToolStripDropDownMenu As ToolStripDropDownButton
    Friend WithEvents DownloadLicenseMenu As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents UploadLicenseKeyToolMenu As ToolStripMenuItem
    Friend WithEvents Label3 As Label
    Friend WithEvents OpenFileDialogLicense As OpenFileDialog
    Friend WithEvents CheckBoxAutoRun As CheckBox
    Friend WithEvents SaveFileDialogLicense As SaveFileDialog
    Friend WithEvents ButtonHistoryTest As Button
    Friend WithEvents Label16 As Label
    Friend WithEvents ComboBoxSummaryHour As ComboBox
    Friend WithEvents CheckBoxSendLineSellCoin As CheckBox
    Friend WithEvents CheckBoxSendLineBuyCoin As CheckBox
    Friend WithEvents CheckBoxSendLineSummary As CheckBox
    Friend WithEvents ButtonUpdateLineToken As Button
    Friend WithEvents TextBoxLineToken As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents GroupBox2 As GroupBox
End Class
