using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 moveDirection = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(0,speed);

        transform.Translate( moveDirection.x,moveDirection.y,0);
    }
    void OnBecameInvisible() {
        Destroy (this.gameObject);
    }
}
