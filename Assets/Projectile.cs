using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public int damage = 25;

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector3 direction = (target.position - transform.position).normalized;
        float moveDistance = speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, moveDistance))
        {
            if (hit.collider.CompareTag("Building") || hit.collider.CompareTag("Wall"))
            {
                if (hit.transform != target)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        transform.position += direction * moveDistance;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            target.GetComponent<Enemy>()?.ReceiveDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}