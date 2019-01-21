Imports System.IO
Imports SldWorks
Imports SwConst
Imports Cotas

Module Desenho
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swDrawing As DrawingDoc
    Dim swSheet As Sheet
    Dim swView As View
    Dim swExt As ModelDocExtension
    Sub Desenhar(fullPath As String)
        swApp = _swApp()
        Dim retBool As Boolean
        'swApp.SendMsgToUser("Conectado")
        'Os parâmetros são zero porque o papel já está definido
        swModel = swApp.NewDocument("C:\Users\54808\Documents\1 - Docs Ricardo\Solidworks utilidades\Templates\A2.drwdot",
                                    swDwgPaperSizes_e.swDwgPapersUserDefined, 0, 0)
        If swModel Is Nothing Then
            Exit Sub
        End If

        'swModel = swApp.ActiveDoc
        'swApp.ActivateDoc(swModel.GetPathName)
        Try
            swDrawing = swModel
        Catch ex As Exception
            'Using dp As New IO.StreamWriter("C:\Users\54808\source\repos\Sandbox-vb\EscreverTXT\TesteFile.txt", False)
            '    dp.WriteLine("This a demo text")
            'End Using
            Exit Sub
        End Try

        'Dim retBool = swDrawing.Create1stAngleViews(fullPath)
        swView = swDrawing.CreateDrawViewFromModelView3(fullPath, "*Front", 0.17, 0.33, 0)
        'swView = swDrawing.CreateDrawViewFromModelView3(fullPath, "*Top", 0.155, 0.246, 0)
        swView = swDrawing.CreateDrawViewFromModelView3(fullPath, "*Left", 0.36, 0.33, 0)
        swView = swDrawing.CreateDrawViewFromModelView3(fullPath, "*Dimetric", 0.2, 0.14, 0)
        'swModel.ClearSelection2(True)

        retBool = swModel.SetUserPreferenceToggle(swUserPreferenceToggle_e.swViewDisplayHideAllTypes, True)
        swSheet = swDrawing.GetCurrentSheet()
        swSheet.SetScale(1.0, 20.0, True, True)
        'swSheet.SetScale(1.0, 10.0, True, True)

        Dim vistas = swSheet.GetViews()
        Dim retLong As Long
        Dim nomeDaIsometrica = "Drawing View3"
        'Dim nomeDaIsometrica = "Drawing View3"
        InserirBOM()

        For Each vista In vistas
            swView = vista
            Dim nomeDaVista = swView.GetName2()
            If nomeDaVista = nomeDaIsometrica Then
                swExt = swModel.Extension
                retLong = swDrawing.ActivateView(nomeDaIsometrica)
                retLong = swExt.SelectByID2(nomeDaIsometrica, "DRAWINGVIEW", 0, 0, 0, False, 0, Nothing, swSelectOption_e.swSelectOptionDefault)
                'swView.UseSheetScale = False
                swView.ScaleDecimal = 1 / 15
                'swView.ScaleDecimal = 1 / 5
                swModel.ForceRebuild3(True)
            End If
            'swView.SelectEntity(swView, True)
            'Console.WriteLine(nomeDaVista)
        Next
        swModel.ShowNamedView2(swStandardViews_e.swIsometricView, -1)
        InserirBaloes("Drawing View3")
        InserirBaloes("Drawing View1")
        'Console.ReadKey()
        'Cotar("Drawing View1")
        Dim valorDaCotaHorizontal = Cotagem.Cotar(swApp, "e_2300081_fle", "e_2300060_fld", "Drawing View1", 0.17, 0.27)
        Dim valorArredondado = (Math.Round(valorDaCotaHorizontal / 1000, 1))
        Select Case valorArredondado
            Case 3.2
                Cotagem.Cotar(swApp, "e_2300055_sup", "e_2300060_inf", "Drawing View1", 0.265, 0.33)
            Case 3.8
                Cotagem.Cotar(swApp, "e_2300055_sup", "e_2300060_inf", "Drawing View1", 0.275, 0.33)
            Case 4.4
                Cotagem.Cotar(swApp, "e_2300055_sup", "e_2300060_inf", "Drawing View1", 0.305, 0.33)
        End Select

        'e_2048275_le
        valorDaCotaHorizontal = Cotagem.Cotar(swApp, "e_2300080", "e_2300080", "Drawing View2", 0.36, 0.27)



        Salvar2DSolidworks(fullPath)
        SalvarPDF(fullPath)

    End Sub

    Private Sub InserirBaloes(nomeDaVista)
        Dim vNotes As Object
        Dim autoballoonParams As AutoBalloonOptions
        Dim retLong As Long
        retLong = swDrawing.ActivateView(nomeDaVista)
        retLong = swExt.SelectByID2(nomeDaVista, "DRAWINGVIEW", 0, 0, 0, False, 0, Nothing, swSelectOption_e.swSelectOptionDefault)
        autoballoonParams = swModel.CreateAutoBalloonOptions()
        autoballoonParams.Layout = 1
        autoballoonParams.ReverseDirection = False
        autoballoonParams.IgnoreMultiple = True
        autoballoonParams.InsertMagneticLine = True
        autoballoonParams.LeaderAttachmentToFaces = True
        autoballoonParams.Style = 1
        autoballoonParams.Size = 2
        autoballoonParams.EditBalloonOption = 1
        autoballoonParams.EditBalloons = 1
        autoballoonParams.UpperTextContent = 1
        autoballoonParams.UpperText = """"
        autoballoonParams.Layername = "DETALHAMENTO"
        vNotes = swModel.AutoBalloon5(autoballoonParams)
    End Sub

    Private Sub Salvar(fullPath As String)
        Try
            Dim erro, aviso As Integer
            swExt = swModel.Extension
            Dim retBool = swExt.SaveAs(fullPath, swSaveAsVersion_e.swSaveAsCurrentVersion, swSaveAsOptions_e.swSaveAsOptions_Silent, Nothing, erro, aviso)
        Catch ex As Exception
            Console.WriteLine("Erro ao salvar o 2d " & ex.Message)
        End Try
    End Sub

    Private Sub Salvar2DSolidworks(fullPath)
        Dim fullpathSldprt = Path.ChangeExtension(fullPath, ".slddrw".ToUpper)
        Salvar(fullpathSldprt)
    End Sub

    Private Sub SalvarPDF(fullPath)
        Dim fullpathPDF = Path.ChangeExtension(fullPath, ".pdf".ToUpper)
        Salvar(fullpathPDF)
    End Sub

    Private Sub InserirBOM()
        swModel = swApp.ActiveDoc
        swDrawing = swModel
        Dim swBomFeature As BomFeature
        swView = swDrawing.GetFirstView
        Dim swActiveView As View = swView.GetNextView
        Dim nomeDaBOM = "C:\Users\54808\Documents\1 - Docs Ricardo\Solidworks utilidades\Templates\LISTA MONTAGEM.sldbomtbt"
        Dim swBomTable As BomTableAnnotation
        Dim swConfig = swActiveView.ReferencedConfiguration()
        swBomTable = swActiveView.InsertBomTable4(True, 0, 0, swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_BottomRight,
                                                  swBomType_e.swBomType_TopLevelOnly, swConfig, nomeDaBOM, False,
                                                  swNumberingType_e.swNumberingType_Detailed, False)
        swBomFeature = swBomTable.BomFeature
        swBomFeature.FollowAssemblyOrder2 = True
    End Sub

    Sub Cotar(nomeDaVista As String)
        Try
            Dim retLong As Long
            retLong = swModel.ActivateView(nomeDaVista)
            'retLong = swExt.SelectByID2(nomeDaVista, "DRAWINGVIEW", 0, 0, 0, False, 0, Nothing, swSelectOption_e.swSelectOptionDefault)
            retLong = swExt.SelectByID2("Point1@Sketch7@4020000-1@Drawing View1/2300142-1@4020000", "EXTSKETCHPOINT", 0, 0, 0, False, 0, Nothing, 0)
            retLong = swExt.SelectByID2("Point1@Sketch8@4020000-1@Drawing View1/2300142-1@4020000", "EXTSKETCHPOINT", 0, 0, 0, True, 0, Nothing, 0)
            Dim myDisplayDim As Object
            myDisplayDim = swDrawing.AddDimension2(0.17, 0.27, 0)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
End Module
