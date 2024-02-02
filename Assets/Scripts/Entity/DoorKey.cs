using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorKey : MonoBehaviour
{
    public UnityEvent OnKeyPicked;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Key touched by: " + other.gameObject.tag);

        if (other.gameObject.CompareTag("Player"))
        {
            OnKeyPicked?.Invoke();
            Destroy(gameObject);
        }
    }
}