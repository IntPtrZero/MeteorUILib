Imports System.Math
Imports System.Drawing
Imports System.ComponentModel
Imports MeteorUiLib.MeteorColorScheme
Imports MeteorUiLib.MeteorThemeManager
Imports System.ComponentModel.Design.Serialization

Namespace Design
    <TypeConverter(GetType(MetroFormStyleConverter))> _
    Public Class MetroFormStyle
#Region "变量"
        Private mBackColor As Color
        Private mBorderColor As Color
        Private mForeColor As Color
        Private mBtnHoverColor As Color
        Private mBtnPressColor As Color
        Private mSubCaptionColor As Color
        Private mMainCaptionColor As Color
        Private mExtendedCaptionColor As Color
        Private mSubCaptionHeight As Byte
        Private mExtendedCaptionHeight As Byte
#End Region

#Region "构造函数"
        Public Sub New()
            Me.New("MetroLight")
        End Sub

        Public Sub New(ByVal strStyle As String)
            Select Case strStyle
                Case "MetroLight"
                    mBackColor = BACKCOLOR_LIGHT
                    mBorderColor = MaterialGrey600
                    mForeColor = FORECOLOR_DARK
                    mBtnHoverColor = MaterialGrey200
                    mBtnPressColor = MaterialGrey300
                    mSubCaptionColor = MetroDodgerBlue
                    mMainCaptionColor = BACKCOLOR_LIGHT
                    mExtendedCaptionColor = BACKCOLOR_LIGHT
                    mSubCaptionHeight = 6
                    mExtendedCaptionHeight = 32

                Case "MetroDark"
                    mBackColor = BACKCOLOR_DARK
                    mBorderColor = MaterialBrown400
                    mForeColor = FORECOLOR_LIGHT
                    mBtnHoverColor = MaterialGrey800
                    mBtnPressColor = MaterialGrey700
                    mSubCaptionColor = MetroChocolate
                    mMainCaptionColor = BACKCOLOR_DARK
                    mExtendedCaptionColor = BACKCOLOR_DARK
                    mSubCaptionHeight = 6
                    mExtendedCaptionHeight = 32

                Case "MaterialLight"
                    mBackColor = BACKCOLOR_LIGHT
                    mBorderColor = MaterialGreen700
                    mForeColor = FORECOLOR_DARK
                    mBtnHoverColor = MaterialGreen800
                    mBtnPressColor = MaterialGreen900
                    mSubCaptionColor = BACKCOLOR_LIGHT
                    mMainCaptionColor = MaterialGreen700
                    mExtendedCaptionColor = MaterialGreen600
                    mSubCaptionHeight = 0
                    mExtendedCaptionHeight = 72

                Case "MaterialDark"
                    mBackColor = BACKCOLOR_DARK
                    mBorderColor = MaterialBlueGrey900
                    mForeColor = FORECOLOR_LIGHT
                    mBtnHoverColor = MaterialBlueGrey700
                    mBtnPressColor = MaterialBlueGrey600
                    mSubCaptionColor = BACKCOLOR_DARK
                    mMainCaptionColor = MaterialBlueGrey900
                    mExtendedCaptionColor = MaterialBlueGrey800
                    mSubCaptionHeight = 0
                    mExtendedCaptionHeight = 72

                Case "PureLight"
                    mBackColor = BACKCOLOR_LIGHT
                    mBorderColor = MetroMalibu
                    mForeColor = FORECOLOR_DARK
                    mBtnHoverColor = MaterialGrey200
                    mBtnPressColor = MaterialGrey300
                    mSubCaptionColor = BACKCOLOR_LIGHT
                    mMainCaptionColor = BACKCOLOR_LIGHT
                    mExtendedCaptionColor = BACKCOLOR_LIGHT
                    mSubCaptionHeight = 0
                    mExtendedCaptionHeight = 32

                Case "PureDark"
                    mBackColor = BACKCOLOR_DARK
                    mBorderColor = MetroMaroon
                    mForeColor = FORECOLOR_LIGHT
                    mBtnHoverColor = MaterialGrey800
                    mBtnPressColor = MaterialGrey700
                    mSubCaptionColor = BACKCOLOR_DARK
                    mMainCaptionColor = BACKCOLOR_DARK
                    mExtendedCaptionColor = BACKCOLOR_DARK
                    mSubCaptionHeight = 0
                    mExtendedCaptionHeight = 32

                Case Else
                    Throw New ArgumentException("无效的参数")
            End Select
        End Sub

        Public Sub New(ByVal bdColor As Color, _
                       ByVal bkColor As Color, _
                       ByVal bhColor As Color, _
                       ByVal bpColor As Color, _
                       ByVal frColor As Color, _
                       ByVal scColor As Color, _
                       ByVal mcColor As Color, _
                       ByVal ecColor As Color, _
                       ByVal scHeight As Byte, _
                       ByVal ecHeight As Byte)
            mBorderColor = bdColor
            mBackColor = bkColor
            mBtnHoverColor = bhColor
            mBtnPressColor = bpColor
            mForeColor = frColor
            mSubCaptionColor = scColor
            mMainCaptionColor = mcColor
            mExtendedCaptionColor = ecColor
            mSubCaptionHeight = scHeight
            mExtendedCaptionHeight = ecHeight
        End Sub
