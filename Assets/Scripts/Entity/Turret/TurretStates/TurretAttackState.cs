using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackState : TurretState
{
    float distanceToPlayer;

    Health playerHealth;
    float damagePerSecond = 20f;

    public TurretAttackState(Turret _turret) : base(_turret)
    {
        playerHealth = _turret.hitTarget.GetComponentInParent<Health>();
    }

    public override void OnStateEnter()
    {
        Debug.Log("Turret Attack State Enter");
    }

    public override void OnStateExit()
    {
        Debug.Log("Turret Attack State Exit");
    }

    public override void OnStateUpdate()
    {
        if (Physics.Raycast(turret.rayEmitter.transform.position, turret.rayEmitter.transform.forward, out RaycastHit hit, turret.laserLineLength))
        {
            turret.hitTarget = hit.transform;

            turret.laserLine.SetPosition(1, hit.point);

            turret.laserLine.startWidth = turret.firingLineWidth;
            turret.laserLine.endWidth = turret.firingLineWidth;

            if (hit.transform.gameObject.CompareTag("Player"))
            {
                Debug.Log("Reducing health: " + damagePerSecond * Time.deltaTime);
                playerHealth.DeductHealth(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            turret.ChangeState(new TurretIdleState (turret));
        }
    }
}
