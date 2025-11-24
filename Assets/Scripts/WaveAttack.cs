using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 moveVector;
    public void Update()
    {
        transform.position += transform.TransformVector(moveVector) * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Fast"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.name.Contains("ShieldSphere"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}