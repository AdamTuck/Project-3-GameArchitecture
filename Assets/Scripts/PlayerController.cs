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
    [SerializeField] private float shootForce;
    [SerializeField] private Transform shootSpawnPoint;

    private CharacterController characterController;

    private float horizontal, vertical, mouseX, mouseY, camXRotation;
    private Vector3 playerVelocity;
    [SerializeField] private bool isGrounded;
    private float moveMultiplier = 1;

    // Start is called before the first frame update
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
        if (Input.GetButtonDown("Jump"))
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
}