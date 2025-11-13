using Unity.VisualScripting;
using UnityEngine;

public class ShieldPowerup : MonoBehaviour
{
    public GameObject ShieldSphere;
    private bool active;
    private GameObject newShieldSphere;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Enable()
    {
        newShieldSphere = Instantiate(ShieldSphere, transform);
        active = true;
    }
    public void Disable()
    {
        Destroy(newShieldSphere);
        active = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            Destroy(other.gameObject);
            Disable();
        }
    }
}
