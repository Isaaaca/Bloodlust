using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn = null;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float timerOffset = 0f;

    private float timer;
    private List<GameObject> objectPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval + timerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Spawn();
            timer = spawnInterval;
        }
    }

    private void Spawn()
    {
        int availableObect = -1;
        //check for available object in pool
        for (int i= 0; i<objectPool.Count && availableObect ==-1; i++)
        {
            if (!objectPool[i].activeSelf) availableObect = i;
        }

        GameObject objToSpawn = null;

        if (availableObect == -1)
        {
            objToSpawn = Instantiate(objectToSpawn, transform);
            objectPool.Add(objToSpawn);
        }
        else
        {
            objToSpawn = objectPool[availableObect];
        }

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = transform.position;
    }
}
