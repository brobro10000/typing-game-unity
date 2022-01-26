using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 originalPos;
    public float speed;
    public int userWPM;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        Vector3 randPos = new Vector3(15, (float)(rand.Next(-3, 3)), 300 + (SpawnManager.lastUsedNum * 10));

        // speed = speed * ((float)rand.Next(1, 1000) / 500);
        gameObject.transform.position = randPos;
        float multiplier = (float)rand.NextDouble();
        speed = speed * ((multiplier > .5f) ? multiplier : .5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Vector3 vector3 = gameObject.transform.position;
        vector3.x -= speed * userWPM;
        vector3.y += (float)Math.Sin((double)timer) * .01f;
        gameObject.transform.SetPositionAndRotation(vector3, gameObject.transform.rotation);
    }
    
}
