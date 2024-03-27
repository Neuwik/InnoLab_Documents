using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerWeapon : Weapon
{
    [SerializeField]
    private string Name;

    private MouseTracker Mouse;

    [SerializeField]
    private int maxAmmo = 10;
    private int currentAmmo = 0;

    [SerializeField]
    private float reloadTime = 1;
    private float reloadTimer = 0;
    private bool reload = false;

    new void Start()
    {
        base.Start();
        Mouse = Player.GetComponent<MouseTracker>();
        currentAmmo = maxAmmo;
    }

    new void Update()
    {
        if (reload)
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0)
            {
                currentAmmo = maxAmmo;
                reload = false;
            }
        }
        else
        {
            base.Update();
        }
    }

    protected override bool Spawn(GameObject gameObject, Vector3 position)
    {
        if (reload)
        {
            return false;
        }
        if (currentAmmo <= 0)
        {
            Reload();
            return false;
        }
        currentAmmo--;
        Attack attack;
        if ((attack = gameObject.GetComponent("Attack") as Attack) != null)
        {
            attack.target = Mouse.mouseTarget;
            base.Spawn(gameObject, position);
        }
            
        return base.Spawn(gameObject, position);
    }

    public override void SetPlayer(GameObject player)
    {
        Player = player;
        Mouse = Player.GetComponent<MouseTracker>();
    }

    public void Enable(string button)
    {
        spawnInput = button;
        canSpawn = false;
    }
    
    public void Disable()
    {
        spawnInput = null;
        canSpawn = false;
        reload = false;
    }

    public void Reload()
    {
        if (reload)
        {
            return;
        }
        if (currentAmmo >= maxAmmo)
        {
            return;
        }
        reloadTimer = reloadTime;
        reload = true;
    }
}
