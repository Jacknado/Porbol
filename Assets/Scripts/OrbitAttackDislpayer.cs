using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitAttackDisplayer : MonoBehaviour
{
    public Transform centerPoint;
    public GameObject waveMesh;
    public float rotationSmooth = 15f;
    public float offsetAngle;
    public float meleeCooldownDuration = 1.5f;

    private float startOffsetAngle;
    private bool isOnCooldown = false;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (centerPoint != null)
        {
            Camera cam = Camera.main;
            Vector3 centerScreen = cam.WorldToScreenPoint(centerPoint.position);
            Vector3 objectScreen = cam.WorldToScreenPoint(transform.position);
            Vector2 v = (Vector2)objectScreen - (Vector2)centerScreen;
            startOffsetAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        if (centerPoint == null)
            return;

        Camera cam = Camera.main;
        Vector3 centerScreen = cam.WorldToScreenPoint(centerPoint.position);
        Vector2 mouseVector = (Vector2)Input.mousePosition - (Vector2)centerScreen;
        float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        float finalAngle = -(mouseAngle + startOffsetAngle + offsetAngle);

        Quaternion targetRot = Quaternion.Euler(0f, finalAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSmooth);
        transform.position = centerPoint.position;

        if (!isOnCooldown && Input.GetMouseButtonDown(0))
        {
            TryAttack(finalAngle);
        }
    }

    private void TryAttack(float finalAngle)
    {
        Quaternion rot = Quaternion.Euler(-90f, finalAngle, 0f);

        // This is the offset in the mesh's LOCAL SPACE
        Vector3 localOffset = new Vector3(-2f, 0.5f, 0f); 
        // ^ move left/right depending on your model's actual offset direction

        // Convert local offset to world-space offset
        Vector3 worldOffset = rot * localOffset;

        Instantiate(waveMesh, centerPoint.position - worldOffset, rot);

        StartCoroutine(HandleMeleeCooldown());
    }

    private IEnumerator HandleMeleeCooldown()
    {
        isOnCooldown = true;
        
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        yield return new WaitForSeconds(meleeCooldownDuration);

        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }
        
        isOnCooldown = false;
    }
}