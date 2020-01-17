<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MeteorForm1
    Inherits MeteorUiLib.MeteorForm

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意:  以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'MeteorForm1
        '
        Me.ClientSize = New System.Drawing.Size(717, 499)
        Me.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Name = "MeteorForm1"
        Me.SizeGrip = True
        Me.SizeGripStyle = MeteorUiLib.MeteorForm.SizeGripStyleEnum.Grid
        Me.Text = "MeteorForm1"
        Me.ResumeLayout(False)

    End Sub
End Class
