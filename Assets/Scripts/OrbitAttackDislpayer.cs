using UnityEngine;

public class OrbitAttackDisplayer : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSmooth = 15f;

    private float startOffsetAngle;
    public float OffsetAngle;

    void Start()
    {
        Camera cam = Camera.main;

        Vector3 centerScreen = cam.WorldToScreenPoint(centerPoint.position);
        Vector3 objectScreen = cam.WorldToScreenPoint(transform.position);

        Vector2 v = (Vector2)objectScreen - (Vector2)centerScreen;
        startOffsetAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        Camera cam = Camera.main;

        Vector3 centerScreen = cam.WorldToScreenPoint(centerPoint.position);
        Vector2 mouseVector = (Vector2)Input.mousePosition - (Vector2)centerScreen;

        float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        // FIX: flip direction so left mouse = left rotation
        float finalAngle = -(mouseAngle + startOffsetAngle + OffsetAngle);

        Quaternion targetRot = Quaternion.Euler(0f, finalAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSmooth);
    }
}
