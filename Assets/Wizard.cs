using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wizard : MonoBehaviour{
    [SerializeField] float MovementSpeed = 5;
    Vector2 movementVector;
    Animator animator;
    [SerializeField] GameObject fierball;
    [SerializeField] Transform wand;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //transform.position += (Vector3.right * horizontal + Vector3.up * vertical) * Time.deltaTime * MovementSpeed;
        movementVector = ((Vector3.right * horizontal + Vector3.up * vertical)).normalized;
        transform.position += new Vector3(movementVector.x, movementVector.y, 0) * Time.deltaTime * MovementSpeed;
        //print(((Vector3.right * horizontal + Vector3.up * vertical) * Time.deltaTime * MovementSpeed).normalized);
        if (Input.GetMouseButton(0))
        //if (Input.GetMouseButtonDown(0))
        {
            GameObject g = Instantiate(fierball, wand.position, Quaternion.identity);
            Vector3 look = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            g.transform.up = Vector3.MoveTowards(wand.position, new Vector3(look.x, look.y, 0), 1);
            g.transform.Rotate(Vector3.forward * 90);
            Destroy(g, 10);
        }
        if (animator != null)
        {
            animator.SetBool("move", movementVector != Vector2.zero);
            if (Input.GetMouseButton(0))
            {
                animator.SetBool("atack", true);
            }
            else
            {
                animator.SetBool("atack", false);
            }
        }
    }
}
