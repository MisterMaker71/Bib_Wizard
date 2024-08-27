using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtackType { Hit, swing, cast }
public class AtackManager : MonoBehaviour
{
    public AtackType atackType;
    public GameObject Projectile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetMouseButton(0))
        {
            Atack(atackType);
        }
    }

    public void Atack(AtackType atack)
    {
        switch (atack)
        {
            case AtackType.Hit:
                break;
            case AtackType.swing:
                break;
            case AtackType.cast:
                Instantiate(Projectile, Camera.main.transform.position, Camera.main.transform.rotation);
                break;
        }
    }
}
