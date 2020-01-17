using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTrap : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    GameObject floor;

    private bool movingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        body.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft) {
            body.velocity = new Vector2(-moveSpeed, 0);
        } else {
            body.velocity = new Vector2(moveSpeed, 0);
        }
        var xThreshold = floor.transform.localScale.x / 2;
        if (transform.localPosition.x < -xThreshold) {
            movingLeft = false;
        } else if (transform.localPosition.x > xThreshold) {
            movingLeft = true;
        }
    }
}
