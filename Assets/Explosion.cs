using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public bool destroyOnExplode = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Explode()
    {
        //Explode

        // play particals

        // give Damage

        if (destroyOnExplode)
            Destroy(gameObject);
    }
}
