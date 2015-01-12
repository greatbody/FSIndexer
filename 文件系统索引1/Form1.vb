Imports System.IO
Public Class Form1
    'all functions that needs to act with current box should follow the following roles.
    '1,creat the direct function that change the data in ActiveX's in the form
    '2,creat the special delegate declaration 
    '3,in threads,we can do what we need to throw the delegate
    '[Mark:]:为了使用任务栏进度条显示技术，这里升级到了.NET 3.5版本，非Client，通用性未测试
    '----------------------------添加Listview中的项------------------------------------------[new thread]
    Dim checkWord As Threading.Thread
    Dim ScanPath As Threading.Thread
    Dim checkContent As Threading.Thread
    Dim totalWatch As Threading.Thread
    Dim pushTask As Threading.Thread
    Public Const WM_DEVICECHANGE = &H219
    Public Const DBT_DEVICEARRIVAL = &H8000
    Public Const DBT_DEVICEREMOVECOMPLETE = &H8004
    Dim DriveLetter As String
    Public wts() As FileSystemWatcher

    Private AllowAutoScan As Boolean = False

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_DEVICECHANGE Then
            Select Case m.WParam
                Case DBT_DEVICEARRIVAL
                    Dim s() As DriveInfo = DriveInfo.GetDrives
                    For Each drive As DriveInfo In s
                        If drive.DriveType = DriveType.Removable Then
                            DriveLetter = drive.Name.ToString()
                            If AllowScan = True Then '正在建立链接
                                insertDrive.Enqueue(DriveLetter)
                            Else
                                AllowScan = True
                                ScanPath = New Threading.Thread(AddressOf ScanControl)
                                ScanPath.Start(DriveLetter)
                                Call forScan_Begin()
                            End If
                        End If
                    Next
                Case DBT_DEVICEREMOVECOMPLETE
                    Dim s() As DriveInfo = DriveInfo.GetDrives
                    For Each drive As DriveInfo In s
                        If drive.ToString = DriveLetter Then Exit Sub
                    Next
                    Debug.WriteLine("U盘:" & DriveLetter & "已卸载！")
            End Select
        End If
        MyBase.WndProc(m)
    End Sub
    Function AddToListView(ByVal task As Integer, ByVal id As Integer, ByVal type As String, ByVal value As String, ByVal path As String) As Boolean
        If task = 0 Then
            Dim s As New ListViewItem
            s.Text = id.ToString
            s.SubItems.Add(type)
            s.SubItems.Add(value)
            s.SubItems.Add(Format(IO.File.GetLastWriteTime(path), "yyyy-MM-dd HH:mm:ss"))
            s.SubItems.Add(path)
            ListView1.Items.Add(s)
            AddToListView = True
        ElseIf task = 1 Then
            ListView1.Items.Clear()
        ElseIf task = 2 Then
            Application.DoEvents()
        End If
        AddToListView = True
    End Function
    Private Delegate Function vAddToListView(ByVal task As Integer, ByVal id As Integer, ByVal type As String, ByVal value As String, ByVal path As String) As Boolean
    '--------------------------添加状态栏中的项--------------------------------------------[new thread]
    Function ShowMsg(ByVal sCom As String, ByVal sMsg As String) '[移植成功]
        '在状态栏显示状态记录
        If sCom = "msg" Then
            With Me
                .msg.Text = sMsg
                .fileshow.Text = ""
                .strCount.Text = ""
            End With
        ElseIf sCom = "dir" Or sCom = "file" Then
            With Me
                .msg.Text = "正在扫描"
                .fileshow.Text = sMsg
                .strCount.Text = lgCount.ToString & "   " & scan.ToString & "文件/秒"
            End With
        ElseIf sCom = "scan" Then
            With Me
                .msg.Text = "正在扫描"
                .fileshow.Text = sMsg
                .strCount.Text = ""
            End With
        ElseIf sCom = "stringfind" Then
            With Me
                .msg.Text = "内容搜索"
                .fileshow.Text = sMsg
                .strCount.Text = "进行中…"
            End With
        ElseIf sCom = "Watcher" Then
            With Me
                .msg.Text = "动态添加"
                .fileshow.Text = sMsg
                .strCount.Text = "进行中…"
            End With
        End If
        ShowMsg = True
    End Function
    Private Delegate Function vShowMsg(ByVal sCom As String, ByVal sMsg As String)
    Sub SetProcess(ByVal idPro As Integer)
        Pro1.Value = idPro
    End Sub
    Private Delegate Sub vSetProcess(ByVal iValue As Integer)
    '--------------------------根据关键字扫描--------------------------------------------【new thread】
    Function ScanKeyWord(ByVal strKeyWord As String) As Integer
        '返回值意义列表
        '0：成功运行
        '1：关键词为空
        '2：未知错误
        'On Error GoTo err
        Dim sqlStr As String
        Dim sqlState As Integer
        Dim totalNum As Integer = 0  '筛选后剩余项
        Dim countNum As Integer = 0  '当前已经筛选了多少项
        If strKeyWord = "" Then ScanKeyWord = 1 : AllowFind = False : Exit Function
        '正式开始扫描
        If isDBon = False Then '数据库不在内存，则加载数据库到内存
            Call LoadDataBase()
            isDBon = True
        End If
        '此时，数据库已经在内存了，可以开始进行扫描
        res = resR.Clone
        If MoHu.Checked = True Then '模糊搜索
            If limitDir.Text = "" Then
                sqlStr = "Value like '%" & KeyWord.Text & "%'"
            Else
                sqlStr = "Path like '" & limitDir.Text & "%' and Value like '%" & KeyWord.Text & "%'"
            End If
            'sqlStr = "Value like '" & Text1.Text & "'"
            Debug.Print("模糊搜索")
            sqlState = 1
        ElseIf JingQue.Checked = True And InStr(1, KeyWord.Text, "*") > 0 Then  '精确模糊搜索
            Debug.Print("精确模糊搜索")
            If limitDir.Text = "" Then
                sqlStr = "Value like '%" & GetSQLKey(KeyWord.Text) & "%'"
            Else
                sqlStr = "Path like '" & limitDir.Text & "%'"
            End If
            Debug.Print("mohu:" & GetSQLKey(KeyWord.Text))
            sqlState = 2
        Else '完全精确搜素
            sqlStr = "Value='" & KeyWord.Text & "'"
            Debug.Print("完全精确搜索")
            sqlState = 3
        End If
        '开始过滤数据
        If sqlStr = "Value like '%%'" Then
        Else
            res.Filter = sqlStr
        End If
        '获取记录集的记录数
        totalNum = res.RecordCount
        '记录数获取完成
        Debug.Print("sqlStr:" & sqlStr)
        Invoke(New vButtonSet(AddressOf forFind_Begin)) '屏蔽掉按钮

        'AllowFind = True
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "正在检索")
        If res.RecordCount = 0 Then
            res.Close()
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "等待指令")
            ScanKeyWord = 2
            AllowFind = False
            Invoke(New vButtonSet(AddressOf forFind_End))  '结束按钮屏蔽
            Exit Function
        End If
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "开始导入列表")
        Invoke(New vAddToListView(AddressOf AddToListView), 1, 0, "", "", "")
        Do While Not res.EOF = True
            countNum += 1
            If (countNum Mod 10) = 0 Or countNum = totalNum Then
                Invoke(New vSetProcess(AddressOf SetProcess), CInt(countNum * 100 / totalNum))
                Threading.Thread.Sleep(50)
            End If
            If AllowFind = False Then res.Close() : GoTo err
            'check if file exist
            If DirMe(res.Fields("Path").Value) = 1 Then
                If sqlState = 1 Or sqlState = 3 Then
                    Invoke(New vAddToListView(AddressOf AddToListView), 0, res.Fields("id").Value, res.Fields("Type").Value, res.Fields("Value").Value, res.Fields("Path").Value)
                Else
                    If LCase(res.Fields("Value").Value) Like LCase(KeyWord.Text) Then
                        Invoke(New vAddToListView(AddressOf AddToListView), 0, res.Fields("id").Value, res.Fields("Type").Value, res.Fields("Value").Value, res.Fields("Path").Value)
                    End If
                End If
            Else
                res.Delete()
                res.Update()
            End If
            res.MoveNext()

            'If ListView1.Items.Count Mod 200 = 0 Then Invoke(New vAddToListView(AddressOf AddToListView), 2, 0, "", "", "")
        Loop
        res.Close()
        '
