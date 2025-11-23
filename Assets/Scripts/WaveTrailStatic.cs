using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]  // This makes the script run in Edit mode too
[RequireComponent(typeof(LineRenderer))]
public class WaveTrailStatic : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1.25f;
    public Vector3 waveAxis = new Vector3(0, 0, 1);
    public Vector3 trailDirection = Vector3.right;
    public float trailLength = 1.5f;
    public float pointSpacing = 0.02f;
    public float thickness = 0.2f;

    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);

    private LineRenderer lr;
    private List<Vector3> basePoints = new List<Vector3>();

    void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        if (lr == null)
            lr = gameObject.AddComponent<LineRenderer>();

        lr.widthMultiplier = thickness;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.textureMode = LineTextureMode.Stretch;

        InitializePoints();
    }

    void OnValidate()
    {
        // Re-initialize points if parameters change in inspector
        InitializePoints();
        UpdateWavePositions();
    }

    void InitializePoints()
    {
        if (lr == null)
            lr = GetComponent<LineRenderer>();

        int pointsCount = Mathf.CeilToInt(trailLength / pointSpacing);
        basePoints.Clear();

        Vector3 startPos = transform.position;
        Vector3 dirNormalized = trailDirection.normalized;

        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 point = startPos + dirNormalized * (i * pointSpacing);
            basePoints.Add(point);
        }

        lr.positionCount = pointsCount;
    }

    void Update()
    {
        UpdateWavePositions();

#if UNITY_EDITOR
        // Keep repainting scene view in editor to see animation
        if (!Application.isPlaying)
            UnityEditor.SceneView.RepaintAll();
#endif
    }

    void UpdateWavePositions()
    {
        if (basePoints.Count == 0)
            return;

        Vector3[] wavePoints = new Vector3[basePoints.Count];
        float time = Application.isPlaying ? Time.time : (float)UnityEditor.EditorApplication.timeSinceStartup;

        for (int i = 0; i < basePoints.Count; i++)
        {
            float wavePhase = time * frequency * Mathf.PI * 2f + i * 0.5f;
            float waveOffsetValue = Mathf.Sin(wavePhase) * amplitude;
            wavePoints[i] = basePoints[i] + waveAxis.normalized * waveOffsetValue;
        }

        lr.SetPositions(wavePoints);
    }
}
