Imports System.Math
Imports System.Drawing
Imports MeteorUiLib.Native
Imports MeteorUiLib.Design
Imports System.Drawing.Text
Imports System.Windows.Forms
Imports System.ComponentModel
Imports MeteorUiLib.MeteorThemeManager
Imports System.Runtime.InteropServices

Public Class MeteorForm : Inherits Form
#Region "变量"
    '//三段式标题栏
    ''' <summary>
    ''' 主标题栏
    ''' </summary>
    ''' <remarks>图标和按钮一般绘制在这里</remarks>
    Private RectCaptionMain As Rectangle
    ''' <summary>
    ''' 次标题栏
    ''' </summary>
    ''' <remarks>放在顶部且高度较小，一般不在其上绘制窗体元素</remarks>
    Private RectCaptionSub As Rectangle
    ''' <summary>
    ''' 扩展标题栏
    ''' </summary>
    ''' <remarks>用于绘制额外的信息或按钮，在主标题栏下方</remarks>
    Private RectCaptionExtended As Rectangle
    ''' <summary>
    ''' 总标题栏区域
    ''' </summary>
    ''' <remarks>用于鼠标命中测试</remarks>
    Private RectCaption As Rectangle

    '//按钮、标题、图标以及大小拖动边框
    Private RectIcon As Rectangle
    Private RectText As Rectangle
    Private RectTextDraw As Rectangle
    Private RectMinButton As Rectangle
    Private RectMaxButton As Rectangle
    Private RectCloseButton As Rectangle
    Private RectMinBtnHT As Rectangle
    Private RectMaxBtnHT As Rectangle
    Private RectCloseBtnHT As Rectangle
    Private RectTop As Rectangle
    Private RectBottom As Rectangle
    Private RectLeft As Rectangle
    Private RectRight As Rectangle
    Private RectTopLeft As Rectangle
    Private RectTopRight As Rectangle
    Private RectBottomLeft As Rectangle
    Private RectBottomRight As Rectangle
    Private RectSizeGrip As Rectangle
    '//局部更新
    Private RectMinMaxBtnArea As Rectangle
    Private RectTextIconArea As Rectangle
    Private RectBorder As Rectangle

    ''' <summary>
    ''' 主标题栏高度
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly HCAPTIONMAIN As Byte = 30
    ''' <summary>
    ''' 窗口图标大小
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly ICONSIZE As Byte = 24
    ''' <summary>
    ''' 窗口图标水平偏移
    ''' </summary>
    ''' <remarks>用于与菜单栏对齐</remarks>
    Private Shared ReadOnly ICONOFFSETX As Byte = 4
    ''' <summary>
    ''' 窗口图标垂直偏移
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly ICONOFFSETY As Byte = (HCAPTIONMAIN - ICONSIZE) / 2
    ''' <summary>
    ''' 标题栏按钮宽度
    ''' </summary>
    ''' <remarks>最小化、最大化和关闭按钮</remarks>
    Private Shared ReadOnly WSYSBTN As Byte = 36
    ''' <summary>
    ''' 标题栏按钮高度
    ''' </summary>
    ''' <remarks>最小化、最大化和关闭按钮</remarks>
    Private Shared ReadOnly HSYSBTN As Byte = 30
    ''' <summary>
    ''' 大小调整手柄
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly SIZEGRIPSIZE As Byte = 16
    ''' <summary>
    ''' 绘制Grid样式的SizeGrip
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly GRIDSIZE As Byte = 2
    ''' <summary>
    ''' 窗体边框
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly WBORDER As Byte = 1
    ''' <summary>
    ''' 边框命中范围
    ''' </summary>
    ''' <remarks>窗口边框大小调整</remarks>
    Private Shared ReadOnly NCHTSIZE As Byte = 4
    ''' <summary>
    ''' 九分之一按钮宽度
    ''' </summary>
    ''' <remarks>用于界面绘制</remarks>
    Private Shared ReadOnly WSYSBTN_ONENINTH As Single = WSYSBTN / 9.0F
    ''' <summary>
    ''' 九分之一按钮高度
    ''' </summary>
    ''' <remarks>用于界面绘制</remarks>
    Private Shared ReadOnly HSYSBTN_ONENINTH As Single = HSYSBTN / 9.0F

    ''' <summary>
    ''' 鼠标命中测试
    ''' </summary>
    ''' <remarks></remarks>
    Private mNCPoint As Point
    Private mMouseState As MouseStateEnum
    Private mControlBox As Boolean
    Private mSizeBox As Boolean
    Private mShowIcon As Boolean
    Private mMinimizeBox As Boolean
    Private mMaximizeBox As Boolean
    Private mSizeGrip As Boolean
    Private mTextAlign As TextAlignEnum
    Private mTextFormat() As TextFormatFlags
    Private mFormStyle As MetroFormStyle
    Private mMinimumSize As Size
    Private mMinSizeReserve As Size
    Private mMaximumSize As Size
    Private ptMax As Point
    ''' <summary>
    ''' 还原按钮右上角
    ''' </summary>
    ''' <remarks></remarks>
    Private mRestoreBtnPoints() As Point
    ''' <summary>
    ''' 窗口标题文字宽度
    ''' </summary>
    ''' <remarks></remarks>
    Private mTextWidth As Integer
    Private mDrawBorder As Boolean
    Private mSizeGripStyle As SizeGripStyleEnum
    Private RectGrids() As Rectangle
    Private mMdiBackColor As Color

    Private mBorderPen As Pen
    Private mSubCaptionBrush As Brush
    Private mMainCaptionBrush As Brush
    Private mExtendedCaptionBrush As Brush
    Private mBtnHoverBrush As Brush
    Private mBtnPressBrush As Brush
    Private mTextPen As Pen
    Private mTextPen2 As Pen
    Private mUseThemeManager As Boolean
