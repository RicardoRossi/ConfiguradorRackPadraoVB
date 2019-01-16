Imports System.Deployment
Imports DAL.ModuloAcessoDados
Imports System.Data.OleDb
'Na engenharia monto a lista de item que ainda não é uma peça, pois não tem o Component2.
Public Module Engenharia
    Private planilhaDeItens As List(Of item)

    Private Function Planilhar(codigoDoKit As String) As List(Of item)
        Dim listaDeItens As New List(Of item)

        Try
            'Criação de cada item do kit com seu codigo, scm e cs.   (Exemplo: 2003742, cs_val_saida_tql, cs_2003742)
            For Each item In ListarTblKit(codigoDoKit) 'Codigo que vai para a DAL para fazer a query no dataset. (Linq to Dataset)
                Dim p As New item With {
                    .itemCodigo = item.kit_cod_item,
                    .scm = item.scm,
                    .cs = "cs_" & item.kit_cod_item,
                    .quantidade = item.qt}
                listaDeItens.Add(p)
            Next
            Return listaDeItens 'Itens são strings ainda sem o componente2
        Catch ex As OleDbException
            Throw
        End Try

    End Function

    Sub GerarPlanilhaDoKit(codigoDoKit As String)
        Try
            planilhaDeItens = Planilhar(codigoDoKit)
        Catch ex As Exception
            Throw
        End Try
        FabricarPecas(planilhaDeItens) 'Para fabricar as peças é necessario o item com suas propriedades itemCodigo, cs_ e scm
    End Sub
End Module
