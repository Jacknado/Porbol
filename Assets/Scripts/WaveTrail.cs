using System.Collections;
using UnityEngine;

public class WaveTrail : MonoBehaviour
{
    public GameObject trail;
    public float trailInterval = 0.1f;
    [Header("Assign trailfolder to an empty gameobject. It will store the trail to not clutter scene")]
    public GameObject trailFolder;
    void Start()
    {
        InvokeRepeating("MakeTrail", 0, trailInterval);
    }
    void MakeTrail()
    {
        Debug.Log(gameObject.transform.position);
        GameObject newTrail = Instantiate(trail, transform.position, transform.rotation, trailFolder.transform);
        StartCoroutine(RemoveTrail(newTrail));
    }
    IEnumerator RemoveTrail(GameObject newTrail)
    {
        yield return new WaitForSeconds(1f);
        Destroy(newTrail);
    }
}
