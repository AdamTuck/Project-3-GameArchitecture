using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdleState : TurretState
{
    public TurretIdleState(Turret _turret) : base(_turret)
    {
        
    }

    public override void OnStateEnter()
    {
        Debug.Log("Turret Idle State Enter");
    }

    public override void OnStateExit()
    {
        Debug.Log("Turret Idle State Exit");
    }

    public override void OnStateUpdate()
    {
        turret.laserLine.SetPosition(0, turret.rayEmitter.transform.position);

        if (Physics.Raycast(turret.rayEmitter.transform.position, turret.rayEmitter.transform.forward, out RaycastHit hit, turret.laserLineLength))
        {
            turret.hitTarget = hit.transform;
            turret.ChangeState(new TurretAttackState(turret));
        }
        else
        {
            turret.laserLine.SetPosition(1, turret.rayEmitter.position + turret.rayEmitter.forward * turret.laserLineLength);

            turret.laserLine.startWidth = turret.idleLineWidth;
            turret.laserLine.endWidth = turret.idleLineWidth;
        }
    }
}
