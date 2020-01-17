Imports System.Math
Imports System.Drawing
Imports MeteorUiLib.Native
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

<Description("Metro风格滑动条控件"), ToolboxBitmap(GetType(TrackBar)), DefaultEvent("Scroll"), DefaultProperty("Value")> _
Public Class MeteorTrackBar : Inherits Control

#Region "变量"
    Private Shared ReadOnly EVENT_SCROLL As Object = New Object()
    Private Shared ReadOnly EVENT_VALUECHANGED As Object = New Object()

    Private mMinValue As Integer = 0
    Private mMaxValue As Integer = 100
    Private mSmallChange As Integer = 1
    Private mLargeChange As Integer = 10
    Private mValueRange As Integer = 100
    Private mValue As Integer = 0
    Private mTrackOrientation As ScrollOrientation

    Private mThumbPos As Point
    Private mRectTrack As Rectangle
    Private mRectTrackHT As Rectangle
    Private Shared ReadOnly TRACK_HT_COMPENSATE As Integer = 5
    Private mRectTrackFilled As Rectangle
    Private mRectThumb As Rectangle

    Private ptMouse As Point
    Private ptMouseOld As Point
    Private ptMouseNew As Point

    Private mTrackBrush As Brush
    Private mThumbBrush As Brush
    Private mDisabledColor As Color
    Private fThumbPress As Boolean = False
    Private mTrackBarStyle As MetroTrackBarStyle
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
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        Me.TabStop = False
        Me.BackColor = Color.Transparent

        mTrackBarStyle = New MetroTrackBarStyle()
        InitDrawing()
        mTrackOrientation = orientation
        SetUpTrackBar()
    End Sub
#End Region

#Region "隐藏"
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
    <DefaultValue(0), Description("滑块位置的最小值")> _
    Public Property Minimum As Integer
        Get
            Return mMinValue
        End Get
        Set(value As Integer)
            If mMinValue <> value And value < mMaxValue Then
                mMinValue = value
                mValueRange = mMaxValue - mMinValue
                mLargeChange = Min(mLargeChange, mValueRange)
                mSmallChange = Min(mSmallChange, mLargeChange)
                mValue = Max(mValue, mMinValue)
                SetUpThumb()
                Me.Invalidate()
            End If
        End Set
    End Property

    <DefaultValue(100), Description("滑块位置的最大值")> _
    Public Property Maximum As Integer
        Get
            Return mMaxValue
        End Get
        Set(value As Integer)
            If mMaxValue <> value And value > mMinValue Then
                mMaxValue = value
                mValueRange = mMaxValue - mMinValue
                mLargeChange = Min(mLargeChange, mValueRange)
                mSmallChange = Min(mSmallChange, mLargeChange)
                mValue = Min(mValue, mMaxValue)
                SetUpThumb()
                Me.Invalidate()
            End If
        End Set
    End Property

    <DefaultValue(1), Description("使用键盘方向键进行滑动时的变化幅度。")> _
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

    <DefaultValue(10), Description("用户按下PageUp、PageDown时滑块的变化幅度。")> _
    Public Property LargeChange As Integer
        Get
            Return mLargeChange
        End Get
        Set(value As Integer)
            If mLargeChange <> value And value >= mSmallChange Then
                mLargeChange = Min(value, mValueRange)
            End If
        End Set
    End Property

    <DefaultValue(0), Description("滑块位置表示的值")> _
    Public Property Value As Integer
        Get
            Return mValue
        End Get
        Set(value As Integer)
            If mValue <> value Then
                Dim newValue As Integer = Min(Max(value, mMinValue), mMaxValue)
                If mValue <> newValue Then
                    mValue = newValue
                    OnValueChanged(EventArgs.Empty)
                End If
            End If
        End Set
    End Property

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Select Case mTrackOrientation
                Case ScrollOrientation.HorizontalScroll
                    Return New Size(200, 21)
                Case ScrollOrientation.VerticalScroll
                    Return New Size(21, 200)
            End Select
        End Get
    End Property

    <Description("滑动条的方向")> _
    Public Property Orientation As ScrollOrientation
        Get
            Return mTrackOrientation
        End Get
        Set(value As ScrollOrientation)
            If mTrackOrientation <> value Then
                mTrackOrientation = value
                Me.Size = New Size(Size.Height, Size.Width)
            End If
        End Set
    End Property

    <Description("滑动条的样式。务必直接修改此属性，禁止在运行时修改下级属性。"), DefaultValue(GetType(MetroTrackBarStyle), "MetroLight")> _
    Public Property TrackBarStyle As MetroTrackBarStyle
        Get
            Return mTrackBarStyle
        End Get
        Set(value As MetroTrackBarStyle)
            If mTrackBarStyle <> value Then
                mTrackBarStyle = value
                InitDrawing()
                SetUpTrackBar()
                Me.Invalidate()
            End If
        End Set
    End Property
