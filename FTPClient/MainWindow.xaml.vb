Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Status

Class MainWindow
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        FTPInput.Focus()
    End Sub

    Dim LS As New List(Of String)
    Dim ListOfFilesOnFTPSite As New List(Of String)
    Dim Username As String
    Dim Password As String
    Dim FTPAdres As String
    Dim FileFTP As String
    Dim ErrorM As String
    Dim DownloadDirectory As String
    Dim UploadDirectory As String
    Dim UploadURL As String
    Dim FileUploadFTP As String
    Private WithEvents FTPLoader As New ComponentModel.BackgroundWorker
    Private WithEvents FTPDownloader As New ComponentModel.BackgroundWorker
    Private WithEvents FTPUploader As New ComponentModel.BackgroundWorker


    Private Sub FTPLoader_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FTPLoader.DoWork
        Try
            Dim ftpRequest As FtpWebRequest = Nothing
            Dim ftpResponse As FtpWebResponse = Nothing
            Dim strReader As StreamReader = Nothing
            Dim sline As String = ""
            Try
                ftpRequest = CType(WebRequest.Create(FTPAdres), FtpWebRequest)
                With ftpRequest
                    .Credentials = New NetworkCredential(Username, Password)
                    .Method = WebRequestMethods.Ftp.ListDirectory
                End With
                ftpResponse = CType(ftpRequest.GetResponse, FtpWebResponse)
                strReader = New StreamReader(ftpResponse.GetResponseStream)
                If strReader IsNot Nothing Then sline = strReader.ReadLine
                While sline IsNot Nothing
                    ListOfFilesOnFTPSite.Add(sline)
                    sline = strReader.ReadLine
                End While
            Catch ex As Exception
                If (ex.Message.Contains("(530)")) Then
                    System.Windows.MessageBox.Show("The information you entered is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                Else
                    If (ex.Message.Contains("(550)")) Then
                        System.Windows.MessageBox.Show("Directory or root not found", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning)
                    End If
                End If
            Finally
                If ftpResponse IsNot Nothing Then
                    ftpResponse.Close()
                    ftpResponse = Nothing
                End If

                If strReader IsNot Nothing Then
                    strReader.Close()
                    strReader = Nothing
                End If
            End Try
        Catch ex As Exception
        End Try
        ListOfFilesOnFTPSite = ListOfFilesOnFTPSite
    End Sub

    Private Sub FTPLoader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FTPLoader.RunWorkerCompleted
        ConnectButton.IsEnabled = True
        If ListOfFilesOnFTPSite.Contains(".") Then
            ListOfFilesOnFTPSite.Remove(".")
        End If
        If ListOfFilesOnFTPSite.Contains("..") Then
            ListOfFilesOnFTPSite.Remove("..")
        End If
        For Each Item As String In ListOfFilesOnFTPSite
            ServerFiles.Items.Add(Item)
            DownloadLocationButton.IsEnabled = True
            UploadLocationButton.IsEnabled = True
            UploadLocationInput.Text = FTPAdres
        Next
        If ListOfFilesOnFTPSite.Count = 0 Then
            ServerFiles.Items.Add("No content in directory...")
            StatusDownloadLabel.Text = "Nothing to download"
            ConnectButton.IsEnabled = True
        End If
    End Sub

    Private Sub FTPDownloader_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FTPDownloader.DoWork
        Dim ftpRequest As FtpWebRequest =
        WebRequest.Create(FTPAdres + FileFTP)
        ftpRequest.Credentials = New NetworkCredential(Username, Password)
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile
        Try
            Using ftpStream As Stream = ftpRequest.GetResponse().GetResponseStream(),
            FileStream As Stream = File.Create(DownloadDirectory)
                ftpStream.CopyTo(FileStream)
            End Using
        Catch ex As UnauthorizedAccessException
            ErrorM = "NoAc"
        Catch ex2 As WebException
            ErrorM = "MapDownload"
        End Try
    End Sub

    Private Sub FTPDownloader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FTPDownloader.RunWorkerCompleted
        If ErrorM = "NoAc" Then
            StatusDownloadLabel.Foreground = Brushes.Red
            StatusDownloadLabel.Text = "Permission error"
            DownloadButton.IsEnabled = True
        Else
            If ErrorM = "MapDownload" Then
                StatusDownloadLabel.Foreground = Brushes.Red
                StatusDownloadLabel.Text = "Not allowed directory's"
                DownloadButton.IsEnabled = True
            Else
                StatusDownloadLabel.Foreground = Brushes.Green
                StatusDownloadLabel.Text = "Download Completed!"
                DownloadButton.IsEnabled = False
                DownloadFileInput.Clear()
                DownloadLocationInput.Clear()
                ServerFiles.Focus()
            End If
        End If
    End Sub

    Private Sub FTPUploader_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FTPUploader.DoWork
        Dim ftpRequest As FtpWebRequest =
        WebRequest.Create(UploadURL + FileUploadFTP)
        ftpRequest.Credentials = New NetworkCredential(Username, Password)
        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
        Try
            Using fileStream As Stream = File.OpenRead(UploadDirectory),
              ftpStream As Stream = ftpRequest.GetRequestStream()
                fileStream.CopyTo(ftpStream)
            End Using
        Catch ex As Exception
            ErrorM = "FileUploadError"
        End Try
    End Sub

    Private Sub FTPUploader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FTPUploader.RunWorkerCompleted
        If ErrorM = "FileUploadError" Then
            StatusUploadLabel.Foreground = Brushes.Red
            StatusUploadLabel.Text = "Upload Error!"
            UploadButton.IsEnabled = True
        Else
            StatusUploadLabel.Foreground = Brushes.Green
            StatusUploadLabel.Text = "Upload Completed!"
            UploadButton.IsEnabled = True
            UploadFileInput.Clear()
            UploadLocationInput.Clear()
            ServerFiles.Items.Clear()
            ListOfFilesOnFTPSite.Clear()
            FTPLoader.RunWorkerAsync()
        End If
    End Sub
    Private Sub FTPInput_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles FTPInput.PreviewMouseDoubleClick
        FTPInput.SelectAll()
    End Sub

    Private Sub UsernameInput_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles UsernameInput.PreviewMouseDoubleClick
        UsernameInput.SelectAll()
    End Sub

    Private Sub PasswordInput_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles PasswordInput.PreviewMouseDoubleClick
        PasswordInput.SelectAll()
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As RoutedEventArgs) Handles ConnectButton.Click
        ServerFiles.Items.Clear()
        ListOfFilesOnFTPSite.Clear()
        If FTPInput.Text = "" Then
            System.Windows.MessageBox.Show("Please fill in your FTP adres", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning)
        Else
            If Not FTPInput.Text.Contains("ftp://") Then
                FTPAdres = "ftp://" + FTPInput.Text
                Username = UsernameInput.Text
                Password = PasswordInput.Password
                ConnectButton.IsEnabled = False
                FTPLoader.RunWorkerAsync()
            Else
                FTPAdres = FTPInput.Text
                Username = UsernameInput.Text
                Password = PasswordInput.Password
                ConnectButton.IsEnabled = False
                FTPLoader.RunWorkerAsync()
            End If
        End If
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As RoutedEventArgs) Handles ClearButton.Click
        FTPInput.Clear()
        UsernameInput.Clear()
        PasswordInput.Clear()
    End Sub
    Private Sub FTPInput_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles FTPInput.PreviewKeyDown
        If (e.Key = Key.Space) Then
            e.Handled = True
        End If
    End Sub

    Private Sub PasswordInput_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles PasswordInput.PreviewKeyDown
        If (e.Key = Key.Space) Then
            e.Handled = True
        End If
        If (e.Key = Key.Enter) Then
            ServerFiles.Items.Clear()
            ListOfFilesOnFTPSite.Clear()
            If String.IsNullOrEmpty(FTPInput.Text) Then
                System.Windows.MessageBox.Show("Please fill in your FTP adres", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning)
            Else
                If Not FTPInput.Text.Contains("ftp://") Then
                    FTPAdres = "ftp://" + FTPInput.Text
                    Username = UsernameInput.Text
                    Password = PasswordInput.Password
                    ConnectButton.IsEnabled = False
                    FTPLoader.RunWorkerAsync()
                Else
                    FTPAdres = FTPInput.Text
                    Username = UsernameInput.Text
                    Password = PasswordInput.Password
                    ConnectButton.IsEnabled = False
                    FTPLoader.RunWorkerAsync()
                End If
            End If
        End If
    End Sub

    Private Sub ServerFiles_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles ServerFiles.PreviewMouseDoubleClick
        If ServerFiles.Items.Count = 0 Then
        Else
            If ServerFiles.SelectedItem.ToString.Contains("No content in directory...") Then
                StatusDownloadLabel.Text = "Nothing to download"
            Else
                DownloadFileInput.Text = String.Empty
                FTPAdres = FTPAdres + "/" + ServerFiles.SelectedItem.ToString()
                ServerFiles.Items.Clear()
                ListOfFilesOnFTPSite.Clear()
                FTPLoader.RunWorkerAsync()
            End If
        End If
    End Sub

    Private Sub ServerFiles_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ServerFiles.SelectionChanged
        If ServerFiles.Items.Count = 0 Then
        Else
            If ServerFiles.SelectedItem.ToString.Contains("No content in directory...") Then
                StatusDownloadLabel.Text = "Nothing to download"
            Else
                DownloadFileInput.Text = ServerFiles.SelectedItem.ToString()
                StatusDownloadLabel.Foreground = Brushes.Black
                StatusDownloadLabel.Text = "Ready to download"
            End If
            If String.IsNullOrEmpty(DownloadFileInput.Text) Then
                DownloadButton.IsEnabled = False
            Else
                If String.IsNullOrEmpty(DownloadLocationInput.Text) Then
                    DownloadButton.IsEnabled = False
                Else
                    DownloadButton.IsEnabled = True
                    StatusDownloadLabel.Text = "Ready to download"
                End If
            End If
        End If
    End Sub

    Private Sub DownloadLocationButton_Click(sender As Object, e As RoutedEventArgs) Handles DownloadLocationButton.Click
        Dim DialogDownload = New System.Windows.Forms.FolderBrowserDialog()
        DialogDownload.ShowDialog()
        DownloadLocationInput.Text = DialogDownload.SelectedPath
        If String.IsNullOrEmpty(DownloadFileInput.Text) Then
            DownloadButton.IsEnabled = False
        Else
            If String.IsNullOrEmpty(DownloadLocationInput.Text) Then
                DownloadButton.IsEnabled = False
            Else
                DownloadButton.IsEnabled = True
            End If
        End If
    End Sub

    Private Sub DownloadButton_Click(sender As Object, e As RoutedEventArgs) Handles DownloadButton.Click
        ErrorM = String.Empty
        DownloadButton.IsEnabled = False
        DownloadDirectory = DownloadLocationInput.Text + "\" + DownloadFileInput.Text
        FileFTP = "/" + DownloadFileInput.Text
        StatusDownloadLabel.Foreground = Brushes.Black
        StatusDownloadLabel.Text = "Downloading..."
        FTPDownloader.RunWorkerAsync()
    End Sub

    Private Sub UploadLocationButton_Click(sender As Object, e As RoutedEventArgs) Handles UploadLocationButton.Click
        Dim dlg As New Microsoft.Win32.OpenFileDialog()
        dlg.Title = "Choose your file to upload"
        Dim result As Nullable(Of Boolean) = dlg.ShowDialog()
        If result = True Then
            FileUploadFTP = System.IO.Path.GetFileName(dlg.FileName)
            UploadFileInput.Text = dlg.FileName
        Else
            If result = False Then
                StatusUploadLabel.Foreground = Brushes.Black
                StatusUploadLabel.Text = "Nothing to upload"
                UploadFileInput.Text = String.Empty
            End If
        End If
        If String.IsNullOrEmpty(UploadFileInput.Text) Then
            UploadButton.IsEnabled = False
        Else
            If String.IsNullOrEmpty(UploadLocationInput.Text) Then
                UploadButton.IsEnabled = False
            Else
                StatusUploadLabel.Foreground = Brushes.Black
                StatusUploadLabel.Text = "Ready for upload"
                UploadButton.IsEnabled = True
            End If
        End If
    End Sub

    Private Sub UploadButton_Click(sender As Object, e As RoutedEventArgs) Handles UploadButton.Click
        ErrorM = String.Empty
        UploadURL = UploadLocationInput.Text + "/"
        UploadButton.IsEnabled = False
        UploadDirectory = UploadFileInput.Text
        StatusDownloadLabel.Foreground = Brushes.Black
        StatusUploadLabel.Text = "Uploading..."
        FTPUploader.RunWorkerAsync()
    End Sub
End Class
