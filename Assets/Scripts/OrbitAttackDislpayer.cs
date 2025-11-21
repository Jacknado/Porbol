using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitAttackDisplayer : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSmooth = 15f;
    private float startOffsetAngle;
    public float OffsetAngle;

    public float meleeCooldownDuration = 1.5f;
    private bool isOnCooldown = false;

    // Track enemies inside trigger
    private List<GameObject> enemiesInRange = new List<GameObject>();

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
        // ROTATION
        Camera cam = Camera.main;
        Vector3 centerScreen = cam.WorldToScreenPoint(centerPoint.position);
        Vector2 mouseVector = (Vector2)Input.mousePosition - (Vector2)centerScreen;
        float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        float finalAngle = -(mouseAngle + startOffsetAngle + OffsetAngle);

        Quaternion targetRot = Quaternion.Euler(0f, finalAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSmooth);
        transform.position = centerPoint.position;

        // CLICK HANDLING HERE (RELIABLE)
        if (!isOnCooldown && Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (enemiesInRange.Count == 0)
            return;

        // Destroy the closest enemy for consistency
        GameObject target = enemiesInRange[0];
        float closestDist = float.MaxValue;

        foreach (GameObject e in enemiesInRange)
        {
            if (e == null) continue;
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                target = e;
            }
        }

        if (target != null)
        {
            Destroy(target);
            enemiesInRange.Remove(target);
            StartCoroutine(HandleMeleeCooldown());
        }
    }

    private IEnumerator HandleMeleeCooldown()
    {
        isOnCooldown = true;
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(meleeCooldownDuration);

        GetComponent<MeshRenderer>().enabled = true;
        isOnCooldown = false;
    }

    // Track enemies entering range
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("FastEnemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    // Remove enemies leaving range
    void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    // Clean up destroyed enemies
    void LateUpdate()
    {
        enemiesInRange.RemoveAll(e => e == null);
    }
}
