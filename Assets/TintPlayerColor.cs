using UnityEngine;
using UnityEngine.UI;

public class TintPlayerColor : MonoBehaviour
{
    [SerializeField] private PlayerNum _playerNum;
    [SerializeField] private SpriteRenderer _rend;
    [SerializeField] private Image _image;

    private PlayerConfig _playerConfig;
    private void Start()
    {
        _playerConfig = PlayerConfig.GetConfig(_playerNum);
        var color = _playerConfig.PlayerColor;
        
        if (_rend)
        {
            _rend.color = color;
        }

        if (_image)
        {
            _image.color = color;
        }
    }
}
