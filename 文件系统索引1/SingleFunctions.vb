Module SingleFunctions
    '这里是所有的不需要对控件进行读写的代码
    'API声明区域
    Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hWnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
    Public Declare Function PathFileExists Lib "shlwapi.dll" Alias "PathFileExistsA" (ByVal pszPath As String) As Integer
    Public Declare Function RtlAdjustPrivilege Lib "ntdll.dll" ( _
    ByVal Privilege As String, _
    ByVal bEnable As Integer, _
    ByVal bCurrentThread As Integer, _
    ByRef bEnabled As Integer) As Integer
    '常量声明区域
    Public Const SE_DEBUG_PRIVILEGE As Integer = 20
    '变量声明
    Public conn As ADODB.Connection '总的连接
    Public res As ADODB.Recordset '存在内存的数据集对象
    Public rs As ADODB.Recordset '用于动态监测条件下的加入数据库
    Public rss As ADODB.Recordset '用于添加数据
    Public resR As ADODB.Recordset '用于显示的数据集对象
    Public resScan As ADODB.Recordset '创建索引使用的数据集对象
    Public isDBon As Boolean = False '判断缓存数据集是否打开
    Public isConn As Boolean = False 'see if connection is alive

    Public AllowScan As Boolean = False '判断是不是允许(在)扫描和搜索
    Public AllowFind As Boolean = False '判断是不是在搜索状态
    Public AllowWord As Boolean = False '判断是不是正在进行文字搜索

    Public StartX As Integer, StartY As Integer '用来基记住程序启动位置的
    Public lgCount As Integer '记录加载的文件的总数
    Public scanSpeed As Integer, scan As Integer '一个是用来记录每秒钟读取的文件数目的，一个是用来显示的
    Public arrange As Boolean = False
    Public loadingDB As Boolean = False
    Public wind As String = "Normal"
    Public orderList As Boolean '对列表的排序顺序进行控制的变量
    Public activeTime As Integer = 10
    Public Const TimeSet As Integer = 10
    Public resC As ADODB.Recordset

    Public waitList As New Collection
    Public insertDrive As New Queue(Of String)
    'FileData
    '--id
    '--Type
    '--Value
    '--Path
    Function ShowShort(ByVal str As String, ByVal lMax As Integer) '[移植成功]
        Dim temp As String
        temp = StrConv(str, VbStrConv.Narrow)
        If lMax < 4 Then lMax = 4
        If Len(temp) < lMax Then
            ShowShort = str
            Exit Function
        Else
        End If
        ShowShort = Mid(str, 1, Int((lMax - 5) / 2)) & "......" & Mid(str, Len(temp) - Int((lMax - 5) / 2))
    End Function
    Function DirMe(ByVal sFile As String) '【移植成功】
        '返回值为1表示文件和文件夹存在，0表示不存在
        DirMe = PathFileExists(sFile)
    End Function
    Sub OpenURL(ByVal urlMe As String) 'ok at 12-09-06[RE]  '【完成：2012-09-06】
        '程序功能：打开文件，且设置文件的父文件夹为其当前运行文件夹路径
        '2012-09-06 改造成为过程
        Dim lngReturn As Long
        If IsNoExFile(urlMe) = False Then
            lngReturn = ShellExecute(0, "open", urlMe, "", Mid(urlMe, 1, InStrRev(urlMe, "\") - 1), 1)
        Else
            lngReturn = Shell("rundll32.exe   shell32.dll   OpenAs_RunDLL   " & urlMe)
        End If
        If lngReturn = 31 Then
            lngReturn = Shell("rundll32.exe   shell32.dll   OpenAs_RunDLL   " & urlMe)
        End If
    End Sub
    Function IsNoExFile(ByVal txt As String) As Boolean  'ok at 12-05-13[RE]  '【完成：2012-09-06】
        '功能：判断路径到底是不是文件，因为可能有的文件是没有扩展名的
        If GetAttr(txt) <> vbDirectory And InStr(1, Mid(txt, InStrRev(txt, "\") + 1), ".") = 0 Then
            '此路径不为目录，且没有扩展名
            IsNoExFile = True
        Else
            IsNoExFile = False
        End If
    End Function
    Sub LocateFile(ByVal fPath As String) 'ok at 12-08-01[RE]【移至成功】  '【完成：2012-09-06】
        '功能：在资源管理其中选中一个文件或文件夹
        Shell("explorer.exe /n ,/select ," & Replace(fPath, "\\", "\"), vbNormalFocus)
    End Sub
    Function OpenTable(ByVal txtPath As String) '【功能：建立数据库连接；状态：完成】
        Try
            conn = New ADODB.Connection
            conn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            conn.Open("PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" & txtPath & ";")
            isConn = True
            OpenTable = "YES"
        Catch
            Kill(txtPath)
            Call Form1.OutDataBase(CurDir() & "\filesearch.mdb")
            conn = New ADODB.Connection
            conn.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            conn.Open("PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" & txtPath & ";")
            isConn = True
            OpenTable = "YES"
        End Try
    End Function
    Function CloseTable() '【功能：关闭数据库连接；状态：完成】
        conn.Close()
        isConn = False
        CloseTable = "YES"
    End Function
    Function GetDirName(ByVal fPath As String) 'ok at 12-05-08[RE]  '【完成：2012-09-06】
        '2012-05-08 初次创立此函数，用于获取dir目录的名字
        '2012-08-01 引进此函数
        Dim a As Integer
        If Len(fPath) < 2 Then GetDirName = fPath : Exit Function
        If Right(fPath, 1) = "\" Then fPath = Left(fPath, Len(fPath) - 1)
        If InStr(1, fPath, "\") = 0 Then GetDirName = fPath : Exit Function
        a = InStrRev(fPath, "\")
        GetDirName = Mid(fPath, a + 1)
    End Function
    Function GetFileName(ByVal strPath As String) 'return the full name of it  '【完成：2012-09-06】
        '返回的文件名是完整的，包含扩展名的，例如“qq.exe”
        GetFileName = Mid(strPath, 1 + InStrRev(strPath, "\"))
    End Function
    '--------------获取数据库filter--------------
    Function GetSQLKey(ByVal s As String)
        Dim ss() As String
        Dim i As Object
        Dim maxStr As String = ""
        ss = Split(s, "*")
        For Each i In ss
            If i <> "" Then
                If Len(maxStr) < Len(i.ToString) Then
                    maxStr = i.ToString
                End If
            End If
        Next i
        GetSQLKey = maxStr
    End Function
   
    '------------------添加数据到数据库---------------
    Sub AddPath(ByVal strType As String, ByVal strPath As String)
        '添加数据到数据库
        With res
            .AddNew()
            .Fields("Type").Value = strType
            If strType = "dir" Then
                .Fields("Value").Value = Jencode(GetDirName(strPath))
            Else
                .Fields("Value").Value = Jencode(GetFileName(strPath))
            End If
            .Fields("Path").Value = strPath
            .Update()
        End With
    End Sub
    Function rep(ByVal s As String) As String
        rep = Replace(s, "'", "''")
    End Function
    Sub NewAddPath(ByVal strType As String, ByVal strPath As String, Optional ByVal s As Integer = 0)
        '添加数据到数据库
        'If AllowScan = False Then
        '            Dim inTest As New ADODB.Recordset
        '            inTest.Open("select Path from FileData where Path='" & rep(Jencode(strPath)) & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        '            If inTest.RecordCount = 0 Then
        '                inTest.Close()
        '            Else
        '                inTest.Close()
        '                Exit Sub
        '            End If
        '        End If
        '        If s = 1 Then
        '            With rss
        '                .AddNew()
        '                .Fields("Type").Value = strType
        '                If strType = "dir" Then
        '                    .Fields("Value").Value = Jencode(GetDirName(strPath))
        '                Else
        '                    .Fields("Value").Value = Jencode(GetFileName(strPath))
        '                End If
        '                .Fields("Path").Value = Jencode(strPath)
        '                .Update()
        '            End With
        '        Else
        '            With rs
        '                .AddNew()
        '                .Fields("Type").Value = strType
        '                If strType = "dir" Then
        '                    .Fields("Value").Value = Jencode(GetDirName(strPath))
        '                Else
        '                    .Fields("Value").Value = Jencode(GetFileName(strPath))
        '                End If
        '                .Fields("Path").Value = Jencode(strPath)
        '                .Update()
        '            End With
        '        End If
        '
        'If AllowScan = False Then
        '            Dim inTest As New ADODB.Recordset
        '            inTest.Open("select Path from FileData where Path='" & rep(Jencode(strPath)) & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        '            If inTest.RecordCount = 0 Then
        '                inTest.Close()
        '            Else
        '                inTest.Close()
        '                Exit Sub
        '            End If
        '        End If
        If AllowScan = False Then
            Dim inTest As New ADODB.Recordset
            inTest.Open("select Path,id,Type,Value from FileData where Path='" & rep(Jencode(strPath)) & "'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
            If inTest.RecordCount = 0 Then
                With inTest
                    .AddNew()
                    .Fields("Type").Value = strType
                    If strType = "dir" Then
                        .Fields("Value").Value = Jencode(GetDirName(strPath))
                    Else
                        .Fields("Value").Value = Jencode(GetFileName(strPath))
                    End If
                    .Fields("Path").Value = Jencode(strPath)
                    .Update()
                End With
                inTest.Close()
            Else
                inTest.Close()
                Exit Sub
            End If
        End If
        If s = 1 Then
        Else
            With rs
                .AddNew()
                .Fields("Type").Value = strType
                If strType = "dir" Then
                    .Fields("Value").Value = Jencode(GetDirName(strPath))
                Else
                    .Fields("Value").Value = Jencode(GetFileName(strPath))
                End If
                .Fields("Path").Value = Jencode(strPath)
                .Update()
            End With
        End If

    End Sub
    Sub DeleteToBin(ByVal sFile As String) '删除文件到回收站
        If DirMe(sFile) = 0 Then Exit Sub
        My.Computer.FileSystem.DeleteFile(sFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
    End Sub
    Function Jencode(ByVal iStr As String)
        Dim F() As String, E() As String
        ReDim E(26)
        For ise As Integer = 0 To 25
            E(ise) = "Jn" & ise.ToString & "|"
        Next
        ReDim F(26)
        F(0) = Chr(-23116)
        F(1) = Chr(-23124)
        F(2) = Chr(-23122)
        F(3) = Chr(-23120)
        F(4) = Chr(-23118)
        F(5) = Chr(-23114)
        F(6) = Chr(-23112)
        F(7) = Chr(-23110)
        F(8) = Chr(-23099)
        F(9) = Chr(-23097)
        F(10) = Chr(-23095)
        F(11) = Chr(-23075)
        F(12) = Chr(-23079)
        F(13) = Chr(-23081)
        F(14) = Chr(-23085)
        F(15) = Chr(-23087)
        F(16) = Chr(-23052)
        F(17) = Chr(-23076)
        F(18) = Chr(-23078)
        F(19) = Chr(-23082)
        F(20) = Chr(-23084)
        F(21) = Chr(-23088)
        F(22) = Chr(-23102)
        F(23) = Chr(-23104)
        F(24) = Chr(-23106)
        F(25) = Chr(-23108)
        Jencode = iStr
        For its As Integer = 0 To 25
            Jencode = Replace(Jencode, F(its), E(its))
        Next
    End Function
    Function Jdecode(ByVal strL As String)
        Dim F() As String, E() As String
        ReDim E(26)
        For ise As Integer = 0 To 25
            E(ise) = "Jn" & ise.ToString & "|"
        Next
        ReDim F(26)
        F(0) = Chr(-23116)
        F(1) = Chr(-23124)
        F(2) = Chr(-23122)
        F(3) = Chr(-23120)
        F(4) = Chr(-23118)
        F(5) = Chr(-23114)
        F(6) = Chr(-23112)
        F(7) = Chr(-23110)
        F(8) = Chr(-23099)
        F(9) = Chr(-23097)
        F(10) = Chr(-23095)
        F(11) = Chr(-23075)
        F(12) = Chr(-23079)
        F(13) = Chr(-23081)
        F(14) = Chr(-23085)
        F(15) = Chr(-23087)
        F(16) = Chr(-23052)
        F(17) = Chr(-23076)
        F(18) = Chr(-23078)
        F(19) = Chr(-23082)
        F(20) = Chr(-23084)
        F(21) = Chr(-23088)
        F(22) = Chr(-23102)
        F(23) = Chr(-23104)
        F(24) = Chr(-23106)
        F(25) = Chr(-23108)
        Jdecode = strL
        For its As Integer = 0 To 25
            Jdecode = Replace(Jdecode, E(its), F(its))
        Next
    End Function
End Module
Class ListViewNF
    Inherits System.Windows.Forms.ListView
    Public Sub New()
        ' Activate double buffering  
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        ' Enable the OnNotifyMessage event so we get a chance to filter out   
        ' Windows messages before they get to the form's WndProc  
        Me.SetStyle(ControlStyles.EnableNotifyMessage, True)
    End Sub

    Protected Overrides Sub OnNotifyMessage(ByVal m As Message)
        'Filter out the WM_ERASEBKGND message  
        If m.Msg <> &H14 Then
            MyBase.OnNotifyMessage(m)
        End If
    End Sub

End Class
Class MyProgressBar
    Inherits System.Windows.Forms.ProgressBar
    Public Sub New()
        ' Activate double buffering  
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        ' Enable the OnNotifyMessage event so we get a chance to filter out   
        ' Windows messages before they get to the form's WndProc  
        Me.SetStyle(ControlStyles.EnableNotifyMessage, True)
    End Sub

    Protected Overrides Sub OnNotifyMessage(ByVal m As Message)
        'Filter out the WM_ERASEBKGND message  
        If m.Msg <> &H14 Then
            MyBase.OnNotifyMessage(m)
        End If
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim f As New Font("宋体", 10)
        ' Dim g As Graphics = Me.CreateGraphics
        Dim b As New SolidBrush(Color.Black)
        Dim format1 As New StringFormat
        format1.Alignment = StringAlignment.Center
        'MyBase.OnNotifyMessage(m)
        'g.DrawString(Value.ToString & "%", f, b, CInt(Me.Width / 2) - 10, 4, format1)
        Dim rect As New Rectangle(0, 0, Me.Width, Me.Height)
        Dim rectF As New Rectangle(0, 0, CInt(Value * Me.Width / 100), Me.Height)

        e.Graphics.DrawRectangle(Pens.DarkGoldenrod, rect)
        e.Graphics.FillRectangle(Brushes.DarkOrange, rectF)
        e.Graphics.DrawString(Value.ToString & "%", f, b, CInt(Me.Width / 2) - 10, 4, format1)
        MyBase.OnPaint(e)
    End Sub
End Class
'FileData
'--id
'--Type
'--Value
'--Path