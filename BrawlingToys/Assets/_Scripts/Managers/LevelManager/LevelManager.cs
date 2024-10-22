using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BrawlingToys.DesignPatterns;
using BrawlingToys.Network;
using Unity.Netcode;

namespace BrawlingToys.Managers
{
    public class LevelManager : NetworkSingleton<LevelManager>
    {
        [SerializeField]
        private Animator _sceneTransition;
        [SerializeField]
        private float _delayToChanceScene = 2f;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            GameObject sceneTransitionGo = GameObject.FindGameObjectWithTag("SceneTransition");

            _sceneTransition = sceneTransitionGo.GetComponent<Animator>();
        }

        public void LoadLevel(int level)
        {
            StartCoroutine(LoadLevelCoroutine(level));
        }

        public void ReloadLevel()
        {
            StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex));
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        }

        private IEnumerator LoadLevelCoroutine(int level)
        {
            _sceneTransition.SetTrigger("FadeOut");

            yield return new WaitForSeconds(_delayToChanceScene);

            SceneManager.LoadScene(level);
        }

        public static void LoadLevelNetwork(int level)
        {
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(level));
        }

        public static void ReloadLevelNetwork()
        {
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(SceneManager.GetActiveScene().buildIndex));
        }

        public static void LoadNextLevelNetwork() 
        {
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        }

        private static IEnumerator LoadLevelNetworkCoroutine(int level)
        {
            StartNetworkSceneTransition();

            yield return new WaitForSeconds(LocalInstance._delayToChanceScene);

            if (LocalInstance.IsHost)
                NetworkManager.Singleton.SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
        }

        public static void StartNetworkSceneTransition() 
        {
            LocalInstance._sceneTransition.SetTrigger("FadeOut");
        }
    }
}
