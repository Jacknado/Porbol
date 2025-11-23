using UnityEngine;

[System.Serializable]
public class ObstacleEntry
{
    public GameObject prefab;
    public Vector3 offset = Vector3.zero;

    [Tooltip("Minimum distance required between this obstacle and others.")]
    public float radius = 2f;
}
