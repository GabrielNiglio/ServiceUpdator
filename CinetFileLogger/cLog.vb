Imports System.IO

Public Class cLog

    ReadOnly _Nombre As String
    Public _EscribeLog As Boolean = True

    Public Sub New()
        _Nombre = "LogNet_"
    End Sub
    Public Sub New(nombre As String)
        _Nombre = nombre
    End Sub

    Private ReadOnly Property AppPath() As String
        Get
            Return My.Application.Info.DirectoryPath & "\" & _Nombre & Date.Today.Year.ToString & Date.Today.Month.ToString & Date.Today.Day.ToString & ".txt"
        End Get
    End Property

    Public Sub EscribeLog(ByVal Proc As String, ByVal Query As String)
        If _EscribeLog Then
            Try
                Dim strArchivo = AppPath

                Dim SW As New StreamWriter(strArchivo, True)
                SW.WriteLine("/*" & Proc & " " & Date.Now.ToString("HH:mm:ss") & "*/ " & Query)
                SW.Close()
            Catch ex As Exception
            End Try
        End If
    End Sub

End Class
