using System.Threading.Tasks;
using BrawlingToys.DesignPatterns;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace BrawlingToys.Network
{
    public class UnityServicesAuthentication : ContextSingleton<UnityServicesAuthentication>
    {
        [field:Header("Auth")]

        [field:SerializeField] public bool IsConnected { get; private set; }

        [Header("Logs")]

        [SerializeField] private bool _enableLogs; 

        private async void Start() => await MakeAuthAsync();

        public async void TryDoAuthenticationAsync() => await MakeAuthAsync();

        private async Task MakeAuthAsync()
        {
            try
            {
                await UnityServices.InitializeAsync(); 
                await AuthenticationService.Instance.SignInAnonymouslyAsync(); 
    
                IsConnected = true; 
                
                if(_enableLogs)
                    Debug.Log($"Unity Services Initialization Completed!");
            }
            catch (System.Exception)
            {
                IsConnected = false; 
                
                if(_enableLogs)
                    Debug.Log($"Fail to connect to unity services!");
            }
        }
    }
}
