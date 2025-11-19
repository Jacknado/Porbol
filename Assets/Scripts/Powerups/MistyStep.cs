using System;
using Unity.VisualScripting;
using UnityEngine;

public class MistyStep : MonoBehaviour
{
    public float distance = 5f;
    public GameObject TargetBlock;
    private bool active;
    private GameObject newTargetBlock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Enable()
    {
        if (!active)
        {
            Vector3 startLoc = GameObject.Find("Player").transform.position;
            Vector3 finalLoc = new Vector3(startLoc.x + distance, startLoc.y, startLoc.z);
            newTargetBlock = Instantiate(TargetBlock, finalLoc, transform.rotation, transform);
            active = true;
        }
    }
    public void Disable()
    {
        if(active)
        {
            Destroy(newTargetBlock);
            active = false;
        }
    }
    public void Step(Vector3 position)
    {
        Disable();
        if (GameObject.Find("Player").GetComponent<PlayerController>().hasStep)
        {
            GameObject.Find("Player").transform.position = newTargetBlock.transform.position; 
            GameObject.Find("Player").GetComponent<PlayerController>().hasStep = false;
        }
    }
}