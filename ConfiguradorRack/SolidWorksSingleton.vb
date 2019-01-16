Module SolidWorksSingleton 'Public torna o modulo disponivel à outros projetos
    Dim swApp As SldWorks.SldWorks
    Dim swModel As SldWorks.ModelDoc2
    Function _swApp() As SldWorks.SldWorks
        Try
            If swApp Is Nothing Then
                swApp = GetObject("", "SldWorks.Application")
                swApp.Visible = True
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
        swApp.ExitApp()
        swApp = Nothing
    End Sub
End Module
