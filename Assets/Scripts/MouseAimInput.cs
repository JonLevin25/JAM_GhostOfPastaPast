using UnityEngine;

public class MouseAimInput : MonoBehaviour
{
    [SerializeField] private Transform _reference;
    [SerializeField] private float _threshold;

    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }
    
    public Vector2 GetAim()
    {
        var screenCursorPos = Input.mousePosition;
        screenCursorPos.z = _reference.position.z; // Make sure we cast to same Z depth

        var worldCursorPos = _cam.ScreenToWorldPoint(screenCursorPos);
        var delta = worldCursorPos - _reference.position;
        
        return new Vector2(Set01(delta.x, _threshold), Set01(delta.y, _threshold));
    }

    private static float Set01(float n, float threshold)
    {
        var abs = Mathf.Abs(n);
        if (abs < threshold) return 0f;
        return n / abs;
    }
}
