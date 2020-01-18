using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private GameObject _playerHudRoot;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private PlayerHealthBar _healthBar;
    [SerializeField] private GameObject _deathOverlay;
    [SerializeField] private TextMeshProUGUI _pressAnykey;
    
    private PlayerConfig _playerConfig;
    private static readonly Dictionary<PlayerNum, PlayerUI> _playerUiDict 
        = new Dictionary<PlayerNum, PlayerUI>();

    public static PlayerUI GetUI(PlayerNum playerNum) => _playerUiDict[playerNum];

    private void Awake() => _playerUiDict[_playerNum] = this;

    // Start is called before the first frame update
    private void Start()
    {
        _playerConfig = PlayerConfig.GetConfig(_playerNum);
        
        _titleText.text = _playerNum.ToString();
        SetColors(_playerConfig.PlayerColor);
        
        _deathOverlay.SetActive(false);
        var health = _playerConfig.PlayerHealth;
        
        // Subscribe
        health.OnHpChanged += OnHpChanged;
        health.OnDeath += OnDeath;
        PlayersManager.OnPlayerEnteredGame += OnPlayerEneteredGame;
        
        _playerHudRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayersManager.OnPlayerEnteredGame -= OnPlayerEneteredGame;
    }

    private void OnPlayerEneteredGame(PlayerNum playerNum)
    {
        if (playerNum != _playerNum) return;
        _pressAnykey.gameObject.SetActive(false);
        _playerHudRoot.SetActive(true);
    }

    private void OnHpChanged(int curr, int total, int delta)
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
        _pressAnykey.color = color;
    }
}