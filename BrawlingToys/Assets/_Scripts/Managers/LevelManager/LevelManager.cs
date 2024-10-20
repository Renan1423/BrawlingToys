using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BrawlingToys.DesignPatterns;
using BrawlingToys.Network;

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

        public static void LoadLevelNetwork(int level) 
        {
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(level));
        }

        public void ReloadLevel()
        {
            StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex));
        }

        public static void ReloadLevelNetwork() 
        {
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(SceneManager.GetActiveScene().buildIndex));
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        }

        public static void LoadNextLevelNetwork() 
        { 
            LocalInstance.StartCoroutine(LoadLevelNetworkCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        }

        private IEnumerator LoadLevelCoroutine(int level)
        {
            _sceneTransition.SetTrigger("FadeOut");

            yield return new WaitForSeconds(_delayToChanceScene);

            SceneManager.LoadScene(level);
        }

        private static IEnumerator LoadLevelNetworkCoroutine(int level)
        {
            LocalInstance._sceneTransition.SetTrigger("FadeOut");

            yield return new WaitForSeconds(LocalInstance._delayToChanceScene);

            if(LocalInstance.IsOwner)
                LocalInstance.NetworkManager.SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(level).name, LoadSceneMode.Single);
        }
    }
}
