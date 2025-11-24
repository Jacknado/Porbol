using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(-10, 2, 0);
    public float multiplier = 0.85f;
    public bool useSmoothFollow = false;
    public float smoothSpeed = 5f;

    private Transform playerTransform;

    void Start()
    {
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("CameraFollow: Player reference is missing!");
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null)
            return;

        Vector3 targetPos = new Vector3(
            playerTransform.position.x, 
            0, 
            playerTransform.position.z * multiplier
        ) + offset;

        if (useSmoothFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPos;
        }
    }
}