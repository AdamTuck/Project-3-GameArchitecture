using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBehaviour : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Player Rotation")]
    [SerializeField] private float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = PlayerInput.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
    }

    void RotatePlayer ()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime * playerInput.mouseX);
    }
}
