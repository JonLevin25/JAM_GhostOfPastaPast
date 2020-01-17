using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private PlayerHealthBar _healthBar;
    [SerializeField] private GameObject _deathOverlay;
    
    private PlayerConfig _playerConfig;
    
    // Start is called before the first frame update
    private void Start()
    {
        _playerConfig = PlayerConfig.GetConfig(_playerNum);
        
        _titleText.text = _playerNum.ToString();
        SetColors(_playerConfig.PlayerColor);
        
        _deathOverlay.SetActive(false);
        var health = _playerConfig.PlayerHealth;
        health.OnHpChanged += OnHpChanged;
        health.OnDeath += OnDeath;
    }

    private void OnHpChanged(int curr, int total)
    {
        var percent = (float) curr / total;
        _healthBar.SetFill(percent);
    }

    private void OnDeath()
    {
        _deathOverlay.SetActive(true);   
    }

    private void SetColors(Color color)
    {
        _titleText.color = color;
        _healthBar.SetColor(color);
    }
}