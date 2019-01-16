Imports SldWorks
Imports SwConst
Module OpcoesDoSistema
    Dim swApp As SldWorks.SldWorks
    Dim swModel As ModelDoc2
    Dim swExt As ModelDocExtension
    Sub KiloDuasCasasDecimais(_swmodel As ModelDoc2)
        swApp = _swApp()
        swModel = _swmodel
        swExt = swModel.Extension

        Dim boolstatus As Boolean
        boolstatus = swModel.Extension.SetUserPreferenceInteger(swUserPreferenceIntegerValue_e.swUnitSystem, 0, swUnitSystem_e.swUnitSystem_Custom)
        boolstatus = swModel.Extension.SetUserPreferenceInteger(swUserPreferenceIntegerValue_e.swUnitsLinearDecimalPlaces, 0, 2)
        boolstatus = swModel.Extension.SetUserPreferenceInteger(swUserPreferenceIntegerValue_e.swUnitsMassPropDecimalPlaces, 0, 2)
        boolstatus = swModel.Extension.SetUserPreferenceInteger(swUserPreferenceIntegerValue_e.swUnitsMassPropMass, 0, swUnitsMassPropMass_e.swUnitsMassPropMass_Kilograms)
    End Sub
End Module
