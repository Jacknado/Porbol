using Unity.VisualScripting;
using UnityEngine;

public class MistyStep : MonoBehaviour
{
    public float explosionRadius = 5f;
    public LayerMask destroyableLayer;
    private GameObject explosionIndicator;

    public void Step(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius);
        explosionIndicator.SetActive(false);
        if (GameObject.Find("Player").GetComponent<PlayerController>().hasExplosion)
        {
            foreach (Collider hit in hits)
            {
                if (hit is BoxCollider && ((1 << hit.gameObject.layer) & destroyableLayer) != 0)
                {
                    hit.gameObject.SetActive(false);
                }
            }
            GameObject.Find("Player").GetComponent<PlayerController>().hasExplosion = false;
        }
    }
    void Start()
    {
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
    }
    // Optional: visualize explosion radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}