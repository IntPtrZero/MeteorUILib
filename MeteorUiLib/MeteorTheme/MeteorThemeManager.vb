Imports System.Drawing
Imports System.ComponentModel
Imports MeteorUiLib.MeteorColorScheme


Public NotInheritable Class MeteorThemeManager

    Private mTheme As MeteorTheme
    Private mColorScheme As MeteorColorScheme
    Private Shared ReadOnly mInstance As MeteorThemeManager = New MeteorThemeManager()

    '//背景颜色
    Friend Shared ReadOnly BACKCOLOR_LIGHT As Color = Color.White
    Friend Shared ReadOnly BACKCOLOR_DARK As Color = Color.FromArgb(51, 51, 51)
    Friend Shared ReadOnly BACKBRUSH_LIGHT As Brush = Brushes.White
    Friend Shared ReadOnly BACKBRUSH_DARK As Brush = New SolidBrush(BACKCOLOR_DARK)

    '//前景字体颜色
    Friend Shared ReadOnly FORECOLOR_LIGHT As Color = Color.FromArgb(234, 234, 234)
    Friend Shared ReadOnly FORECOLOR_DARK As Color = Color.FromArgb(31, 31, 31)
    Friend Shared ReadOnly FOREPEN_LIGHT As Pen = New Pen(FORECOLOR_LIGHT)
    Friend Shared ReadOnly FOREPEN2_LIGHT As Pen = New Pen(FORECOLOR_LIGHT, 2.0F)
    Friend Shared ReadOnly FOREPEN_DARK As Pen = New Pen(FORECOLOR_DARK)
    Friend Shared ReadOnly FOREPEN2_DARK As Pen = New Pen(FORECOLOR_DARK, 2.0F)
    Friend Shared ReadOnly DISABLEDTEXTCOLOR_LIGHT As Color = Color.FromArgb(204, 204, 204)
    Friend Shared ReadOnly DISABLEDTEXTCOLOR_DARK As Color = Color.FromArgb(81, 81, 81)

    Public Enum MeteorTheme As Byte
        Light = 0
        Dark = 1
    End Enum

    Private Sub New()
        mTheme = MeteorTheme.Light
        mColorScheme = New MeteorColorScheme(0)
    End Sub

    ''' <summary>
    ''' 主题
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Theme As MeteorTheme
        Get
            Return mTheme
        End Get
        Set(value As MeteorTheme)
            If value <> mTheme Then
                mTheme = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' 配色方案
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ColorScheme As MeteorColorScheme
        Get
            Return mColorScheme
        End Get
        Set(value As MeteorColorScheme)
            mColorScheme = value
        End Set
    End Property

    ''' <summary>
    ''' 获取MeteorThemeManager的实例
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Instance As MeteorThemeManager
        Get
            Return mInstance
        End Get
    End Property

    ''' <summary>
    ''' 窗口和某些容器控件的背景色
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BackColor As Color
        Get
            Select Case mTheme
                Case MeteorTheme.Light
                    Return BACKCOLOR_LIGHT
                Case MeteorTheme.Dark
                    Return BACKCOLOR_DARK
            End Select
        End Get
    End Property

    ''' <summary>
    ''' 窗口和控件的前景色
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ForeColor As Color
        Get
            Select Case mTheme
                Case MeteorTheme.Light
                    Return FORECOLOR_DARK
                Case MeteorTheme.Dark
                    Return FORECOLOR_LIGHT
            End Select
        End Get
    End Property

    ''' <summary>
    ''' 背景画刷
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BackBrush As Brush
        Get
            Select Case mTheme
                Case MeteorTheme.Light
                    Return BACKBRUSH_LIGHT
                Case MeteorTheme.Dark
                    Return BACKBRUSH_DARK
            End Select
            Return Brushes.Transparent
        End Get
    End Property

    ''' <summary>
    ''' 普通文字画笔
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TextPen As Pen
        Get
            Select Case mTheme
                Case MeteorTheme.Light
                    Return FOREPEN_DARK
                Case MeteorTheme.Dark
                    Return FOREPEN_LIGHT
            End Select
            Return Pens.Transparent
        End Get
    End Property

    ''' <summary>
    ''' 普通文字画笔，2px
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TextPen2 As Pen
        Get
            Select Case mTheme
                Case MeteorTheme.Light
                    Return FOREPEN2_DARK
                Case MeteorTheme.Dark
                    Return FOREPEN2_LIGHT
            End Select
            Return Pens.Transparent
        End Get
    End Property

End Class
