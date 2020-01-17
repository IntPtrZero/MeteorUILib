Imports System.Math
Imports System.Drawing
Imports MeteorUiLib.Native
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

<DefaultProperty("Value"), DefaultEvent("Scroll"), ToolboxBitmap(GetType(HScrollBar)), Description("Metro风格滚动条控件")> _
Public Class MeteorScrollBar : Inherits Control
#Region "变量"
    Private Shared ReadOnly EVENT_SCROLL As Object = New Object()
    Private Shared ReadOnly EVENT_VALUECHANGED As Object = New Object()

    ''' <summary>
    ''' 滚动条默认长度
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly SCROLLBAR_LENGTH As Integer = 142
    ''' <summary>
    ''' 滚动条默认宽度
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly SCROLLBAR_WIDTH As Integer = 21

    ''' <summary>
    ''' 箭头区域命中范围默认大小
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly ARROW_HT_SIZE As Integer = 21
    ''' <summary>
    ''' 滑块最短长度
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ReadOnly MIN_THUMB_LEN As Integer = ARROW_HT_SIZE

    ''' <summary>
    ''' 滑块长度
    ''' </summary>
    ''' <remarks></remarks>
    Private mThumbLength As Integer
    ''' <summary>
    ''' 滑块位置
    ''' </summary>
    ''' <remarks></remarks>
    Private mThumbPos As Integer

    Private mMinValue As Integer = 0
    Private mMaxValue As Integer = 100
    Private mValueRange As Integer = 100
    Private mSmallChange As Integer = 1
    Private mLargeChange As Integer = 10
    Private mValue As Integer = 0
    Private mWheelDelta As Integer = 0
    Private mScrollOrientation As ScrollOrientation

    Private RectArrowHeadHT, RectArrowEndHT As Rectangle
    Private RectThumbHT As Rectangle
    Private RectThumbDraw As Rectangle
    Private RectTrack As Rectangle
    Private PtArrowHead(), PtArrowEnd() As PointF

    Private mArrowHeadColor As Color
    Private mArrowEndColor As Color
    Private mThumbColor As Color
    Private mArrowSize As Integer

    ''' <summary>
    ''' 拖动滚动条滑块时的鼠标位置
    ''' </summary>
    ''' <remarks></remarks>
    Private ptMouseOld As Point
    Private ptMouseNew As Point
    ''' <summary>
    ''' 长按滚动条轨道时的鼠标位置
    ''' </summary>
    ''' <remarks>可以用ptMouseOld代替,为了安全起见另外定义一个变量</remarks>
    Private ptMouse As Point

    Private mScrollTimer As Timer
    Private Shared ReadOnly SCROLL_TRIGGER_INTERVAL As Integer = 300
    Private Shared ReadOnly CONTINUOUS_SCROLL_INTERVAL As Integer = 20
    Private mScrollType As ScrollEventType

    Private mMouseState As MouseStateEnum
    Private mScrollBarStyle As MetroScrollBarStyle

    ''' <summary>
    ''' 防溢出
    ''' </summary>
    ''' <remarks></remarks>
    Private fRestrictedSize As Boolean

    Private mArrowHeadPen As Pen
    Private mArrowEndPen As Pen
    Private mArrowHeadBrush As Brush
    Private mArrowEndBrush As Brush
    Private mThumbBrush As Brush
    Private mDisabledColor As Color
#End Region

#Region "枚举"
    <Flags> _
    Private Enum MouseStateEnum As Byte
        None = 0
        ArrowHeadHover = 1
        ArrowHeadPress = 2
        <EditorBrowsable(EditorBrowsableState.Never)> _
        ARROWHEADFLAG = ArrowHeadHover Or ArrowHeadPress
        ArrowEndHover = 4
        ArrowEndPress = 8
        <EditorBrowsable(EditorBrowsableState.Never)> _
        ARROWENDFLAG = ArrowEndHover Or ArrowEndPress
        ThumbHover = 16
        ThumbPress = 32
        <EditorBrowsable(EditorBrowsableState.Never)> _
        THUMBFLAG = ThumbHover Or ThumbPress
    End Enum

    ''' <summary>
    ''' 滚动条箭头样式枚举
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ArrowTypeEnum As Byte
        ''' <summary>
        ''' 无箭头
        ''' </summary>
        ''' <remarks></remarks>
        None = 0
        ''' <summary>
        ''' 普通箭头
        ''' </summary>
        ''' <remarks></remarks>
        Arrow = 1
        ''' <summary>
        ''' 三角形箭头
        ''' </summary>
        ''' <remarks></remarks>
        Triangle = 2
    End Enum
#End Region

#Region "构造函数"
    Public Sub New()
        Me.New(ScrollOrientation.HorizontalScroll)
    End Sub

    Public Sub New(ByVal orientation As ScrollOrientation)
        MyBase.New()
        SetStyle(ControlStyles.UseTextForAccessibility Or ControlStyles.StandardClick, False)
        SetStyle(ControlStyles.UserPaint Or _
                 ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.Selectable Or _
                 ControlStyles.OptimizedDoubleBuffer, True)

        mScrollOrientation = orientation
        mScrollBarStyle = New MetroScrollBarStyle()
        mMouseState = MouseStateEnum.None
        InitDrawing()

        mScrollTimer = New Timer
        mScrollTimer.Interval = SCROLL_TRIGGER_INTERVAL
        AddHandler mScrollTimer.Tick, AddressOf Timer_Tick
        Me.TabStop = False
        ReDim PtArrowHead(2), PtArrowEnd(2)
        SetUpScrollBar()
    End Sub
#End Region

