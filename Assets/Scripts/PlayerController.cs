using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public float MovementSpeed = 5;
    public float RunSpeed = 5;
    public float JumpPower = 2.5f;
    [Min(0)]
    public float gravetyMultyplyer = 1;
    public static CharacterController controller;
    public LayerMask GroundLayer;
    //public Rigidbody rb;
    //[HideInInspector]
    public Vector3 velocity;
    public float mouseSensebility = 100f;
    Vector3 beforPosition = new Vector3(0, 0, 0);
    float needToJump = -1;
    float timeSincGrounded = 100;

    float camXRotation;
    float camYRotation;

    public static bool CanMove = true;
    public static bool CanLock = true;
    public static Vector2 CamXClamp = new Vector2(-90, 90);
    public static Vector2 CamYClamp = new Vector2(-Mathf.Infinity, Mathf.Infinity);
    public static GameObject Player;

    private void Awake()
    {
        Player = gameObject;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        beforPosition = transform.position;
        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static Vector3 InputVector
    {
        get { return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Jump"), Input.GetAxisRaw("Vertical")); }
    }
    // Update is called once per frame
    void Update()
    {
        if(Player == null)
            Player = gameObject;
        if(controller == null)
            controller = GetComponent<CharacterController>();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensebility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensebility * Time.deltaTime;

        camXRotation -= mouseY;
        camYRotation += mouseX;

        camXRotation = Mathf.Clamp(camXRotation, CamXClamp.x, CamXClamp.y);
        camYRotation = Mathf.Clamp(camYRotation, CamYClamp.x, CamYClamp.y);


        if (CanLock)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(camXRotation, 0, 0);
            transform.localRotation = Quaternion.Euler(0, camYRotation, 0);
        }
        //transform.Rotate(Vector3.up *mouseX);



        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Vector3.down * 0.1f, Color.red, 10);
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 0.3f, Color.grey, 10);


        //Interact
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.GetComponent<Interactor>() != null)
            {
                if (Vector3.Distance(Camera.main.transform.position, hit.point) < 3)
                {
                    Debug.DrawLine(hit.point, hit.point + -Camera.main.transform.forward * 0.2f, Color.green, 5);
                    if(Input.GetKeyDown(KeyCode.E))
                        hit.transform.GetComponent<Interactor>().Interact.Invoke();
                }
                else
                    Debug.DrawLine(hit.point, hit.point + -Camera.main.transform.forward * 0.2f, Color.blue, 5);
            }
            else
                Debug.DrawLine(hit.point, hit.point + -Camera.main.transform.forward * 0.2f, Color.red, 5);
        }



        if (CanMove)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                controller.Move(transform.right * InputVector.x * Time.deltaTime * RunSpeed + transform.forward * InputVector.z * RunSpeed * Time.deltaTime + Vector3.up * velocity.y * Time.deltaTime);
            else
                controller.Move(transform.right * InputVector.x * Time.deltaTime * MovementSpeed + transform.forward * InputVector.z * MovementSpeed * Time.deltaTime + Vector3.up * velocity.y * Time.deltaTime);
        }
        if (animator != null)
            animator.SetFloat("Speed", controller.velocity.magnitude);

        Vector3 colo = Vector3.Normalize(new Vector3(transform.position.x - beforPosition.x, transform.position.y - beforPosition.y, transform.position.z - beforPosition.z) + (Vector3.one * 0.5f));
        Debug.DrawLine(beforPosition, transform.position, new Color(colo.x, colo.y, colo.z), 10);

        beforPosition = transform.position;

        if (Physics.Raycast(transform.position, Vector3.down, 0.7f, GroundLayer) && velocity.y < 0)
        {
            if (InputVector.y > 0.5f)
            {
                Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.7f, Color.cyan, 10);
                needToJump = 0.25f;
            }
            else
                Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.7f, Color.blue, 10);
        }
        //print((InputVector.y > 0.5f) + " " + IsGrounded());
        if ((InputVector.y > 0.5f && (IsGrounded() || timeSincGrounded<0.1f)) || (needToJump > 0 && (IsGrounded() || timeSincGrounded < 0.1f)))
        {
            velocity.y = JumpPower * 2.5f;
        }
        else if(IsGrounded())
        {
            velocity = Vector3.zero;
        }
        else if(needToJump > 0 && !Physics.Raycast(transform.position, Vector3.down, 1.0f, GroundLayer))
        {
            needToJump = -1;
        }
        needToJump -= Time.deltaTime;

        velocity += (Physics.gravity / 50) * gravetyMultyplyer;

        if(!IsGrounded())
        {
            if (timeSincGrounded == 100)
                timeSincGrounded = 0;
            timeSincGrounded += Time.deltaTime;
        }
        else
        {
            timeSincGrounded = 100;
        }


        if (Input.GetMouseButtonDown(0))
        {

        }
    }
    public bool IsGrounded()
    {
        //if (Physics.Raycast(transform.position, Vector3.down, 0.2f, GroundLayer))
        if (Physics.CheckSphere(transform.position + Vector3.up * 0.25f, 0.4f, GroundLayer))
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.3f, Color.green, 10);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.3f, Color.red, 10);

            if (needToJump < 0)
                needToJump = -1;


            return false;
        }
    }
    private void OnDrawGizmos()
    {
        if (IsGrounded())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.25f, 0.4f);
    }
}
