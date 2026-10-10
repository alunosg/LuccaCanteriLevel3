using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rig;
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
}
