using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image _progress;

    private float _maxHp;

    public void Initialize(float maxHp, NetworkVariable<float> health)
    {
        gameObject.SetActive(true);
        _maxHp = maxHp;
        health.OnValueChanged += UpdateBar;
    }

    public void UpdateBar(float previousValue, float currentValue)
    {
        if(currentValue < 0) 
            currentValue = 0;
        var progress = currentValue / _maxHp;
        _progress.fillAmount = progress;
    }
}
