using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{

    [SerializeField]
    protected GameObject objectPrefab;

    //[SerializeField]
    //protected bool spawnRandonPoint;
    [SerializeField]
    protected Transform[] spawnPoints;

    [SerializeField]
    protected bool startOnAwake;
    [MinMax(0, 60, 1, 60)]
    public Vector2 timeBetweenSpawn = new Vector2(0, 1);

    [SerializeField]
    protected int  maxSize;

    protected List<ISpawnebleObject> objectPool = new List<ISpawnebleObject>();

   
    protected Vector3 GetSpawnPosition()
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomPoint].position;
    }

    protected ISpawnebleObject CreateObj()
    {
        GameObject newObj = Instantiate(objectPrefab);
        ISpawnebleObject spawnableObj = newObj.GetComponent<ISpawnebleObject>();
        if (spawnableObj != null)
        {
            objectPool.Add(spawnableObj);
            return spawnableObj;
        }
        return null;
    }

    protected ISpawnebleObject GetFromPool()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].IsActiveInHierarchy())
            {
                return objectPool[i];
            }
        }

        if (objectPool.Count < maxSize)
        {
            return CreateObj();
        }

        return null;

    }

    protected void SpawnObject()
    {
        ISpawnebleObject obj = GetFromPool();
        if (obj != null)
        {
            obj.SetStartPosition(GetSpawnPosition());
        }
    }

    protected IEnumerator SpawnObjectCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenSpawn.x, timeBetweenSpawn.y));
            SpawnObject();
        }
    }

    protected void Awake()
    {
        if (spawnPoints.Length == 0)
        {
            gameObject.SetActive(false);
        }

        if (startOnAwake)
        {
            StartCoroutine(SpawnObjectCoroutine());
        }
    }

}

public interface ISpawnebleObject
{
    bool IsActiveInHierarchy();
    void SetStartPosition(Vector3 position);

}