#End Region

#Region "重载"
        Public Overloads Shared Operator =(ByVal param1 As MetroFormStyle, ByVal param2 As MetroFormStyle)
            If param1.mBorderColor = param2.mBorderColor AndAlso _
                param1.mBackColor = param2.mBackColor AndAlso _
                param1.mBtnHoverColor = param2.mBtnHoverColor AndAlso _
                param1.mBtnPressColor = param2.mBtnPressColor AndAlso _
                param1.mForeColor = param2.mForeColor AndAlso _
                param1.mSubCaptionColor = param2.mSubCaptionColor AndAlso _
                param1.mMainCaptionColor = param2.mMainCaptionColor AndAlso _
                param1.mExtendedCaptionColor = param2.mExtendedCaptionColor AndAlso _
                param1.mSubCaptionHeight = param2.mSubCaptionHeight AndAlso _
                param1.mExtendedCaptionHeight = param2.mExtendedCaptionHeight Then
                Return True
            End If
            Return False
        End Operator

        Public Overloads Shared Operator <>(ByVal param1 As MetroFormStyle, ByVal param2 As MetroFormStyle)
            Return Not (param1 = param2)
        End Operator

        Public Overrides Function ToString() As String
            Select Case Me
                Case Is = New MetroFormStyle("MetroLight")
                    Return "MetroLight"
                Case Is = New MetroFormStyle("MetroDark")
                    Return "MetroDark"
                Case Is = New MetroFormStyle("MaterialLight")
                    Return "MaterialLight"
                Case Is = New MetroFormStyle("MaterialDark")
                    Return "MaterialDark"
                Case Is = New MetroFormStyle("PureLight")
                    Return "PureLight"
                Case Is = New MetroFormStyle("PureDark")
                    Return "PureDark"
                Case Else
                    Return "Custom"
            End Select
        End Function
#End Region