#End Region

#Region "枚举"
    <Flags> _
    Private Enum MouseStateEnum As Byte
        None = 0
        MinHover = 1
        MinPress = 2
        MINFLAG = MinHover Or MinPress
        MaxHover = 4
        MaxPress = 8
        MAXFLAG = MaxHover Or MaxPress
        CloseHover = 16
        ClosePress = 32
        CLOSEFLAG = CloseHover Or ClosePress
    End Enum

    Public Enum TextAlignEnum As Byte
        Left = 0
        Center = 1
    End Enum

    Public Enum SizeGripStyleEnum As Byte
        None = 0
        Grid = 1
    End Enum
#End Region

#Region "构造函数"
    Public Sub New()
        MyBase.New()
        MyBase.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        MyBase.AutoScaleMode = Windows.Forms.AutoScaleMode.None
        MyBase.StartPosition = FormStartPosition.CenterScreen
        MyBase.SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.CacheText Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.ResizeRedraw, True)

        InitParams()
        InitDrawing()
        SetupForm()
    End Sub
#End Region

#Region "隐藏"
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property AutoScroll As Boolean
        Get
            Return MyBase.AutoScroll
        End Get
        Set(value As Boolean)
            MyBase.AutoScroll = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property AutoScrollMargin As Size
        Get
            Return MyBase.AutoScrollMargin
        End Get
        Set(value As Size)
            MyBase.AutoScrollMargin = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property AutoScrollMinSize As Size
        Get
            Return MyBase.AutoScrollMinSize
        End Get
        Set(value As Size)
            MyBase.AutoScrollMinSize = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property AutoSize As Boolean
        Get
            Return MyBase.AutoSize
        End Get
        Set(value As Boolean)
            MyBase.AutoSize = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event AutoSizeChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property AutoSizeMode As AutoSizeMode
        Get
            Return MyBase.AutoSizeMode
        End Get
        Set(value As AutoSizeMode)
            MyBase.AutoSizeMode = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property FormBorderStyle As FormBorderStyle
        Get
            Return MyBase.FormBorderStyle
        End Get
        Set(value As FormBorderStyle)
            MyBase.FormBorderStyle = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property HelpButton As Boolean
        Get
            Return MyBase.HelpButton
        End Get
        Set(value As Boolean)
            MyBase.HelpButton = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event HelpButtonClicked As CancelEventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event MaximumSizeChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event MinimumSizeChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property RightToLeft As RightToLeft
        Get
            Return MyBase.RightToLeft
        End Get
        Set(value As RightToLeft)
            MyBase.RightToLeft = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property RightToLeftLayout As Boolean
        Get
            Return MyBase.RightToLeftLayout
        End Get
        Set(value As Boolean)
            MyBase.RightToLeftLayout = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event RightToLeftLayoutChanged As EventHandler
#End Region

