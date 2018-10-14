using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {


    public float xSpawn;
    public float xDestroy;
    private float speed;
    private void Start()
    {
        speed = Random.Range(0.1f, 0.7f);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(-speed * Time.deltaTime, 0,0);
        if (transform.position.x < xDestroy)
            transform.position = new Vector2(xSpawn, transform.position.y);
    }
}
