using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winnerName;
    [SerializeField] private TextMeshProUGUI _coinsCollected;

    public void Leave()
    {
        MatchmakingService.Instance.Leave();
        SceneManager.LoadScene(Scenes.GetNameByType(Scenes.Types.Lobby));
    }

    public void Activate(string winnerName ="", string moneyCount = "")
    {
        gameObject.SetActive(true);

        _winnerName.text = winnerName;
        _coinsCollected.text = moneyCount;
    }
}
