using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Projectile3D : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 4;
    public float liveTIme = 8;
    public LayerMask hitMask;
    public float hitDetection = 0.05f;
    public UnityEvent OnHit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        liveTIme -= Time.deltaTime;
        if(liveTIme <= 0)
        {
            transform.localScale -= Vector3.one * (Time.deltaTime / 5);
            if (!GetComponent<MeshRenderer>().isVisible || transform.localScale.x <= 0)
                Destroy(gameObject);

        }
        if (rb.velocity.magnitude < 0.2)
            liveTIme = 0;

        if(Physics.CheckSphere(transform.position, GetComponent<SphereCollider>().radius + 0.1f, hitMask))
        {
            OnHit.Invoke();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(GetComponent<SphereCollider>() != null)
            Gizmos.DrawWireSphere(transform.position, (GetComponent<SphereCollider>().radius + hitDetection) * transform.localScale.x );
    }
}
