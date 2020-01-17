using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField]
    private LayerMask trapLayerMask;

    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var isTrap = ContainsLayer(trapLayerMask, other.gameObject.layer);
        if (!isTrap) return;
        
        // player.TakeDamage();
    }


    private bool ContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
