using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using BrawlingToys.Actors;
using BrawlingToys.Managers;


namespace BrawlingToys.UI
{
    public class LoadingScreen : BaseScreen
    {
        [Header("Loading...")]
        [SerializeField] List<AssetReference> _maps;
        private int _indexOfMaps;
        AsyncOperationHandle<GameObject> _currentMapHandle;
        [SerializeField] GameObject[] _spawnPoints;
         private Dictionary<Player, PlayerRoundInfo> _playerMatchInfo = new();

        private void Awake()
        {
            OnScreenEnable();
        }

        protected override async void OnScreenEnable()
        {
            _indexOfMaps = UnityEngine.Random.Range(0, 3);
            var _mapLoaded = RoundMap();
            await _mapLoaded;
            Instantiate(_currentMapHandle.Result);
            _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        }

    #region Métodos assíncronos 
        private async Task RoundMap(){
            try
            { 
                if (_currentMapHandle.IsValid()){
                    Addressables.Release(_currentMapHandle);
                }
                var _mapReference = _maps[_indexOfMaps];
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
    #endregion
    }
}