#Region "隐藏"
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property BackColor As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(value As Color)
            MyBase.BackColor = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event BackColorChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property BackgroundImage As Image
        Get
            Return MyBase.BackgroundImage
        End Get
        Set(value As Image)
            MyBase.BackgroundImage = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event BackGroundImageChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property BackgroundImageLayout As ImageLayout
        Get
            Return MyBase.BackgroundImageLayout
        End Get
        Set(value As ImageLayout)
            MyBase.BackgroundImageLayout = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event BackGroundImageLayoutChanged As EventHandler

    Protected Overrides ReadOnly Property DefaultImeMode As ImeMode
        Get
            Return Windows.Forms.ImeMode.Disable
        End Get
    End Property

    Protected Overrides ReadOnly Property DefaultMargin As Padding
        Get
            Return Padding.Empty
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Protected Overrides Property DoubleBuffered As Boolean
        Get
            Return MyBase.DoubleBuffered
        End Get
        Set(value As Boolean)
            MyBase.DoubleBuffered = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property ForeColor As Color
        Get
            Return MyBase.ForeColor
        End Get
        Set(value As Color)
            MyBase.ForeColor = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event ForeColorChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(value As Font)
            MyBase.Font = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event FontChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property ImeMode As ImeMode
        Get
            Return MyBase.ImeMode
        End Get
        Set(value As ImeMode)
            MyBase.ImeMode = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event ImeModeChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property Padding As Padding
        Get
            Return MyBase.Padding
        End Get
        Set(value As Padding)
            MyBase.Padding = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event PaddingChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event TextChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property RightToLeft As RightToLeft
        Get
            Return MyBase.RightToLeft
        End Get
        Set(value As RightToLeft)
            MyBase.RightToLeft = value
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event RightToLeftChanged As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event Click As EventHandler

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Shadows Event DoubleClick As EventHandler

#End Region

#Region "属性"
    <DefaultValue(10), Description("当用户单击滚动条轨道或按下PageUp、PageDown时，滑块位置的变化幅度")> _
    Public Property LargeChange As Integer
        Get
            Return mLargeChange
        End Get
        Set(value As Integer)
            If mLargeChange <> value And value >= mSmallChange Then
                mLargeChange = Min(value, mValueRange)
                SetUpThumb()
                Me.Invalidate(RectTrack)
            End If
        End Set
    End Property

    <DefaultValue(100), Description("可滚动范围的最大值")> _
    Public Property Maximum As Integer
        Get
            Return mMaxValue
        End Get
        Set(value As Integer)
            If mMaxValue <> value And value > 0 And value > mMinValue Then
                mMaxValue = value
                mValueRange = mMaxValue - mMinValue
                mLargeChange = Min(mLargeChange, mValueRange)
                mSmallChange = Min(mSmallChange, mLargeChange)
                mValue = Min(mValue, mMaxValue - mLargeChange + 1)
                SetUpThumb()
                Me.Invalidate(RectTrack)
            End If
        End Set
    End Property

    <DefaultValue(0), Description("可滚动范围的最小值")> _
    Public Property Minimum As Integer
        Get
            Return mMinValue
        End Get
        Set(value As Integer)
            If mMinValue <> value And value >= 0 And value < mMaxValue Then
                mMinValue = value
                mValueRange = mMaxValue - mMinValue
                mLargeChange = Min(mLargeChange, mValueRange)
                mSmallChange = Min(mSmallChange, mLargeChange)
                mValue = Max(mValue, mMinValue)
                SetUpThumb()
                Me.Invalidate(RectTrack)
            End If
        End Set
    End Property

    <DefaultValue(1), Description("当用户单击箭头或按下方向键时，滑块位置的变化幅度")> _
    Public Property SmallChange As Integer
        Get
            Return mSmallChange
        End Get
        Set(value As Integer)
            If mSmallChange <> value And value > 0 And value <= mLargeChange Then
                mSmallChange = value
            End If
        End Set
    End Property

    <DefaultValue(False)> _
    Public Overloads Property TabStop As Boolean
        Get
            Return MyBase.TabStop
        End Get
        Set(value As Boolean)
            MyBase.TabStop = value
        End Set
    End Property

    <DefaultValue(0), Description("滑块位置表示的值")> _
    Public Property Value As Integer
        Get
            Return mValue
        End Get
        Set(value As Integer)
            If mValue <> value Then
                Dim newValue = Max(Min(value, mMaxValue - mLargeChange + 1), mMinValue)
                If mValue <> newValue Then
                    mValue = newValue
                    OnValueChanged(EventArgs.Empty)
                End If
            End If
        End Set
    End Property

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Select Case mScrollOrientation
                Case ScrollOrientation.HorizontalScroll
                    Return New Size(SCROLLBAR_LENGTH, SCROLLBAR_WIDTH)
                Case ScrollOrientation.VerticalScroll
                    Return New Size(SCROLLBAR_WIDTH, SCROLLBAR_LENGTH)
            End Select
        End Get
    End Property

    <Description("滚动条的方向")> _
    Public Property Orientation As ScrollOrientation
        Get
            Return mScrollOrientation
        End Get
        Set(value As ScrollOrientation)
            If mScrollOrientation <> value Then
                mScrollOrientation = value
                Me.Size = New Size(Size.Height, Size.Width)
            End If
        End Set
    End Property

    '==可以考虑设计为DesignOnly，以防止用户骚操作==
    <Description("滚动条的样式。禁止在运行时修改下级属性。"), DefaultValue(GetType(MetroScrollBarStyle), "MetroLight")> _
    Public Property ScrollBarStyle As MetroScrollBarStyle
        Get
            Return mScrollBarStyle
        End Get
        Set(value As MetroScrollBarStyle)
            If mScrollBarStyle <> value Then
                mScrollBarStyle = value
                InitDrawing()
                SetUpScrollBar()
                Me.Invalidate()
            End If
        End Set
    End Property

