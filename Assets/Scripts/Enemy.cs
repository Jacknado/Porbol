using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public bool stopOnCollision = true;

    private NavMeshAgent agent;
    private bool hasCollided = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent == null)
        {
            Debug.LogError($"Enemy: NavMeshAgent component missing on {gameObject.name}");
            return;
        }
        
        agent.updateRotation = true;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (agent == null || target == null)
            return;

        if (!hasCollided)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (stopOnCollision && !hasCollided)
        {
            hasCollided = true;
            agent.isStopped = true;
        }
    }

    public void ResumeChasing()
    {
        if (agent != null)
        {
            hasCollided = false;
            agent.isStopped = false;
        }
    }
}