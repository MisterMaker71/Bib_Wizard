using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    public Transform target;
    public RectTransform bounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = Camera.main.WorldToScreenPoint(target.position);

        if (bounds != null)
        {
            //if (Camera.main.WorldToScreenPoint(target.position).x > bounds.rect.xMax)
            //{
            //    transform.position = new Vector2(bounds.rect.xMax, transform.position.y);
            //}
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, bounds.rect.xMin, bounds.rect.xMax), Mathf.Clamp(transform.position.y, bounds.rect.yMin, bounds.rect.yMax));
            
        }
    }
}
