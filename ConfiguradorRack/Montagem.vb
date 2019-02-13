Imports SwConst
Imports SldWorks
Module Montagem
    Dim swApp As SldWorks.SldWorks = _swApp()
    Dim swModel As ModelDoc2
    Dim swFeature As Feature
    Sub _MontarPecas(_listaDepecas As List(Of Peca))

        Dim swModel As ModelDoc2
        Dim swFeaturePecaOrigem As Feature = Nothing
        Dim swView As ModelView
        swApp = _swApp()
        swModel = swApp.ActiveDoc
        swView = swModel.ActiveView
        swView.EnableGraphicsUpdate = False 'Desabilita atualização da tela.

        For Each p In _listaDepecas
            Dim cs_ = p.item.cs
            Dim scm_ = p.item.scm
            'If não existir a feature sai for e vai pra próxima peça
            swFeaturePecaOrigem = p.componente.FeatureByName(cs_)
            If swFeaturePecaOrigem Is Nothing Then
                Continue For
            End If
            swFeaturePecaOrigem.Select(True)
            Dim pecaOrigem = p.componente.Name
            For Each peca In _listaDepecas
                If scm_ = "cs_0001" Then
                    'Dim nomeDoTemplate = swModel.GetPathName
                    'swModel = _swApp.ActivateDoc(nomeDoTemplate)
                    Dim swExt As ModelDocExtension
                    swExt = swModel.Extension
                    Dim boolstatus = swExt.SelectByID2(scm_, "COORDSYS", 0.0, 0.0, 0.0, True, 0, Nothing, swSelectOption_e.swSelectOptionDefault) 'Hardcode de string em "COORDSYS", com o enum swSelectType_e.swSelCOORDSYS não estava selecionando
                    Exit For
                End If
                Dim swFeaturePecaDestino As Feature = peca.componente.FeatureByName(scm_)
                Dim pecaDestino = peca.componente.Name
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
        swView.EnableGraphicsUpdate = True 'Habilita atualização da tela.
    End Sub
End Module
