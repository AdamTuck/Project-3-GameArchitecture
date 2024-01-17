using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer doorRenderer;
    [SerializeField] private Material defaultDoorMaterial, detectedDoorMaterial;
    [SerializeField] private Animator doorAnimator;

    private bool isLocked = true;

    //private float doorOpenDelayTimer = 0;
    //private const float doorOpenWaitTime = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocked && other.gameObject.CompareTag("Player"))
        {
            //doorOpenDelayTimer = 0;
            doorRenderer.material = detectedDoorMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isLocked)
            return;

        if (!other.gameObject.CompareTag("Player"))
            return;

        doorAnimator.SetBool("Door", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //doorOpenDelayTimer = 0;
            ShutDoor();
        }
    }

    private void ShutDoor()
    {
        doorRenderer.material = defaultDoorMaterial;
        doorAnimator.SetBool("Door", false);
    }

    public void UnlockDoor ()
    {
        isLocked = false;
    }

    public void LockDoor ()
    {
        isLocked = true;
        ShutDoor();
    }
}
