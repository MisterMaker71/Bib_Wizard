using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Vector2 range = new Vector2(20, 20);
    void Start()
    {
        transform.position = new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject, 1);
        collision.GetComponent<SpriteRenderer>().enabled = false;
        if(collision.GetComponent<Projectile>() != null);
            collision.GetComponent<Projectile>().speed = 0;
        collision.GetComponentInChildren<ParticleSystem>().Stop();
        transform.position = new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));


        FindObjectOfType<PointManager>().AddPoints(1);
    }
}
