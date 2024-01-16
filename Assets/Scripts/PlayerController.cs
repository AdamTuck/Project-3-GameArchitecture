using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private bool invertMouse;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance;

    [Header("Player Shooting")]
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Rigidbody rocketPrefab;
    [SerializeField] private float shootForce;
    [SerializeField] private Transform shootSpawnPoint;

    [Header("Player Interactions")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactionLayer;

    // References
    private CharacterController characterController;

    // Player Movement
    private float horizontal, vertical, mouseX, mouseY, camXRotation;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float moveMultiplier = 1;

    // Raycasting
    private RaycastHit raycastHit;
    private ISelectable selectable;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        GroundedCheck();
        PlayerJump();

        MovePlayer();
        RotatePlayer();

        ShootBullet();
        ShootRocket();

        Interact();
    }

    void GetInput ()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        moveMultiplier = Input.GetButton("Sprint") ? sprintMultiplier : 1;
    }

    void MovePlayer ()
    {
        characterController.Move((transform.forward * vertical + transform.right * horizontal)* moveSpeed * Time.deltaTime * moveMultiplier);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f;
        }

        playerVelocity.y += gravity * Time.deltaTime;

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void GroundedCheck ()
    {
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayerMask);
    }

    void RotatePlayer ()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * mouseX);

        camXRotation += mouseY * Time.deltaTime * turnSpeed * (invertMouse ? 1 : -1);
        camXRotation = Mathf.Clamp(camXRotation, -40, 40);

        cameraTransform.localRotation = Quaternion.Euler(camXRotation, 0, 0);
    }

    void PlayerJump ()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = jumpVelocity;
        }
    }

    void ShootBullet ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody bullet = Instantiate(bulletPrefab, shootSpawnPoint.position, shootSpawnPoint.rotation);
            bullet.AddForce(shootSpawnPoint.forward * shootForce);

            Destroy(bullet.gameObject, 5.0f);
        }
    }

    void ShootRocket()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Rigidbody rocket = Instantiate(rocketPrefab, shootSpawnPoint.position, shootSpawnPoint.rotation);
            rocket.AddForce(shootSpawnPoint.forward * shootForce);

            Destroy(rocket.gameObject, 5.0f);
        }
    }

    void Interact ()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out raycastHit, interactionDistance, interactionLayer))
        {
            selectable = raycastHit.transform.GetComponent<ISelectable>();

            if (selectable != null)
            {
                selectable.OnHoverEnter();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    selectable.OnSelect();
                }
            }
        }

        if (raycastHit.transform == null && selectable != null)
        {
            selectable.OnHoverExit();
            selectable = null;
        }
    }
}