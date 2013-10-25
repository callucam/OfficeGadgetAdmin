#Region "Imports directives"

Imports System.Reflection
Imports System.IO

Imports Excel = Microsoft.Office.Interop.Excel

Imports System.Runtime.InteropServices
Imports Scripting
Imports Microsoft.VisualBasic

#End Region

Public Class Form1

    Public j As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Label1.Text = "Version: " & System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString
        TextBox1.Text = My.Settings.ProjectListFile
        TextBox2.Text = My.Settings.EmployeeListFile
        DataSet2.Clear()
        DataSet2.ReadXml(TextBox2.Text)

        For kk = 0 To DataSet2.Tables(0).Rows.Count - 1
            CheckedListBox1.Items(kk) = DataSet2.Tables(0).Rows(kk).Item(0) & " " & DataSet2.Tables(0).Rows(kk).Item(1)

        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DataSet1.Clear()
        If My.Computer.FileSystem.FileExists(TextBox1.Text) Then
            DataSet1.ReadXml(TextBox1.Text)
            DataGridView1.DataSource = DataSet1
            DataGridView1.DataMember = "row"
        Else
            MsgBox("The selected folder does not exist.  Please choose another location.")
        End If

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DataSet1.Tables(0).AcceptChanges()
        DataSet1.AcceptChanges()
        DataSet1.WriteXml(TextBox1.Text)
        MsgBox("Change Recorded")
    End Sub

    Private Sub Close_Click(sender As Object, e As EventArgs) Handles Button3.Click
        End
    End Sub


    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        DataSet2.Clear()

        If My.Computer.FileSystem.FileExists(TextBox2.Text) Then
            DataSet2.ReadXml(TextBox2.Text)
            DataGridView2.DataSource = DataSet2
            DataGridView2.DataMember = "row"
            DataGridView2.EditMode = DataGridViewEditMode.EditOnEnter
            DataGridView2.AllowUserToAddRows = True
            DataGridView2.AllowUserToDeleteRows = True
            DataGridView2.AllowUserToResizeColumns = False

            Dim column1 As DataGridViewColumn = DataGridView2.Columns(0)
            column1.Width = 120

            Dim column2 As DataGridViewColumn = DataGridView2.Columns(0)
            column2.Width = 120

            Dim column3 As DataGridViewColumn = DataGridView2.Columns(0)
            column3.Width = 120

            Button22.Focus()
        Else
            MsgBox("The selected folder does not exist.  Please choose another location.")
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        DataSet2.Tables(0).AcceptChanges()
        DataSet2.AcceptChanges()
        DataSet2.WriteXml(TextBox2.Text)
        MsgBox("Change Recorded")
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        End
    End Sub
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        End
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        SearchDirectory.Text = ""
        ClientNumber.Text = ""
        ProjectNumberTextBox.Text = ""
        StartDate.Value = DateSerial(2013, 1, 1)
        EndDate.Value = Now()
    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Dim aa As String
        Dim bb As String
        Dim zz As String
        Dim cc As Date
        Dim dd As Date
        Dim ii As Object
        Dim oXL As Microsoft.Office.Interop.Excel.Application = Nothing
        Dim oWBs As Microsoft.Office.Interop.Excel.Workbooks = Nothing
        Dim oWB As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim oWB2 As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim oSheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
        Dim oSheet2 As Microsoft.Office.Interop.Excel.Worksheet = Nothing
        Dim oCells As Microsoft.Office.Interop.Excel.Range = Nothing
        Dim oRng1 As Microsoft.Office.Interop.Excel.Range = Nothing
        Dim oRng2 As Microsoft.Office.Interop.Excel.Range = Nothing
        Dim foundFileindex1 As Integer
        Dim foundFileindex2 As Integer

        Label8.Text = "Working ... "

        Try

            Dim NameArray(0 To 100) As String
            For kk = 0 To DataSet2.Tables(0).Rows.Count - 1
                NameArray(kk) = DataSet2.Tables(0).Rows(kk).Item(2)
            Next

            oXL = New Microsoft.Office.Interop.Excel.Application
            oXL.Visible = False
            Label8.Text = "Working ... " & "Excel.Application is started"

            oWBs = oXL.Workbooks

            oWB2 = oWBs.Add

            Label8.Text = "Working ... " & "A new workbook is created"

            oSheet2 = oWB2.Worksheets(1)

            oSheet2.Range("A1").Offset(0, 0).Value = "Timesheet Filename"
            oSheet2.Range("A1").Offset(0, 1).Value = "Employee Name"
            oSheet2.Range("A1").Offset(0, 2).Value = "Date of Timesheet Record"
            oSheet2.Range("A1").Offset(0, 3).Value = "Client & Project Number"
            oSheet2.Range("A1").Offset(0, 4).Value = "Hours Recorded"
            oSheet2.Range("A1").Offset(0, 5).Value = "Task Description"

            j = 0

            aa = SearchDirectory.Text
            bb = ClientNumber.Text
            zz = ProjectNumberTextBox.Text
            cc = StartDate.Value
            dd = EndDate.Value
            ii = CheckedListBox1.CheckedIndices

            ProgressBar1.Value = 0
            ProgressBar1.Maximum = 100
            ProgressBar1.Step = 1

            'ProgressBar1.Style = ProgressBarStyle.Marquee
            'ProgressBar1.MarqueeAnimationSpeed = 30

            Label8.Text = "Working ... " & "now searching " & aa


            For Each foundFile As String In My.Computer.FileSystem.GetFiles(aa)
                foundFileindex1 = InStr(Path.GetFileName(foundFile), " ")
                foundFileindex2 = InStr(Path.GetFileName(foundFile), ".")
                ' MsgBox(My.Computer.FileSystem.GetFiles(aa).Count)
                ProgressBar1.Step = 1
                ProgressBar1.PerformStep()
                Dothething(foundFileindex1, foundFileindex2, foundFile, dd, cc, ii, NameArray, oWB, oWBs, oSheet, oWB2, oSheet2, bb, zz)
            Next

            For Each foundDirectory2 As String In My.Computer.FileSystem.GetDirectories(aa)
                Label8.Text = "Working ... " & "now searching " & foundDirectory2
                For Each foundFile As String In My.Computer.FileSystem.GetFiles(foundDirectory2)
                    foundFileindex1 = InStr(Path.GetFileName(foundFile), " ")
                    foundFileindex2 = InStr(Path.GetFileName(foundFile), ".")
                    ProgressBar1.Step = 1
                    'MsgBox(My.Computer.FileSystem.GetFiles(foundDirectory2).Count)
                    ProgressBar1.PerformStep()
                    Dothething(foundFileindex1, foundFileindex2, foundFile, dd, cc, ii, NameArray, oWB, oWBs, oSheet, oWB2, oSheet2, bb, zz)
                Next

                For Each foundDirectory1 As String In My.Computer.FileSystem.GetDirectories(foundDirectory2)
                    Label8.Text = "Working ... " & "now searching " & foundDirectory1

                    For Each foundFile As String In My.Computer.FileSystem.GetFiles(foundDirectory1)
                        foundFileindex1 = InStr(Path.GetFileName(foundFile), " ")
                        foundFileindex2 = InStr(Path.GetFileName(foundFile), ".")
                        ProgressBar1.Step = 1
                        'MsgBox(My.Computer.FileSystem.GetFiles(foundDirectory1).Count)
                        ProgressBar1.PerformStep()
                        Dothething(foundFileindex1, foundFileindex2, foundFile, dd, cc, ii, NameArray, oWB, oWBs, oSheet, oWB2, oSheet2, bb, zz)
                    Next

                    For Each foundDirectory As String In My.Computer.FileSystem.GetDirectories(foundDirectory1)
                        Label8.Text = "Working ... " & "now searching " & foundDirectory

                        For Each foundFile As String In My.Computer.FileSystem.GetFiles(foundDirectory)
                            foundFileindex1 = InStr(Path.GetFileName(foundFile), " ")
                            foundFileindex2 = InStr(Path.GetFileName(foundFile), ".")
                            ProgressBar1.Step = 1
                            'MsgBox(My.Computer.FileSystem.GetFiles(foundDirectory1).Count)
                            ProgressBar1.PerformStep()
                            Dothething(foundFileindex1, foundFileindex2, foundFile, dd, cc, ii, NameArray, oWB, oWBs, oSheet, oWB2, oSheet2, bb, zz)
                        Next
                    Next
                Next
            Next

            'ProgressBar1.Style = ProgressBarStyle.Marquee
            'ProgressBar1.MarqueeAnimationSpeed = 0

            If j = 0 Then
                Label8.Text = "No records found."
            Else
                Label8.Text = "Search complete."
                oXL.Visible = True
                oWB2.Save()
                Console.WriteLine("Results Presented")
            End If
        Catch ex As Exception
        Finally

            If Not oRng2 Is Nothing Then
                Marshal.FinalReleaseComObject(oRng2)
                oRng2 = Nothing
            End If
            If Not oRng1 Is Nothing Then
                Marshal.FinalReleaseComObject(oRng1)
                oRng1 = Nothing
            End If
            If Not oCells Is Nothing Then
                Marshal.FinalReleaseComObject(oCells)
                oCells = Nothing
            End If
            If Not oSheet Is Nothing Then
                Marshal.FinalReleaseComObject(oSheet)
                oSheet = Nothing
            End If
            If Not oWB Is Nothing Then
                Marshal.FinalReleaseComObject(oWB)
                oWB = Nothing
            End If
            If Not oWBs Is Nothing Then
                Marshal.FinalReleaseComObject(oWBs)
                oWBs = Nothing
            End If
            If Not oXL Is Nothing Then
                Marshal.FinalReleaseComObject(oXL)
                oXL = Nothing
            End If

        End Try

    End Sub

    Public Sub Dothething(foundFileindex1, foundFileindex2, foundFile, dd, cc, ii, NameArray, oWB, OWBs, oSheet, OWB2, oSheet2, bb, zz)

        Dim foundFileInitials As String
        Dim foundFileDate As String
        Dim foundFileYear As Integer
        Dim foundFileMonth As Integer
        Dim foundFileDay As Integer
        Dim projectnumber As String
        Dim Description As String
        Dim hours As Double
        Dim k As Integer

        If foundFileindex1 = 3 And foundFileindex2 = 14 Or Microsoft.VisualBasic.Left(Path.GetFileName(foundFile), 4) = "RWeb" Then
            foundFileInitials = Microsoft.VisualBasic.Left(Path.GetFileName(foundFile), foundFileindex1 - 1)
            foundFileDate = Microsoft.VisualBasic.Right(Microsoft.VisualBasic.Left(Path.GetFileName(foundFile), foundFileindex2 - 1), Len(Microsoft.VisualBasic.Left(Path.GetFileName(foundFile), foundFileindex2 - 1)) - foundFileindex1)
            foundFileYear = Microsoft.VisualBasic.Left(foundFileDate, 4)
            foundFileMonth = Microsoft.VisualBasic.Right(Microsoft.VisualBasic.Left(foundFileDate, 7), 2)
            foundFileDay = Microsoft.VisualBasic.Right(Microsoft.VisualBasic.Left(foundFileDate, 10), 2)
            Label8.Text = "Working ... " & "now searching " & Path.GetFileName(foundFile) ' & " [" & foundFileInitials & " " & foundFileYear & " " & foundFileMonth & " " & foundFileDay & "]"
            If DateSerial(foundFileYear, foundFileMonth, foundFileDay) > cc And DateSerial(foundFileYear, foundFileMonth, foundFileDay) < dd Then
                For Each IndexChecked In ii
                    If foundFileInitials = NameArray(IndexChecked) Then
                        oWB = OWBs.Open(foundFile, , True)
                        For m = 1 To oWB.Worksheets.Count
                            If (oWB.Worksheets(m).name <> "Template" And oWB.Worksheets(m).range("e10").value = "Description") Or oWB.Worksheets(m).name = "Sheet1" Then
                                oSheet = oWB.Worksheets(m)
                            Else
                            End If
                        Next
                        For k = 0 To 100
                            If (Microsoft.VisualBasic.Left(oSheet.Range("A1").Offset(k, 0).Value, 3) = bb) And ((Microsoft.VisualBasic.Right(oSheet.Range("A1").Offset(k, 0).Value, 3) = zz) Or (zz = "")) Then
                                'If oSheet.Range("A1").Offset(k, 12).Value <> "" Or oSheet.Range("A1").Offset(k, 12).Value > 0 Then
                                projectnumber = oSheet.Range("A1").Offset(k, 0).Value
                                Description = oSheet.Range("A1").Offset(k, 4).Value
                                hours = oSheet.Range("A1").Offset(k, 12).Value
                                OWB2.Activate()
                                oSheet2.Range("A2").Offset(j, 0).Value = Path.GetFileName(foundFile)
                                oSheet2.Range("A2").Offset(j, 1).Value = NameArray(IndexChecked)
                                oSheet2.Range("A2").Offset(j, 2).Value = DateSerial(foundFileYear, foundFileMonth, foundFileDay)
                                oSheet2.Range("A2").Offset(j, 3).Value = projectnumber
                                oSheet2.Range("A2").Offset(j, 4).Value = hours
                                oSheet2.Range("A2").Offset(j, 5).Value = Description
                                oWB.Activate()
                                j = j + 1
                                'Else
                                'End If
                            Else
                            End If
                        Next
                        oWB.Close(False)
                    Else
                    End If
                Next
            Else
            End If
        Else
        End If
    End Sub

End Class