#Region "属性"
        <Description("窗口边框颜色")> _
        Public Property BorderColor As Color
            Get
                Return mBorderColor
            End Get
            Set(value As Color)
                mBorderColor = value
            End Set
        End Property

        <Description("窗口背景色")> _
        Public Property BackColor As Color
            Get
                Return mBackColor
            End Get
            Set(value As Color)
                mBackColor = value
            End Set
        End Property

        <Description("标题栏按钮悬停时的颜色")> _
        Public Property ButtonHoverColor As Color
            Get
                Return mBtnHoverColor
            End Get
            Set(value As Color)
                mBtnHoverColor = value
            End Set
        End Property

        <Description("标题栏按钮按下时的颜色")> _
        Public Property ButtonPressColor As Color
            Get
                Return mBtnPressColor
            End Get
            Set(value As Color)
                mBtnPressColor = value
            End Set
        End Property

        <Description("窗口前景色")> _
        Public Property ForeColor As Color
            Get
                Return mForeColor
            End Get
            Set(value As Color)
                mForeColor = value
            End Set
        End Property

        <Description("次标题栏颜色")> _
        Public Property SubCaptionColor As Color
            Get
                Return mSubCaptionColor
            End Get
            Set(value As Color)
                mSubCaptionColor = value
            End Set
        End Property

        <Description("主标题栏颜色")> _
        Public Property MainCaptionColor As Color
            Get
                Return mMainCaptionColor
            End Get
            Set(value As Color)
                mMainCaptionColor = value
            End Set
        End Property

        <Description("扩展标题栏颜色")> _
        Public Property ExtendedCaptionColor As Color
            Get
                Return mExtendedCaptionColor
            End Get
            Set(value As Color)
                mExtendedCaptionColor = value
            End Set
        End Property

        <Description("次标题栏高度")> _
        Public Property SubCaptionHeight As Byte
            Get
                Return mSubCaptionHeight
            End Get
            Set(value As Byte)
                mSubCaptionHeight = Min(Max(0, value), 8)
            End Set
        End Property

        <Description("扩展标题栏高度")> _
        Public Property ExtendedCaptionHeight As Byte
            Get
                Return mExtendedCaptionHeight
            End Get
            Set(value As Byte)
                mExtendedCaptionHeight = Min(Max(0, value), 100)
            End Set
        End Property
#End Region
    End Class

    Public Class MetroFormStyleConverter
        Inherits ExpandableObjectConverter

        Private stdValues() As String = New String() {"MetroLight", "MetroDark", "MaterialLight", "MaterialDark", "PureLight", "PureDark"}

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
                Return New MetroFormStyle(CType(value, String))
            End If
            Return MyBase.ConvertFrom(context, culture, value)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
            If value.GetType = GetType(MetroFormStyle) Then
                If destinationType = GetType(String) Then
                    Return CType(value, MetroFormStyle).ToString
                End If
                If destinationType = GetType(InstanceDescriptor) Then
                    Dim tmp = CType(value, MetroFormStyle)
                    Dim ctor = GetType(MetroFormStyle).GetConstructor(New Type() {GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Color), _
                                                                                  GetType(Byte), _
                                                                                  GetType(Byte)})
                    Return New InstanceDescriptor(ctor, New Object() {tmp.BorderColor, _
                                                                      tmp.BackColor, _
                                                                      tmp.ButtonHoverColor, _
                                                                      tmp.ButtonPressColor, _
                                                                      tmp.ForeColor, _
                                                                      tmp.SubCaptionColor, _
                                                                      tmp.MainCaptionColor, _
                                                                      tmp.ExtendedCaptionColor, _
                                                                      tmp.SubCaptionHeight, _
                                                                      tmp.ExtendedCaptionHeight})
                End If
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

        Public Overrides Function CreateInstance(context As ITypeDescriptorContext, propertyValues As Collections.IDictionary) As Object
            Return New MetroFormStyle(propertyValues("BorderColor"), _
                                      propertyValues("BackColor"), _
                                      propertyValues("ButtonHoverColor"), _
                                      propertyValues("ButtonPressColor"), _
                                      propertyValues("ForeColor"), _
                                      propertyValues("SubCaptionColor"), _
                                      propertyValues("MainCaptionColor"), _
                                      propertyValues("ExtendedCaptionColor"), _
                                      propertyValues("SubCaptionHeight"), _
                                      propertyValues("ExtendedCaptionHeight"))
        End Function

        Public Overrides Function GetCreateInstanceSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(stdValues)
        End Function

        Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class
End Namespace
