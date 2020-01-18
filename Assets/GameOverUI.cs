using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiRoot;
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _uiRoot.SetActive(false);
        PlayersManager.OnLastManStanding += OnLastManStanding;
    }

    private void OnDestroy()
    {
        PlayersManager.OnLastManStanding -= OnLastManStanding;
    }

    private void OnLastManStanding(PlayerNum winner)
    {
        var color = PlayerConfig.GetConfig(winner).PlayerColor;
        SetWinText(winner, color);
        
        _uiRoot.SetActive(true);
    }

    private void SetWinText(PlayerNum winner, Color color)
    {
        var colorHex = ColorHex(color);
        Debug.Log($"Converting color ({color} -> {colorHex}");
        _text.text = $"<color={colorHex}>{winner}</color> WINS!";
    }

    private static string ColorHex(Color color) => $"#{ColorFloatToHex(color.r)}{ColorFloatToHex(color.g)}{ColorFloatToHex(color.b)}";

    private static string ColorFloatToHex(float normalizedColorVal)
    {
        var colorInt = (int) (normalizedColorVal * 256);
        return colorInt.ToString("X2");
    }
}