#End Region

#Region "方法重写"
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        DrawTrackBar(e.Graphics)
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        SetUpTrackBar()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        ptMouse = e.Location
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If mRectThumb.Contains(ptMouse) Then
                fThumbPress = True
                ptMouseOld = ptMouse
                Return
            End If
            If mRectTrackHT.Contains(ptMouse) Then
                DoScroll(ScrollEventType.ThumbPosition)
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        fThumbPress = False
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If fThumbPress Then
                ptMouseNew = e.Location
                DoScroll(ScrollEventType.ThumbTrack)
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        mThumbBrush = New SolidBrush(mTrackBarStyle.HighLightColor)
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        mThumbBrush = New SolidBrush(mTrackBarStyle.ThumbColor)
        Me.Invalidate()
    End Sub

    Protected Overrides Function ProcessDialogKey(keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Left
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.SmallDecrement)
                    Return True
                End If
            Case Keys.Right
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.SmallIncrement)
                    Return True
                End If
            Case Keys.Up
                If mTrackOrientation = ScrollOrientation.VerticalScroll Then
                    DoScroll(ScrollEventType.SmallIncrement)
                    Return True
                End If
            Case Keys.Down
                If mTrackOrientation = ScrollOrientation.VerticalScroll Then
                    DoScroll(ScrollEventType.SmallDecrement)
                    Return True
                End If
            Case Keys.PageUp
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.LargeDecrement)
                Else
                    DoScroll(ScrollEventType.LargeIncrement)
                End If
                Return True
            Case Keys.PageDown
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.LargeIncrement)
                Else
                    DoScroll(ScrollEventType.LargeDecrement)
                End If
                Return True
            Case Keys.Home
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.First)
                Else
                    DoScroll(ScrollEventType.Last)
                End If
                Return True
            Case Keys.End
                If mTrackOrientation = ScrollOrientation.HorizontalScroll Then
                    DoScroll(ScrollEventType.Last)
                Else
                    DoScroll(ScrollEventType.First)
                End If
                Return True
        End Select
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        If Me.Enabled Then
            mThumbBrush = New SolidBrush(mTrackBarStyle.ThumbColor)
        Else
            mThumbBrush = New SolidBrush(mDisabledColor)
        End If
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overridable Sub OnValueChanged(e As EventArgs)
        SetUpThumb()
        Me.Invalidate()
        RaiseEvent ValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnScroll(e As ScrollEventArgs)
        RaiseEvent Scroll(Me, e)
    End Sub
#End Region

