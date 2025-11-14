using Unity.VisualScripting;
using UnityEngine;

public class ExplosionPowerup : MonoBehaviour
{
    public float explosionRadius = 5f;
    public LayerMask destroyableLayer;
    public GameObject ExplosionIndicator;

    public void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        ExplosionIndicator.SetActive(false);
        foreach (Collider hit in hits)
        {
            if (hit is MeshCollider && ((1 << hit.gameObject.layer) & destroyableLayer) != 0)
            {
                hit.gameObject.SetActive(false);
            }
        }
    }
    void Start()
    {
        ExplosionIndicator = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }
    // Optional: visualize explosion radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
