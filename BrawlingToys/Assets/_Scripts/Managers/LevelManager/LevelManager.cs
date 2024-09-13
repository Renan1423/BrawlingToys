using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BrawlingToys.DesignPatterns;

namespace BrawlingToys.Managers
{
    public class LevelManager : Singleton<LevelManager>
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
    }
}
