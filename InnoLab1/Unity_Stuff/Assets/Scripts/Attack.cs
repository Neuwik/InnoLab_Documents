using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    public int range = 20;

    [SerializeField]
    public int damage = 1;

    public Vector3 target;

    protected Vector3 direction;
    protected Vector3 spawnPoint;
}
