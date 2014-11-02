Imports Newtonsoft.Json
Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub
    Dim YiffList As New List(Of YiffEntry)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        YiffList.Clear()
        Dim thr As New Threading.Thread(AddressOf thr1)
        thr.SetApartmentState(Threading.ApartmentState.MTA)
        thr.Start()
    End Sub
    Delegate Sub SetTextCallback([text] As String)
    Private Sub SetTitle(ByVal [text] As String)

        ' InvokeRequired required compares the thread ID of the 
        ' calling thread to the thread ID of the creating thread. 
        ' If these threads are different, it returns true. 
        If Me.TextBox1.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetTitle)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.Text = [text]
        End If
    End Sub


    Sub thr1()
        Dim linez As Integer = 1
        Dim linex As Integer = 1
        For Each line As String In System.IO.File.ReadAllLines("index.db")
            linez += 1
        Next

        For Each line As String In System.IO.File.ReadAllLines("index.db")
            Try
                Application.DoEvents()
                Dim a As saLoginResponse = JsonConvert.DeserializeObject(Of saLoginResponse)(line)
                Application.DoEvents()
                YiffList.Add(New YiffEntry(a.id, Split(a.tags, " ")))
                linex += 1
                SetTitle("Search (loading, " & linex & " of " & linez & ")")
            Catch ex As Exception

            End Try
        Next
        SetTitle("Search")
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim resultX As Integer
        Dim TextBoxTags() As String = Split(TextBox1.Text, " ")
        For Each STS In TextBoxTags
            Try
                For Each Posts As YiffEntry In YiffList
                    Try
                        For Each STX In Posts.getTags
                            Try
                                If STS.Contains(STX) Then ListBox1.Items.Add(Posts.getId & "   " & BindTags(Posts)) : resultX += 1
                            Catch : End Try
                        Next
                    Catch : End Try
                Next
            Catch : End Try
        Next
        Me.Text = "Search (" & resultX & " results)"
    End Sub

    Function BindTags(ByVal OBJ As YiffEntry) As String
        Dim xx As String
        For Each xTag In OBJ.getTags
            xx = xx & " " & xTag
        Next
        Return xx
    End Function


    Function SearchX(ByVal tags() As String, ByVal tagsSearch() As String)
        Dim result As List(Of String)
        For Each iTag In tags
            For Each iTagSearch In tagsSearch
                If iTag = iTagSearch Then result.Add(iTagSearch)
            Next
        Next
        Return result
    End Function



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()
        Me.Text = "Search"
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        MsgBox(Mid(ListBox1.SelectedItem.ToString, 1, 6))
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Me.Close()
    End Sub



    Private IsFormBeingDragged As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown

        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = True
            MouseDownX = e.X
            MouseDownY = e.Y
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseUp

        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = False
        End If
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove

        If IsFormBeingDragged Then
            Dim temp As Point = New Point()

            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
            temp = Nothing
        End If
    End Sub



End Class

Public Class YiffEntry
    Private _p1 As Integer
    Private _p2 As String()
    Sub New(p1 As Integer, p2 As String())
        _p1 = p1
        _p2 = p2
    End Sub
    Function getTags() As String()
        Return _p2
    End Function
    Function getId() As Integer
        Return _p1
    End Function
End Class