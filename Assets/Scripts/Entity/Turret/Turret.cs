using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private TurretState currentState;

    [Header("Turret")]
    [SerializeField] public Transform rayEmitter;
    //[SerializeField] public float rotationAngle;
    //[SerializeField] public float viewRadius;

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
    }

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
