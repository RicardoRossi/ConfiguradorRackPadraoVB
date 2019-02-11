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
    Dim retBool As Boolean

    Public Sub Main()

        'Dim codigos As IEnumerable(Of Integer) = Enumerable.Range(4020001, 5)
        'Dim listaDeCodigos = lerTXT.LerTXT
        Dim listaDeCodigos As List(Of String) = New List(Of String) From {"4020046", "4020001"}

        For Each codigo In listaDeCodigos
            Dim fullNameSaveAs = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\RACK PADRAO TESTE\" & codigo & ".SLDASM"
            Try
                swApp = _swApp() 'Atribui o objeto Sldworks do singleton a variável local swApp
            Catch ex As Exception
                WriteLine(ex.Message) 'Pegar a exception lançada no singleton em caso de erro
                Stop
            End Try

            Try
                'Dim caminhoTemplate = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM"
                Dim caminhoTemplate = "C:\Users\54808\Documents\template_00_rp.SLDASM"

                swModel = swApp.OpenDoc6(caminhoTemplate, swDocumentTypes_e.swDocASSEMBLY, swOpenDocOptions_e.swOpenDocOptions_ReadOnly, "", erro, aviso)
                If swModel Is Nothing Then
                    Dim erroMsg As swFileLoadError_e
                    erroMsg = CType(erro, swFileLoadError_e)
                    Console.WriteLine(erroMsg)
                End If
                'swApp.ActivateDoc(caminhoTemplate)
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
            'swApp.ActivateDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM")
            'swModel = swApp.ActiveDoc

            Try
                SalvarRack(fullNameSaveAs)
            Catch ex As Exception
                Continue For
            End Try

            'Preencher propriedades
            Propriedades.AdicionarPropriedades(codigo)

            swApp = _swApp()
            swModel = swApp.ActiveDoc
            swExt = swModel.Extension
            'Exclude peça da BOM
            Dim selecao = "SCM_RP-1@" & codigo
            retBool = swExt.SelectByID2(selecao, "COMPONENT", 0, 0, 0, False, 0, Nothing, 0)
            swAsm = swModel
            retBool = swAsm.CompConfigProperties5(2, 0, False, True, "", True, False)

            Configuracao.TrocarConfiguracaoBase(swAsm)
            swModel.Save()

            '''''''''''''''''''''''''''''''''''''''''''''''''
            'Cria desenho
            'Desenhar(fullNameSaveAs)

            'swAsm = swModel
            'Dim pecas() As Object = swAsm.GetComponents(True)
            'Dim comp As Component2
            'For Each p In pecas
            '    comp = p
            '    comp.Select(True)
            'Next
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'swModel.EditRebuild3()
            'ReadKey()
            'Stop

            swApp.CloseAllDocuments(True)

            swApp.ExitApp()

            Console.WriteLine(codigo)
            'FinalizarSolidWorks()
        Next
        FinalizarSolidWorks()
    End Sub

    Private Sub SalvarRack(fullNameSaveAs As String)
        Dim swApp = _swApp()
        Dim swModel As ModelDoc2
        Dim swExt As ModelDocExtension

        Try
            swModel = swApp.ActiveDoc
            swExt = swModel.Extension

            'Salva a montagem
            retBool = swExt.SaveAs(fullNameSaveAs, swSaveAsVersion_e.swSaveAsCurrentVersion,
                                   swSaveAsOptions_e.swSaveAsOptions_Silent, Nothing, erro, aviso)
        Catch ex As Exception
            Throw
        End Try

    End Sub
End Module
