using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HorizontalTrap : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5;

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    GameObject start;

    [SerializeField]
    GameObject end;


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
        if (transform.localPosition.x < start.transform.localPosition.x) {
            movingLeft = false;
        } else if (transform.localPosition.x > end.transform.localPosition.x) {
            movingLeft = true;
        }
    }
}
