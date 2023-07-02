using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [field: SerializeField] public FloatingJoystick Joystick { get; private set; }
    [field: SerializeField] public Button ShootButton { get; private set; }
    [field: SerializeField] public HpBar HpBar { get; private set; }
    [field: SerializeField] public FinishScreen FinishScreen { get; private set; }
    [field: SerializeField] public CoinsGameUI CoinsGameUI { get; private set; }
}
