Module SolidWorksSingleton 'Public torna o modulo disponivel à outros projetos
    Dim swApp As SldWorks.SldWorks
    Dim swModel As SldWorks.ModelDoc2
    Function _swApp() As SldWorks.SldWorks
        Try
            If swApp Is Nothing Then
                swApp = GetObject("", "SldWorks.Application")
                swApp.Visible = False
                'Prevents the user from having control over the application
                swApp.UserControlBackground = False
                Return swApp
            Else
                Return swApp
            End If

        Catch ex As Exception
            Throw 'Lança exception para o chamador
            Return Nothing
        End Try
    End Function

    Sub FinalizarSolidWorks()
        swModel = Nothing
        swApp.ExitApp()
        swApp = Nothing
        Dim swProc() As Process = Process.GetProcessesByName("SLDWORKS")
        For Each proc In swProc
            Dim ID = proc.Id
            proc.Kill()
        Next
    End Sub
End Module
