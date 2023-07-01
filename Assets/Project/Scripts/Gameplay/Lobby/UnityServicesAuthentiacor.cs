using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class UnityServicesAuthentiacor 
{
    public static bool IsInitialized => UnityServices.State != ServicesInitializationState.Uninitialized;

    public static async Task<IAuthenticationService> InitializeServices()
    {
        try
        {
            if (!IsInitialized)
            {
                var options = new InitializationOptions();

                await UnityServices.InitializeAsync(options);
                AuthenticationService.Instance.SignedIn += () =>
                {
                    Debug.Log("Signed id: " + AuthenticationService.Instance.PlayerId);
                };
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            return AuthenticationService.Instance;
        }
        catch
        {
            Debug.LogWarning("Unity Services Authentication Error");
        }
     
        return null;
    }
}
