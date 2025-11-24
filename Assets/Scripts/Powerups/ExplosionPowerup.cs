using UnityEngine;
using System.Collections;

public class ExplosionPowerup : MonoBehaviour
{
    public float explosionRadius = 5f;
    public LayerMask destroyableLayer;
    public GameObject explosionSpherePrefab; // Assign your sphere prefab here

    private GameObject explosionIndicator;
    private PlayerController playerController;

    void Start()
    {
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
        playerController = GetComponent<PlayerController>();
    }

    public void Explode(Vector3 position, bool respawn)
    {
        if (playerController == null || !playerController.hasExplosion)
            return;

        // Spawn and animate the explosion sphere
        

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
        if (explosionSpherePrefab != null && !respawn)
        {
            GameObject sphere = Instantiate(explosionSpherePrefab, position, Quaternion.identity);
            StartCoroutine(AnimateExplosionSphere(sphere));
        }

        playerController.hasExplosion = false;
    }

    private IEnumerator AnimateExplosionSphere(GameObject sphere)
    {
        float duration = 0.3f; // How fast it expands and fades
        float timer = 0f;

        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * explosionRadius * 2f; // Diameter

        Renderer rend = sphere.GetComponent<Renderer>();
        Material mat = null;
        Color originalColor = Color.white;

        if (rend != null)
        {
            mat = rend.material;
            originalColor = mat.color;
            Color c = originalColor;
            c.a = 1f;
            mat.color = c;
        }

        // Expand sphere
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            sphere.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            if (mat != null)
            {
                Color c = originalColor;
                c.a = Mathf.Lerp(1f, 0f, t); // Fade alpha from 1 to 0
                mat.color = c;
            }

            yield return null;
        }

        Destroy(sphere);
    }

    private bool ShouldDestroyObject(Collider hit)
    {
        if (hit == null)
            return false;

        if (((1 << hit.gameObject.layer) & destroyableLayer) == 0)
            return false;

        string objName = hit.gameObject.name;
        if (objName == "ShieldSphere(Clone)" || objName == "PolyShape" || objName.Contains("Coin"))
            return false;

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
