using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private TurretState currentState;

    [Header("Turret")]
    [SerializeField] public Transform rayEmitter;
    [SerializeField] public float rotationAngle;
    [SerializeField] public float viewRadius;

    [Header("Laser Line")]
    [SerializeField] public LineRenderer laserLine;
    [SerializeField] public float laserLineLength;
    [SerializeField] public float idleLineWidth;
    [SerializeField] public float firingLineWidth;

    [HideInInspector] public Transform hitTarget;

    void Start()
    {
        laserLine.startWidth = idleLineWidth;
        laserLine.endWidth = idleLineWidth;

        currentState = new TurretIdleState(this);
        currentState.OnStateEnter();
    }

    void Update()
    {
        currentState.OnStateUpdate();
        //FireLaser();
    }

    //private void FireLaser ()
    //{
    //    laserLine.SetPosition(0, rayEmitter.transform.position);

    //    if (Physics.Raycast(rayEmitter.transform.position, rayEmitter.transform.forward, out RaycastHit hit, laserLineLength))
    //    {
    //        laserLine.SetPosition(1, hit.point);

    //        laserLine.startWidth = firingLineWidth;
    //        laserLine.endWidth = firingLineWidth;

    //        if (hit.transform.gameObject.CompareTag("Player"))
    //        {
    //            player = hit.transform;
    //        }
    //    }
    //    else
    //    {
    //        laserLine.SetPosition(1, rayEmitter.position + rayEmitter.forward * laserLineLength);

    //        laserLine.startWidth = idleLineWidth;
    //        laserLine.endWidth = idleLineWidth;

    //        player = null;
    //    }
    //}

    public void ChangeState(TurretState state)
    {
        currentState.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(rayEmitter.position, rayEmitter.position + rayEmitter.transform.forward * laserLineLength);
    }
}
