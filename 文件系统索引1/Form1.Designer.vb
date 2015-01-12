<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

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

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.KeyWord = New System.Windows.Forms.TextBox()
        Me.MoHu = New System.Windows.Forms.RadioButton()
        Me.JingQue = New System.Windows.Forms.RadioButton()
        Me.AutoSelect = New System.Windows.Forms.CheckBox()
        Me.bFind = New System.Windows.Forms.Button()
        Me.bStop = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.msg = New System.Windows.Forms.ToolStripStatusLabel()
        Me.fileshow = New System.Windows.Forms.ToolStripStatusLabel()
        Me.strCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.bBuildDrive = New System.Windows.Forms.Button()
        Me.bBuildDIY = New System.Windows.Forms.Button()
        Me.bBuildDisk = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rdDuilie = New System.Windows.Forms.RadioButton()
        Me.rdDigui = New System.Windows.Forms.RadioButton()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.显示界面ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.退出程序ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.limitDir = New System.Windows.Forms.TextBox()
        Me.Pro1 = New 文件系统索引.MyProgressBar()
        Me.ListView1 = New 文件系统索引.ListViewNF()
        Me.id = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Type = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Value = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Modify = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Path = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.fileType = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.findContent = New System.Windows.Forms.TextBox()
        Me.KeepWatch = New System.Windows.Forms.CheckBox()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "文件(夹)"
        '
        'KeyWord
        '
        Me.KeyWord.Location = New System.Drawing.Point(73, 17)
        Me.KeyWord.Name = "KeyWord"
        Me.KeyWord.Size = New System.Drawing.Size(350, 21)
        Me.KeyWord.TabIndex = 1
        '
        'MoHu
        '
        Me.MoHu.AutoSize = True
        Me.MoHu.Location = New System.Drawing.Point(438, 11)
        Me.MoHu.Name = "MoHu"
        Me.MoHu.Size = New System.Drawing.Size(71, 16)
        Me.MoHu.TabIndex = 2
        Me.MoHu.TabStop = True
        Me.MoHu.Text = "模糊搜索"
        Me.MoHu.UseVisualStyleBackColor = True
        '
        'JingQue
        '
        Me.JingQue.AutoSize = True
        Me.JingQue.Location = New System.Drawing.Point(438, 33)
        Me.JingQue.Name = "JingQue"
        Me.JingQue.Size = New System.Drawing.Size(71, 16)
        Me.JingQue.TabIndex = 3
        Me.JingQue.TabStop = True
        Me.JingQue.Text = "精确搜索"
        Me.JingQue.UseVisualStyleBackColor = True
        '
        'AutoSelect
        '
        Me.AutoSelect.AutoSize = True
        Me.AutoSelect.Location = New System.Drawing.Point(515, 12)
        Me.AutoSelect.Name = "AutoSelect"
        Me.AutoSelect.Size = New System.Drawing.Size(48, 16)
        Me.AutoSelect.TabIndex = 4
        Me.AutoSelect.Text = "Auto"
        Me.AutoSelect.UseVisualStyleBackColor = True
        '
        'bFind
        '
        Me.bFind.Location = New System.Drawing.Point(618, 18)
        Me.bFind.Name = "bFind"
        Me.bFind.Size = New System.Drawing.Size(52, 31)
        Me.bFind.TabIndex = 5
        Me.bFind.Text = "查询"
        Me.bFind.UseVisualStyleBackColor = True
        '
        'bStop
        '
        Me.bStop.Location = New System.Drawing.Point(676, 18)
        Me.bStop.Name = "bStop"
        Me.bStop.Size = New System.Drawing.Size(82, 31)
        Me.bStop.TabIndex = 6
        Me.bStop.Text = "终止扫描"
        Me.bStop.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.msg, Me.fileshow, Me.strCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 446)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1023, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'msg
        '
        Me.msg.AutoSize = False
        Me.msg.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.msg.Name = "msg"
        Me.msg.Size = New System.Drawing.Size(150, 17)
        '
        'fileshow
        '
        Me.fileshow.AutoSize = False
        Me.fileshow.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.fileshow.Name = "fileshow"
        Me.fileshow.Size = New System.Drawing.Size(550, 17)
        Me.fileshow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'strCount
        '
        Me.strCount.AutoSize = False
        Me.strCount.BorderSides = CType((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) _
                    Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.strCount.Name = "strCount"
        Me.strCount.Size = New System.Drawing.Size(150, 17)
        Me.strCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.bBuildDrive)
        Me.GroupBox1.Controls.Add(Me.bBuildDIY)
        Me.GroupBox1.Controls.Add(Me.bBuildDisk)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.ShapeContainer1)
        Me.GroupBox1.Location = New System.Drawing.Point(868, 75)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(122, 169)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "索引"
        '
        'bBuildDrive
        '
        Me.bBuildDrive.Location = New System.Drawing.Point(13, 127)
        Me.bBuildDrive.Name = "bBuildDrive"
        Me.bBuildDrive.Size = New System.Drawing.Size(97, 25)
        Me.bBuildDrive.TabIndex = 4
        Me.bBuildDrive.Text = "全盘索引"
        Me.bBuildDrive.UseVisualStyleBackColor = True
        '
        'bBuildDIY
        '
        Me.bBuildDIY.Location = New System.Drawing.Point(13, 94)
        Me.bBuildDIY.Name = "bBuildDIY"
        Me.bBuildDIY.Size = New System.Drawing.Size(97, 27)
        Me.bBuildDIY.TabIndex = 3
        Me.bBuildDIY.Text = "自定义索引"
        Me.bBuildDIY.UseVisualStyleBackColor = True
        '
        'bBuildDisk
        '
        Me.bBuildDisk.Location = New System.Drawing.Point(13, 51)
        Me.bBuildDisk.Name = "bBuildDisk"
        Me.bBuildDisk.Size = New System.Drawing.Size(97, 26)
        Me.bBuildDisk.TabIndex = 1
        Me.bBuildDisk.Text = "索引驱动器"
        Me.bBuildDisk.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"C:\", "D:\", "E:\", "F:\", "G:\", "H:\", "I:\", "J:\", "K:\", "L:\", "M:\", "N:\", "O:\", "P:\", "Q:\", "R:\", "S:\", "T:\"})
        Me.ComboBox1.Location = New System.Drawing.Point(13, 21)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(98, 20)
        Me.ComboBox1.TabIndex = 0
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(3, 17)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(116, 149)
        Me.ShapeContainer1.TabIndex = 2
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape1
        '
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = -5
        Me.LineShape1.X2 = 121
        Me.LineShape1.Y1 = 69
        Me.LineShape1.Y2 = 69
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowItemToolTips = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(149, 70)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(148, 22)
        Me.ToolStripMenuItem1.Text = "打开文件"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(148, 22)
        Me.ToolStripMenuItem2.Text = "打开文件目录"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(148, 22)
        Me.ToolStripMenuItem3.Text = "删除文件"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rdDuilie)
        Me.GroupBox2.Controls.Add(Me.rdDigui)
        Me.GroupBox2.Location = New System.Drawing.Point(868, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(150, 64)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "文件索引方式"
        '
        'rdDuilie
        '
        Me.rdDuilie.AutoSize = True
        Me.rdDuilie.Location = New System.Drawing.Point(10, 42)
        Me.rdDuilie.Name = "rdDuilie"
        Me.rdDuilie.Size = New System.Drawing.Size(137, 16)
        Me.rdDuilie.TabIndex = 1
        Me.rdDuilie.TabStop = True
        Me.rdDuilie.Text = "队列(稳定,大量文件)"
        Me.rdDuilie.UseVisualStyleBackColor = True
        '
        'rdDigui
        '
        Me.rdDigui.AutoSize = True
        Me.rdDigui.Location = New System.Drawing.Point(10, 20)
        Me.rdDigui.Name = "rdDigui"
        Me.rdDigui.Size = New System.Drawing.Size(137, 16)
        Me.rdDigui.TabIndex = 0
        Me.rdDigui.TabStop = True
        Me.rdDigui.Text = "递归(高速,少量文件)"
        Me.rdDigui.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip2
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "文件系统索引"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.显示界面ToolStripMenuItem, Me.退出程序ToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(125, 48)
        '
        '显示界面ToolStripMenuItem
        '
        Me.显示界面ToolStripMenuItem.Name = "显示界面ToolStripMenuItem"
        Me.显示界面ToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.显示界面ToolStripMenuItem.Text = "显示界面"
        '
        '退出程序ToolStripMenuItem
        '
        Me.退出程序ToolStripMenuItem.Name = "退出程序ToolStripMenuItem"
        Me.退出程序ToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.退出程序ToolStripMenuItem.Text = "退出程序"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "目录限定"
        '
        'limitDir
        '
        Me.limitDir.Location = New System.Drawing.Point(73, 44)
        Me.limitDir.Name = "limitDir"
        Me.limitDir.Size = New System.Drawing.Size(349, 21)
        Me.limitDir.TabIndex = 12
        '
        'Pro1
        '
        Me.Pro1.Location = New System.Drawing.Point(852, 446)
        Me.Pro1.Name = "Pro1"
        Me.Pro1.Size = New System.Drawing.Size(171, 22)
        Me.Pro1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.Pro1.TabIndex = 13
        '
        'ListView1
        '
        Me.ListView1.AutoArrange = False
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.id, Me.Type, Me.Value, Me.Modify, Me.Path})
        Me.ListView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(15, 72)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.ShowGroups = False
        Me.ListView1.Size = New System.Drawing.Size(842, 368)
        Me.ListView1.TabIndex = 7
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'id
        '
        Me.id.Text = "No."
        Me.id.Width = 0
        '
        'Type
        '
        Me.Type.Text = "类型"
        Me.Type.Width = 40
        '
        'Value
        '
        Me.Value.Text = "文件/文件夹"
        Me.Value.Width = 170
        '
        'Modify
        '
        Me.Modify.Text = "修改日期"
        Me.Modify.Width = 140
        '
        'Path
        '
        Me.Path.Text = "路径"
        Me.Path.Width = 500
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.fileType)
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Controls.Add(Me.findContent)
        Me.GroupBox3.Location = New System.Drawing.Point(868, 250)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(150, 190)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "文件内容"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(9, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(24, 52)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "以"";""分割"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 12)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "类型"
        '
        'fileType
        '
        Me.fileType.Location = New System.Drawing.Point(40, 112)
        Me.fileType.Multiline = True
        Me.fileType.Name = "fileType"
        Me.fileType.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.fileType.Size = New System.Drawing.Size(104, 43)
        Me.fileType.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(80, 161)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(67, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "文本搜索"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'findContent
        '
        Me.findContent.Location = New System.Drawing.Point(7, 23)
        Me.findContent.Multiline = True
        Me.findContent.Name = "findContent"
        Me.findContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.findContent.Size = New System.Drawing.Size(136, 83)
        Me.findContent.TabIndex = 0
        '
        'KeepWatch
        '
        Me.KeepWatch.AutoSize = True
        Me.KeepWatch.Location = New System.Drawing.Point(514, 35)
        Me.KeepWatch.Name = "KeepWatch"
        Me.KeepWatch.Size = New System.Drawing.Size(96, 16)
        Me.KeepWatch.TabIndex = 15
        Me.KeepWatch.Text = "监视文件系统"
        Me.KeepWatch.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1023, 468)
        Me.Controls.Add(Me.KeepWatch)
        Me.Controls.Add(Me.Pro1)
        Me.Controls.Add(Me.limitDir)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.bStop)
        Me.Controls.Add(Me.bFind)
        Me.Controls.Add(Me.AutoSelect)
        Me.Controls.Add(Me.JingQue)
        Me.Controls.Add(Me.MoHu)
        Me.Controls.Add(Me.KeyWord)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "文件系统索引"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents KeyWord As System.Windows.Forms.TextBox
    Friend WithEvents MoHu As System.Windows.Forms.RadioButton
    Friend WithEvents JingQue As System.Windows.Forms.RadioButton
    Friend WithEvents AutoSelect As System.Windows.Forms.CheckBox
    Friend WithEvents bFind As System.Windows.Forms.Button
    Friend WithEvents bStop As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents bBuildDrive As System.Windows.Forms.Button
    Friend WithEvents bBuildDIY As System.Windows.Forms.Button
    Friend WithEvents bBuildDisk As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Friend WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents msg As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents fileshow As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents strCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents id As System.Windows.Forms.ColumnHeader
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents Value As System.Windows.Forms.ColumnHeader
    Friend WithEvents Path As System.Windows.Forms.ColumnHeader
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListView1 As 文件系统索引.ListViewNF
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rdDuilie As System.Windows.Forms.RadioButton
    Friend WithEvents rdDigui As System.Windows.Forms.RadioButton
    Friend WithEvents Modify As System.Windows.Forms.ColumnHeader
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 显示界面ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 退出程序ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents limitDir As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Pro1 As 文件系统索引.MyProgressBar
    Friend WithEvents findContent As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents fileType As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents KeepWatch As System.Windows.Forms.CheckBox

End Class
