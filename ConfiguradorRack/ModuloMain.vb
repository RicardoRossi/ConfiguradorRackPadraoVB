Imports SwConst
Imports SldWorks
Imports System.Console
Imports TrocaConfiguracao
Imports System.IO

Module ModuloMain
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swExt As ModelDocExtension
    Dim swAsm As AssemblyDoc
    Dim swView As ModelView

    Public Sub Main()
        Dim contador = 1
        'Dim codigos As IEnumerable(Of Integer) = Enumerable.Range(4020001, 5)
        'Dim listaDeCodigos = lerTXT.LerTXT
        Dim listaDeCodigos As List(Of String) = New List(Of String) From {"4020141"}
        Const caminhoTemplate = "C:\Users\54808\Documents\template_00_rp.SLDASM"
        Dim fullNameSaveAs As String = Nothing

        For Each codigo In listaDeCodigos
            fullNameSaveAs = "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\RACK PADRAO TESTE\" & codigo & ".SLDASM"
            Try
                swApp = _swApp() 'Atribui o objeto Sldworks do singleton a variável local swApp
            Catch ex As Exception
                WriteLine(ex.Message) 'Pegar a exception lançada no singleton em caso de erro
                Stop
            End Try

            Dim erro, aviso As Integer
            Dim retBool As Boolean
            swModel = swApp.OpenDoc6(caminhoTemplate, swDocumentTypes_e.swDocASSEMBLY, swOpenDocOptions_e.swOpenDocOptions_ReadOnly, "", erro, aviso)

            'Prevente feature tree from updating
            swModel.FeatureManager.EnableFeatureTree = False

            If erro <> 0 Or aviso <> 0 Then
                Console.WriteLine("Erro " & erro & " - Abrir template")
                Console.WriteLine("Aviso " & aviso & " - Abrir template")
            End If

            Try
                GerarPlanilhaDoKit(codigo)
            Catch ex As Exception
                WriteLine(ex.Message)
            End Try

            swApp.DocumentVisible(True, swDocumentTypes_e.swDocASSEMBLY)
            swApp.DocumentVisible(True, swDocumentTypes_e.swDocPART)
            'swApp.ActivateDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\template_00_rp.SLDASM")
            'swModel = swApp.ActiveDoc

            'Preencher propriedades
            Propriedades.AdicionarPropriedades(codigo)

            swApp = _swApp()
            swModel = swApp.ActiveDoc
            swExt = swModel.Extension
            'Exclude peça da BOM
            Dim selecao = "SCM_RP-1@" & Path.GetFileNameWithoutExtension(swModel.GetPathName)
            retBool = swExt.SelectByID2(selecao, "COMPONENT", 0, 0, 0, False, 0, Nothing, 0)
            swAsm = swModel
            retBool = swAsm.CompConfigProperties5(2, 0, False, True, "", True, False)

            Configuracao.TrocarConfiguracaoBase(swAsm)
            'swModel.Save()

            Try
                SalvarRack(fullNameSaveAs)
            Catch ex As Exception
                swApp.CloseAllDocuments(True)
                Continue For
            End Try

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

            Console.WriteLine(codigo)
            fullNameSaveAs = Nothing
            'swExt = Nothing
            'swModel = Nothing
            'swApp = Nothing
            'swApp.ExitApp()

            If contador = 5 Then
                FinalizarSolidWorks()
                contador = 1
            End If
            contador += 1
        Next
        FinalizarSolidWorks()
    End Sub

    Private Sub SalvarRack(fullNameSaveAs As String)
        Dim swApp = _swApp()
        Dim swModel As ModelDoc2
        Dim swExt As ModelDocExtension
        Dim erro, aviso As Integer
        Dim retBool As Boolean

        Try
            swModel = swApp.ActiveDoc
            swExt = swModel.Extension

            'Salva a montagem
            retBool = swExt.SaveAs(fullNameSaveAs, swSaveAsVersion_e.swSaveAsCurrentVersion,
                                   swSaveAsOptions_e.swSaveAsOptions_Silent, Nothing, erro, aviso)
            If erro <> 0 Or aviso <> 0 Then
                Console.WriteLine("Erro " & erro & " - ao salvar")
                Console.WriteLine("Aviso " & aviso & " - ao salvar")
            End If

        Catch ex As Exception
            Dim erroCodigo As Object = Nothing
            Dim varMessage As Object = Nothing
            Dim varPath As Object = Nothing
            varMessage = swApp.GetLastSaveError(varPath, erroCodigo)
            Console.WriteLine("Documento gerador do erro " & varPath)
            Console.WriteLine("Codigo do erro " & erroCodigo)
            Console.WriteLine("Menssagem do erro " & varMessage)
            Throw
        End Try

    End Sub
End Module
