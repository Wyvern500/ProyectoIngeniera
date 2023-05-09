using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private float timer;
    private float shootingDelay = 1f;
    public float range = 200f;
    public Canvas canvas;
    public GameObject estadisticas;
    public TextMeshProUGUI targets;
    public GameObject targetManager;

    private int tirosTotales;
    private int tirosAcertados;
    private int tirosFallados;

    public GameObject menu; // Assign in inspector
    private bool isShowing;

    private AudioSource audioSource;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    TextMeshProUGUI[] labels;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menu.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();
        //labels = canvas.transform.GetComponents<TextMeshProUGUI>();
        //Debug.Log(labels.Length);
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            isShowing = !isShowing;
            menu.SetActive(isShowing);
            estadisticas.GetComponent<EstadisticasScript>().setData(tirosAcertados, tirosFallados, tirosTotales);
            if(isShowing) 
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                targetManager.GetComponent<targetSpawner>().getTargetManager().pause();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                targetManager.GetComponent<targetSpawner>().getTargetManager().resume();
            }
            
        }
        if (timer < shootingDelay)
        {
            timer += Time.deltaTime;
            if(timer > shootingDelay)
            {
                timer = shootingDelay;
            }
        }
        // Shoot

        if (Input.GetMouseButton(0) && timer >= shootingDelay)
        {
            shoot();
        }

        // Misc

        Transform t = playerCamera.transform;
        Vector3 look = playerCamera.transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(t.position, look, Color.green, 14);

        // Create a vector at the center of our camera's viewport
        /*Vector3 lineOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        // Draw a line in the Scene View  from the point lineOrigin 
        // in the direction of fpsCam.transform.forward * weaponRange, using the color green
        Debug.DrawRay(lineOrigin, playerCamera.transform.forward * Range, Color.green);*/
        /*if (Physics.Raycast(t.position, look, out $$anonymous$$t, weaponlength, layerMask))
        {
            
        }*/
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void shoot()
    {
        tirosTotales++;
        timer = 0;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        int layerMask = LayerMask.GetMask("Target");
        audioSource.Play(0);

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            if(canvas != null)
            {
                //Component[] components = canvas.gameObject.GetComponents(typeof(Text));
                //Debug.Log(canvas.gameObject.GetComponent<Text>());
                //Debug.Log(GameObject.FindGameObjectWithTag("Text1").GetComponent<Text>().text);
                /*List<GameObject> canvasChilds = GetChildren(canvas.gameObject);
                foreach (GameObject go in canvasChilds)
                {
                    Debug.Log(go.GetComponent<Text>().text);
                }*/
                tirosAcertados++;
                hit.collider.gameObject.GetComponent<TargetScript>().kill();
                //Destroy(hit.collider.gameObject);
                //Debug.Log(targets.text);
            }
            Debug.Log("hit " + hit.collider.gameObject);
        }
        else
        {
            tirosFallados++;
        }
    }

    public List<GameObject> GetChildren(GameObject go)
    {
        List<GameObject> list = new List<GameObject>();
        return GetChildrenHelper(go, list);
    }

    private List<GameObject> GetChildrenHelper(GameObject go, List<GameObject> list)
    {
        if (go == null || go.transform.childCount == 0)
        {
            return list;
        }
        foreach (Transform t in go.transform)
        {
            list.Add(t.gameObject);
            GetChildrenHelper(t.gameObject, list);
        }
        return list;
    }
}
