Imports SldWorks
Imports SwConst
Imports DAL
Imports System.IO
Public Module Propriedades
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swCustPropMgr As CustomPropertyManager
    Dim swConfigMgr As ConfigurationManager
    Dim configuracao As Configuration
    Dim swExt As ModelDocExtension

    Sub AdicionarPropriedades(codigo As String)
        swApp = _swApp()
        swModel = swApp.ActiveDoc
        swExt = swModel.Extension
        swConfigMgr = swModel.ConfigurationManager
        configuracao = swConfigMgr.ActiveConfiguration
        'Pega o manager da config específica
        swCustPropMgr = swExt.CustomPropertyManager(configuracao.Name)

        'Dim codigo = Path.GetFileNameWithoutExtension(swModel.GetPathName)
        Dim tuplaDescricaoRack = LerTblItem(codigo)
        Dim descricaoRack = tuplaDescricaoRack.Item1
        Dim grupoItem = tuplaDescricaoRack.Item2

        swCustPropMgr.Add3("DESCRIÇÃO".ToUpper, swCustomInfoType_e.swCustomInfoText, descricaoRack, swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        swCustPropMgr.Add3("GRUPO ITEM".ToUpper, swCustomInfoType_e.swCustomInfoText, grupoItem, swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        swCustPropMgr.Add3("CODIGO".ToUpper, swCustomInfoType_e.swCustomInfoText, codigo, swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        swCustPropMgr.Add3("PROJETISTA".ToUpper, swCustomInfoType_e.swCustomInfoText, "RICARDO R.", swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        swCustPropMgr.Add3("PROJETISTA2D".ToUpper, swCustomInfoType_e.swCustomInfoText, "RICARDO R.", swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
        swCustPropMgr.Add3("PESO BRUTO".ToUpper, swCustomInfoType_e.swCustomInfoText, Chr(34) & "SW-Mass" & Chr(34), swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)

    End Sub

    Sub AdicionarPesoBruto(_swModel As ModelDoc2)
        Dim nome = _swModel.GetPathName
        Dim swAsm As AssemblyDoc
        swApp = _swApp()
        swModel = _swModel
        swExt = swModel.Extension

        'Se o tipo for uma montagem o swModel é convertido para o tipo swAsm 
        Dim tipo As swDocumentTypes_e
        tipo = swModel.GetType
        If tipo = swDocumentTypes_e.swDocASSEMBLY Then
            swAsm = swModel
            swExt = swAsm.Extension
        End If

        swConfigMgr = swModel.ConfigurationManager
        configuracao = swConfigMgr.ActiveConfiguration

        'Pega o manager da config específica
        swCustPropMgr = swExt.CustomPropertyManager(configuracao.Name)
        Dim rInt = swCustPropMgr.Add3("PESO BRUTO".ToUpper, swCustomInfoType_e.swCustomInfoText, Chr(34) & "SW-Mass" & Chr(34), swCustomPropertyAddOption_e.swCustomPropertyReplaceValue)
    End Sub

    'Pegar informação da tabela item do acess onde tem a descrição, grupo item, etc.
    'Valores que vão aparecer na legenda.
    Private Function LerTblItem(codigo) As Tuple(Of String, String)
        Try
            Dim descricaoGrupoItem As Tuple(Of String, String)
            For Each item In ListarTblItem(codigo)
                descricaoGrupoItem = New Tuple(Of String, String)(item.item_desc, item.item_grupo_item) 'Retorna uma tupla com duas strings
            Next
            Return descricaoGrupoItem
        Catch ex As Exception
            Console.WriteLine("TblItem com problema " & ex.Message)
        End Try
    End Function
End Module
