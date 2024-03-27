using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameObjectSpawner
{
    protected GameObject Player;

    protected void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    public virtual void SetPlayer(GameObject player)
    {
        Player = player;
    }
}
