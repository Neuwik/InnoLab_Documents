using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    protected override bool Spawn(GameObject gameObject, Vector3 position)
    {
        Attack attack;
        if ((attack = gameObject.GetComponent("Attack") as Attack) != null)
        {
            Vector3 playerPostiotion = Player.transform.position;
            float distance = Vector3.Distance(position, playerPostiotion);
            if (distance <= attack.range)
            {
                attack.target = Player.transform.position;
                return base.Spawn(gameObject, position);
            }
            return false;
        }
        return base.Spawn(gameObject, position);
    }
}
