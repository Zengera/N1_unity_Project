using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbeam : MonoBehaviour
{
    public float speed = 2;

    void OnBecameInvisible() 
    {
        Destroy (this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate (speed, 0, 0);
    }
}
