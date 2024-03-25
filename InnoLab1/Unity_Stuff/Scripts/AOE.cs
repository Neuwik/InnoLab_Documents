using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AOE : Attack
{
    [SerializeField]
    private int radius = 5;
    [SerializeField]
    private float duration = 1;
    private bool isSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(radius, 0.1f, radius);
        spawnPoint = new Vector3(transform.position.x, 0.1f, transform.position.z);
        direction = new Vector3(target.x - spawnPoint.x, 0, target.z - spawnPoint.z);

        double distance = Math.Sqrt(Math.Pow(direction.x, 2) + Math.Pow(direction.z, 2));

        if(distance > range)
        {
            direction = direction.normalized;
            transform.position = spawnPoint + direction * range;
        }
        else
        {
            transform.position = new Vector3(target.x, 0.1f, target.z);
        }

        isSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isSpawned)
        {
            if (collision.gameObject.layer != 0 && collision.gameObject.layer != 3)
            {
                Health healthObject;
                if ((healthObject = collision.gameObject.GetComponent("Health") as Health) != null)
                {
                    Debug.Log("AOE hit " + collision.GetInstanceID());
                    healthObject.TakeDamage(damage);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
