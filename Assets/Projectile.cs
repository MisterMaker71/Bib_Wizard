using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileEffect { damage, fire, explosion, stun }
public enum ProjectileBehaviour { Standert, Homing }
public class Projectile : MonoBehaviour
{
    public ProjectileEffect effect;
    public ProjectileBehaviour behaviour;
    [SerializeField] GameObject hitObject;
    public float speed = 10;
    [SerializeField] float liveTimeAfterHitGround = 1;
    Vector3 moveDir;
    [HideInInspector]
    public bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        moveDir = transform.right;
        transform.right = new Vector3(transform.right.x, transform.right.y, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (behaviour == ProjectileBehaviour.Homing)
        {
            foreach (Target target in FindObjectsOfType<Target>())
            {
                if(Vector3.Distance(target.transform.position, transform.position) < 8)
                {
                    moveDir = Vector3.MoveTowards(transform.position, target.transform.position, 1).normalized;
                }
            }
        }
        transform.position += moveDir * Time.deltaTime * speed;

        if (transform.position.z > 0 && !hit)
        {
            hit = true;
            Instantiate(hitObject, transform.position, hitObject.transform.rotation);
            Destroy(gameObject, liveTimeAfterHitGround);
            moveDir = Vector3.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            FindObjectOfType<PointManager>().AddPoints(-2);
        }
    }
}