#Region "属性"
    <Browsable(False)> _
    Public Overloads ReadOnly Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
    End Property

    <Browsable(False)> _
    Public Overloads ReadOnly Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
    End Property

    <DefaultValue(True)> _
    Public Overloads Property ControlBox As Boolean
        Get
            Return mControlBox
        End Get
        Set(value As Boolean)
            If mControlBox <> value Then
                mControlBox = value
                SetUpTextArea()
                SetUpTextDraw()
                Me.Invalidate(RectCaptionMain)
            End If
        End Set
    End Property

    <DefaultValue(True), Description("指示窗口是否绘制边框。注意：当绘制边框且窗口不透明度<100%时，改变窗口大小会产生较严重的拖影。")> _
    Public Property DrawBorder As Boolean
        Get
            Return mDrawBorder
        End Get
        Set(value As Boolean)
            If mDrawBorder <> value Then
                mDrawBorder = value
                Me.Invalidate()
            End If
        End Set
    End Property

    <DefaultValue(True), Description("当此属性为True时，控件的颜色样式由ThemeManager控制。")> _
    Public Property UseThemeManager As Boolean
        Get
            Return mUseThemeManager
        End Get
        Set(value As Boolean)
            If mUseThemeManager <> value Then
                mUseThemeManager = value
                InitDrawing()
                Me.Invalidate()
            End If
        End Set
    End Property

    <DefaultValue(True)> _
    Public Overloads Property MaximizeBox As Boolean
        Get
            Return mMaximizeBox
        End Get
        Set(value As Boolean)
            If mMaximizeBox <> value Then
                mMaximizeBox = value
                SetUpCaptionButton()
                Me.Invalidate(RectMinMaxBtnArea)
            End If
        End Set
    End Property

    <DefaultValue(GetType(Size), "0,0")> _
    Public Overloads Property MaximumSize As Size
        Get
            Return mMaximumSize
        End Get
        Set(value As Size)
            If mMaximumSize <> value Then
                If value.IsEmpty Then
                    mMaximumSize = Size.Empty
                    ptMax = Point.Empty
                Else
                    If mMinimumSize.IsEmpty Then
                        mMaximumSize = New Size(Min(Max(value.Width, mMinSizeReserve.Width), SystemInformation.WorkingArea.Width), _
                                                Min(Max(value.Height, mMinSizeReserve.Height), SystemInformation.WorkingArea.Height))
                    Else
                        mMaximumSize = New Size(Min(Max(value.Width, mMinimumSize.Width), SystemInformation.WorkingArea.Width), _
                                                Min(Max(value.Height, mMinimumSize.Height), SystemInformation.WorkingArea.Height))
                    End If
                    If mMaximumSize = SystemInformation.WorkingArea.Size Then
                        mMaximumSize = Size.Empty
                        ptMax = Point.Empty
                    Else
                        ptMax = New Point((SystemInformation.WorkingArea.Width - mMaximumSize.Width) / 2.0F, (SystemInformation.WorkingArea.Height - mMaximumSize.Height) / 2.0F)
                        Me.Size = New Size(Min(Size.Width, mMaximumSize.Width), Min(Size.Height, mMaximumSize.Height))
                    End If
                End If
            End If
        End Set
    End Property

    <DefaultValue(True)> _
    Public Overloads Property MinimizeBox As Boolean
        Get
            Return mMinimizeBox
        End Get
        Set(value As Boolean)
            If mMinimizeBox <> value Then
                mMinimizeBox = value
                Me.Invalidate(RectMinButton)
            End If
        End Set
    End Property

    <DefaultValue(GetType(Size), "0,0")> _
    Public Overloads Property MinimumSize As Size
        Get
            Return mMinimumSize
        End Get
        Set(value As Size)
            If mMinimumSize <> value Then
                If value.IsEmpty Then
                    mMinimumSize = Size.Empty
                Else
                    If mMaximumSize.IsEmpty Then
                        mMinimumSize = New Size(Min(Max(value.Width, mMinSizeReserve.Width), SystemInformation.WorkingArea.Width), _
                                                Min(Max(value.Height, mMinSizeReserve.Height), SystemInformation.WorkingArea.Height))
                    Else
                        mMinimumSize = New Size(Min(Max(value.Width, mMinSizeReserve.Width), mMaximumSize.Width), _
                                                Min(Max(value.Height, mMinSizeReserve.Height), mMaximumSize.Height))
                    End If
                    If mMinimumSize = mMinSizeReserve Then
                        mMinimumSize = Size.Empty
                    Else
                        Me.Size = New Size(Max(Size.Width, mMinimumSize.Width), Max(Size.Height, mMinimumSize.Height))
                    End If
                End If
            End If
        End Set
    End Property

    <Description("设置窗口的MdiClient背景颜色。")> _
    Public Property MdiBackColor As Color
        Get
            Return mMdiBackColor
        End Get
        Set(value As Color)
            mMdiBackColor = value
            If Me.IsMdiContainer Then
                For i = 0 To Me.Controls.Count - 1
                    If Me.Controls(i).GetType = GetType(MdiClient) Then
                        Me.Controls(i).BackColor = mMdiBackColor
                        Return
                    End If
                Next
            End If
        End Set
    End Property

    <DefaultValue(True)> _
    Public Overloads Property ShowIcon As Boolean
        Get
            Return mShowIcon
        End Get
        Set(value As Boolean)
            If mShowIcon <> value Then
                mShowIcon = value
                SetUpTextArea()
                SetUpTextDraw()
                Me.Invalidate(RectTextIconArea)
            End If
        End Set
    End Property

    <DefaultValue(True), Description("指示窗口是否具有可调整大小的边框。")> _
    Public Property SizeBox As Boolean
        Get
            Return mSizeBox
        End Get
        Set(value As Boolean)
            mSizeBox = value
        End Set
    End Property

    <DefaultValue(False), Description("指示窗口右下角是否具有可以调整大小的SizeGrip。")> _
    Public Property SizeGrip As Boolean
        Get
            Return mSizeGrip
        End Get
        Set(value As Boolean)
            If mSizeGrip <> value Then
                mSizeGrip = value
            End If
        End Set
    End Property

    <DefaultValue(0), Description("设置SizeGrip的样式。")> _
    Public Overloads Property SizeGripStyle As SizeGripStyleEnum
        Get
            Return mSizeGripStyle
        End Get
        Set(value As SizeGripStyleEnum)
            If mSizeGripStyle <> value Then
                mSizeGripStyle = value
                If mSizeGrip Then
                    Me.Invalidate(RectSizeGrip)
                End If
            End If
        End Set
    End Property

    <DefaultValue(0), Description("设置窗口标题的对齐方式。")> _
    Public Property TextAlign As TextAlignEnum
        Get
            Return mTextAlign
        End Get
        Set(value As TextAlignEnum)
            If mTextAlign <> value Then
                mTextAlign = value
                SetUpTextDraw()
                Me.Invalidate(RectText)
            End If
        End Set
    End Property

    <DefaultValue(GetType(MetroFormStyle), "MetroLight"), Description("设置窗口的外观样式。")> _
    Public Property FormStyle As MetroFormStyle
        Get
            Return mFormStyle
        End Get
        Set(value As MetroFormStyle)
            If mFormStyle <> value Then
                mFormStyle = value
                SetupForm()
                InitDrawing()
                Me.Invalidate()
            End If
        End Set
    End Property

    Public Overloads Property WindowState As FormWindowState
        Get
            Return MyBase.WindowState
        End Get
        Set(value As FormWindowState)
            If (MyBase.WindowState Or value) = (FormWindowState.Maximized Or FormWindowState.Normal) Then
                If mControlBox AndAlso mMaximizeBox Then
                    Me.Invalidate(RectMaxButton)
                End If
            End If
            MyBase.WindowState = value
        End Set
    End Property
