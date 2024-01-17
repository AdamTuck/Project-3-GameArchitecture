using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableCube : MonoBehaviour, IPickable
{
    Rigidbody cubeRB;

    void Start()
    {
        cubeRB = GetComponent<Rigidbody>();
    }

    public void OnDropped()
    {
        cubeRB.useGravity = true;
        cubeRB.isKinematic = false;
        transform.SetParent(null);
    }

    public void OnPicked(Transform attachPoint)
    {
        transform.position = attachPoint.position;
        transform.rotation = attachPoint.rotation;
        transform.SetParent(attachPoint);

        cubeRB.useGravity = false;
        cubeRB.isKinematic = true;
    }
}
