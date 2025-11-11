using System;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float horizontal;
    public float zMinimum;
    public float zMaximum;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        float MoveMagnitude = horizontal * speed * Time.deltaTime;
        if(!((transform.position.z + MoveMagnitude) > zMaximum || (transform.position.z + MoveMagnitude) < zMinimum))
        {
            transform.Translate(new Vector3(0, 0, MoveMagnitude));
        }
    }
}