#End Region

#Region "方法重写"
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.Clear(mScrollBarStyle.BackColor)
        If Not fRestrictedSize Then
            DrawScrollBar(g, e.ClipRectangle)
        End If
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        mWheelDelta = e.Delta
        While Abs(mWheelDelta) >= WHEEL_DELTA
            If (mWheelDelta > 0) Then
                mWheelDelta -= WHEEL_DELTA
                DoScroll(ScrollEventType.SmallDecrement)
            Else
                mWheelDelta += WHEEL_DELTA
                DoScroll(ScrollEventType.SmallIncrement)
            End If
        End While

        If e Is GetType(HandledMouseEventArgs) Then
            CType(e, HandledMouseEventArgs).Handled = True
        End If
        MyBase.OnMouseWheel(e)
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        If (mScrollOrientation = ScrollOrientation.HorizontalScroll And Me.Width <= ARROW_HT_SIZE + mArrowSize * 2) Or _
            (mScrollOrientation = ScrollOrientation.VerticalScroll And Me.Height <= ARROW_HT_SIZE + mArrowSize * 2) Then
            fRestrictedSize = True
            Me.Enabled = False
        Else
            fRestrictedSize = False
            Me.Enabled = True
        End If
        SetUpScrollBar()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        If Me.Enabled Then
            mArrowHeadPen = New Pen(mArrowHeadColor, 2)
            mArrowHeadBrush = New SolidBrush(mArrowHeadColor)
            mArrowEndPen = New Pen(mArrowEndColor, 2)
            mArrowEndBrush = New SolidBrush(mArrowEndColor)
        Else
            mArrowHeadPen = New Pen(mDisabledColor, 2)
            mArrowHeadBrush = New SolidBrush(mDisabledColor)
            mArrowEndPen = New Pen(mDisabledColor, 2)
            mArrowEndBrush = New SolidBrush(mDisabledColor)
        End If
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        ptMouse = e.Location
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If RectArrowHeadHT.Contains(ptMouse) Then
                SetMouseState(MouseStateEnum.ArrowHeadPress)
                DoScroll(ScrollEventType.SmallDecrement)
                mScrollType = ScrollEventType.SmallDecrement
                mScrollTimer.Start()
                Return
            End If
            If RectArrowEndHT.Contains(ptMouse) Then
                SetMouseState(MouseStateEnum.ArrowEndPress)
                DoScroll(ScrollEventType.SmallIncrement)
                mScrollType = ScrollEventType.SmallIncrement
                mScrollTimer.Start()
                Return
            End If
            If RectThumbHT.Contains(ptMouse) Then
                SetMouseState(MouseStateEnum.ThumbPress)
                ptMouseOld = ptMouse
                Return
            End If
            If RectTrack.Contains(ptMouse) Then
                SetMouseState(MouseStateEnum.None)
                Select Case mScrollOrientation
                    Case ScrollOrientation.VerticalScroll
                        If e.Y < RectThumbHT.Y Then
                            DoScroll(ScrollEventType.LargeDecrement)
                            mScrollType = ScrollEventType.LargeDecrement
                            mScrollTimer.Start()
                        Else
                            DoScroll(ScrollEventType.LargeIncrement)
                            mScrollType = ScrollEventType.LargeIncrement
                            mScrollTimer.Start()
                        End If

                    Case ScrollOrientation.HorizontalScroll
                        If e.X < RectThumbHT.X Then
                            DoScroll(ScrollEventType.LargeDecrement)
                            mScrollType = ScrollEventType.LargeDecrement
                            mScrollTimer.Start()
                        Else
                            DoScroll(ScrollEventType.LargeIncrement)
                            mScrollType = ScrollEventType.LargeIncrement
                            mScrollTimer.Start()
                        End If
                End Select
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        If mScrollTimer.Enabled Then
            mScrollTimer.Stop()
            mScrollTimer.Interval = SCROLL_TRIGGER_INTERVAL
        End If
        If RectArrowHeadHT.Contains(e.Location) Then
            SetMouseState(MouseStateEnum.ArrowHeadHover)
            Return
        End If
        If RectArrowEndHT.Contains(e.Location) Then
            SetMouseState(MouseStateEnum.ArrowEndHover)
            Return
        End If
        If RectThumbHT.Contains(e.Location) Then
            SetMouseState(MouseStateEnum.ThumbHover)
            Return
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Select Case e.Button
            Case Windows.Forms.MouseButtons.None
                If RectArrowHeadHT.Contains(e.Location) Then
                    SetMouseState(MouseStateEnum.ArrowHeadHover)
                    Return
                End If
                If RectArrowEndHT.Contains(e.Location) Then
                    SetMouseState(MouseStateEnum.ArrowEndHover)
                    Return
                End If
                If RectThumbHT.Contains(e.Location) Then
                    SetMouseState(MouseStateEnum.ThumbHover)
                    Return
                End If
                SetMouseState(MouseStateEnum.None)

            Case Windows.Forms.MouseButtons.Left
                If mMouseState = MouseStateEnum.ThumbPress Then
                    ptMouseNew = e.Location
                    DoScroll(ScrollEventType.ThumbTrack)
                End If
        End Select
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        SetMouseState(MouseStateEnum.None)
    End Sub

    Protected Overrides Function ProcessDialogKey(keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Left
                If mScrollOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.SmallDecrement)
                    Return True
                End If
            Case Keys.Right
                If mScrollOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.SmallIncrement)
                    Return True
                End If
            Case Keys.Up
                If mScrollOrientation = ScrollOrientation.VerticalScroll Then
                    DoScroll(ScrollEventType.SmallDecrement)
                    Return True
                End If
            Case Keys.Down
                If mScrollOrientation = ScrollOrientation.VerticalScroll Then
                    DoScroll(ScrollEventType.SmallIncrement)
                    Return True
                End If
            Case Keys.PageUp
                DoScroll(ScrollEventType.LargeDecrement)
                Return True
            Case Keys.PageDown
                DoScroll(ScrollEventType.LargeIncrement)
                Return True
            Case Keys.Home
                DoScroll(ScrollEventType.First)
                Return True
            Case Keys.End
                DoScroll(ScrollEventType.Last)
                Return True
        End Select
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overridable Sub OnScroll(e As ScrollEventArgs)
        RaiseEvent Scroll(Me, e)
    End Sub

    Protected Overridable Sub OnValueChanged(e As EventArgs)
        SetUpThumb()
        Me.Invalidate(RectTrack)
        RaiseEvent ValueChanged(Me, e)
    End Sub
