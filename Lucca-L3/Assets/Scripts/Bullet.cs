using JetBrains.Annotations;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rig;

    public float damage = 1; 
    
    public GameObject hitFX;
    public Transform model;

    public float spinSpeed = 720f;

    void FixedUpdate()
    {
        if(rig.linearVelocity.sqrMagnitude > 0.01f) transform.forward = rig.linearVelocity.normalized;
    }

    void Update()
    {
        model.Rotate(transform.up * spinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().GetHit(damage);

        }

        if(hitFX) Instantiate(hitFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }

}
