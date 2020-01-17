''' <summary>
''' 继承此接口以自定义动画效果
''' </summary>
''' <remarks>使用动画效果的控件必须继承该接口</remarks>
Public Interface IMeteorAnimation
    ''' <summary>
    ''' 返回下一帧动画数据
    ''' </summary>
    ''' <param name="data">前一帧数据</param>
    ''' <returns>动画将要使用的数据</returns>
    ''' <remarks>控件根据动画数据绘制自己的动画效果</remarks>
    Function NextAnimationData(ByVal data As Single) As Single
End Interface
