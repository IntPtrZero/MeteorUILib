Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class MeteorButton : Inherits Button
    Implements IMeteorAnimation

    Private mRectImage As Rectangle
    Private mRectText As Rectangle
    ''' <summary>
    ''' 边框
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BORDER As Byte = 1
    ''' <summary>
    ''' 样式
    ''' </summary>
    ''' <remarks></remarks>
    Private mBorderStyle As BorderStyles

    Private mMouseState As MouseStates

    Private WithEvents mClickAnimationMgr As MeteorAnimationManager

    Public Enum BorderStyles As Byte
        None = 0
        Standard
    End Enum

    Private Enum MouseStates As Byte
        None = 0
        Hover
    End Enum

    Public Sub New()
        MyBase.New()
        mBorderStyle = BorderStyles.None
        mMouseState = MouseStates.None
        mClickAnimationMgr = New MeteorAnimationManager(False, AddressOf NextAnimationData)
    End Sub

    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        'MyBase.OnPaint(pevent)
        Dim g As Graphics = pevent.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        g.Clear(Parent.BackColor)

        '---鼠标悬停---
        If mMouseState = MouseStates.Hover Then
            g.FillRectangle(New SolidBrush(Color.FromArgb(100, Color.Gray)), Me.ClientRectangle)
        End If

        '---鼠标点击动画---
        If mClickAnimationMgr.IsAnimating Then
            For i = 0 To mClickAnimationMgr.Count - 1
                g.FillEllipse(New SolidBrush(Color.FromArgb(100 - 100 * mClickAnimationMgr.Value(i), Color.Black)), _
                              New Rectangle(mClickAnimationMgr.Loaction(i).X - mClickAnimationMgr.Value(i) * Me.Width, _
                                            mClickAnimationMgr.Loaction(i).Y - mClickAnimationMgr.Value(i) * Me.Width, _
                                            mClickAnimationMgr.Value(i) * Me.Width * 2, _
                                            mClickAnimationMgr.Value(i) * Me.Width * 2 _
                                            ) _
                              )
            Next
        End If

        '---焦点---
        If Me.Focused And ShowFocusCues Then
            Dim pen As New Pen(Color.Black, 1)
            pen.DashStyle = Drawing2D.DashStyle.Dot
            g.DrawRectangle(pen, New Rectangle(4, 4, Me.Width - 8, Me.Height - 8))
        End If

    End Sub

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        mClickAnimationMgr.Start(mevent.Location)
        MyBase.OnMouseDown(mevent)
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        mMouseState = MouseStates.Hover
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        mMouseState = MouseStates.None
        MyBase.OnMouseLeave(e)
    End Sub

    Private Function NextAnimationData(ByVal data As Single) As Single Implements IMeteorAnimation.NextAnimationData
        data += 0.03!
        Return IIf(data > mClickAnimationMgr.MaxValue, mClickAnimationMgr.MaxValue, data)
    End Function

    Private Sub OnAnimation(sender As Object, e As EventArgs) Handles mClickAnimationMgr.OnAnimation
        Me.Invalidate()
    End Sub

End Class
