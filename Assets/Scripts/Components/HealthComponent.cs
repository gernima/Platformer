using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private TextMeshProUGUI _text;
    private void Start()
    {
        CheckText();
    }
    public void ApplyDamage(int damageValue)
    {
        _health -= damageValue;
        if (damageValue > 0)
        {
            _onDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }
        CheckText();
    }
    void CheckText()
    {
        if (_text != null)
        {
            _text.text = "HP: " + _health;
        }
    }
}
