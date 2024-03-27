using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : Attack
{
    [SerializeField]
    private int speed = 20;
    [SerializeField]
    private bool firstTargetOnly = true;
    [SerializeField]
    private bool flyThroughWalls = false;

    private float ttl;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;

        if (target.x == spawnPoint.x || target.z == spawnPoint.z)
        {
            direction = transform.forward;
        }
        else
        {
            direction = new Vector3(target.x - spawnPoint.x, 0 , target.z - spawnPoint.z);
            direction = direction.normalized;
        }

        ttl = range / speed;
    }

    // Update is called once per frame
    void Update()
    {
        ttl -= Time.deltaTime;

        if (ttl <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log($"{name} hit {collision.name}");
        if (collision.gameObject.layer != 0 && collision.gameObject.layer != 3)
        {
            Health healthObject;
            if ((healthObject = collision.gameObject.GetComponent("Health") as Health) != null)
            {
                //Debug.Log("Bullet hit " + collision.GetInstanceID());
                healthObject.TakeDamage(damage);
            }
            else
            {
                Destroy(collision.gameObject);
            }
            if (firstTargetOnly)
            {
                Destroy(gameObject);
            }
        }
        else if (!flyThroughWalls)
        {
            Destroy(gameObject);
        }
    }
}
