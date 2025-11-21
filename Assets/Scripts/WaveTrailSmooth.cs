using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WaveTrailSmooth : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1.25f;
    public Vector3 waveAxis = new Vector3(0, 0, 1);
    public Vector3 waveOffset = new Vector3(-0.5f, 0, 0);

    public float trailLength = 1.5f;
    public float pointSpacing = 0.02f;
    public float thickness = 0.2f;

    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);

    private LineRenderer lr;
    private float timer;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 0;
        lr.widthMultiplier = thickness;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.textureMode = LineTextureMode.Stretch;
    }

    public void RemoveTrail()
    {
        points.Clear();
        if (lr != null)
        {
            lr.positionCount = 0;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= pointSpacing)
        {
            float t = Time.time;
            float waveValue = Mathf.Sin(t * Mathf.PI * 2f * frequency) * amplitude;
            Vector3 offset = waveAxis.normalized * waveValue;
            Vector3 pos = transform.position + offset + waveOffset;
            
            points.Add(pos);
            timer = 0;
        }

        float maxPoints = trailLength / pointSpacing;
        while (points.Count > maxPoints)
        {
            points.RemoveAt(0);
        }

        if (lr != null)
        {
            lr.positionCount = points.Count;
            if (points.Count > 0)
            {
                lr.SetPositions(points.ToArray());
            }
        }
    }
}