#Region "私有方法"
    Private Sub SetUpTrackBar()
        Select Case mTrackOrientation
            Case ScrollOrientation.HorizontalScroll
                mRectTrack = New Rectangle(0, (Me.Height - mTrackBarStyle.TrackThickness) / 2, Me.Width, mTrackBarStyle.TrackThickness)
                mRectTrackHT = New Rectangle(mRectTrack.X, mRectTrack.Y - TRACK_HT_COMPENSATE, mRectTrack.Width, mRectTrack.Height + TRACK_HT_COMPENSATE * 2)

            Case ScrollOrientation.VerticalScroll
                mRectTrack = New Rectangle((Me.Width - mTrackBarStyle.TrackThickness) / 2, 0, mTrackBarStyle.TrackThickness, Me.Height)
                mRectTrackHT = New Rectangle(mRectTrack.X - TRACK_HT_COMPENSATE, mRectTrack.Y, mRectTrack.Width + TRACK_HT_COMPENSATE * 2, mRectTrack.Height)
        End Select
        SetUpThumb()
    End Sub

    Private Sub SetUpThumb()
        mThumbPos = GetThumbPos()
        Select Case mTrackOrientation
            Case ScrollOrientation.HorizontalScroll
                mRectThumb = New Rectangle(mThumbPos, mTrackBarStyle.ThumbSize)
                mRectTrackFilled = New Rectangle(mRectTrack.X, mRectTrack.Y, mRectThumb.X - mRectTrack.X, mTrackBarStyle.TrackThickness)
            Case ScrollOrientation.VerticalScroll
                mRectThumb = New Rectangle(mThumbPos, New Size(mTrackBarStyle.ThumbSize.Height, mTrackBarStyle.ThumbSize.Width))
                mRectTrackFilled = New Rectangle(mRectTrack.X, mRectThumb.Y + mTrackBarStyle.ThumbSize.Width, mTrackBarStyle.TrackThickness, mRectTrack.Height - mRectThumb.Y - mTrackBarStyle.ThumbSize.Width)
        End Select
    End Sub

    Private Function GetThumbPos() As Point
        Select Case mTrackOrientation
            Case ScrollOrientation.HorizontalScroll
                Return New Point((Me.Width - mTrackBarStyle.ThumbSize.Width) * (mValue - mMinValue) / mValueRange, (Me.Height - mTrackBarStyle.ThumbSize.Height) / 2)
            Case ScrollOrientation.VerticalScroll
                Return New Point((Me.Width - mTrackBarStyle.ThumbSize.Height) / 2, (Me.Height - mTrackBarStyle.ThumbSize.Width) * (1 - (mValue - mMinValue) / mValueRange))
        End Select
        Return Point.Empty
    End Function

    Private Function PosToValue(ByVal pos As Point) As Integer
        Select Case mTrackOrientation
            Case ScrollOrientation.HorizontalScroll
                Return pos.X / (Me.Width - mTrackBarStyle.ThumbSize.Width) * mValueRange + mMinValue
            Case ScrollOrientation.VerticalScroll
                Return (1 - pos.Y / (Me.Height - mTrackBarStyle.ThumbSize.Height)) * mValueRange + mMinValue
        End Select
        Return 0
    End Function

    Private Sub DoScroll(ByVal type As ScrollEventType)
        Dim newValue As Integer = mValue
        Dim oldValue As Integer = mValue
        Select Case type
            Case ScrollEventType.First
                newValue = mMinValue

            Case ScrollEventType.Last
                newValue = mMaxValue

            Case ScrollEventType.SmallDecrement
                newValue = Max(oldValue - mSmallChange, mMinValue)

            Case ScrollEventType.SmallIncrement
                newValue = Min(oldValue + mSmallChange, mMaxValue)

            Case ScrollEventType.LargeDecrement
                newValue = Max(oldValue - mLargeChange, mMinValue)

            Case ScrollEventType.LargeIncrement
                newValue = Min(oldValue + mLargeChange, mMaxValue)

            Case ScrollEventType.ThumbPosition
                newValue = Min(Max(PosToValue(ptMouse), mMinValue), mMaxValue)

            Case ScrollEventType.ThumbTrack
                newValue = Min(Max(oldValue + PosToValue(ptMouseNew) - PosToValue(ptMouseOld), mMinValue), mMaxValue)
                ptMouseOld = ptMouseNew
        End Select

        If newValue <> oldValue Then
            Value = newValue
            Dim e As New ScrollEventArgs(type, oldValue, newValue, mTrackOrientation)
            OnScroll(e)
        End If
    End Sub

    Private Sub InitDrawing()
        mTrackBrush = New SolidBrush(mTrackBarStyle.TrackColor)
        mThumbBrush = New SolidBrush(mTrackBarStyle.ThumbColor)
        mDisabledColor = Color.FromArgb(mTrackBarStyle.TrackColor.R + (CInt(mTrackBarStyle.ThumbColor.R) - mTrackBarStyle.TrackColor.R) * 0.6F, _
                                        mTrackBarStyle.TrackColor.G + (CInt(mTrackBarStyle.ThumbColor.G) - mTrackBarStyle.TrackColor.G) * 0.6F, _
                                        mTrackBarStyle.TrackColor.B + (CInt(mTrackBarStyle.ThumbColor.B) - mTrackBarStyle.TrackColor.B) * 0.6F)
    End Sub

    Private Sub DrawTrackBar(ByVal g As Graphics)
        g.FillRectangle(mTrackBrush, mRectTrack)
        g.FillRectangle(mThumbBrush, mRectTrackFilled)
        g.FillRectangle(mThumbBrush, mRectThumb)
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
    <TypeConverter(GetType(MetroTrackBarStyleConverter))> _
    Public Class MetroTrackBarStyle
#Region "变量"
        Private mTrackColor As Color
        Private mThumbColor As Color
        Private mHighLightColor As Color
        Private mThumbSize As Size
        Private mTrackThickness As Integer
#End Region

