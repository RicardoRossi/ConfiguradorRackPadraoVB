Imports SwConst
Imports SldWorks
Imports System.Console
Module ModuloMain
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swExt As ModelDocExtension
    Dim swAsm As AssemblyDoc
    Dim swView As ModelView

    Public Sub Main()

        Dim codigos As IEnumerable(Of Integer) = Enumerable.Range(4020230, 1)

        For Each codigo In codigos
            Dim fullNameSaveAs = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\RACK PADRAO TESTE\" & codigo & ".SLDASM"
            Try
                swApp = _swApp() 'Atribui o objeto Sldworks do singleton a variável local swApp
            Catch ex As Exception
                WriteLine(ex.Message) 'Pegar a exception lançada no singleton em caso de erro
                Stop
            End Try

            Try
                swApp.OpenDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM", swDocumentTypes_e.swDocASSEMBLY)
                swApp.ActivateDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM")
            Catch ex As Exception
                Threading.Thread.Sleep(60000)
                Continue For
            End Try

            Try
                GerarPlanilhaDoKit(codigo)
            Catch ex As Exception
                WriteLine(ex.Message)
            End Try

            swApp.DocumentVisible(True, swDocumentTypes_e.swDocASSEMBLY)
            swApp.DocumentVisible(True, swDocumentTypes_e.swDocPART)
            swApp.ActivateDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM")
            swModel = swApp.ActiveDoc

            'Preencher propriedades
            Propriedades.AdicionarPropriedades(codigo)

            'Salva a montagem
            swModel.SaveAs3(fullNameSaveAs, 0, 0)

            'Exclude peça da BOM
            swExt = swModel.Extension
            Dim selecao = "SCM_RP-1@" & codigo
            Dim retBool = swExt.SelectByID2(selecao, "COMPONENT", 0, 0, 0, False, 0, Nothing, 0)
            swAsm = swModel
            retBool = swAsm.CompConfigProperties5(2, 0, False, True, "", True, False)
            'Cria desenho
            Desenhar(fullNameSaveAs)

            'swAsm = swModel
            'Dim pecas() As Object = swAsm.GetComponents(True)
            'Dim comp As Component2
            'For Each p In pecas
            '    comp = p
            '    comp.Select(True)
            'Next
            swModel.EditRebuild3()
            'ReadKey()
            'Stop
            swApp.CloseAllDocuments(True)
            'If True Then
            'swApp.ExitApp()
            'End If
            FinalizarSolidWorks()
        Next
    End Sub

End Module
