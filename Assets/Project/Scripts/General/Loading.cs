using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private async void Start()
    {
        await UnityServicesAuthentiacor.InitializeServices();
        Application.targetFrameRate = 120;
        SceneManager.LoadScene(Scenes.GetNameByType(Scenes.Types.Lobby));
    }
}