#Region "构造函数"
        Public Sub New()
            Me.New("MetroLight")
        End Sub

        Public Sub New(ByVal strStyle As String)
            Select Case strStyle
                Case "MetroDark"
                    mTrackColor = Color.FromArgb(51, 51, 51)
                    mThumbColor = Color.FromArgb(160, 160, 160)
                    mHighLightColor = Color.FromArgb(204, 204, 204)
                    mThumbSize = New Size(7, 21)
                    mTrackThickness = 5

                Case "MetroLight"
                    mTrackColor = Color.FromArgb(204, 204, 204)
                    mThumbColor = Color.FromArgb(96, 96, 96)
                    mHighLightColor = Color.FromArgb(48, 48, 48)
                    mThumbSize = New Size(7, 21)
                    mTrackThickness = 5

                Case Else
                    Throw New ArgumentException("无效的参数")
            End Select
        End Sub

        Public Sub New(ByVal tkColor As Color, _
                       ByVal tbColor As Color, _
                       ByVal hlColor As Color, _
                       ByVal tbSize As Size, _
                       ByVal tkThick As Integer)
            mTrackColor = tkColor
            mThumbColor = tbColor
            mHighLightColor = hlColor
            mThumbSize = tbSize
            mTrackThickness = tkThick
        End Sub
#End Region

#Region "重载"
        Public Overloads Shared Operator =(ByVal param1 As MetroTrackBarStyle, ByVal param2 As MetroTrackBarStyle)
            If param1.mTrackColor = param2.mTrackColor And _
                param1.mThumbColor = param2.mThumbColor And _
                param1.mHighLightColor = param2.mHighLightColor And _
                param1.mThumbSize = param2.mThumbSize And _
                param1.mTrackThickness = param2.mTrackThickness Then
                Return True
            End If
            Return False
        End Operator

        Public Overloads Shared Operator <>(ByVal param1 As MetroTrackBarStyle, ByVal param2 As MetroTrackBarStyle)
            Return Not (param1 = param2)
        End Operator

        Public Overrides Function ToString() As String
            Select Case Me
                Case Is = New MetroTrackBarStyle("MetroDark")
                    Return "MetroDark"
                Case Is = New MetroTrackBarStyle("MetroLight")
                    Return "MetroLight"
                Case Else
                    Return "Custom"
            End Select
        End Function
#End Region

#Region "属性"
        <Description("轨道颜色")> _
        Public Property TrackColor As Color
            Get
                Return mTrackColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mTrackColor = value
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

        <Description("鼠标进入控件时的颜色")> _
        Public Property HighLightColor As Color
            Get
                Return mHighLightColor
            End Get
            Set(value As Color)
                If value = Color.Transparent OrElse value = Color.Empty Then
                    Return
                End If
                mHighLightColor = value
            End Set
        End Property

        <Description("滑块大小")> _
        Public Property ThumbSize As Size
            Get
                Return mThumbSize
            End Get
            Set(value As Size)
                mThumbSize = value
            End Set
        End Property

        <Description("轨道宽度")> _
        Public Property TrackThickness As Integer
            Get
                Return mTrackThickness
            End Get
            Set(value As Integer)
                mTrackThickness = value
            End Set
        End Property
#End Region
    End Class

    Private Class MetroTrackBarStyleConverter
        Inherits ExpandableObjectConverter

        Private mStdValues() As String = New String() {"MetroDark", "MetroLight"}

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
                Return New MetroTrackBarStyle(CType(value, String))
            End If
            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
            If value.GetType = GetType(MetroTrackBarStyle) Then
                If destinationType = GetType(String) Then
                    Return CType(value, MetroTrackBarStyle).ToString
                End If
                If destinationType = GetType(InstanceDescriptor) Then
                    Dim temp As MetroTrackBarStyle = CType(value, MetroTrackBarStyle)
                    Dim ctor = GetType(MetroTrackBarStyle).GetConstructor(New Type() {GetType(Color), _
                                                                                      GetType(Color), _
                                                                                      GetType(Color), _
                                                                                      GetType(Size), _
                                                                                      GetType(Integer)})
                    Return New InstanceDescriptor(ctor, New Object() {temp.TrackColor, _
                                                                      temp.ThumbColor, _
                                                                      temp.HighLightColor, _
                                                                      temp.ThumbSize, _
                                                                      temp.TrackThickness})
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
            Dim trColor = propertyValues("TrackColor")
            Dim tbColor = propertyValues("ThumbColor")
            Dim hlColor = propertyValues("HighLightColor")
            Dim tbSize = propertyValues("ThumbSize")
            Dim tkThickness = propertyValues("TrackThickness")
            Return New MetroTrackBarStyle(trColor, tbColor, hlColor, tbSize, tkThickness)
        End Function

        Public Overrides Function GetCreateInstanceSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class
#End Region
End Class
