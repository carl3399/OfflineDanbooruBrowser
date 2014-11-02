Imports Newtonsoft.Json
Imports System.IO
Imports System.Net

Public Class Form1
    Dim IOPJSON As String
    Dim Opfolder As String = "E:\DB\e621 (Furry)\"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '  Dim a As saLoginResponse = JsonConvert.DeserializeObject(Of saLoginResponse)(IOPJSON)

        ' MsgBox(a.tags, MsgBoxStyle.DefaultButton1, a.id)
        ProcessThings()
    End Sub
    Dim allFiles As List(Of String)
    Dim sortedFiles() As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' allFiles = FileHelper.GetFilesRecursive("E:\DB\e621 (Furry)\q\")
        ' GetInfo(150155)
        IOPJSON = My.Computer.FileSystem.ReadAllText("iop.json")
    End Sub


    Sub ProcessThings()


        ' Make a reference to a directory.
        Dim di As New DirectoryInfo(Opfolder)
        ' Get a reference to each file in that directory.
        Dim fiArr As FileInfo() = di.GetFiles("*", SearchOption.AllDirectories)
        ' Display the names of the files.
        Dim fri As FileInfo
        For Each fri In fiArr
            ListBox1.Items.Add(Mid(fri.Name, 1, 6))
        Next fri
        yfrom.Text = ListBox1.Items.Count

    End Sub








    Sub GetInfo(ByVal id As Integer)
        My.Computer.Network.DownloadFile("https://e621.net/post/show.json?id=" & id, "iop.json", "", "", False, 100000, True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ''SubProcess Things
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = ListBox1.Items.Count

        Using client As WebClient = New WebClient()
            Using sw As New StreamWriter("Index.db")
                For Each yiff In ListBox1.Items
                    Application.DoEvents()
                    sw.WriteLine(client.DownloadString("https://e621.net/post/show.json?id=" & yiff))
                    Application.DoEvents()
                    Try : ProgressBar1.Value += 1 : Catch : End Try
                    xof.Text = ProgressBar1.Value
                    Label1.Text = (Math.Round(ProgressBar1.Value / ProgressBar1.Maximum, 10) * 100).ToString("F4") & " %"
                Next
            End Using
        End Using
    End Sub


    Public Shared Function GetFiles(ByVal path As String) As Integer
        ' Make a reference to a directory.
        Dim di As New DirectoryInfo(path)
        ' Get a reference to each file in that directory.
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim x As Integer
        ' Display the names of the files.
        Dim fri As FileInfo
        For Each fri In fiArr
            x += 1
        Next fri
        Return x
    End Function 'Main


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        FolderBrowserDialog1.ShowDialog()
        Opfolder = FolderBrowserDialog1.SelectedPath


    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form2.Show()
    End Sub
End Class




Public Class saLoginResponse
    Public Property hasNotes As Boolean
    Public Property previewHeight As String
    Public Property description As String
    Public Property sources As String()
    Public Property hash As String
    Public Property creatorId As String
    Public Property status As String
    Public Property fileSize As String
    Public Property sampleWidth As String
    Public Property sampleUrl As String
    Public Property change As String
    Public Property author As String
    Public Property sampleHeight As String
    Public Property fileUrl As String
    Public Property width As String
    Public Property fileExt As String
    Public Property source As String
    Public Property hasComments As Boolean
    Public Property score As Int32
    Public Property tags As String
    Public Property children As String
    Public Property previewWidth As String
    Public Property previewUrl As String
    Public Property id As String
    Public Property height As String
    Public Property parentId As String
    Public Property hasChildren As String
    Public Property rating As String
    Public Property createdAt As Date
End Class


''' <summary>
''' This class contains directory helper method(s).
''' </summary>
Public Class FileHelper

    ''' <summary>
    ''' This method starts at the specified directory, and traverses all subdirectories.
    ''' It returns a List of those directories.
    ''' </summary>
    Public Shared Function GetFilesRecursive(ByVal initial As String) As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, "*.*"))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function

End Class