Imports DAL.ConfiguradorV1DataSetTableAdapters
Imports System.Data.OleDb
Public Module ModuloAcessoDados
    Function ListarTblKit(codigo As String) As EnumerableRowCollection(Of ConfiguradorV1DataSet.tbl_kitRow)
        'Instancias de:
        'DataSet
        'TableAdapter
        'TableAdapterManager
        Dim configDataSet As New ConfiguradorV1DataSet 'Banco de dados access ConfiguradorV1.accdb
        Dim tblAdapter As New tbl_kitTableAdapter 'tabela tbl_kit
        'Dim tblAdapterManager As New TableAdapterManager 'Adapter manager. Caso tenha mais de um adapter será necessário

        Try
            tblAdapter.Fill(configDataSet.tbl_kit)
            'tblAdapterManager.tbl_kitTableAdapter = tblAdapter

            Dim query = (From ord In configDataSet.tbl_kit
                         Where ord.IsNull("kit_cod") = False AndAlso ord.kit_cod = codigo 'Primeiro checa se existe null na coluna kit_cod e depois checa se cada linha do dataset é igual ao codigo que veio do parâmetro.
                         Order By ord.kit_cod_item Descending
                         Select ord)
            Return query
        Catch ex As OleDbException
            Throw
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function ListarTblItem(codigo As String) As EnumerableRowCollection(Of ConfiguradorV1DataSet.tbl_ItemRow)
        'Instancias de:
        'DataSet
        'TableAdapter
        'TableAdapterManager
        Dim configDataSet As New ConfiguradorV1DataSet 'Banco de dados access ConfiguradorV1.accdb
        Dim tblAdapter As New tbl_ItemTableAdapter 'tabela tbl_kit
        'Dim tblAdapterManager As New TableAdapterManager 'Adapter manager. Caso tenha mais de um adapter será necessário

        Try
            tblAdapter.Fill(configDataSet.tbl_Item)
            'tblAdapterManager.tbl_kitTableAdapter = tblAdapter

            Dim query = (From ord In configDataSet.tbl_Item
                         Where ord.IsNull("item_cod") = False AndAlso ord.item_cod = codigo 'Primeiro checa se existe null na coluna item_cod e depois checa se cada linha do dataset é igual ao codigo que veio do parâmetro.
                         Order By ord.item_cod Descending
                         Select ord)
            Return query
        Catch ex As OleDbException
            Throw
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
    End Function
End Module

'O check ---- ord.IsNull("kit_cod") = False ---- nem seria necessário, pois a tabela kit_kit, na coluna "kit_cod" requer entrada de dados e não permite comprimento zero, ou seja nunca será null.
