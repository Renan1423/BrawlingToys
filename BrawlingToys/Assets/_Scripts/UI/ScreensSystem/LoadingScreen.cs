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
            await CanStartNewRound();
            SpawnMapAndGetPoints();
            Instantiate(bonecoTeste, _spawnPoints[0]);
        }

        public void SpawnMapAndGetPoints(){
            if (_currentMap != null){
                Destroy(_currentMap);
                _currentMap = null;
            }
            _currentMap = Instantiate(_currentMapHandle.Result);
            GetSpawnPoints(_currentMap);
        }
        
        //Método criado para fins de teste
        public async void RnadimoizaMap(){
            await RandomizeMap();
        }

        private void GetSpawnPoints(GameObject map){
            _spawnPoints.Clear();
            Transform _spawns = map.transform.GetChild(0);
            foreach (Transform spawn in _spawns){
                if (spawn.tag == "Spawn") { _spawnPoints.Add(spawn); }
            }
        }


    #region Métodos assíncronos 
        /*
        Randomiza o mapa a ser instanciado a partir de uma List<AssetReference>
        */
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

        //Este método checaria se os players foram carregados assincronamente com sucesso
        private async Task ArePlayersLoaded(){
            try
            {
                
            }
            catch (Exception)
            {

            }
        }
        //Este método checaria se os players foram instanciados na cena com sucesso
        private async Task ArePlayersSpawned(){

        }

        /*
        Checa se as três Tasks assíncronas (randomização de mapa, carregamento de jogadores e 
        instanciação de jogadores) foram completadas antes de devolver true ou false
        */
        private async Task<bool> CanStartNewRound(){
            bool isMapLoaded = await TryAsyncTask(RandomizeMap, "Randomização de mapas");
            bool arePlayersLoaded = await TryAsyncTask(ArePlayersLoaded, "Carregamento de jogadores");
            bool arePlayersSpawned = await TryAsyncTask(ArePlayersSpawned, "Instanciação de jogadores");

            if(isMapLoaded && arePlayersLoaded && arePlayersSpawned) return true;
            else{
                Debug.Log($"Ocorreu um erro em um dos processos assíncronos!");
                return false;
            }
        }

        /*
        Checa se uma Task assíncrona foi realizada com sucesso antes de devolver true ou false.
        Após uma tentativa falha, dá um delay de 500ms antes de tentar novamente; depois de três
        tentativas retorna false.
        */
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
