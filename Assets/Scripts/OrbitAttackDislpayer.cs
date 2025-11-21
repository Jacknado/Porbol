using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitAttackDisplayer : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSmooth = 15f;
    public float offsetAngle;
    public float meleeCooldownDuration = 1.5f;

    private float startOffsetAngle;
    private bool isOnCooldown = false;
    private List<GameObject> enemiesInRange = new List<GameObject>();
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
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (enemiesInRange.Count == 0)
            return;

        GameObject target = FindClosestEnemy();
        
        if (target != null)
        {
            Destroy(target);
            enemiesInRange.Remove(target);
            StartCoroutine(HandleMeleeCooldown());
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject closest = null;
        float closestDist = float.MaxValue;

        foreach (GameObject e in enemiesInRange)
        {
            if (e == null)
                continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < closestDist)
            {
                closestDist = d;
                closest = e;
            }
        }

        return closest;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("FastEnemy"))
        {
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void LateUpdate()
    {
        enemiesInRange.RemoveAll(e => e == null);
    }
}