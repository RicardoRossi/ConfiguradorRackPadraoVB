using SldWorks;
using SwConst;
namespace Cotas
{
    public class Cotagem
    {
        static SldWorks.SldWorks swApp;
        static ModelDoc2 swModel;
        static ModelDocExtension swExt;
        static SelectionMgr swSelMgr;
        static Component2 swComp;
        static View swView;
        static Entity swEntity;
        static bool boolstatus;
        static SelectData swSelData;

        public static double Cotar(SldWorks.SldWorks _swApp, string nomeAresta1, string nomeAresta2, string nomeDaVista, double x, double y)
        {
            swApp = _swApp;
            swModel = swApp.ActiveDoc;
            swExt = swModel.Extension;
            swSelMgr = swModel.SelectionManager;

            DrawingDoc swDraw;
            swDraw = (DrawingDoc)swModel;
            boolstatus = swDraw.ActivateView(nomeDaVista);
            boolstatus = swExt.SelectByID2(nomeDaVista, "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
            swView = swSelMgr.GetSelectedObject5(1);

            var vComps = swView.GetVisibleComponents();
            int sair = 0;

            foreach (var vComp in vComps)
            {
                if (sair == 2)
                {
                    break;
                }
                Component2 comp = (Component2)vComp;
                var nome = comp.Name2;
                if (nome.Contains("2300081") || nome.Contains("2300060") || nome.Contains("2300055") || nome.Contains("2300080"))
                {
                    var vEdges = swView.GetVisibleEntities(comp, (int)swViewEntityType_e.swViewEntityType_Edge);
                    swSelData = swSelMgr.CreateSelectData();
                    swSelData.View = swView;

                    foreach (var vEdge in vEdges)
                    {
                        Edge edge = (Edge)vEdge;
                        swEntity = (Entity)edge;
                        var nomeDaEntidade = swModel.GetEntityName(swEntity);
                        if (nomeDaEntidade == nomeAresta1 || nomeDaEntidade == nomeAresta2)
                        {
                            boolstatus = swEntity.Select4(true, swSelData);
                            sair++;
                            break;
                        }
                    }
                }
            }

            object swDimen;
            swDimen = swModel.AddDimension2(x, y, 0.0);
            DisplayDimension dispDim = (DisplayDimension)swDimen;
            Dimension dimen = dispDim.GetDimension();
            var valorDaDimen = dimen.Value;
            swModel.ClearSelection2(true);
            return valorDaDimen;
        }
    }
}
