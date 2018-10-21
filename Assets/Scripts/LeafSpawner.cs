using UnityEngine;

public class LeafSpawner : MonoBehaviour
{

    [SerializeField]
    protected GameObject objectPrefab;


    public void Spawn()
    {
        GameObject newObj = Instantiate(objectPrefab);
        newObj.transform.position = Vector3.zero;
    }

}
