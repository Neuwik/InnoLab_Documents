using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
internal class SpawnTableEntry
{
    [SerializeField]
    internal GameObject gameObject;
    [SerializeField]
    internal int spawnCount = -1;
    [SerializeField]
    internal bool useStandardSpawnOffset = true;
    [SerializeField]
    internal Vector3 spawnOffset = Vector3.zero;
}

public class GameObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private bool spawnRandomEntry = true;
    [SerializeField]
    private bool autoSpawn = true;
    [SerializeField]
    protected string spawnInput;
    protected bool canSpawn = false;
    [SerializeField]
    protected bool canSpawnMultiple = true;
    private GameObject currentSpawn = null;
    [SerializeField]
    private float spawnInterval = 1;
    private float spawnCooldown = 0;
    [SerializeField]
    private Vector3 standardSpawnOffset = Vector3.zero;
    [SerializeField]
    private List<SpawnTableEntry> SpawnTable;

    // Update is called once per frame
    public void Update()
    {
        canSpawn = autoSpawn;

        if (!canSpawnMultiple)
        {
            canSpawn = currentSpawn == null;
        }

        if (!autoSpawn)
        {
            if (spawnInput != null && spawnInput != "")
            {
                canSpawn = Input.GetAxis(spawnInput) > 0;
            }
        }

        if(spawnCooldown > 0)
        {
            spawnCooldown -= Time.deltaTime;
        }
        else if (canSpawn && SpawnTable.Count > 0)
        {
            SpawnTableEntry spawnTableEntry;
            if (spawnRandomEntry)
            {
                spawnTableEntry = getRandomSpawnTableEntry();
            }
            else
            {
                spawnTableEntry = getFirstSpawnTableEntry();
            }

            if(spawnTableEntry != null)
            {
                Vector3 position = transform.position;
                if (!spawnTableEntry.useStandardSpawnOffset)
                {
                    position += spawnTableEntry.spawnOffset;
                }
                else
                {
                    position += standardSpawnOffset;
                }
                if(Spawn(spawnTableEntry.gameObject, position))
                {
                    spawnCooldown = spawnInterval;
                }
            }
        }
    }

    protected virtual bool Spawn(GameObject gameObject, Vector3 position)
    {
        currentSpawn = Instantiate(gameObject, position, Quaternion.identity);
        return true;
    }

    private SpawnTableEntry getRandomSpawnTableEntry()
    {
        if (SpawnTable.Count <= 0)
        {
            return null;
        }

        int rnd = UnityEngine.Random.Range(0, SpawnTable.Count);
        while (SpawnTable[rnd].spawnCount == 0)
        {
            SpawnTable.RemoveAt(rnd);

            if (SpawnTable.Count == 0)
            {
                return null;
            }

            rnd = UnityEngine.Random.Range(0, SpawnTable.Count);
        }

        SpawnTableEntry spawnTableEntry = SpawnTable[rnd];

        if (SpawnTable[rnd].spawnCount > 0)
        {
            SpawnTable[rnd].spawnCount -= 1;
            if (SpawnTable[rnd].spawnCount == 0)
            {
                SpawnTable.RemoveAt(rnd);
            }
        }

        return spawnTableEntry;
    }

    private SpawnTableEntry getFirstSpawnTableEntry()
    {
        if (SpawnTable.Count == 0)
        {
            return null;
        }

        while (SpawnTable[0].spawnCount == 0)
        {
            SpawnTable.RemoveAt(0);

            if (SpawnTable.Count == 0)
            {
                return null;
            }
        }

        SpawnTableEntry spawnTableEntry = SpawnTable[0];

        if (SpawnTable[0].spawnCount > 0)
        {
            SpawnTable[0].spawnCount -= 1;
            if (SpawnTable[0].spawnCount == 0)
            {
                SpawnTable.RemoveAt(0);
            }
        }

        return spawnTableEntry;
    }
}
