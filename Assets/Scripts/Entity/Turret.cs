using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] Transform rayEmitter;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float viewRadius;

    [Header("Laser Line")]
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float laserLineLength;
    [SerializeField] private float idleLineWidth;
    [SerializeField] private float firingLineWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FireLaser();
    }

    private void FireLaser ()
    {
        Ray laserRay = new Ray(rayEmitter.position, rayEmitter.forward);

        if (Physics.SphereCast(rayEmitter.position, viewRadius, rayEmitter.forward, out RaycastHit hit, laserLineLength))
        {
            Debug.Log("Hit: " + hit);
            laserLine.SetPosition(1, new Vector3(0,0, Mathf.Min(laserLineLength, hit.distance)));
        }
        else
        {
            laserLine.SetPosition(1, new Vector3(0, 0, Mathf.Min(laserLineLength)));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rayEmitter.position, viewRadius);
        Gizmos.DrawWireSphere(rayEmitter.position + rayEmitter.forward * laserLineLength, viewRadius);

        Gizmos.DrawLine(rayEmitter.position, rayEmitter.position + rayEmitter.forward * laserLineLength);
    }
}
