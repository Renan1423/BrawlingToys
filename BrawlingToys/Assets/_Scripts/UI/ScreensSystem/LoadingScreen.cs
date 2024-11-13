using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BrawlingToys.Actors;
using BrawlingToys.Managers;
using Unity.Netcode;


namespace BrawlingToys.UI
{
    public class LoadingScreen : BaseScreen
    {
        [Header("Things to load")]

        [SerializeField]
        private List<AssetReference> _maps;
        private AsyncOperationHandle<GameObject> _currentMapHandle;
        private List<Transform> _spawnPoints = new();
        public List<Transform> SpawnPoints { get => _spawnPoints; }
        public GameObject bonecoTeste;
        private GameObject _currentMap;

        private void Start()
        {
            OnScreenEnable();    
        }

        protected override async void OnScreenEnable()
        {
            await CanStartNewRoundAlternate();
            SpawnMapAndGetPoints();
            Instantiate(bonecoTeste, _spawnPoints[0]);

        }

        private void GetSpawnPoints(GameObject map){
            Transform _spawns = map.transform.GetChild(0);
            foreach (Transform spawn in _spawns){
                if (spawn.tag == "Spawn") { _spawnPoints.Add(spawn); }
            }
        }

        private void SpawnMapAndGetPoints(){
            if (_currentMap != null){
                _currentMap = null;
            }
            _currentMap = Instantiate(_currentMapHandle.Result);
            GetSpawnPoints(_currentMap);
        }


    #region Métodos assíncronos 
        private async Task RandomizeMap(){
            try
            { 
                if (_currentMapHandle.IsValid()){
                    Debug.Log("Mapa antigo descarregado!");
                    Addressables.Release(_currentMapHandle);
                }
                int i = UnityEngine.Random.Range(0,_maps.Count);
                var _mapReference = _maps[i];
                _currentMapHandle = _mapReference.LoadAssetAsync<GameObject>();
                await _currentMapHandle.Task;

                Debug.Log($"Mapa carregado!");
            }

            catch (Exception e)
            {
                Debug.LogError($"Mapa não pode ser instanciado: {e.Message}");
            }
        }

        private async Task ArePlayersLoaded(){
            try
            {
                
            }
            catch (Exception)
            {

            }
        }

        private async Task ArePlayersSpawned(){

        }

        private async Task<bool> CanStartNewRound(){
            try{
            await Task.WhenAll(RandomizeMap(), ArePlayersLoaded(), ArePlayersSpawned());
            return true;

            } catch (System.Exception){
                throw;
            }
        }

        private async Task<bool> CanStartNewRoundAlternate(){
            bool isMapLoaded = await TryAsyncTask(RandomizeMap, "Randomização de mapas");
            bool arePlayersLoaded = await TryAsyncTask(ArePlayersLoaded, "Carregamento de jogadores");
            bool arePlayersSpawned = await TryAsyncTask(ArePlayersSpawned, "Instanciação de jogadores");

            if(isMapLoaded && arePlayersLoaded && arePlayersSpawned) return true;
            else{
                Debug.Log($"Ocorreu um erro em um dos processos assíncronos!");
                return false;
            }
        }

        private async Task<bool> TryAsyncTask(Func<Task> taskFunc, string taskName){
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                await taskFunc();
                Debug.Log($"Task {taskName} realizada com sucesso!");
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Erro ao executar {taskName}, tentativa {attempt} de 3: {ex.Message}");
                if (attempt == 3)
                {
                    Debug.LogError($"{taskName} falhou após 3 tentativas.");
                    return false;
                }
                await Task.Delay(500);
            }
        }
        return false;
    }

    #endregion
    }
}