err:
        If Err.Number = 3704 Then
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "等待指令")
        ElseIf Err.Number = 3001 Then
            ListView1.Items.Clear()
        End If
        If Err.Number > 0 Then Debug.Print(Err.Description & "-" & Err.Number)
        Invoke(New vButtonSet(AddressOf forFind_End))  '结束按钮屏蔽
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "等待指令")
        Invoke(New vSetProcess(AddressOf SetProcess), 0)
        AllowFind = False
        ScanKeyWord = 0
    End Function
    Function ScanKeyWord2(ByVal strKeyWord As String) As Integer
        '返回值意义列表
        '0：成功运行
        '1：关键词为空
        '2：未知错误
        'On Error Resume Next
        'using res
        Dim sqlStr As String
        Dim sqlState As Integer
        Dim totalNum As Integer = 0  '筛选后剩余项
        Dim countNum As Integer = 0  '当前已经筛选了多少项
        If strKeyWord = "" Then ScanKeyWord2 = 1 : AllowFind = False : Exit Function
        '正式开始扫描
        '此时，数据库已经在内存了，可以开始进行扫描
        res = New ADODB.Recordset
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "读取数据库")
        If MoHu.Checked = True Then '模糊搜索
            If limitDir.Text = "" Then
                'select id,Type,Value,Path from FileData order by Type ASC
                sqlStr = "select DISTINCT Path,id,Type,Value from FileData where Value like '%" & KeyWord.Text & "%' order by Type ASC"
            Else
                sqlStr = "select DISTINCT Path,id,Type,Value from FileData where Path like '" & limitDir.Text & "%' and Value like '%" & KeyWord.Text & "%' order by Type ASC"
            End If
            'sqlStr = "Value like '" & Text1.Text & "'"
            Debug.Print("模糊搜索")
            sqlState = 1
        ElseIf JingQue.Checked = True And InStr(1, KeyWord.Text, "*") > 0 Then  '精确模糊搜索
            Debug.Print("精确模糊搜索")
            Dim sqlTT As String = Jencode(KeyWord.Text)
            sqlTT = Replace(sqlTT, "*", "%")
            sqlTT = Replace(sqlTT, "[", "<")
            sqlTT = Replace(sqlTT, "]", "]")
            sqlTT = Replace(sqlTT, "<", "[[]")
            If limitDir.Text = "" Then
                sqlStr = "select DISTINCT Path,id,Type,Value from FileData where (Value like '" & sqlTT & "') order by Type ASC"
            Else
                sqlStr = "select DISTINCT Path,id,Type,Value from FileData where Path like '" & limitDir.Text & "%' and (Value like '" & sqlTT & "') order by Type ASC"
            End If
            sqlState = 2
        Else '完全精确搜素
            sqlStr = "select DISTINCT Path,id,Type,Value from FileData where Value='" & KeyWord.Text & "' order by Type ASC"
            Debug.Print("完全精确搜索")
            sqlState = 3
        End If
        Invoke(New vButtonSet(AddressOf forFind_Begin)) '屏蔽掉按钮

        res.Open(sqlStr, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        totalNum = res.RecordCount
        'totalNum = 1000
        '记录数获取完成
        Debug.Print("sqlStr:" & sqlStr)
        'AllowFind = True
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "正在检索")
        If res.RecordCount = 0 Then
            res.Close()
            res = Nothing
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
            ScanKeyWord2 = 2
            AllowFind = False
            Invoke(New vButtonSet(AddressOf forFind_End))  '结束按钮屏蔽
            Exit Function
        End If
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "正在导入列表")
        Invoke(New vAddToListView(AddressOf AddToListView), 1, 0, "", "", "")
        Do While Not res.EOF = True
            countNum += 1
            Invoke(New vSetProcess(AddressOf SetProcess), CInt(countNum * 100 / totalNum))
            If AllowFind = False Then res.Close() : GoTo err
            'check if file exist
            If IO.File.Exists(Jdecode(res.Fields("Path").Value)) = True Or IO.Directory.Exists(Jdecode(res.Fields("Path").Value)) = True Then
                If sqlState = 1 Or sqlState = 3 Then
                    Invoke(New vAddToListView(AddressOf AddToListView), 0, res.Fields("id").Value, res.Fields("Type").Value, Jdecode(res.Fields("Value").Value), Jdecode(res.Fields("Path").Value))
                Else
                    'If LCase(res.Fields("Value").Value) Like LCase(KeyWord.Text) Then
                    Invoke(New vAddToListView(AddressOf AddToListView), 0, res.Fields("id").Value, res.Fields("Type").Value, Jdecode(res.Fields("Value").Value), Jdecode(res.Fields("Path").Value))
                    'End If
                End If
            Else
                Try
                    res.Delete()
                Catch
                End Try
                res.Update()
            End If
            res.MoveNext()
            'If ListView1.Items.Count Mod 200 = 0 Then Invoke(New vAddToListView(AddressOf AddToListView), 2, 0, "", "", "")
        Loop
        res.Close()
        '
