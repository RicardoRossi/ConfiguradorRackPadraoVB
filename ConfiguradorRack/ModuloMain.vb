Imports SwConst
Imports SldWorks
Imports System.Console
Imports TrocaConfiguracao
Module ModuloMain
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swExt As ModelDocExtension
    Dim swAsm As AssemblyDoc
    Dim swView As ModelView
    Dim erro, aviso As Integer

    Public Sub Main()

        Dim codigos As IEnumerable(Of Integer) = Enumerable.Range(4020001, 3)

        For Each codigo In codigos
            Dim fullNameSaveAs = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\RACK PADRAO TESTE\" & codigo & ".SLDASM"
            Try
                swApp = _swApp() 'Atribui o objeto Sldworks do singleton a variável local swApp
            Catch ex As Exception
                WriteLine(ex.Message) 'Pegar a exception lançada no singleton em caso de erro
                Stop
            End Try

            Try
                Dim caminhoTemplate = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM"
                swApp.OpenDoc6(caminhoTemplate, swDocumentTypes_e.swDocASSEMBLY, swOpenDocOptions_e.swOpenDocOptions_ReadOnly, "", erro, aviso)
                swApp.ActivateDoc(caminhoTemplate)
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

            'Exclude peça da BOM
            swExt = swModel.Extension
            Dim selecao = "SCM_RP-1@" & codigo
            Dim retBool = swExt.SelectByID2(selecao, "COMPONENT", 0, 0, 0, False, 0, Nothing, 0)
            swAsm = swModel
            retBool = swAsm.CompConfigProperties5(2, 0, False, True, "", True, False)

            Configuracao.TrocarConfiguracaoBase(swAsm)

            'Salva a montagem
            retBool = swExt.SaveAs(fullNameSaveAs, swSaveAsVersion_e.swSaveAsCurrentVersion, swSaveAsOptions_e.swSaveAsOptions_Silent, Nothing, erro, aviso)

            'swModel.Save()

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
