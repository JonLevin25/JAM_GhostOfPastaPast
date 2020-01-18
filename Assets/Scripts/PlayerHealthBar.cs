using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void SetColor(Color color) => _fillImage.color = color;

    public void SetFill(float amount) => _fillImage.fillAmount = amount;
}