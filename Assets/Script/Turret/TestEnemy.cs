using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{

    [Header("speed")]
    public float speed = 10.0F;
    //public float rotationSpeed = 100.0F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Time.deltaTime * -speed;
        // float rotation =  rotationSpeed * Time.deltaTime;       
        transform.Translate(0, 0, translation);
        // transform.Rotate(0, rotation, 0);
    }
}
