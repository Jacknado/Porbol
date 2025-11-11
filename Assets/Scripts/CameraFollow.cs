using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float rate = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 location = transform.position;
        location.x += rate * Time.deltaTime;
        transform.position = location;
    }
}
