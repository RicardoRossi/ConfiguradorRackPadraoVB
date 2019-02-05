Imports System.IO
Module lerTXT
    Function LerTXT() As List(Of String)
        Dim pathTXT = "C:\Users\54808\source\repos\ConfiguradorRackPadraoVB\numeracao 4020001-4020256.txt"
        Dim ListaDecodigos As List(Of String) = New List(Of String)
        Using sr As New StreamReader(pathTXT)
            While Not sr.EndOfStream
                Dim linha = sr.ReadLine
                ListaDecodigos.Add(linha)
            End While
        End Using
        Return ListaDecodigos
        'Dim descricaoVetor() = ListaDecodigos.ToArray 'Converte lista para array
    End Function
End Module