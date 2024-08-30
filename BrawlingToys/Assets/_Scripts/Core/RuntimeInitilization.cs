using UnityEngine;

namespace BrowlingToys.Core
{
    /// <summary>
    /// Gerencia as ações que devem acontecer assim que a build for inicializada
    /// </summary>
    public class RuntimeInitilization
    {
        private const string MAIN_PATH = "InitializationObjects/"; 

        private const string INGAMECONSONE_PATH = MAIN_PATH + "IngameDebugConsole"; 
        private const string NETOWRK_MANAGER_PATH = MAIN_PATH + "Network Manager"; 

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            TryInstantieteDevelopmentObjects();
            InstantieteCoreObjects(); 
        }

        private static void TryInstantieteDevelopmentObjects()
        {
            if (!Debug.isDebugBuild) return; 

            GameObject.Instantiate(Resources.Load(INGAMECONSONE_PATH)); 
        }

        private static void InstantieteCoreObjects()
        {
            GameObject.Instantiate(Resources.Load(NETOWRK_MANAGER_PATH));
        }
    }
}
