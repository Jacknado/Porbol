using UnityEngine;

public class MistyStep : MonoBehaviour
{
    public float distance = 5f;
    public GameObject targetBlock;

    private bool isActive;
    private GameObject activeTargetBlock;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Enable()
    {
        if (isActive || targetBlock == null)
            return;

        Vector3 startLoc = transform.position;
        Vector3 finalLoc = new Vector3(startLoc.x + distance, startLoc.y, startLoc.z);
        
        activeTargetBlock = Instantiate(targetBlock, finalLoc, transform.rotation, transform);
        isActive = true;
    }

    public void Disable()
    {
        if (!isActive || activeTargetBlock == null)
            return;

        Destroy(activeTargetBlock);
        activeTargetBlock = null;
        isActive = false;
    }

    public void Step(Vector3 position)
    {
        if (playerController == null || !playerController.hasStep || activeTargetBlock == null)
            return;

        transform.position = activeTargetBlock.transform.position;
        playerController.hasStep = false;
        
        Disable();
    }
}