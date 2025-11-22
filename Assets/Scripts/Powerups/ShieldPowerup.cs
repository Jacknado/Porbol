using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    public GameObject shieldSphere;

    private bool isActive;
    private GameObject activeShieldSphere;

    public void Enable()
    {
        if (isActive || shieldSphere == null)
            return;

        activeShieldSphere = Instantiate(shieldSphere, transform);
        isActive = true;
    }

    public void Disable()
    {
        if (!isActive)
            return;

        if (activeShieldSphere != null)
        {
            Destroy(activeShieldSphere);
        }
        
        activeShieldSphere = null;
        isActive = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive)
            return;

        string objName = other.gameObject.name;
        
        if (objName.EndsWith("Powerup") || objName == "MeleeShower" || objName.Contains("Wave"))
            return;

        Destroy(other.gameObject);
        Disable();
    }
}