err:
        If Err.Number = 3704 Then
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
        ElseIf Err.Number = 3001 Then
            ListView1.Items.Clear()
        End If
        If Err.Number > 0 Then Debug.Print(Err.Description & "-" & Err.Number)
        Invoke(New vButtonSet(AddressOf forFind_End))  '结束按钮屏蔽
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
        Invoke(New vSetProcess(AddressOf SetProcess), 0)
        AllowFind = False
        ScanKeyWord2 = 0
    End Function
    '--------------------------完成委托程序--------------------------------------------
    '--------------------完整装载数据库---------------
    '数据库结构
    '+FileData
    '---id
    '---Type
    '---Value
    '---Path
    Sub LoadDataBase()
        '功能：加载数据库到内存
        loadingDB = True
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "正在加载数据")
        resR = New ADODB.Recordset
        resR.Open("select id,Type,Value,Path from FileData order by Type ASC", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        isDBon = True
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
        loadingDB = False
    End Sub
    '------------------------文件索引程序------------
    Sub GetFiles(ByVal ParentFolder As String)
        Try
            Dim sFolders(), sFiles() As String
            sFolders = IO.Directory.GetDirectories(ParentFolder)
            For Each sFolder As String In sFolders
                Call NewAddPath("dir", sFolder)
                GetFiles(sFolder)
                Application.DoEvents()
            Next
            sFiles = IO.Directory.GetFiles(ParentFolder)
            For Each sFile As String In sFiles
                NewAddPath("file", sFile)
                Application.DoEvents()
                lgCount = lgCount + 1
                scanSpeed = scanSpeed + 1
                Invoke(New vShowMsg(AddressOf ShowMsg), "file", sFile)
                If AllowScan = False Then
                    res.Close()
                    ScanPath.Abort()
                End If
            Next
        Catch
            MsgBox("递归索引堆栈溢出，请使用队列方式进行索引", MsgBoxStyle.OkOnly, "系统错误")
        End Try
    End Sub
    ''' <summary>
    ''' 队列方式进行文件索引
    ''' </summary>
    ''' <param name="folderStr">遍历目标文件夹，遍历这个文件夹</param>
    ''' <remarks></remarks>
    Sub replaceGetFiles(ByVal folderStr As String)
        '创建日期：2012-07-30
        '创建人：孙瑞
        '功能：队列方式完成文件夹遍历
        '独立性：非独立函数
        Dim sFiles() As String              '保存所有被处理的目录中的文件名
        Dim files As New List(Of String)
        Dim Dirs As New Queue(Of String)    '目录队列，用于进行遍历，对待遍历的目录进行保存
        Dirs.Enqueue(folderStr)             '将初始路径入队
        Dim currentDir As String = ""       '当前路径
        Do
            currentDir = Dirs.Dequeue()     '将队列的第一个元素出列
            'Debug.Print(currentDir)
            Try
                '将这个目录中的子目录（不含子目录的子目录）都遍历出来
                For Each dirme As String In Directory.GetDirectories(currentDir)
                    '将这个目录路径加入到数据库，标记为“dir”（目录）类型
                    NewAddPath("dir", dirme)
                    lgCount = lgCount + 1
                    scanSpeed = scanSpeed + 1
                    '在状态栏显示信息，提示添加的状态
                    Invoke(New vShowMsg(AddressOf ShowMsg), "dir", dirme)
                    '将这个子目录加入队列，等待遍历
                    Dirs.Enqueue(dirme)
                    '检测系统状态，如果为不允许遍历，则停止
                    If AllowScan = False Then
                        rs.Close()
                        ScanPath.Abort()
                    End If
                Next
                '获取这个文件夹中的文件
                sFiles = IO.Directory.GetFiles(currentDir)
                '遍历这些文件
                For Each sfile As String In sFiles
                    NewAddPath("file", sfile)
                    lgCount = lgCount + 1
                    scanSpeed = scanSpeed + 1
                    Invoke(New vShowMsg(AddressOf ShowMsg), "file", sfile)
                    If AllowScan = False Then
                        rs.Close()
                        ScanPath.Abort()
                    End If
                Next
            Catch
                '如果遇到意外的错误，则忽略，继续执行循环。
                Continue Do
            End Try
        Loop Until Dirs.Count = 0
    End Sub
    ''' <summary>
    ''' 检测当前程序进程是否是程序的第二次运行
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function preInstanceCheck() As Boolean
        'This uses the Process class to check for the name of the current applications process and ‘see whether or not there a
        're more than 1x instance loaded.‘The end result of this code is similar to Visual Basic 6.0′s App.Previnstance feature.
        Dim appName As String = Process.GetCurrentProcess.ProcessName   '获取当前进程的进程名
        Dim sameProcessTotal As Integer = Process.GetProcessesByName(appName).Length    '获取进程中同名的进程
        If sameProcessTotal > 1 Then
            'MessageBox.Show("A previous instance of this application is already open!", " App.PreInstance Detected!", _
            'MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        End If
        appName = Nothing
        sameProcessTotal = Nothing
        Return False
    End Function
    ''' <summary>
    ''' 根据选项，以递归或队列方式进行文件夹遍历
    ''' </summary>
    ''' <param name="sPath">目标路径</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EnumFiles(ByVal sPath As String) As String  '【完成：2012-09-06】
        '2012-07-31 修改显示方法，改用状态栏显示
        '2012-09-06 完善文件索引建立逻辑
        '清除指定目录的相关数据，准备扫描新的内容进入
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "正在清除原" & sPath & "数据……")
        'Following：删除数据库中相关数据
        Dim resDEL As New ADODB.Recordset
        resDEL.Open("delete * from FileData where Path like '" & sPath & "%'", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        'Following：打开全局数据集对象，让另外的函数NewAddPath可以直接完成数据上载。
        rs = New ADODB.Recordset
        rs.Open("FileData", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        Debug.Print("open rs")
        If rdDigui.Checked = True Then
            GetFiles(sPath)  '递归
        Else
            replaceGetFiles(sPath) '队列
        End If
        rs.Close()
        Debug.Print("close rs")
        'Following：显示一些提示信息，设置按钮恢复原来状态
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
        Invoke(New vButtonSet(AddressOf forScan_End))
        EnumFiles = "ok"
        Exit Function
err:
        'MsgBox "未知文件错误", , "提醒"
        MsgBox(Err.Description)
        EnumFiles = "ERROR"
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
        Invoke(New vButtonSet(AddressOf forScan_End))
    End Function

    '-----------加载数据期间，不可以按下的按钮(锁定和解锁)---
    Delegate Sub vButtonSet()
    Sub forDBLoad_Begin()
        KeyWord.Enabled = False
        bBuildDisk.Enabled = False
        bBuildDIY.Enabled = False
        bFind.Enabled = False
        bFind.Enabled = False
        bStop.Enabled = False
    End Sub
    Sub forDBLoad_End()
        KeyWord.Enabled = True
        bBuildDisk.Enabled = True
        bBuildDIY.Enabled = True
        bFind.Enabled = True
        bFind.Enabled = True
        bStop.Enabled = True
    End Sub
    '-----------搜索期间，不可以按下的按钮(锁定和解锁)---
    Sub forFind_Begin()
        bFind.Enabled = False
        bBuildDisk.Enabled = False
        bBuildDIY.Enabled = False
        bBuildDrive.Enabled = False
    End Sub
    Sub forFind_End()
        bFind.Enabled = True
        bBuildDisk.Enabled = True
        bBuildDIY.Enabled = True
        bBuildDrive.Enabled = True
    End Sub
    '-----------建立索引期间，不可以按下的按钮(锁定和解锁)---
    Sub forScan_Begin()
        bFind.Enabled = False
        bBuildDisk.Enabled = False
        bBuildDIY.Enabled = False
        bBuildDrive.Enabled = False
    End Sub

    Sub forScan_End()
        bFind.Enabled = True
        bBuildDisk.Enabled = True
        bBuildDIY.Enabled = True
        bBuildDrive.Enabled = True
    End Sub
    Sub forWordFind()
        Button1.Enabled = True
    End Sub
    '--------------[[[[[[[[[[以下全部为按钮事件程序，上部为函数等程序]]]]]]]]]]----------
    Private Sub bFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bFind.Click  '【完成：2012-09-06】
        'Button=查询
        activeTime = TimeSet
        KeyWord.Focus()
        checkWord = New Threading.Thread(AddressOf ScanKeyWord2)
        If AllowFind = True Then
            checkWord.Abort()
            AllowFind = False
            checkWord = New Threading.Thread(AddressOf ScanKeyWord2)
        End If

        ListView1.Items.Clear()
        AllowFind = True '表示开始了
        checkWord.Start(KeyWord.Text)

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        wind = "Hide"
        'If Me.WindowState = FormWindowState.Minimized Then
        'Me.Hide()
        'End If
        Me.Hide()
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '在上次关闭的地方打开软件界面
        'Dim thrLoadDataBase As New Threading.Thread(AddressOf LoadDataBase)
        If preInstanceCheck() = True Then
            MsgBox("已经存在一个实例，本进程自动结束！")
            NotifyIcon1.Dispose()
            End
        End If
        If IO.File.Exists(CurDir() & "\loc.dat") = True Then
            Dim i As String '内部定义的
            Dim iome As IO.StreamReader
            iome = New IO.StreamReader(CurDir() & "\loc.dat")
            i = iome.ReadToEnd
            StartX = CLng(i.Split(",")(0))
            StartY = CLng(i.Split(",")(1))
            iome.Close()
            If StartX < 0 Then StartX = 100
            If StartY < 0 Then StartY = 100
            Me.Left = StartX
            Me.Top = StartY
        End If
        ReDim wts(0)
        wts(0) = New FileSystemWatcher
        '如果数据库文件不存在，则释放出来

        If IO.File.Exists(CurDir() & "\filesearch.mdb") = False Then
            Call OutDataBase(CurDir() & "\filesearch.mdb")
        End If

        '执行到这一步，就说明数据库文件已经存在了，所以就可以开始建立连接了
        Call OpenTable(CurDir() & "\filesearch.mdb")
        '开始加载数据库内容到内存
        'thrLoadDataBase.Start()
        '初始化一些控件的状态
        'radio控件
        MoHu.Checked = True  '默认选定模糊搜索
        rdDuilie.Checked = True
        'combo控件
        ComboBox1.Items.Clear()
        For i As Integer = 0 To 25
            If DirMe(Chr(Asc("A") + i) & ":\") = 1 Then
                ComboBox1.Items.Add(Chr(Asc("A") + i) & ":\")
            End If
        Next
        ComboBox1.SelectedIndex = 0 '默认选定C盘
        '完成加载
        totalWatch = New Threading.Thread(AddressOf FileWatcherManager)
        totalWatch.Start()

        pushTask = New Threading.Thread(AddressOf PushToDB)
        pushTask.Start()
    End Sub
    Sub OutDataBase(ByVal outPath As String)
        '功能：输出程序中集成的数据库文件
        '2012-0802
        'Dim resources As System.Resources.ResourceManager = My.Resources.ResourceManager
        'ResourceManagerGetStream
        'Dim br As BinaryReader = New BinaryReader(My.Resources.filesearch)
        Dim b() As Byte = My.Resources.filesearch
        'System.IO.File.WriteAllBytes(CurDir() & "\filesearch.mdb", br.ReadBytes(br.BaseStream.Length))
        Dim ko As New IO.FileStream(CurDir() & "\filesearch.mdb", IO.FileMode.Create)
        ko.Write(b, 0, b.Length)
        ko.Close()
    End Sub
    Private Sub MoHu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MoHu.Click
        '完成，勿改
        KeyWord.Focus()
    End Sub

    Private Sub JingQue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles JingQue.Click
        '完成，勿改
        KeyWord.Focus()
    End Sub

    Private Sub AutoSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoSelect.Click
        '完成，勿改
        KeyWord.Focus()
    End Sub
    Private Sub bStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bStop.Click
        'Button=终止扫描
        If AllowFind = True Then
            checkWord.Abort()
            ShowMsg("msg", "就绪")
            Call forFind_End()
            AllowFind = False
        End If
        If AllowScan = True Then
            ScanPath.Abort()
            ShowMsg("msg", "就绪")
            AllowScan = False
            Call forScan_End()
            '
            'If 'isDBon = True Then
            '    resR.Close()
            'End If
            'Dim thrLoadDB As New Threading.Thread(AddressOf LoadDataBase)
            'thrLoadDB.Start()
            'come here
        End If
        If AllowWord = True Then
            checkContent.Abort()
            Button1.Enabled = True
            With Me
                .msg.Text = "就绪"
                .fileshow.Text = ""
                .strCount.Text = ""
            End With
            AllowWord = False
        End If
        '无论如何，这个进度条需要归零
        SetProcess(0)
    End Sub
    Sub ScanControl(ByVal strRange As String)

        Dim startTime As Date = System.DateTime.Now
        If strRange = "All" Then
            For Each i As String In ComboBox1.Items
                Call EnumFiles(i)
            Next
            Do While insertDrive.Count > 0
                Call EnumFiles(insertDrive.Dequeue)
            Loop
        Else
            Call EnumFiles(strRange)
            Do While insertDrive.Count > 0
                Call EnumFiles(insertDrive.Dequeue)
            Loop
            Debug.Print("zidingyi called")
        End If
        '判断是否存在内存中 的数据库
        If isDBon = True Then resR.Close()
        ' Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "载入数据库...")
        ' resR = New ADODB.Recordset
        ' resR.Open("select id,Type,Value,Path from FileData", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        ' End If
        'Call LoadDataBase()
        Invoke(New vButtonSet(AddressOf forScan_End))
        Dim endTime As Date = System.DateTime.Now
        Debug.Print("用时：" & (endTime - startTime).ToString)
        AllowScan = False
        ScanPath.Abort()
    End Sub

    Private Sub bBuildDisk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bBuildDisk.Click
        'Button=索引驱动器
        '2012-08-06 更新，增加按键屏蔽的功能
        If AllowScan = True Then '说明正在进行索引建立
            Exit Sub
        End If
        '这里就说明不处于索引建立状态
        If ComboBox1.Text = "" Or DirMe(ComboBox1.Text) = 0 Then MsgBox("路径不存在或者不合法", MsgBoxStyle.Critical, "文件索引提醒") : Exit Sub
        Call forScan_Begin()
        AllowScan = True
        ScanPath = New Threading.Thread(AddressOf ScanControl)
        ScanPath.Start(ComboBox1.Text)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        scan = scanSpeed
        scanSpeed = 0
    End Sub

    Private Sub bBuildDrive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bBuildDrive.Click
        'Button=全盘索引
        If AllowScan = True Then '说明正在进行索引建立
            MsgBox("正在进行索引……")
            Exit Sub
        End If
        '这里就说明不处于索引建立状态
        '于是，开始将数据库清除
        If isDBon = True Then
            resR.Close() ' 卸载内存数据库
            isDBon = False
        End If

        'isDBon = False
        Call CloseTable() '关闭数据库文件连接
        '删除数据库，并释放原始数据库文件
        If IO.File.Exists(CurDir() & "\filesearch.mdb") = True Then
            Kill(CurDir() & "\filesearch.mdb")
            Call OutDataBase(CurDir() & "\filesearch.mdb")
        Else
            Call OutDataBase(CurDir() & "\filesearch.mdb")
        End If
        '重新连接数据库
        Call OpenTable(CurDir() & "\filesearch.mdb")
        '
        AllowScan = True
        Call forScan_Begin()
        ScanPath = New Threading.Thread(AddressOf ScanControl)
        ScanPath.Start("All")
    End Sub

    Private Sub KeyWord_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles KeyWord.KeyUp
        Dim h As New System.EventArgs
        activeTime = TimeSet
        If e.KeyValue = 13 Then Call KeyWord_TextChanged(sender, h, 1)
    End Sub

    Private Sub KeyWord_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs, Optional ByVal comInt As Integer = 0) Handles KeyWord.TextChanged
        '判断是不是在加载数据库期间，如果是的，则直接退出
        If loadingDB = True Then Exit Sub
        '判断是否是自动状态

        If AutoSelect.Checked = False And comInt = 0 Then Exit Sub

        '如果输入框为空则停止当前搜索，且将搜索框清空
        If KeyWord.Text = "" Then
            If AllowFind = True Then
                checkWord.Abort()
                ListView1.Items.Clear()
                Call forFind_End()
                AllowFind = False
            End If
        End If
        '如果输入框不为空，则开始进行搜索：
        '如果正在搜索状态，则：
        If AllowFind = True Then
            checkWord.Abort()
            AllowFind = False
        End If

        If AllowScan = True Then
            Exit Sub
        End If
        '既不是正搜索状态，输入框内又有文字。还需要检查数据库是否存在内存。
        'If 'isDBon = False Then
        'Call LoadDataBase()
        'End If
        '这里，所有的条件都满足，则开始搜索吧！
        AllowFind = True
        ListView1.Items.Clear()
        checkWord = New Threading.Thread(AddressOf ScanKeyWord2)
        checkWord.Start(KeyWord.Text)
    End Sub
    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        If ListView1.Items.Count < 2 Then Exit Sub '如果小于两条记录则退出本过程
        ListView1.ListViewItemSorter = New ListViewItemComparerByString(e.Column, orderList)
        orderList = Not orderList
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Dim pathOf As String
        If ListView1.Items.Count > 0 And ListView1.SelectedItems.Count > 0 Then
            pathOf = ListView1.Items(ListView1.SelectedIndices.Item(0)).SubItems(4).Text
            'Stop
            OpenURL(pathOf)
            'Stop
        End If
    End Sub
    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        '        打开文件按钮()
        Dim selPath As String
        If ListView1.Items.Count = 0 Or ListView1.SelectedItems.Count = 0 Then Exit Sub
        selPath = ListView1.Items(ListView1.SelectedIndices.Item(0)).SubItems(4).Text
        Call OpenURL(selPath)
    End Sub
    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        '打开文件目录
        Dim tmp As String
        Dim tmpDir As String
        If ListView1.Items.Count = 0 Then Exit Sub
        tmp = ListView1.Items(ListView1.SelectedIndices.Item(0)).SubItems(4).Text
        If DirMe(tmp) = 1 Then
            tmpDir = Mid(tmp, 1, InStrRev(tmp, "\") - 1)
            If DirMe(tmp) = 1 And GetAttr(tmp) = vbDirectory Then '打开的是目录路径
                Call OpenURL(tmp)
            ElseIf DirMe(tmpDir) = 1 Then '打开的是文件所在的目录，所以要定位文件
                Call LocateFile(tmp)
            End If
        End If
    End Sub

    Private Sub ListView1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'ContextMenuStrip1.Show(e.X + Me.Left + ListView1.Left, e.Y + Me.Top + ListView1.Top)
            ContextMenuStrip1.Show(ListView1, e.X, e.Y)
        End If
    End Sub

    Private Sub bBuildDIY_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bBuildDIY.Click
        'Button=自定义索引
        '2012-08-06 建立此代码，完成自定义索引建立
        Dim dirPath As String
        If AllowScan = True Then MsgBox("请等待索引完成再提交索引任务") : Exit Sub
        '这里，则一定是没有开始扫描
        dirPath = InputBox("请输入您需要建立索引的路径", "自定义索引设置")
        '判断是否真实路径
        If DirMe(dirPath) = 0 Then MsgBox("对不起，您输入的路径不存在！", , "提醒") : Exit Sub
        '路径存在，可以开始进行自定义搜索
        AllowScan = True
        Call forScan_Begin()
        ScanPath = New Threading.Thread(AddressOf ScanControl)
        ScanPath.Start(dirPath)
    End Sub
    Private Sub ToolStripMenuItem3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        'Button=删除文件
        Dim selPath As String
        If ListView1.Items.Count = 0 Or ListView1.SelectedItems.Count = 0 Then Exit Sub
        selPath = ListView1.Items(ListView1.SelectedIndices.Item(0)).SubItems(4).Text
        Call DeleteToBin(selPath)
        Dim delTarget As New ListViewItem
        delTarget = ListView1.Items(ListView1.SelectedIndices.Item(0))
        ListView1.Items.Remove(delTarget)
    End Sub
    Private Sub 显示界面ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 显示界面ToolStripMenuItem.Click
        '2012-09-08 显示界面 按钮处理程序
        Me.Show()
        wind = "Normal"
        If Me.WindowState = Windows.Forms.FormWindowState.Maximized Then
        ElseIf Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub
    Private Sub 退出程序ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 退出程序ToolStripMenuItem.Click
        '2012-09-08 退出程序  按钮的处理程序
        Dim ios As New IO.StreamWriter(CurDir() & "\loc.dat", False)
        ios.Write(CLng(Me.Left))
        ios.Write(",")
        ios.Write(CLng(Me.Top))
        ios.Close()
        '对几个进程进行控制
        '关闭内存中的数据库
        If isDBon = True Then
            resR.Close()
        End If
        NotifyIcon1.Dispose()
        End
    End Sub
    Private Sub NotifyIcon1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        '2012-09-08 任务栏图标右键单击处理程序
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If wind = "Hide" Then
                wind = "Normal"
                Me.Show()
                Me.WindowState = FormWindowState.Normal
            Else
                If Me.WindowState = Windows.Forms.FormWindowState.Maximized Or Me.WindowState = Windows.Forms.FormWindowState.Normal Then
                    Me.Hide()
                    wind = "Hide"
                ElseIf Me.WindowState = FormWindowState.Minimized Then
                    Me.WindowState = FormWindowState.Normal
                    wind = "Normal"
                End If
            End If
        End If
    End Sub
    'FileData
    '--id
    '--Type
    '--Value
    '--Path
    Structure Me1   '用来传递文件内容查询请求的文件类型
        Dim sContent As String
        Dim sFilter As String
    End Structure
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '2012-09-08 调整按钮的程序执行顺序
        '2013-12-02 加入搜索条件的判断，避免出问题
        If fileType.Text = "" Or findContent.Text = "" Then
            MsgBox("请输入需要搜索的内容")
            Exit Sub
        End If
        checkContent = New Threading.Thread(AddressOf FindStrContent)
        Dim s As Me1
        s.sFilter = fileType.Text
        s.sContent = findContent.Text
        AllowWord = True
        Button1.Enabled = False '先设置为失效，让后面的程序来解锁。
        checkContent.Start(s)
    End Sub
    Function SearchFile(ByVal sFile As String, ByVal sContent As String) As Boolean
        '2012-08-27 搜索一个文件，看其中是不是有你需要的文件
        '2012-08-28 解决小文件反复读取有害磁盘的问题
        '[代码完成]
        If IO.File.Exists(sFile) = False Then SearchFile = False : Exit Function
        Dim stmp As String = ""
        Try
            Dim i As New IO.StreamReader(sFile, True)
            Dim myReader As New IO.StreamReader(sFile, System.Text.Encoding.Default)
            If FileLen(sFile) > 1024 * 1024 * 10 Then
                Do While i.EndOfStream = False
                    Dim s As String = i.ReadLine
                    Dim myS As String = myReader.ReadLine
                    If InStr(1, s, sContent) > 0 Or InStr(1, myS, sContent) > 0 Then
                        SearchFile = True
                        i.Close()
                        Exit Function
                    End If
                Loop
            Else
                stmp = i.ReadToEnd
                If InStr(1, stmp, sContent) > 0 Then
                    SearchFile = True
                    i.Close()
                    Exit Function
                End If
                stmp = myReader.ReadToEnd
                If InStr(1, stmp, sContent) > 0 Then
                    SearchFile = True
                    i.Close()
                    Exit Function
                End If
            End If
            SearchFile = False
            i.Close()
        Catch
            SearchFile = False
            Exit Function
        End Try
    End Function
    Sub FindStrContent(ByVal strContent As Me1)
        '主函数。载入线程的那个【在文本文件搜索文本内容的程序段，这个程序段将载入独立线程运行】
        '2012-09-07 文本搜索功能实现
        '2012-09-08 增加对文件内容搜索按钮的支持，对进度条的置零，对状态栏的恢复
        Dim strFileType As String = strContent.sFilter
        Dim strType() As String = Split(strFileType, ";")
        Dim sqlStr As String = "Select * From FileData Where Type='file'"
        Dim c As Integer = 0
        Dim cTotal As Integer = 0, cCount As Integer = 0

        If isConn = False Then Exit Sub 'if conn is not on ,then exit this sub
        Invoke(New vAddToListView(AddressOf AddToListView), 1, 0, "", "", "")  '清空listview列表框。
        For Each a As String In strType
            If a <> "" Then
                If c = 0 Then
                    a = Replace(a, "*", "%")
                    sqlStr = sqlStr & " and (Value like '%." & a & "'"
                Else
                    sqlStr = sqlStr & " Or Value like '%." & a & "'"
                End If
            End If
            c += 1
        Next
        sqlStr = sqlStr & ")"
        resC = New ADODB.Recordset
        resC.Open(sqlStr, conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
        If resC.RecordCount = 0 Then
            resC.Close()
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪") '设置状态了恢复空闲显示状态
            Invoke(New vSetProcess(AddressOf SetProcess), 0) '将进度条置零
            Invoke(New vButtonSet(AddressOf forWordFind)) '将按钮设置为可以按下
            Exit Sub
        End If

        cTotal = resC.RecordCount
        Do While resC.EOF = False
            cCount += 1
            Invoke(New vSetProcess(AddressOf SetProcess), CInt(cCount * 100 / cTotal))
            '开始处理文件
            If SearchFile(resC.Fields("Path").Value, strContent.sContent) Then
                Invoke(New vAddToListView(AddressOf AddToListView), 0, CInt(resC.Fields("id").Value), resC.Fields("Type").Value, resC.Fields("Value").Value, resC.Fields("Path").Value)
                Invoke(New vShowMsg(AddressOf ShowMsg), "stringfind", resC.Fields("Path").Value)
                Application.DoEvents()
            End If
            resC.MoveNext()
        Loop
        resC.Close()
        Invoke(New vButtonSet(AddressOf forWordFind))
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪")
    End Sub
    Private Sub OnCreat(ByVal sender As System.Object, ByVal e As System.IO.FileSystemEventArgs) '【完成：2012-09-07】
        '2012-09-07 处理监控的回调信息
        '2012-09-08 增加对文件重命名的支持
        '2012-09-10 改为使用集合，这样可以避免重复项
        If e.ChangeType = IO.WatcherChangeTypes.Created Or e.ChangeType = IO.WatcherChangeTypes.Renamed Then
            Debug.Print(e.ChangeType.ToString & ":" & e.FullPath)
            Try
                If AllowAutoScan = True Then
                    waitList.Add(e.FullPath, e.FullPath) '将创建的文件的信息加入到缓冲列表，等待加入数据库
                End If
            Catch
            End Try
        End If
    End Sub
    Function isinList(ByVal i As String, ByRef setAll() As String) '【完成：2012-09-06】
        '2012-09-06 判断i是否在setAll数组中。 
        For k As Integer = 0 To setAll.Length - 1
            If setAll(k) = i Then
                isinList = True
                Exit Function
            End If
        Next
        isinList = False
    End Function
    Sub PushToDB() '【完成：2012-09-06】
        '2012-09-06 这个过程是在新的线程中运行的，用于监控缓冲区的数据。当缓冲区数据可以被加入到数据库的时候，就加入。这就是一个中转队列
        '2012-09-07 更改了这个自动频繁更新状态栏的情况。
        '2012-09-08 修改了每次运行的间隔时间为60s
        '2014-02-26 修改了每次最外层Do Loop循环的检查情况，将间隔由无增加到0.5秒
        Do While 1 = 1
            Dim sc As Integer = 0
            If AllowFind = True Or AllowScan = True Or AllowAutoScan = True Then
                'Threading.Thread.
                Continue Do
            End If

            If isConn = True And AllowFind = False And AllowScan = False And waitList.Count > 0 Then
                Debug.Print("【【【【正在导入到库】】】】")
                rss = New ADODB.Recordset
                rss.Open("FileData", conn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)
                Do While waitList.Count > 0
                    sc = sc + 1
                    Dim f As String = waitList.Item(1)
                    waitList.Remove(1)
                    If IO.File.Exists(f) = True Then NewAddPath("file", f, 1)
                    If IO.Directory.Exists(f) = True Then NewAddPath("dir", f, 1)
                    Invoke(New vShowMsg(AddressOf ShowMsg), "Watcher", f) '委托方式进行处理
                Loop
                sc = 0
                rss.Close()
                Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "就绪") '此命令不显示，保证静默。
                Debug.Print("【【【【完成到库】】】】")
                Threading.Thread.Sleep(30000) '30s才会进行一次推入
                'Threading.Thread.SpinWait(5000)
            End If
            Threading.Thread.Sleep(500)
        Loop
    End Sub
    Function compareGR(ByRef newState() As String, ByRef oldState() As String) As Boolean  '【完成：2012-09-06】
        '2012-09-06 比较新旧字符串，如果新的包含旧的不包含的内容，则将新的导入到旧的
        Dim c As Integer = 0, co As Integer = 0
        For Each k As String In newState
            For Each j As String In oldState
                If k = j Then
                    c += 1
                    Exit For
                End If
            Next
            If c = 0 Then  '表示新数组中存在旧数组中不存在的内容
                co = 0
                Exit For
            Else
                co += 1
            End If
        Next

        If co = 0 Then
            ReDim oldState(newState.Length - 1)
            For ll As Integer = 0 To newState.Length - 1
                oldState(ll) = newState(ll)
            Next
            compareGR = True '有改变
        Else
            compareGR = False '无改变
        End If
    End Function
    Sub FileWatcherManager() '【完成：2012-09-06】
        '2012-09-06 此代码将在新建的线程中运行，用于更改监视系统磁盘的情况
        Static i As Integer = 600
        Dim disks() As String, oldDisk() As String
        Dim sets() As String
        ReDim disks(0)
        ReDim oldDisk(0)
        ReDim sets(0)
        Threading.Thread.Sleep(1000)
        i = 0
        '进入文件观察管理阶段
        '调用韩式，获取系统中所有的盘符
        Call GetAllDisks(disks)
        '对所有的盘符进行监视
        '如果文件系统监视的个数为1
        If wts.Length = 1 Then
            '根据盘符的数目来重新定义文件监视对象的数目
            ReDim wts(disks.Length - 1)
            For ikk As Integer = 0 To disks.Length - 1
                If disks(ikk) = "" Then
                Else
                    wts(ikk) = New IO.FileSystemWatcher '实例化对象
                    wts(ikk).Path = disks(ikk)  '定下监控路径
                    wts(ikk).NotifyFilter = IO.NotifyFilters.LastWrite Or IO.NotifyFilters.DirectoryName Or IO.NotifyFilters.FileName Or IO.NotifyFilters.Attributes '确认监控的事情
                    wts(ikk).IncludeSubdirectories = True '确认监控子目录
                    'wts(ikk).SynchronizingObject = Me
                    AddHandler wts(ikk).Created, AddressOf OnCreat '确认事件处理程序。
                    AddHandler wts(ikk).Renamed, AddressOf OnCreat
                    wts(ikk).EnableRaisingEvents = True
                End If
            Next
        Else
            ReDim sets(wts.Length - 1)
            For kl As Integer = 0 To wts.Length - 1
                If wts(kl) Is Nothing Then
                Else
                    sets(kl) = wts(kl).Path
                End If
            Next
            For Each kits As String In disks
                If isinList(kits, sets) = False And kits <> "" Then
                    ReDim Preserve wts(wts.Length)
                    wts(wts.Length - 1) = New IO.FileSystemWatcher
                    wts(wts.Length - 1).Path = kits
                    wts(wts.Length - 1).NotifyFilter = IO.NotifyFilters.LastWrite
                    wts(wts.Length - 1).IncludeSubdirectories = True
                    wts(wts.Length - 1).SynchronizingObject = Me
                    AddHandler wts(wts.Length - 1).Created, AddressOf OnCreat
                    AddHandler wts(wts.Length - 1).Renamed, AddressOf OnCreat
                    wts(wts.Length - 1).EnableRaisingEvents = True
                End If
            Next
        End If
        Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "监控未启动")
    End Sub
    Sub GetAllDisks(ByRef r() As String) '【完成：2012-09-06】
        '2012-09-06 把系统所有的磁盘返回到r()数组中。
        '这里的i是临时变量，用于遍历
        Dim i As Integer = 1
        For i = 1 To 26
            '判断构造的路径是否存在来判断盘符是否存在
            If IO.Directory.Exists((Chr(Asc("A") + i - 1)) & ":\") = True Then
                If r.Length = 1 Then
                    r(r.Length - 1) = (Chr(Asc("A") + i - 1)) & ":\"
                    ReDim Preserve r(r.Length)
                Else
                    r(r.Length - 1) = (Chr(Asc("A") + i - 1)) & ":\"
                    ReDim Preserve r(r.Length)
                End If
            End If
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub KeepWatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles KeepWatch.Click
        If KeepWatch.CheckState = CheckState.Checked Then
            AllowAutoScan = True
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "监控已启动")
        Else
            AllowAutoScan = False
            Invoke(New vShowMsg(AddressOf ShowMsg), "msg", "监控未启动")
        End If
    End Sub
End Class
Class ListViewItemComparerByString
    '********************ListView 排序程序*****************
    '*完成日期：2012-08-26                                *
    '*功能：将listview的某一项按照降序或者升序排序        *
    '*                                                    *
    '******************************************************
    Implements IComparer

    Private col As Integer
    Private direct As Boolean

    Public Sub New()
        col = 0
        direct = True
    End Sub

    Public Sub New(ByVal column As Integer, ByVal dr As Integer)
        col = column
        direct = dr
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
       Implements IComparer.Compare
        If direct Then
            Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text) '按文本比较
        Else
            Return [String].Compare(CType(y, ListViewItem).SubItems(col).Text, CType(x, ListViewItem).SubItems(col).Text) '按文本比较
        End If
    End Function
End Class