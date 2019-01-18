using SldWorks;
namespace TrocaConfiguracao
{
    public static class Configuracao
    {
        static SldWorks.SldWorks swApp;
        static ModelDoc2 swModel;
        static ModelDocExtension swExt;
        static ConfigurationManager swConfigMgr;
        static Configuration swConfiguration;
        static AssemblyDoc swAsm;
        static Component2 swComp;
        static Component2 componenteDaBase;

        public static void TrocarConfiguracaoBase(AssemblyDoc _swAsm)
        {
            swAsm = _swAsm;
            var componentes = swAsm.GetComponents(true);
            foreach (var comp in componentes)
            {
                // Será usado dois component2, uma que vai guardar o master model e outro
                // para achar qual base esta na montagem.
                swComp = (Component2)comp;
                var nomeDoComp = swComp.Name;

                // O componente for master model arnazena na variável
                // para depois trocar a config.
                if (nomeDoComp.Contains("SCM_RP"))
                {
                    componenteDaBase = (Component2)comp;
                    break;
                }                
            }

            foreach (var comp in componentes)
            {
                swComp = (Component2)comp;
                var nomeDoComp = swComp.Name;

                // Verifica qual base está na montagem
                // Troca a condiguração do master model
                string ret = null;
                switch (nomeDoComp)
                {
                    case "2300142-1":
                        ret = componenteDaBase.ReferencedConfiguration = "2cp";
                        break;
                    case "2300143-1":
                        ret = componenteDaBase.ReferencedConfiguration = "3cp";
                        break;
                    case "2300144-1":
                        ret = componenteDaBase.ReferencedConfiguration = "4cp";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
