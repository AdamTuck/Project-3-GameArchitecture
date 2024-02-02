using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInteractor : Interact
{
    [Header("Gun")]
    public MeshRenderer gunRenderer;
    public Color bulletColour;
    public Color rocketColour;

    [Header("Player Shooting")]
    public ObjectPool bulletPool;
    public ObjectPool rocketPool;
    [SerializeField] private PlayerMovementBehaviour playerMovementBehaviour;
    [SerializeField] private float shootForce;
    [SerializeField] private Transform shootSpawnPoint;

    private float finalShootVelocity;
    private iShootStrategy currentWeapon;

    public override void Interaction()
    {
        // Setting a default weapon
        if (currentWeapon == null)
            currentWeapon = new BulletShootStrategy(this);

        // Switching weapons
        if (playerInput.weapon1Pressed)
            currentWeapon = new BulletShootStrategy(this);
        if (playerInput.weapon2Pressed)
            currentWeapon = new RocketShootStrategy(this);
        if (playerInput.leftBtn)
            currentWeapon.Shoot();
    }

    /*void Shoot ()
    {
        finalShootVelocity = playerMovementBehaviour.GetForwardSpeed() + shootForce;

        PooledObject pooledBullet = bulletPool.GetPooledObject();
        pooledBullet.gameObject.SetActive(true);

        //Rigidbody bullet = Instantiate(pooledBullet, shootSpawnPoint.position, shootSpawnPoint.rotation);
        Rigidbody bullet = pooledBullet.GetComponent<Rigidbody>();
        bullet.transform.position = shootSpawnPoint.position;
        bullet.transform.rotation = shootSpawnPoint.rotation;

        bullet.velocity = shootSpawnPoint.forward * finalShootVelocity;
        //bullet.AddForce(shootSpawnPoint.forward * finalShootVelocity);
        //Destroy(bullet.gameObject, 5.0f);
        bulletPool.DestroyPooledObject(pooledBullet, 5.0f);
    }*/

    public Transform GetShootPoint ()
    {
        return shootSpawnPoint;
    }

    public float GetShootVelocity()
    {
        finalShootVelocity = playerMovementBehaviour.GetForwardSpeed() + shootForce;
        return finalShootVelocity;
    }
}