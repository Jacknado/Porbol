using UnityEngine;

public class ExplosionPowerup : MonoBehaviour
{
    public float explosionRadius = 5f;
    public LayerMask destroyableLayer;

    private GameObject explosionIndicator;
    private PlayerController playerController;

    void Start()
    {
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
        playerController = GetComponent<PlayerController>();
    }

    public void Explode(Vector3 position)
    {
        if (playerController == null || !playerController.hasExplosion)
            return;

        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        
        if (explosionIndicator != null)
        {
            explosionIndicator.SetActive(false);
        }

        foreach (Collider hit in hits)
        {
            if (ShouldDestroyObject(hit))
            {
                hit.gameObject.SetActive(false);
            }
        }

        playerController.hasExplosion = false;
    }

    private bool ShouldDestroyObject(Collider hit)
    {
        if (hit == null)
            return false;

        if (((1 << hit.gameObject.layer) & destroyableLayer) == 0)
            return false;

        string objName = hit.gameObject.name;
        if (objName == "ShieldSphere(Clone)" || objName == "PolyShape")
            return false;

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}