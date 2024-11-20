using UnityEngine;

namespace BrawlingToys.Core
{
    /// <summary>
    /// Gerencia as ações que devem acontecer assim que a build for inicializada
    /// </summary>
    public class RuntimeInitilization
    {
        private const string MAIN_PATH = "InitializationObjects/"; 

        private const string INGAMECONSONE_PATH = MAIN_PATH + "IngameDebugConsole"; 
        private const string NETWORK_MANAGER_PATH = MAIN_PATH + "Network Manager"; 
        private const string NETWORK_AUTHENTICATION = MAIN_PATH + "Unity Services Auth"; 
        private const string SCREEN_MANAGER = MAIN_PATH + "Screen Manager"; 

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            InstantiateCoreObjects(); 

            if (Debug.isDebugBuild)
            {
                InstantiateDevelopmentObjects();
                MakeDevelopmentBuildOptimizations(); 
            }
        }

        private static void InstantiateDevelopmentObjects()
        {
            GameObject.Instantiate(Resources.Load(INGAMECONSONE_PATH)); 
        }

        private static void InstantiateCoreObjects()
        {
            GameObject.Instantiate(Resources.Load(NETWORK_MANAGER_PATH));
            GameObject.Instantiate(Resources.Load(NETWORK_AUTHENTICATION)); 
            GameObject.Instantiate(Resources.Load(SCREEN_MANAGER)); 
        }

        private static void MakeDevelopmentBuildOptimizations()
        {
            #if !UNITY_EDITOR
                Application.targetFrameRate = 25; 
                //QualitySettings.shadows = ShadowQuality.Disable;
                //QualitySettings.SetQualityLevel(4);
            #endif
        }
    }
}
