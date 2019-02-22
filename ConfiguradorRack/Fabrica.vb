Imports System.IO
Imports SwConst
Imports SldWorks
Module Fabrica
    Dim swApp As SldWorks.SldWorks = _swApp()
    Dim swModel As ModelDoc2
    Dim swFeatureMgr As FeatureManager
    Dim swFeature As Feature
    Dim swComp As Component2
    Private arquivosDoPdm() As String
    'Exceuta somente uma vez. "Construtor"
    Sub New()
        arquivosDoPdm = Directory.GetFiles("C:\ELETROFRIO\ENGENHARIA SMR", ".", SearchOption.AllDirectories) 'Pega todos os caminhos do PDM
    End Sub
    'Fabricar peça é pegar o item da planilha de itens
    'instanciar uma classe peça com item e component2
    Sub FabricarPecas(planilhaDeItens As List(Of item))
        Dim i = 1
        Dim _tipoArquivo As swDocumentTypes_e
        Dim listaDePecas As New List(Of Peca)
        For Each item In planilhaDeItens
            'Passa um codigo do item para localizar o caminho completo dele no PDM
            'o metodo retorna um IEnumerable de string que normalmente terá uma ocorrencia somente
            'poderá ter mais de um arquivo no IEnumerable, por exemplo se achar uma part e uma montagem com o mesmo codigo ou o mesmo codigo em caminhos diferentes.
            'Ex:
            '   C:\ELETROFRIO\ENGENHARIA SMR\produtos finais eletrofrio\mecânica\Rack padrao\COLETOR SUCCAO\2047866.SLDASM
            '   C:\ELETROFRIO\ENGENHARIA SMR\produtos finais eletrofrio\mecânica\Rack padrao\COLETOR SUCCAO\2047866.SLDPRT
            '   C:\ELETROFRIO\ENGENHARIA SMR\produtos finais eletrofrio\mecânica\Rack padrao\2047866.SLDPRT
            Dim listaArquivosEncontrados = ProcurarArquivoNoPDM(item.itemCodigo)

            'Como a lista pode ter mais de um arquivo, deverá ser feito alguma checagem para ver qual deles será adicionado na montagem.
            'Talves pegar todos os caminhos que vieram no IEnumerable e abrir na memória pegar o componente e localizar o cs_ dele, se não tiver o cs_ não inserir na montagem.
            For Each arquivo In listaArquivosEncontrados
                'Aqui abrir os arquivos. OpenDoc6
                'Cada vez que abrir um arquivo pegar instanciar uma peca, atribuir o item correspondente e pegar o seu Component2
                'Console.WriteLine($"{i} {arquivo}")

                'If arquivo.Contains("3010001") Or arquivo.Contains("2300082") _
                '    Or arquivo.Contains("2048261") Or arquivo.Contains("2047621") Or arquivo.Contains("2048115") _
                '    Or arquivo.Contains("04216") Or arquivo.Contains("02237") Or arquivo.Contains("2048294") _
                '    Or arquivo.Contains("2300049") Or arquivo.Contains("2300166") Or arquivo.Contains("2300168") _
                '    Or arquivo.Contains("2300167") Or arquivo.Contains("3002932") Then
                '    Continue For
                'End If

                'If arquivo.Contains("3010001") Or arquivo.Contains("MONTCP") Or arquivo.Contains("2300082") _
                '    Or arquivo.Contains("2048261") Or arquivo.Contains("2047621") Or arquivo.Contains("2048115") _
                '    Or arquivo.Contains("04216") Or arquivo.Contains("02237") Or arquivo.Contains("2048294") _
                '    Or arquivo.Contains("2300049") Or arquivo.Contains("2300166") Or arquivo.Contains("2300168") _
                '    Or arquivo.Contains("2300142") Or arquivo.Contains("2300143") Or arquivo.Contains("2300144") _
                '    Or arquivo.Contains("2300167") Or arquivo.Contains("3002932") Then
                '    Continue For
                'End If


                i += 1
                _tipoArquivo = TipoArquivo(arquivo)

                'A verificação é necessária pois aconteceu a tentativa de abrir arquivos temporários com ~$ na frente.
                If arquivo.Contains("~$") Then
                    Continue For
                End If

                swModel = AbrirArquivoNaMemoria(_tipoArquivo, arquivo)

                'Se o método AbrirArquivoNaMemoria retornar null vai para o próximo arquivo do For Each.
                If swModel Is Nothing Then
                    Continue For
                End If

                'Se a peça não tiver um cs_ vai para o próximo arquivo do For Each.
                'A verificação é para evitar inserir peças duplicadas, por exemplo uma com o mesmo código,
                'porém uma sldprt e uma sldasm. Somente uma delas terá o cs_
                If Not VerificarCs(swModel) Then
                    Continue For
                End If

                'Para cada componente aberto na memória será preenchida a prop "PESO BRUTO"
                AdicionarPesoBruto(swModel)
                KiloDuasCasasDecimais(swModel)

                For i = 1 To item.quantidade
                    swComp = InsertPeca(arquivo)
                    'Para cada peça inserida na montagem será criado um novo item com o scm atualizado como a posição dele na montagem.
                    'ex:
                    'Posição do item na planilhade itens não tem o digito final que indica a posição na montagem "cs_carcaca_cp"
                    'Para cada loop do For é criado um novo item com o scm atualizado
                    'cs_carcaca_cp1
                    'cs_carcaca_cp2
                    'cs_carcaca_cp3
                    'cs_carcaca_cp4
                    Dim itemComScmAtualizado As New item With {.itemCodigo = item.itemCodigo, .cs = item.cs, .quantidade = item.quantidade, .scm = item.scm & i}
                    Dim peca As New Peca With {.componente = swComp, .item = itemComScmAtualizado}
                    listaDePecas.Add(peca)
                Next
            Next
        Next

        'For Each peca In listaDePecas
        '    Console.WriteLine(peca.componente.Name2.ToString)
        'Next
        _MontarPecas(listaDePecas)

    End Sub

    Private Function AbrirArquivoNaMemoria(tipoArquivo As swDocumentTypes_e, arquivo As String) As ModelDoc2
        Dim swApp As SldWorks.SldWorks
        Dim swModel As ModelDoc2
        swApp = _swApp()
        swModel = swApp.ActiveDoc
        Try
            'Testa se o arquivo já esta aberto. No caso da insersão de várias instâncias
            swModel = swApp.GetOpenDocumentByName(arquivo)
            If swModel IsNot Nothing Then
                Return swModel
            Else
                Dim _error As Integer
                Dim warning As Integer
                swApp.DocumentVisible(False, tipoArquivo) 'Não mostra a peça na tela. o Documento estará somente na memoria.
                swModel = swApp.OpenDoc6(arquivo, tipoArquivo, swOpenDocOptions_e.swOpenDocOptions_LoadLightweight, "", _error, warning)
                Return swModel
            End If
        Catch ex As Exception
            Console.WriteLine("Problema para abrir o arquivo. OpenDoc6")
        End Try
    End Function

    Private Function ProcurarArquivoNoPDM(codigo As String) As IEnumerable(Of String)
        Dim fullPath = From i In arquivosDoPdm
                       Where Path.GetFileName(i).ToUpper().Equals(codigo + ".SLDPRT") Or Path.GetFileName(i).ToUpper().Equals(codigo + ".SLDASM")
                       Select i
        Return fullPath
    End Function
    'Where i.ToUpper().Contains(codigo + ".SLDPRT") Or i.ToUpper().Contains(codigo + ".SLDASM") 'ToUpper deixar tudo igual. Só seleciona peça e montagem

    Function TipoArquivo(arquivo As String) As swDocumentTypes_e
        Dim _tipoArquivo As swDocumentTypes_e
        Select Case Path.GetExtension(arquivo.ToUpper)
            Case ".SLDPRT"
                _tipoArquivo = swDocumentTypes_e.swDocPART
            Case ".SLDASM"
                _tipoArquivo = swDocumentTypes_e.swDocASSEMBLY
        End Select
        Return _tipoArquivo
    End Function

    Function InsertPeca(arquivo As String) As Component2
        Dim swApp = _swApp()
        swModel = swApp.ActiveDoc
        Dim swAsm As AssemblyDoc = swModel
        Dim swComp As Component2
        'Dim randonX = New Random
        'Dim cx = randonX.NextDouble
        'Dim randonY = New Random
        'Dim cY = randonX.NextDouble

        swComp = swAsm.AddComponent4(arquivo, "", 0, 0, 0) 'Adiciona na montagem
        swComp.Select(False)
        swAsm.UnfixComponent() 'Deixa float, para posteriormente add mate entre cs_ e scm
        Return swComp 'Retorna o Component2
    End Function

    Function VerificarCs(swModel As ModelDoc2) As Boolean
        swFeatureMgr = swModel.FeatureManager
        Dim features As Object = swFeatureMgr.GetFeatures(True)
        Dim csName = String.Concat("cs_", Path.GetFileNameWithoutExtension(swModel.GetPathName))
        Dim csExist As Boolean
        For Each f In features
            swFeature = f
            If swFeature.Name = csName Then
                csExist = True
                Exit For
            Else
                csExist = False
            End If
        Next
        Return csExist
        'Usar swApp.CloseDoc(name) para fechar o doc que não possuir cs_
    End Function
End Module
