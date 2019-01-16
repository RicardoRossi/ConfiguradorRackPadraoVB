Imports System.IO
Imports SldWorks
Imports SwConst

Module ModuleMain
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swAsm As AssemblyDoc

    Private Sub Main()
        Try
            swApp = _swApp()
        Catch ex As Exception
            Exit Sub
        End Try

        swModel = swApp.ActiveDoc

        swModel.ReloadOrReplace(False, "C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\RACK PADRAO TESTE\4020003.SLDASM", True)

        'swApp.SendMsgToUser("Conectado no SolidWorks")
        'Dim nomeDoAssembly As String
        'nomeDoAssembly = swModel.GetPathName

        'OpenDocTemplate()

        'Dim listaDeArquivos As New List(Of String)
        'listaDeArquivos.Add("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\BASE\2048212.sldprt")
        'listaDeArquivos.Add("C:\ELETROFRIO\ENGENHARIA SMR\BIBLIOTECA FORNECEDORES\MECÂNICA\BOIA ELETRÔNICA\KRIWAN\2044505.SLDPRT")
        'listaDeArquivos.Add("C:\ELETROFRIO\ENGENHARIA SMR\BIBLIOTECA FORNECEDORES\MECÂNICA\COMPRESSORES\BITZER\SEMI HERMÉTICO\ALTERNATIVO\COM CONTROLE CAP\2025541.SLDPRT")
        'listaDeArquivos.Add("C:\ELETROFRIO\ENGENHARIA SMR\BIBLIOTECA FORNECEDORES\MECÂNICA\COMPRESSORES\BITZER\SEMI HERMÉTICO\ALTERNATIVO\VENTILADOR CABEÇOTE\2025694.SLDPRT")
        'listaDeArquivos.Add("C:\ELETROFRIO\ENGENHARIA SMR\BIBLIOTECA ELETROFRIO\MECÂNICA\ADAPTADORES\TRAX OIL OCTAGON\2002053.SLDASM")

        'Dim listaDePecas As New List(Of Peca)
        'Dim peca As New Peca

        'For Each arquivo In listaDeArquivos

        '    Dim extensaoArquivo = Path.GetExtension(arquivo)
        '    Dim tipo As swDocumentTypes_e
        '    Select Case extensaoArquivo.ToUpper
        '        Case ".SLDPRT"
        '            tipo = swDocumentTypes_e.swDocPART
        '        Case ".SLDASM"
        '            tipo = swDocumentTypes_e.swDocASSEMBLY
        '    End Select


        '    _OpenDoc(arquivo, tipo)
        '    Dim swComp = _InsertPeca(arquivo) 'Recebe o Component2 do arquivo.

        '    Select Case Path.GetFileNameWithoutExtension(swComp.GetPathName)
        '        Case "2048212"
        '            peca = _ConfigurarPeca(swComp, "cs_000")
        '        Case "2044505"
        '            peca = _ConfigurarPeca(swComp, "cs_boia")
        '        Case "2025541"
        '            peca = _ConfigurarPeca(swComp, "cs_2048212")
        '        Case "2025694"
        '            peca = _ConfigurarPeca(swComp, "cs_ventilador")
        '        Case "2002053"
        '            peca = _ConfigurarPeca(swComp, "cs_adaptador")
        '    End Select

        '    listaDePecas.Add(peca)
        'Next

        'swAsm = swModel
        'Dim pecas() As Object = swAsm.GetComponents(True)

        'Dim comp As Component2
        'For Each p In pecas
        '    comp = p
        '    Dim nomecomp = comp.Name
        'Next


        '_MontarPecas(listaDePecas)

        '_MontarPeca(peca)
        'WriteLine(peca.swComp.Name2)
        ''WriteLine(peca.scm)
        ''WriteLine(peca.cs)
        'swModel.EditRebuild3()


    End Sub

    Sub OpenDocTemplate()
        swApp.OpenDoc("C:\ELETROFRIO\ENGENHARIA SMR\PRODUTOS FINAIS ELETROFRIO\MECÂNICA\RACK PADRAO\KIT COMPRESSOR\template_MONTCPRP.SLDASM", swDocumentTypes_e.swDocASSEMBLY)
    End Sub

    Sub _OpenDoc(fullPath As String, tipo As swDocumentTypes_e)
        Dim erro As Integer
        Dim aviso As Integer
        swApp.DocumentVisible(False, tipo) 'Não mostra a peça na tela. o Documento estará somente na memoria.
        swApp.OpenDoc6(fullPath, tipo, swOpenDocOptions_e.swOpenDocOptions_Silent, "", erro, aviso)
    End Sub

    Function _InsertPeca(fullPath As String) As Component2
        swModel = swApp.ActiveDoc
        Dim swAsm As AssemblyDoc = swModel
        Dim swComp As Component2
        Dim randonX = New Random
        Dim cx = randonX.NextDouble
        Dim randonY = New Random
        Dim cY = randonX.NextDouble

        swComp = swAsm.AddComponent4(fullPath, "", cx, cY, 0) 'Adiciona na montagem
        'swModel = swComp.GetModelDoc2
        swComp.Select(False)
        swAsm.UnfixComponent() 'Deixa float, para posteriormente add mate entre cs_ e scm
        Return swComp 'Retorna o Component2
    End Function

    Function _ConfigurarPeca(swCompParam As Component2, scmParam As String) As Peca
        Dim cs = "cs_" & Path.GetFileNameWithoutExtension(swCompParam.GetPathName)
        Dim peca As New Peca With {.cs = cs, .scm = scmParam, .swComp = swCompParam}
        Return peca
    End Function

    Sub _MontarPecas(_listaDepecas As List(Of Peca))
        For Each p In _listaDepecas
            Dim cs_ = p.cs
            Dim scm_ = p.scm
            Dim swFeaturePecaOrigem As Feature = p.swComp.FeatureByName(cs_)
            swFeaturePecaOrigem.Select(True)
            Dim pecaOrigem = p.swComp.Name
            For Each peca In _listaDepecas
                If scm_ = "cs_000" Then
                    Dim swModel As ModelDoc2
                    swModel = swApp.ActiveDoc
                    'Dim nomeDoTemplate = swModel.GetPathName
                    'swModel = _swApp.ActivateDoc(nomeDoTemplate)

                    Dim swExt As ModelDocExtension
                    swExt = swModel.Extension
                    Dim boolstatus = swExt.SelectByID2(scm_, "COORDSYS", 0.0, 0.0, 0.0, True, 0, Nothing, swSelectOption_e.swSelectOptionDefault) 'Hardcode de string em "COORDSYS", com o enum swSelectType_e.swSelCOORDSYS não estava selecionando
                    Exit For
                End If
                Dim swFeaturePecaDestino As Feature = peca.swComp.FeatureByName(scm_)
                Dim pecaDestino = peca.swComp.Name
                If Not swFeaturePecaDestino Is Nothing Then
                    Dim boolstatus = swFeaturePecaDestino.Select(True)
                    Exit For
                End If
            Next
            Dim swAsm As AssemblyDoc = swModel
            Dim erro As Integer
            swAsm.AddMate5(swMateType_e.swMateCOORDINATE, swMateAlign_e.swAlignNONE, False, 0, 0, 0, 0, 0, 0, 0, 0, False, True, 0, erro)
            swModel.ClearSelection()
        Next
    End Sub

    Private Sub _SelecionarGeometria(geometriaDeReferencia As String)
        Dim boolstatus
        Dim swExt = swModel.Extension
        boolstatus = swExt.SelectByID2(geometriaDeReferencia, swSelectType_e.swSelCOORDSYS, 0, 0, 0, True, 0, Nothing, swSelectOption_e.swSelectOptionDefault) 'Seleciona a geometria e append
    End Sub
End Module
