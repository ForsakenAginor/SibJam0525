using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSpace.AI;
using NSpace;

public class Atack : IState
{
    float fireRate;
    IWeapon weapon;
    NPCController host;
    float lastShotTime;

    public Atack(NPCController host, float fireRate, IWeapon weapon)
    {

        this.fireRate = fireRate;
        this.weapon = weapon;
        this.host = host;
    }

    public bool complete { get; private set; }
    public void OnEnter()
    {
        complete = false;
        host.Stop();
        host.GetWeapon(true);
        lastShotTime = Time.time;

    }

    public void OnExit()
    {
        host.GetWeapon(false);
    }

    public void OnUpdate()
    {
        if (host.enemies.Count <= 0 || weapon==null) { complete = true; return; }
        if (Time.time - lastShotTime < fireRate)
        {
            host.AimAt(host.enemies[0].entity.spotPoints[1].position);
        }
        else
        {
            weapon.Fire();
            lastShotTime=Time.time;

        }
    }
}
