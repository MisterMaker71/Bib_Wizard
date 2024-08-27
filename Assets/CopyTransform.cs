using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CopyTransform : MonoBehaviour
{
    [SerializeField] Transform taraget;
    void Update()
    {
        if(taraget != null)
        {
            transform.position = taraget.position;
            transform.rotation = taraget.rotation;
            transform.localScale = taraget.localScale;
        }
    }
}
