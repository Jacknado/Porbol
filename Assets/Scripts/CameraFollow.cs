using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(-10, 2, 0);
    public float multiplier = 0.85f;
    // Update is called once per frame
    void Update()
    {
        Transform playerTransform = player.gameObject.transform;
        Vector3 cameraPos = new Vector3(playerTransform.position.x, 0, playerTransform.position.z * multiplier);
        transform.position = cameraPos + offset;
    }
}
