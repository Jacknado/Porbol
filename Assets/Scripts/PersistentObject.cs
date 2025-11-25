using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private void Awake()
    {
        // If another instance already exists, destroy this one
        // (prevents duplicates after scene loads)
        int count = FindObjectsOfType<PersistentObject>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
