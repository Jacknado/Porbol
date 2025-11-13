using UnityEngine;

public class ExplosionPowerup : MonoBehaviour
{
    public float explosionRadius = 5f;
    public LayerMask destroyableLayer;

    public void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit is BoxCollider && ((1 << hit.gameObject.layer) & destroyableLayer) != 0)
            {
                hit.gameObject.SetActive(false);
            }
        }
    }

    // Optional: visualize explosion radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
