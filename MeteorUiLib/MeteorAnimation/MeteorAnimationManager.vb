Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class MeteorAnimationManager

    ''' <summary>
    ''' 动画时钟
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnimationTimer As Timer
    ''' <summary>
    ''' 是否同时支持多个动画
    ''' </summary>
    ''' <remarks></remarks>
    Private mSingleAnimation As Boolean
    ''' <summary>
    ''' 动画过渡效果
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnimationReplaceType As AnimationReplaceType
    ''' <summary>
    ''' 动画数据
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnimationData As List(Of Single)
    ''' <summary>
    ''' 动画发生的坐标
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnimationLocation As List(Of Point)
    ''' <summary>
    ''' 动画间隔(毫秒)
    ''' </summary>
    ''' <remarks></remarks>
    Private mAnimationInterval As Integer
    ''' <summary>
    ''' 下一帧动画
    ''' </summary>
    ''' <remarks></remarks>
    Private NextKeyFrameDataCallBack As Func(Of Single, Single)

    Public ReadOnly MaxValue As Single
    Public ReadOnly MinValue As Single

    Public Event OnAnimation As EventHandler

    ''' <summary>
    ''' 当只支持单个动画时，指示动画之间如何过渡
    ''' </summary>
    ''' <remarks>SingleAnimation为True时才有效</remarks>
    Public Enum AnimationReplaceType As Byte
        ''' <summary>
        ''' 中断前一个动画
        ''' </summary>
        ''' <remarks></remarks>
        Interrupt = 0
        ''' <summary>
        ''' 等待前一个动画完成
        ''' </summary>
        ''' <remarks></remarks>
        AwaitEnds
    End Enum

    Public Sub New(ByVal singular As Boolean, ByVal nkfdCallBack As Func(Of Single, Single), Optional ByVal art As AnimationReplaceType = AnimationReplaceType.Interrupt, Optional ByVal itv As Integer = 15, Optional ByVal min As Single = 0.0!, Optional ByVal max As Single = 1.0!)
        mSingleAnimation = singular
        mAnimationReplaceType = art
        MinValue = min
        MaxValue = max
        mAnimationInterval = itv
        mAnimationData = New List(Of Single)
        mAnimationLocation = New List(Of Point)
        mAnimationTimer = New Timer With {.Interval = mAnimationInterval, .Enabled = False}
        AddHandler mAnimationTimer.Tick, AddressOf AnimateTimer_Tick
        NextKeyFrameDataCallBack = nkfdCallBack
    End Sub

    ''' <summary>
    ''' 当前动画队列
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Count As Integer
        Get
            Return mAnimationData.Count
        End Get
    End Property

    Public ReadOnly Property Loaction(ByVal index As Integer) As Point
        Get
            Return mAnimationLocation(index)
        End Get
    End Property

    Public ReadOnly Property Value(ByVal index As Integer) As Single
        Get
            Return mAnimationData(index)
        End Get
    End Property

    Public ReadOnly Property IsAnimating As Boolean
        Get
            Return mAnimationTimer.Enabled
        End Get
    End Property

    Private Sub AnimateTimer_Tick(sender As Object, e As EventArgs)
        If (mAnimationData.Count) And (NextKeyFrameDataCallBack IsNot Nothing) Then
            For i = 0 To mAnimationData.Count - 1
                mAnimationData(i) = NextKeyFrameDataCallBack.Invoke(mAnimationData(i))
            Next
            '单个动画多用于Hover和Focus等效果，动画结束后需要保留最后一帧，因此不移除
            If mSingleAnimation Then
                If mAnimationData(0) < MaxValue Then
                    RaiseEvent OnAnimation(Me, Nothing)
                Else
                    mAnimationTimer.Stop()
                End If
            Else
                '可叠加动画多用于Click效果，动画结束后需要即时移除
                For i = mAnimationData.Count - 1 To 0 Step -1
                    If mAnimationData(i) >= MaxValue Then
                        mAnimationData.RemoveAt(i)
                        mAnimationLocation.RemoveAt(i)
                    End If
                Next
                RaiseEvent OnAnimation(Me, Nothing)
            End If
        Else
            mAnimationTimer.Stop()
        End If
    End Sub

    Public Sub Start(ByVal loc As Point, Optional ByVal value As Single = 0.0!)
        If mSingleAnimation And Count Then
            Select Case mAnimationReplaceType
                Case AnimationReplaceType.Interrupt
                    mAnimationData(0) = value
                    mAnimationLocation(0) = loc
                Case AnimationReplaceType.AwaitEnds
                    '等待动画完成
            End Select
        Else
            mAnimationLocation.Add(loc)
            mAnimationData.Add(value)
        End If
        mAnimationTimer.Start()
    End Sub
End Class