using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    private class PlayerSlot
    {
        public readonly PlayerNum PlayerNum;
        public readonly PlayerConfig PlayerConfig;
        public readonly PlayerUI PlayerUI;
        public bool WasInitialized;
        public bool IsAlive;

        public PlayerSlot(PlayerConfig playerConfig, PlayerUI playerUI)
        {
            PlayerConfig = playerConfig;
            PlayerNum = playerConfig.PlayerNum;
        }
    }
    
    public static event Action<PlayerNum> OnPlayerEnteredGame;
    public static event Action<PlayerNum> OnLastManStanding;

    private IEnumerable<PlayerSlot> PlayersWaitingForInit 
        => _playerSlots.Where(slot => !slot.WasInitialized);
    
    // private int 
    private PlayerSlot[] _playerSlots;

    private void Start()
    {
        _playerSlots = GetSlotsFromScenePlayers().ToArray();
        foreach (var playerSlot in _playerSlots)
        {
            playerSlot.PlayerConfig.PlayerHealth.OnDeath += () => OnPlayerDeath(playerSlot);
            playerSlot.PlayerConfig.PlayerRootObject.SetActive(false);
        }
    }

    private void OnPlayerDeath(PlayerSlot playerSlot) 
    {
        playerSlot.IsAlive = false;
        var livePlayers = _playerSlots.Where(slot => slot.IsAlive).ToArray();
        if (livePlayers.Length == 1)
        {
            var lastPlayerNum = livePlayers[0].PlayerNum;
            OnLastManStanding?.Invoke(lastPlayerNum);
        }
    }

    private static IEnumerable<PlayerSlot> GetSlotsFromScenePlayers()
    {
        return Enum.GetValues(typeof(PlayerNum)).Cast<PlayerNum>()
            .Select(playerNum =>
            {
                var config = PlayerConfig.GetConfig(playerNum);
                var ui = PlayerUI.GetUI(playerNum);

                return new PlayerSlot(config, ui);
            });
    }

    private void Update()
    {
        foreach (var player in PlayersWaitingForInit)
        {
            if (PollAnyKey(player)) InitPlayer(player);
        }
    }

    private static bool PollAnyKey(PlayerSlot player)
    {
        return player.PlayerNum.GetPlayer().GetAnyButton();
        
    }

    private static void InitPlayer(PlayerSlot playerSlot)
    {
        playerSlot.PlayerConfig.PlayerRootObject.SetActive(true);
        playerSlot.WasInitialized = true;
        playerSlot.IsAlive = true;
        
        OnPlayerEnteredGame?.Invoke(playerSlot.PlayerNum);
    }
}