#End Region

#Region "方法重写"
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim mCP = MyBase.CreateParams
            If Not DesignMode Then
                mCP.Style = mCP.Style Or WS_CAPTION Or WS_THICKFRAME Or WS_MINIMIZEBOX
            End If
            Return mCP
        End Get
    End Property

    Protected Overrides ReadOnly Property DefaultPadding As Padding
        Get
            Return New Padding(WBORDER, mFormStyle.SubCaptionHeight + HCAPTIONMAIN, WBORDER, WBORDER)
        End Get
    End Property

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                WmNCHitTest(m)

            Case WM_NCLBUTTONDOWN
                WmNCLButtonDown(m)

            Case WM_NCLBUTTONUP
                WmNCLButtonUp(m)

            Case WM_NCMOUSELEAVE
                SetMouseState(MouseStateEnum.None)
                MyBase.WndProc(m)

            Case WM_NCCALCSIZE
                WmNCCalcSize(m)

            Case WM_WINDOWPOSCHANGED
                WmWindowPosChanged(m)

            Case WM_GETMINMAXINFO
                WmGetMinMaxInfo(m)

            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        SetUpTextDraw()
        Me.Invalidate(RectText)
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        SetUpTextDraw()
        MyBase.OnFontChanged(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        SetupForm()
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        '//覆盖MDI容器背景颜色
        '//参见.NET Source @Form.cs/OnPaint
        If IsMdiContainer Then
            e.Graphics.Clear(MyBase.BackColor)
        End If
        DrawForm(e.Graphics, e.ClipRectangle)
    End Sub
#End Region

#Region "私有方法"
    Private Sub WmNCHitTest(ByRef m As Message)
        mNCPoint = PointToClient(MousePosition)

        '//标题栏按钮
        '//不返回按钮对应的HT常数是为了防止系统画蛇添足的绘制按钮
        If mControlBox Then
            If mMinimizeBox AndAlso RectMinBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MinHover)
                m.Result = HTCAPTION
                Return
            End If
            If mMaximizeBox AndAlso RectMaxBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MaxHover)
                m.Result = HTCAPTION
                Return
            End If
            If RectCloseBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.CloseHover)
                m.Result = HTCAPTION
                Return
            End If
        End If
        SetMouseState(MouseStateEnum.None)

        '//可调整大小边框
        '//最大化时很明显不应允许拖动边框调整大小
        If mSizeBox AndAlso Me.WindowState <> FormWindowState.Maximized Then
            If RectTop.Contains(mNCPoint) Then
                m.Result = HTTOP
                Return
            End If
            If RectBottom.Contains(mNCPoint) Then
                m.Result = HTBOTTOM
                Return
            End If
            If RectLeft.Contains(mNCPoint) Then
                m.Result = HTLEFT
                Return
            End If
            If RectRight.Contains(mNCPoint) Then
                m.Result = HTRIGHT
                Return
            End If
            If RectTopLeft.Contains(mNCPoint) Then
                m.Result = HTTOPLEFT
                Return
            End If
            If RectTopRight.Contains(mNCPoint) Then
                m.Result = HTTOPRIGHT
                Return
            End If
            If RectBottomLeft.Contains(mNCPoint) Then
                m.Result = HTBOTTOMLEFT
                Return
            End If
            If RectBottomRight.Contains(mNCPoint) Then
                m.Result = HTBOTTOMRIGHT
                Return
            End If
        End If
        If mSizeGrip AndAlso Me.WindowState <> FormWindowState.Maximized AndAlso RectSizeGrip.Contains(mNCPoint) Then
            m.Result = HTBOTTOMRIGHT
            Return
        End If

        '//标题栏
        If RectCaption.Contains(mNCPoint) Then
            m.Result = HTCAPTION
            Return
        End If

        '//可以直接返回HTCLIENT
        '//保险起见牺牲一点点性能
        DefWndProc(m)
    End Sub

    Private Sub WmNCCalcSize(ByRef m As Message)
        If m.WParam <> IntPtr.Zero Then
            Dim ncparams As New NCCALCSIZE_PARAMS
            ncparams = Marshal.PtrToStructure(m.LParam, GetType(NCCALCSIZE_PARAMS))
            '屏幕坐标
            'S:新窗口矩形||原窗口矩形||原客户矩形
            'R:新客户矩形||新窗口矩形||原窗口矩形
            Dim rcNew = ncparams.recs(0)
            '全屏最大化窗口客户区对齐
            If rcNew.Left < 0 AndAlso rcNew.Right - rcNew.Left >= SystemInformation.WorkingArea.Width AndAlso rcNew.Top < 0 AndAlso rcNew.Bottom - rcNew.Top >= SystemInformation.WorkingArea.Height Then
                With ncparams.recs(0)
                    .Left = 0
                    .Top = 0
                    .Right = SystemInformation.WorkingArea.Width
                    .Bottom = SystemInformation.WorkingArea.Height
                End With
                '隐藏任务栏时减少1像素以免影响任务栏呼出
                If rcNew.Bottom >= SystemInformation.VirtualScreen.Height Then
                    ncparams.recs(0).Bottom -= 1
                End If
            End If
            Marshal.StructureToPtr(ncparams, m.LParam, True)
        End If
        m.Result = 0
    End Sub

    Private Sub WmNCLButtonDown(ByRef m As Message)
        mNCPoint = PointToClient(MousePosition)
        If mControlBox Then
            If mMinimizeBox AndAlso RectMinBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MinPress)
                Return
            End If
            If mMaximizeBox AndAlso RectMaxBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MaxPress)
                Return
            End If
            If RectCloseBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.ClosePress)
                Return
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub WmNCLButtonUp(ByRef m As Message)
        mNCPoint = PointToClient(MousePosition)
        If mControlBox Then
            If mMinimizeBox AndAlso RectMinBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MinHover)
                SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MINIMIZE, 0)
                Return
            End If
            If mMaximizeBox AndAlso RectMaxBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.MinHover)
                If Me.WindowState = FormWindowState.Maximized Then
                    SendMessage(Me.Handle, WM_SYSCOMMAND, SC_RESTORE, 0)
                Else
                    SendMessage(Me.Handle, WM_SYSCOMMAND, SC_MAXIMIZE, 0)
                End If
                Return
            End If
            If RectCloseBtnHT.Contains(mNCPoint) Then
                SetMouseState(MouseStateEnum.CloseHover)
                SendMessage(Me.Handle, WM_SYSCOMMAND, SC_CLOSE, 0)
                Return
            End If
        End If
        SetMouseState(MouseStateEnum.None)
        MyBase.WndProc(m)
    End Sub

    Private Sub WmWindowPosChanged(ByRef m As Message)
        Dim wndplm As New WINDOWPLACEMENT
        wndplm.length = Marshal.SizeOf(wndplm)
        GetWindowPlacement(Me.Handle, wndplm)
        Select Case wndplm.showCmd
            Case SW_NORMAL, SW_RESTORE, SW_SHOW, SW_SHOWNA, SW_SHOWNOACTIVATE
                If Me.WindowState <> FormWindowState.Normal Then
                    Me.WindowState = FormWindowState.Normal
                End If
            Case SW_SHOWMAXIMIZED
                Me.WindowState = FormWindowState.Maximized
            Case SW_MINIMIZE, SW_SHOWMINIMIZED, SW_SHOWMINNOACTIVE
                Me.WindowState = FormWindowState.Minimized
        End Select
        UpdateBounds()
        DefWndProc(m)
    End Sub

    Private Sub WmGetMinMaxInfo(ByRef m As Message)
        Dim minmaxinfo As New MINMAXINFO
        minmaxinfo = Marshal.PtrToStructure(m.LParam, GetType(MINMAXINFO))
        With minmaxinfo
            .ptMaxSize = IIf(mMaximumSize.IsEmpty, SystemInformation.WorkingArea.Size, mMaximumSize)
            .ptMaxPosition = ptMax
            .ptMaxTrackSize = IIf(mMaximumSize.IsEmpty, SystemInformation.WorkingArea.Size, mMaximumSize)
            .ptMinTrackSize = IIf(mMinimumSize.IsEmpty, mMinSizeReserve, mMinimumSize)
        End With
        Marshal.StructureToPtr(minmaxinfo, m.LParam, True)
        m.Result = 0
    End Sub

    Private Sub SetMouseState(ByVal value As MouseStateEnum)
        If mMouseState <> value Then
            Dim flag = mMouseState Or value
            mMouseState = value
            If flag And MouseStateEnum.MINFLAG Then
                Me.Invalidate(RectMinButton)
            End If
            If flag And MouseStateEnum.MAXFLAG Then
                Me.Invalidate(RectMaxButton)
            End If
            If flag And MouseStateEnum.CLOSEFLAG Then
                Me.Invalidate(RectCloseButton)
            End If
        End If
    End Sub

    Private Sub InitParams()
        mDrawBorder = True
        mControlBox = True
        '//ToDo:正式版应改为true
        mUseThemeManager = False
        mSizeBox = True
        mShowIcon = True
        mMaximizeBox = True
        mMinimizeBox = True
        mSizeGrip = False
        mSizeGripStyle = SizeGripStyleEnum.None
        ReDim RectGrids(9)
        mTextWidth = 0
        mMouseState = MouseStateEnum.None
        mTextAlign = TextAlignEnum.Left
        mFormStyle = New MetroFormStyle()
        ReDim mRestoreBtnPoints(4)
        ReDim mTextFormat(1)
        mTextFormat(0) = TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis
        mTextFormat(1) = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis
        mMinimumSize = Size.Empty
        mMaximumSize = Size.Empty
        ptMax = Point.Empty
        RectTopLeft = New Rectangle(0, 0, NCHTSIZE, NCHTSIZE)
        mMdiBackColor = SystemColors.AppWorkspace
    End Sub

    Private Sub InitDrawing()
        If mUseThemeManager Then

        Else
            mBorderPen = New Pen(mFormStyle.BorderColor)
            mSubCaptionBrush = New SolidBrush(mFormStyle.SubCaptionColor)
            mMainCaptionBrush = New SolidBrush(mFormStyle.MainCaptionColor)
            mExtendedCaptionBrush = New SolidBrush(mFormStyle.ExtendedCaptionColor)
            mBtnHoverBrush = New SolidBrush(mFormStyle.ButtonHoverColor)
            mBtnPressBrush = New SolidBrush(mFormStyle.ButtonPressColor)
            mTextPen = New Pen(mFormStyle.ForeColor)
            mTextPen2 = New Pen(mFormStyle.ForeColor, 2.0F)
            MyBase.BackColor = mFormStyle.BackColor
            MyBase.ForeColor = mFormStyle.ForeColor
        End If
    End Sub

    Private Sub SetupForm()
        SetUpCaption()
        SetUpCaptionButton()
        SetUpTextArea()
        SetUpTextDraw()
        SetUpBorderHT()
        mMinSizeReserve = New Size(ICONSIZE + ICONOFFSETX * 2 + WSYSBTN * 3, mFormStyle.SubCaptionHeight + HCAPTIONMAIN)
        RectBorder = New Rectangle(0, 0, Me.ClientSize.Width - WBORDER, Me.ClientSize.Height - WBORDER)
    End Sub

    Private Sub SetUpCaption()
        RectCaptionSub = New Rectangle(0, 0, Me.ClientSize.Width, mFormStyle.SubCaptionHeight)
        RectCaptionMain = New Rectangle(0, RectCaptionSub.Height, Me.ClientSize.Width, HCAPTIONMAIN)
        RectCaptionExtended = New Rectangle(0, RectCaptionMain.Y + RectCaptionMain.Height, Me.ClientSize.Width, mFormStyle.ExtendedCaptionHeight)
        RectCaption = New Rectangle(0, 0, Me.ClientSize.Width, RectCaptionExtended.Y + RectCaptionExtended.Height)
        RectIcon = New Rectangle(ICONOFFSETX, RectCaptionMain.Y + ICONOFFSETY, ICONSIZE, ICONSIZE)
    End Sub

    Private Sub SetUpTextArea()
        RectTextIconArea = New Rectangle(ICONOFFSETX, RectCaptionMain.Y, RectCaptionMain.Width - ICONOFFSETX - WSYSBTN * 3, RectCaptionMain.Height)
        If mControlBox AndAlso mShowIcon Then
            RectText = New Rectangle(ICONSIZE + ICONOFFSETX * 2, RectCaptionMain.Y, RectCaptionMain.Width - ICONSIZE - ICONOFFSETX * 2 - WSYSBTN * 3, RectCaptionMain.Height)
        Else
            RectText = RectTextIconArea
        End If
    End Sub

    Private Sub SetUpTextDraw()
        mTextWidth = TextRenderer.MeasureText(Me.Text, Me.Font).Width
        Select Case mTextAlign
            Case TextAlignEnum.Left
                RectTextDraw = RectText

            Case TextAlignEnum.Center
                If (Me.ClientSize.Width - mTextWidth) / 2 + mTextWidth > RectMinButton.X Then
                    RectTextDraw = New Rectangle(ICONSIZE + ICONOFFSETX * 2, RectCaptionMain.Y, RectCaptionMain.Width - ICONSIZE - ICONOFFSETX * 2 - WSYSBTN * 3, RectCaptionMain.Height)
                Else
                    RectTextDraw = RectCaptionMain
                End If
        End Select
    End Sub

    Private Sub SetUpCaptionButton()
        RectCloseButton = New Rectangle(RectCaptionMain.Width - WSYSBTN, RectCaptionMain.Y, WSYSBTN, HSYSBTN)
        RectMaxButton = New Rectangle(RectCloseButton.X - WSYSBTN, RectCloseButton.Y, WSYSBTN, HSYSBTN)
        If mControlBox AndAlso mMaximizeBox Then
            RectMinButton = New Rectangle(RectMaxButton.X - WSYSBTN, RectMaxButton.Y, WSYSBTN, HSYSBTN)
        Else
            RectMinButton = RectMaxButton
        End If
        RectMinMaxBtnArea = New Rectangle(RectCloseButton.X - WSYSBTN * 2, RectCloseButton.Y, WSYSBTN * 2, HSYSBTN)

        RectMinBtnHT = New Rectangle(RectMinButton.X + NCHTSIZE, RectMinButton.Y + NCHTSIZE, RectMinButton.Width - NCHTSIZE * 2, RectMinButton.Height - NCHTSIZE * 2)
        RectMaxBtnHT = New Rectangle(RectMaxButton.X + NCHTSIZE, RectMaxButton.Y + NCHTSIZE, RectMaxButton.Width - NCHTSIZE * 2, RectMaxButton.Height - NCHTSIZE * 2)
        RectCloseBtnHT = New Rectangle(RectCloseButton.X + NCHTSIZE, RectCloseButton.Y + NCHTSIZE, RectCloseButton.Width - NCHTSIZE * 2, RectCloseButton.Height - NCHTSIZE * 2)

        mRestoreBtnPoints(0) = New Point(RectMaxButton.X + WSYSBTN_ONENINTH * 4, RectMaxButton.Y + HSYSBTN_ONENINTH * 4)
        mRestoreBtnPoints(1) = New Point(RectMaxButton.X + WSYSBTN_ONENINTH * 4, RectMaxButton.Y + HSYSBTN_ONENINTH * 3)
        mRestoreBtnPoints(2) = New Point(RectMaxButton.X + WSYSBTN_ONENINTH * 6, RectMaxButton.Y + HSYSBTN_ONENINTH * 3)
        mRestoreBtnPoints(3) = New Point(RectMaxButton.X + WSYSBTN_ONENINTH * 6, RectMaxButton.Y + HSYSBTN_ONENINTH * 5)
        mRestoreBtnPoints(4) = New Point(RectMaxButton.X + WSYSBTN_ONENINTH * 5, RectMaxButton.Y + HSYSBTN_ONENINTH * 5)
    End Sub

    Private Sub SetUpBorderHT()
        RectTop = New Rectangle(NCHTSIZE, 0, Me.Width - NCHTSIZE * 2, NCHTSIZE)
        RectBottom = New Rectangle(NCHTSIZE, Me.Height - NCHTSIZE, Me.Width - NCHTSIZE * 2, NCHTSIZE)
        RectLeft = New Rectangle(0, NCHTSIZE, NCHTSIZE, Me.Height - NCHTSIZE * 2)
        RectRight = New Rectangle(Me.Width - NCHTSIZE, NCHTSIZE, NCHTSIZE, Me.Height - NCHTSIZE * 2)
        RectTopRight = New Rectangle(Me.Width - NCHTSIZE, 0, NCHTSIZE, NCHTSIZE)
        RectBottomLeft = New Rectangle(0, Me.Height - NCHTSIZE, NCHTSIZE, NCHTSIZE)
        RectBottomRight = New Rectangle(Me.Width - NCHTSIZE, Me.Height - NCHTSIZE, NCHTSIZE, NCHTSIZE)
        RectSizeGrip = New Rectangle(Me.ClientSize.Width - SIZEGRIPSIZE - NCHTSIZE, Me.ClientSize.Height - SIZEGRIPSIZE - NCHTSIZE, SIZEGRIPSIZE, SIZEGRIPSIZE)
        RectGrids(0) = New Rectangle(RectSizeGrip.X + SIZEGRIPSIZE - 1 - GRIDSIZE, RectSizeGrip.Y + 1, GRIDSIZE, GRIDSIZE)
        RectGrids(1) = New Rectangle(RectGrids(0).X, RectGrids(0).Y + GRIDSIZE * 2, GRIDSIZE, GRIDSIZE)
        RectGrids(2) = New Rectangle(RectGrids(1).X - GRIDSIZE * 2, RectGrids(1).Y, GRIDSIZE, GRIDSIZE)
        RectGrids(3) = New Rectangle(RectGrids(1).X, RectGrids(1).Y + GRIDSIZE * 2, GRIDSIZE, GRIDSIZE)
        RectGrids(4) = New Rectangle(RectGrids(3).X - GRIDSIZE * 2, RectGrids(3).Y, GRIDSIZE, GRIDSIZE)
        RectGrids(5) = New Rectangle(RectGrids(3).X - GRIDSIZE * 4, RectGrids(3).Y, GRIDSIZE, GRIDSIZE)
        RectGrids(6) = New Rectangle(RectGrids(3).X, RectGrids(3).Y + GRIDSIZE * 2, GRIDSIZE, GRIDSIZE)
        RectGrids(7) = New Rectangle(RectGrids(6).X - GRIDSIZE * 2, RectGrids(6).Y, GRIDSIZE, GRIDSIZE)
        RectGrids(8) = New Rectangle(RectGrids(6).X - GRIDSIZE * 4, RectGrids(6).Y, GRIDSIZE, GRIDSIZE)
        RectGrids(9) = New Rectangle(RectGrids(6).X - GRIDSIZE * 6, RectGrids(6).Y, GRIDSIZE, GRIDSIZE)
    End Sub

    Private Sub DrawForm(g As Graphics, rcClip As Rectangle)
        '---标题栏---
        If rcClip.IntersectsWith(RectCaptionSub) Then
            g.FillRectangle(mSubCaptionBrush, RectCaptionSub)
        End If
        If rcClip.IntersectsWith(RectCaptionMain) Then
            g.FillRectangle(mMainCaptionBrush, RectCaptionMain)
        End If
        If rcClip.IntersectsWith(RectCaptionExtended) Then
            g.FillRectangle(mExtendedCaptionBrush, RectCaptionExtended)
        End If

        '---窗口标题---
        If rcClip.IntersectsWith(RectText) AndAlso (Not String.IsNullOrEmpty(Me.Text)) Then
            TextRenderer.DrawText(g, Me.Text, Me.Font, RectTextDraw, mFormStyle.ForeColor, mTextFormat(mTextAlign))
        End If

        '---最小化---
        If mControlBox AndAlso mMinimizeBox AndAlso rcClip.IntersectsWith(RectMinButton) Then
            Select Case mMouseState
                Case MouseStateEnum.MinHover
                    g.FillRectangle(mBtnHoverBrush, RectMinButton)
                Case MouseStateEnum.MinPress
                    g.FillRectangle(mBtnPressBrush, RectMinButton)
            End Select
            g.DrawLine(mTextPen2, RectMinButton.X + WSYSBTN_ONENINTH * 3, RectMinButton.Y + HSYSBTN_ONENINTH * 6, RectMinButton.X + WSYSBTN_ONENINTH * 6, RectMinButton.Y + HSYSBTN_ONENINTH * 6)
        End If

        '---最大化---
        If mControlBox AndAlso mMaximizeBox AndAlso rcClip.IntersectsWith(RectMaxButton) Then
            Select Case mMouseState
                Case MouseStateEnum.MaxHover
                    g.FillRectangle(mBtnHoverBrush, RectMaxButton)
                Case MouseStateEnum.MaxPress
                    g.FillRectangle(mBtnPressBrush, RectMaxButton)
            End Select
            If Me.WindowState <> FormWindowState.Maximized Then
                g.DrawRectangle(mTextPen, RectMaxButton.X + CInt(WSYSBTN / 3), RectMaxButton.Y + CInt(HSYSBTN / 3), CInt(WSYSBTN / 3), CInt(HSYSBTN / 3))
                g.DrawLine(mTextPen2, RectMaxButton.X + CInt(WSYSBTN / 3), RectMaxButton.Y + CInt(HSYSBTN / 3) + 1, RectMaxButton.X + CInt(WSYSBTN / 3 * 2), RectMaxButton.Y + CInt(HSYSBTN / 3) + 1)
            Else
                g.DrawLines(mTextPen, mRestoreBtnPoints)
                g.DrawLine(mTextPen, mRestoreBtnPoints(1).X, mRestoreBtnPoints(1).Y + 1, mRestoreBtnPoints(2).X, mRestoreBtnPoints(2).Y + 1)
                g.DrawRectangle(mTextPen, RectMaxButton.X + WSYSBTN_ONENINTH * 3, RectMaxButton.Y + HSYSBTN_ONENINTH * 4, WSYSBTN_ONENINTH * 2, HSYSBTN_ONENINTH * 2)
                g.DrawLine(mTextPen, RectMaxButton.X + WSYSBTN_ONENINTH * 3, RectMaxButton.Y + HSYSBTN_ONENINTH * 4 + 1, RectMaxButton.X + WSYSBTN_ONENINTH * 5, RectMaxButton.Y + HSYSBTN_ONENINTH * 4 + 1)
            End If
        End If

        '---关闭---
        If mControlBox AndAlso rcClip.IntersectsWith(RectCloseButton) Then
            Select Case mMouseState
                Case MouseStateEnum.CloseHover
                    g.FillRectangle(mBtnHoverBrush, RectCloseButton)
                Case MouseStateEnum.ClosePress
                    g.FillRectangle(mBtnPressBrush, RectCloseButton)
            End Select
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.DrawLine(mTextPen2, RectCloseButton.X + WSYSBTN_ONENINTH * 3 + 1, RectCloseButton.Y + HSYSBTN_ONENINTH * 3, RectCloseButton.X + WSYSBTN_ONENINTH * 6 - 1, RectCloseButton.Y + HSYSBTN_ONENINTH * 6)
            g.DrawLine(mTextPen2, RectCloseButton.X + WSYSBTN_ONENINTH * 3 + 1, RectCloseButton.Y + HSYSBTN_ONENINTH * 6, RectCloseButton.X + WSYSBTN_ONENINTH * 6 - 1, RectCloseButton.Y + HSYSBTN_ONENINTH * 3)
            g.SmoothingMode = Drawing2D.SmoothingMode.Default
        End If

        '---窗口图标---
        If mControlBox AndAlso mShowIcon AndAlso rcClip.IntersectsWith(RectIcon) Then
            If Me.Icon IsNot Nothing Then
                g.DrawIcon(Me.Icon, RectIcon)
            End If
        End If

        '---SizeGrip---
        If mSizeGrip AndAlso rcClip.IntersectsWith(RectSizeGrip) AndAlso (Me.Height > mMinSizeReserve.Height + SIZEGRIPSIZE) Then
            Select Case mSizeGripStyle
                Case SizeGripStyleEnum.Grid
                    g.FillRectangles(Brushes.Gray, RectGrids)
            End Select
        End If

        '---窗体边框---
        If mDrawBorder Then
            g.DrawRectangle(mBorderPen, RectBorder)
        End If
    End Sub
#End Region
End Class

