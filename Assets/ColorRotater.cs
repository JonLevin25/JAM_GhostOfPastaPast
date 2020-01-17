using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRotater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _rend;

    [SerializeField] private float _degreesPerSec;
    [SerializeField] private bool _reverse;

    private void Update()
    {
        var delta = (_degreesPerSec/360) * Time.deltaTime;
        if (_reverse) delta *= -1;
        
        Color.RGBToHSV(_rend.color, out var h, out var s, out var v);
        
        h += delta;
        h %= 360;
        var newColor = Color.HSVToRGB(h, s, v);

        _rend.color = newColor;
    }
}
