using System.Collections;
using UnityEngine;

public class WaveTrail : MonoBehaviour
{
    public GameObject trail;
    public GameObject trailFolder;
    public float trailInterval = 0.01f;
    public float removeTrailTime = 1.3f;
    public float amplitude = 0.25f;
    public float frequency = 1.25f;
    public Vector3 waveAxis = new Vector3(0, 0, 1);
    public Vector3 waveOffset = new Vector3(-0.5f, 0, 0);

    private float timeElapsed = 0f;

    void Start()
    {
        InvokeRepeating(nameof(MakeTrail), 0, trailInterval);
    }

    void MakeTrail()
    {
        float waveValue = Mathf.Sin(timeElapsed * Mathf.PI * 2f * frequency) * amplitude;
        Vector3 offset = waveAxis.normalized * waveValue;

        Vector3 spawnPos = transform.position + offset + waveOffset;
        GameObject newTrail = Instantiate(trail, spawnPos, transform.rotation, trailFolder.transform);

        StartCoroutine(RemoveTrail(newTrail));

        timeElapsed += trailInterval;
    }

    IEnumerator RemoveTrail(GameObject newTrail)
    {
        yield return new WaitForSeconds(removeTrailTime);
        Destroy(newTrail);
    }
}