#End Region

#Region "私有方法"
    ''' <summary>
    ''' 构建滚动条
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUpScrollBar()
        SetUpThumb()
        Select Case mScrollOrientation
            Case ScrollOrientation.VerticalScroll
                RectArrowHeadHT = New Rectangle(0, 0, Me.Width, mArrowSize)
                RectArrowEndHT = New Rectangle(0, Me.Height - mArrowSize, Me.Width, mArrowSize)
                RectTrack = New Rectangle(0, mArrowSize, Me.Width, Me.Height - mArrowSize * 2)
                PtArrowHead(0) = New PointF((Me.Width - ARROW_HT_SIZE) / 2.0F + mScrollBarStyle.ArrowMargin.Left - 0.5F, ARROW_HT_SIZE - mScrollBarStyle.ArrowMargin.Bottom - 0.5F)
                PtArrowHead(1) = New PointF(Me.Width / 2.0F - 0.5F, mScrollBarStyle.ArrowMargin.Top - 0.5F)
                PtArrowHead(2) = New PointF((Me.Width + ARROW_HT_SIZE) / 2.0F - mScrollBarStyle.ArrowMargin.Right - 0.5F, ARROW_HT_SIZE - mScrollBarStyle.ArrowMargin.Bottom - 0.5F)
                PtArrowEnd(0) = New PointF((Me.Width - ARROW_HT_SIZE) / 2.0F + mScrollBarStyle.ArrowMargin.Left - 0.5F, Me.Height - ARROW_HT_SIZE + mScrollBarStyle.ArrowMargin.Bottom - 0.5F)
                PtArrowEnd(1) = New PointF(Me.Width / 2.0F - 0.5F, Me.Height - mScrollBarStyle.ArrowMargin.Top - 0.5F)
                PtArrowEnd(2) = New PointF((Me.Width + ARROW_HT_SIZE) / 2.0F - mScrollBarStyle.ArrowMargin.Right - 0.5F, Me.Height - ARROW_HT_SIZE + mScrollBarStyle.ArrowMargin.Bottom - 0.5F)

            Case ScrollOrientation.HorizontalScroll
                RectArrowHeadHT = New Rectangle(0, 0, mArrowSize, Me.Height)
                RectArrowEndHT = New Rectangle(Me.Width - mArrowSize, 0, mArrowSize, Me.Height)
                RectTrack = New Rectangle(mArrowSize, 0, Me.Width - mArrowSize * 2, Me.Height)
                PtArrowHead(0) = New PointF(ARROW_HT_SIZE - mScrollBarStyle.ArrowMargin.Bottom - 0.5F, (Me.Height - ARROW_HT_SIZE) / 2.0F + mScrollBarStyle.ArrowMargin.Right - 0.5F)
                PtArrowHead(1) = New PointF(mScrollBarStyle.ArrowMargin.Top - 0.5F, Me.Height / 2.0F - 0.5F)
                PtArrowHead(2) = New PointF(ARROW_HT_SIZE - mScrollBarStyle.ArrowMargin.Bottom - 0.5F, (Me.Height + ARROW_HT_SIZE) / 2.0F - mScrollBarStyle.ArrowMargin.Left - 0.5F)
                PtArrowEnd(0) = New PointF(Me.Width - ARROW_HT_SIZE + mScrollBarStyle.ArrowMargin.Bottom - 0.5F, (Me.Height - ARROW_HT_SIZE) / 2.0F + mScrollBarStyle.ArrowMargin.Right - 0.5F)
                PtArrowEnd(1) = New PointF(Me.Width - mScrollBarStyle.ArrowMargin.Top - 0.5F, Me.Height / 2.0F - 0.5F)
                PtArrowEnd(2) = New PointF(Me.Width - ARROW_HT_SIZE + mScrollBarStyle.ArrowMargin.Bottom - 0.5F, (Me.Height + ARROW_HT_SIZE) / 2.0F - mScrollBarStyle.ArrowMargin.Left - 0.5F)
        End Select
    End Sub

    ''' <summary>
    ''' 构建滑块
    ''' </summary>
    ''' <remarks>注意:GDI+的FillRectangle填充的是像素宽度，像素实际上是一个小矩形区域.例如FillRectangle(0,0,3,3)填充3个像素宽度，因此将从0像素的右下角填充到3像素的左上角</remarks>
    Private Sub SetUpThumb()
        mThumbLength = GetThumbLength()
        mThumbPos = GetThumbPos()
        Select Case mScrollOrientation
            Case ScrollOrientation.VerticalScroll
                RectThumbHT = New Rectangle(0, mThumbPos, Me.Width, mThumbLength)
                RectThumbDraw = New Rectangle(mScrollBarStyle.ThumbMargin, mThumbPos, Me.Width - mScrollBarStyle.ThumbMargin * 2, mThumbLength)
            Case ScrollOrientation.HorizontalScroll
                RectThumbHT = New Rectangle(mThumbPos, 0, mThumbLength, Me.Height)
                RectThumbDraw = New Rectangle(mThumbPos, mScrollBarStyle.ThumbMargin, mThumbLength, Me.Height - mScrollBarStyle.ThumbMargin * 2)
        End Select
    End Sub

    ''' <summary>
    ''' 计算滑块位置
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetThumbPos() As Integer
        Select Case mScrollOrientation
            Case ScrollOrientation.VerticalScroll
                Return (Me.Height - mArrowSize * 2 - mThumbLength) * (mValue - mMinValue) / (mValueRange - mLargeChange + 1) + mArrowSize
            Case ScrollOrientation.HorizontalScroll
                Return (Me.Width - mArrowSize * 2 - mThumbLength) * (mValue - mMinValue) / (mValueRange - mLargeChange + 1) + mArrowSize
        End Select
        Return 0
    End Function

    ''' <summary>
    ''' 计算滑块长度
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetThumbLength() As Integer
        Select Case mScrollOrientation
            Case ScrollOrientation.VerticalScroll
                Return Max(MIN_THUMB_LEN, (Me.Height - mArrowSize * 2) * mLargeChange / (mValueRange + 1))
            Case ScrollOrientation.HorizontalScroll
                Return Max(MIN_THUMB_LEN, (Me.Width - mArrowSize * 2) * mLargeChange / (mValueRange + 1))
        End Select
        Return MIN_THUMB_LEN
    End Function

    Private Function PosToValue(ByVal pos As Point) As Integer
        Select Case mScrollOrientation
            Case ScrollOrientation.HorizontalScroll
                Return (pos.X - mArrowSize) * (mValueRange - mLargeChange + 1) / (Me.Width - mArrowSize * 2 - mThumbLength) + mMinValue
            Case ScrollOrientation.VerticalScroll
                Return (pos.Y - mArrowSize) * (mValueRange - mLargeChange + 1) / (Me.Height - mArrowSize * 2 - mThumbLength) + mMinValue
        End Select
        Return 0
    End Function

    ''' <summary>
    ''' 滚动
    ''' </summary>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Private Sub DoScroll(type As ScrollEventType)
        Dim newValue As Integer = mValue
        Dim oldValue As Integer = mValue
        Select Case type
            Case ScrollEventType.First
                newValue = mMinValue

            Case ScrollEventType.Last
                newValue = mMaxValue - mLargeChange + 1

            Case ScrollEventType.SmallDecrement
                newValue = Max(oldValue - mSmallChange, mMinValue)

            Case ScrollEventType.SmallIncrement
                newValue = Min(oldValue + mSmallChange, mMaxValue - mLargeChange + 1)

            Case ScrollEventType.LargeDecrement
                newValue = Max(oldValue - mLargeChange, mMinValue)

            Case ScrollEventType.LargeIncrement
                newValue = Min(oldValue + mLargeChange, mMaxValue - mLargeChange + 1)

            Case ScrollEventType.ThumbTrack
                newValue = Min(Max(oldValue + PosToValue(ptMouseNew) - PosToValue(ptMouseOld), mMinValue), mMaxValue - mLargeChange + 1)
                ptMouseOld = ptMouseNew
        End Select

        If newValue <> oldValue Then
            Value = newValue
            Dim e As New ScrollEventArgs(type, oldValue, newValue, mScrollOrientation)
            OnScroll(e)
        End If
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs)
        mScrollTimer.Interval = CONTINUOUS_SCROLL_INTERVAL
        Select Case mScrollType
            Case ScrollEventType.SmallDecrement, ScrollEventType.SmallIncrement
                DoScroll(mScrollType)
            Case ScrollEventType.LargeDecrement
                If (mScrollOrientation = ScrollOrientation.HorizontalScroll AndAlso ptMouse.X < mThumbPos) OrElse (mScrollOrientation = ScrollOrientation.VerticalScroll AndAlso ptMouse.Y < mThumbPos) Then
                    DoScroll(mScrollType)
                End If
            Case ScrollEventType.LargeIncrement
                If (mScrollOrientation = ScrollOrientation.HorizontalScroll AndAlso ptMouse.X > mThumbPos + mThumbLength) OrElse (mScrollOrientation = ScrollOrientation.VerticalScroll AndAlso ptMouse.Y > mThumbPos + mThumbLength) Then
                    DoScroll(mScrollType)
                End If
        End Select
    End Sub

    Private Sub SetMouseState(ByVal value As MouseStateEnum)
        If mMouseState <> value Then
            Dim flag = mMouseState Or value
            mMouseState = value
            If flag And MouseStateEnum.ARROWHEADFLAG Then
                Select Case mMouseState
                    Case MouseStateEnum.ArrowHeadHover
                        mArrowHeadColor = mScrollBarStyle.ArrowHoverColor
                    Case MouseStateEnum.ArrowHeadPress
                        mArrowHeadColor = mScrollBarStyle.ArrowPressColor
                    Case Else
                        mArrowHeadColor = mScrollBarStyle.ArrowColor
                End Select
                mArrowHeadPen = New Pen(mArrowHeadColor, 2)
                mArrowHeadBrush = New SolidBrush(mArrowHeadColor)
                Me.Invalidate(RectArrowHeadHT)
            End If
            If flag And MouseStateEnum.ARROWENDFLAG Then
                Select Case mMouseState
                    Case MouseStateEnum.ArrowEndHover
                        mArrowEndColor = mScrollBarStyle.ArrowHoverColor
                    Case MouseStateEnum.ArrowEndPress
                        mArrowEndColor = mScrollBarStyle.ArrowPressColor
                    Case Else
                        mArrowEndColor = mScrollBarStyle.ArrowColor
                End Select
                mArrowEndPen = New Pen(mArrowEndColor, 2)
                mArrowEndBrush = New SolidBrush(mArrowEndColor)
                Me.Invalidate(RectArrowEndHT)
            End If
            If flag And MouseStateEnum.THUMBFLAG Then
                Select Case mMouseState
                    Case MouseStateEnum.ThumbHover
                        mThumbColor = mScrollBarStyle.ThumbHoverColor
                    Case MouseStateEnum.ThumbPress
                        mThumbColor = mScrollBarStyle.ThumbPressColor
                    Case Else
                        mThumbColor = mScrollBarStyle.ThumbColor
                End Select
                mThumbBrush = New SolidBrush(mThumbColor)
                Me.Invalidate(RectThumbHT)
            End If
        End If
    End Sub

    Private Sub InitDrawing()
        mArrowHeadColor = mScrollBarStyle.ArrowColor
        mArrowEndColor = mScrollBarStyle.ArrowColor
        mThumbColor = mScrollBarStyle.ThumbColor
        mArrowHeadPen = New Pen(mArrowHeadColor, 2)
        mArrowEndPen = New Pen(mArrowEndColor, 2)
        mArrowHeadBrush = New SolidBrush(mArrowHeadColor)
        mArrowEndBrush = New SolidBrush(mArrowEndColor)
        mThumbBrush = New SolidBrush(mThumbColor)
        mDisabledColor = Color.FromArgb((CInt(mScrollBarStyle.ArrowColor.R) - mScrollBarStyle.BackColor.R) * 0.6 + mScrollBarStyle.BackColor.R, _
                                        (CInt(mScrollBarStyle.ArrowColor.G) - mScrollBarStyle.BackColor.G) * 0.6 + mScrollBarStyle.BackColor.G, _
                                        (CInt(mScrollBarStyle.ArrowColor.B) - mScrollBarStyle.BackColor.B) * 0.6 + mScrollBarStyle.BackColor.B)
        mArrowSize = IIf(mScrollBarStyle.ArrowType = ArrowTypeEnum.None, 0, ARROW_HT_SIZE)
    End Sub

    Private Sub DrawScrollBar(g As Graphics, rcClip As Rectangle)
        If rcClip.IntersectsWith(RectThumbHT) AndAlso Me.Enabled Then
            g.FillRectangle(mThumbBrush, RectThumbDraw)
        End If
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Select Case mScrollBarStyle.ArrowType
            Case ArrowTypeEnum.Arrow
                If rcClip.IntersectsWith(RectArrowHeadHT) Then
                    g.DrawLines(mArrowHeadPen, PtArrowHead)
                End If
                If rcClip.IntersectsWith(RectArrowEndHT) Then
                    g.DrawLines(mArrowEndPen, PtArrowEnd)
                End If
            Case ArrowTypeEnum.Triangle
                If rcClip.IntersectsWith(RectArrowHeadHT) Then
                    g.FillPolygon(mArrowHeadBrush, PtArrowHead)
                End If
                If rcClip.IntersectsWith(RectArrowEndHT) Then
                    g.FillPolygon(mArrowEndBrush, PtArrowEnd)
                End If
            Case ArrowTypeEnum.None
        End Select
        g.SmoothingMode = Drawing2D.SmoothingMode.Default
    End Sub
#End Region

#Region "事件"
    <Description("移动滑块时发生")> _
    Public Custom Event Scroll As ScrollEventHandler
        AddHandler(value As ScrollEventHandler)
            Events.AddHandler(EVENT_SCROLL, value)
        End AddHandler

        RemoveHandler(value As ScrollEventHandler)
            Events.RemoveHandler(EVENT_SCROLL, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As ScrollEventArgs)
            Dim scEH As ScrollEventHandler = Events(EVENT_SCROLL)
            If scEH IsNot Nothing Then
                scEH(sender, e)
            End If
        End RaiseEvent
    End Event

    <Description("控件的值更改时发生")> _
    Public Custom Event ValueChanged As EventHandler
        AddHandler(value As EventHandler)
            Events.AddHandler(EVENT_VALUECHANGED, value)
        End AddHandler

        RemoveHandler(value As EventHandler)
            Events.RemoveHandler(EVENT_VALUECHANGED, value)
        End RemoveHandler

        RaiseEvent(sender As Object, e As EventArgs)
            Dim vcEH As EventHandler = Events(EVENT_VALUECHANGED)
            If vcEH IsNot Nothing Then
                vcEH(sender, e)
            End If
        End RaiseEvent
    End Event
#End Region

#Region "设计时"
    <TypeConverter(GetType(MetroScrollBarStyleConverter))> _
    Public Class MetroScrollBarStyle
#Region "变量"
        Private mArrowColor As Color
        Private mArrowHoverColor As Color
        Private mArrowPressColor As Color
        Private mArrowMargin As Padding
        Private mArrowType As ArrowTypeEnum
        Private mBackColor As Color
        Private mThumbColor As Color
        Private mThumbHoverColor As Color
        Private mThumbPressColor As Color
        Private mThumbMargin As Byte
#End Region

#Region "构造函数"
        Public Sub New()
            Me.New("MetroLight")
        End Sub

        Public Sub New(ByVal strStyle As String)
            Select Case strStyle
                Case "VsDark"
                    mArrowColor = Color.FromArgb(144, 144, 144)
                    mArrowHoverColor = Color.FromArgb(51, 153, 255)
                    mArrowPressColor = Color.FromArgb(0, 122, 204)
                    mArrowMargin = New Padding(5, 8, 5, 7)
                    mArrowType = ArrowTypeEnum.Triangle
                    mBackColor = Color.FromArgb(62, 62, 66)
                    mThumbColor = Color.FromArgb(112, 112, 112)
                    mThumbHoverColor = Color.FromArgb(160, 160, 160)
                    mThumbPressColor = Color.FromArgb(239, 235, 239)
                    mThumbMargin = 5

                Case "VsLight"
                    mArrowColor = Color.FromArgb(160, 160, 160)
                    mArrowHoverColor = Color.FromArgb(86, 156, 227)
                    mArrowPressColor = Color.FromArgb(0, 106, 193)
                    mArrowMargin = New Padding(5, 8, 5, 7)
                    mArrowType = ArrowTypeEnum.Triangle
                    mBackColor = Color.FromArgb(234, 234, 234)
                    mThumbColor = Color.FromArgb(184, 186, 199)
                    mThumbHoverColor = Color.FromArgb(136, 136, 136)
                    mThumbPressColor = Color.FromArgb(106, 106, 106)
                    mThumbMargin = 5

                Case "MetroDark"
                    mArrowColor = Color.FromArgb(144, 144, 144)
                    mArrowHoverColor = Color.FromArgb(51, 153, 255)
                    mArrowPressColor = Color.FromArgb(0, 122, 204)
                    mArrowMargin = System.Windows.Forms.Padding.Empty
                    mArrowType = ArrowTypeEnum.None
                    mBackColor = Color.FromArgb(38, 38, 38)
                    mThumbColor = Color.FromArgb(80, 80, 80)
                    mThumbHoverColor = Color.FromArgb(144, 144, 144)
                    mThumbPressColor = Color.FromArgb(214, 214, 214)
                    mThumbMargin = 0

                Case "MetroLight"
                    mArrowColor = Color.FromArgb(160, 160, 160)
                    mArrowHoverColor = Color.FromArgb(86, 156, 227)
                    mArrowPressColor = Color.FromArgb(0, 106, 193)
                    mArrowMargin = System.Windows.Forms.Padding.Empty
                    mArrowType = ArrowTypeEnum.None
                    mBackColor = Color.FromArgb(234, 234, 234)
                    mThumbColor = Color.FromArgb(196, 196, 196)
                    mThumbHoverColor = Color.FromArgb(104, 104, 104)
                    mThumbPressColor = Color.FromArgb(38, 38, 38)
                    mThumbMargin = 0

                Case "SystemDark"
                    mArrowColor = Color.FromArgb(144, 144, 144)
                    mArrowHoverColor = Color.FromArgb(51, 153, 255)
                    mArrowPressColor = Color.FromArgb(0, 122, 204)
                    mArrowMargin = New Padding(6, 9, 6, 8)
                    mArrowType = ArrowTypeEnum.Arrow
                    mBackColor = Color.FromArgb(38, 38, 38)
                    mThumbColor = Color.FromArgb(51, 51, 51)
                    mThumbHoverColor = Color.FromArgb(160, 160, 160)
                    mThumbPressColor = Color.FromArgb(209, 205, 209)
                    mThumbMargin = 1

                Case "SystemLight"
                    mArrowColor = Color.FromArgb(160, 160, 160)
                    mArrowHoverColor = Color.FromArgb(86, 156, 227)
                    mArrowPressColor = Color.FromArgb(0, 106, 193)
                    mArrowMargin = New Padding(6, 9, 6, 8)
                    mArrowType = ArrowTypeEnum.Arrow
                    mBackColor = Color.FromArgb(234, 234, 234)
                    mThumbColor = Color.FromArgb(184, 186, 199)
                    mThumbHoverColor = Color.FromArgb(136, 136, 136)
                    mThumbPressColor = Color.FromArgb(106, 106, 106)
                    mThumbMargin = 1

                Case Else
                    Throw New ArgumentException("无效的参数")
            End Select
        End Sub

        Public Sub New(ByVal arColor As Color, _
                       ByVal arhColor As Color, _
                       ByVal arpColor As Color, _
                       ByVal arMargin As Padding, _
                       ByVal arType As ArrowTypeEnum, _
                       ByVal bColor As Color, _
                       ByVal tbColor As Color, _
                       ByVal tbhColor As Color, _
                       ByVal tbpColor As Color, _
                       ByVal tbMargin As Byte)
            mArrowColor = arColor
            mArrowHoverColor = arhColor
            mArrowPressColor = arpColor
            mArrowMargin = arMargin
            mArrowType = arType
            mBackColor = bColor
            mThumbColor = tbColor
            mThumbHoverColor = tbhColor
            mThumbPressColor = tbpColor
            mThumbMargin = tbMargin
        End Sub
#End Region

#Region "重载"
        Public Overrides Function ToString() As String
            Select Case Me
                Case Is = New MetroScrollBarStyle("VsDark")
                    Return "VsDark"
                Case Is = New MetroScrollBarStyle("VsLight")
                    Return "VsLight"
                Case Is = New MetroScrollBarStyle("MetroDark")
                    Return "MetroDark"
                Case Is = New MetroScrollBarStyle("MetroLight")
                    Return "MetroLight"
                Case Is = New MetroScrollBarStyle("SystemDark")
                    Return "SystemDark"
                Case Is = New MetroScrollBarStyle("SystemLight")
                    Return "SystemLight"
                Case Else
                    Return "Custom"
            End Select
        End Function

        Public Overloads Shared Operator =(ByVal param1 As MetroScrollBarStyle, ByVal param2 As MetroScrollBarStyle)
            If param1.mArrowColor = param2.mArrowColor AndAlso _
                param1.mArrowHoverColor = param2.mArrowHoverColor AndAlso _
                param1.mArrowPressColor = param2.mArrowPressColor AndAlso _
                param1.mArrowMargin = param2.mArrowMargin AndAlso _
                param1.mArrowType = param2.mArrowType AndAlso _
                param1.mBackColor = param2.mBackColor AndAlso _
                param1.mThumbColor = param2.ThumbColor AndAlso _
                param1.mThumbHoverColor = param2.mThumbHoverColor AndAlso _
                param1.mThumbPressColor = param2.mThumbPressColor AndAlso _
                param1.mThumbMargin = param2.mThumbMargin Then
                Return True
            End If
            Return False
        End Operator

        Public Overloads Shared Operator <>(ByVal param1 As MetroScrollBarStyle, ByVal param2 As MetroScrollBarStyle)
            Return Not (param1 = param2)
        End Operator
#End Region

#Region "属性"
        <Description("箭头颜色")> _
        Public Property ArrowColor As Color
            Get
                Return mArrowColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mArrowColor = value
            End Set
        End Property

        <Description("鼠标悬停在箭头上的颜色")> _
        Public Property ArrowHoverColor As Color
            Get
                Return mArrowHoverColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mArrowHoverColor = value
            End Set
        End Property

        <Description("鼠标点击箭头时的颜色")> _
        Public Property ArrowPressColor As Color
            Get
                Return mArrowPressColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mArrowPressColor = value
            End Set
        End Property

        <Description("箭头边距。左右边距影响箭头的底边长，上下边距控制箭头的扁平程度")> _
        Public Property ArrowMargin As Padding
            Get
                Return mArrowMargin
            End Get
            Set(value As Padding)
                If mArrowMargin <> value Then
                    mArrowMargin = value
                    With mArrowMargin
                        .Left = Min(Max(.Left, 0), 9)
                        .Top = Min(Max(.Top, 0), 9)
                        .Right = Min(Max(.Right, 0), 9)
                        .Bottom = Min(Max(.Bottom, 0), 9)
                    End With
                End If
            End Set
        End Property

        <Description("滚动条箭头样式")> _
        Public Property ArrowType As ArrowTypeEnum
            Get
                Return mArrowType
            End Get
            Set(value As ArrowTypeEnum)
                mArrowType = value
            End Set
        End Property

        <Description("滚动条背景颜色")> _
        Public Property BackColor As Color
            Get
                Return mBackColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mBackColor = value
            End Set
        End Property

        <Description("滑块颜色")> _
        Public Property ThumbColor As Color
            Get
                Return mThumbColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mThumbColor = value
            End Set
        End Property

        <Description("鼠标悬停在滑块上的颜色")> _
        Public Property ThumbHoverColor As Color
            Get
                Return mThumbHoverColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mThumbHoverColor = value
            End Set
        End Property

        <Description("鼠标点击滑块时的颜色")> _
        Public Property ThumbPressColor As Color
            Get
                Return mThumbPressColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mThumbPressColor = value
            End Set
        End Property

        <Description("滑块边距")> _
        Public Property ThumbMargin As Byte
            Get
                Return mThumbMargin
            End Get
            Set(value As Byte)
                mThumbMargin = Min(Max(value, 0), 9)
            End Set
        End Property
#End Region
    End Class

    Private Class MetroScrollBarStyleConverter
        Inherits ExpandableObjectConverter

        Private mStdValues() As String = New String() {"VsDark", "VsLight", "MetroDark", "MetroLight", "SystemDark", "SystemLight"}

        Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
            If sourceType = GetType(String) Then
                Return True
            End If
            Return MyBase.CanConvertFrom(context, sourceType)
        End Function

        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            If destinationType = GetType(InstanceDescriptor) Then
                Return True
            End If
            Return MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
            If value.GetType = GetType(String) Then
                Return New MetroScrollBarStyle(CType(value, String))
            End If
            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
            If value.GetType = GetType(MetroScrollBarStyle) Then
                If destinationType = GetType(String) Then
                    Return CType(value, MetroScrollBarStyle).ToString
                End If
                If destinationType = GetType(InstanceDescriptor) Then
                    Dim temp = CType(value, MetroScrollBarStyle)
                    Dim ctor = GetType(MetroScrollBarStyle).GetConstructor(New Type() {GetType(Color), _
                                                                                       GetType(Color), _
                                                                                       GetType(Color), _
                                                                                       GetType(Padding), _
                                                                                       GetType(ArrowTypeEnum), _
                                                                                       GetType(Color), _
                                                                                       GetType(Color), _
                                                                                       GetType(Color), _
                                                                                       GetType(Color), _
                                                                                       GetType(Byte)})
                    Return New InstanceDescriptor(ctor, New Object() {temp.ArrowColor, _
                                                                      temp.ArrowHoverColor, _
                                                                      temp.ArrowPressColor, _
                                                                      temp.ArrowMargin, _
                                                                      temp.ArrowType, _
                                                                      temp.BackColor, _
                                                                      temp.ThumbColor, _
                                                                      temp.ThumbHoverColor, _
                                                                      temp.ThumbPressColor, _
                                                                      temp.ThumbMargin})
                End If
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

        Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(mStdValues)
        End Function

        Public Overrides Function CreateInstance(context As ITypeDescriptorContext, propertyValues As Collections.IDictionary) As Object
            Dim arColor = propertyValues("ArrowColor")
            Dim arhColor = propertyValues("ArrowHoverColor")
            Dim arpColor = propertyValues("ArrowPressColor")
            Dim arMargin = propertyValues("ArrowMargin")
            Dim arType = propertyValues("ArrowType")
            Dim bColor = propertyValues("BackColor")
            Dim tbColor = propertyValues("ThumbColor")
            Dim tbhColor = propertyValues("ThumbHoverColor")
            Dim tbpColor = propertyValues("ThumbPressColor")
            Dim tbMargin = propertyValues("ThumbMargin")
            Return New MetroScrollBarStyle(arColor, arhColor, arpColor, arMargin, arType, bColor, tbColor, tbhColor, tbpColor, tbMargin)
        End Function

        Public Overrides Function GetCreateInstanceSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class
#End Region
End Class
