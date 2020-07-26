using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // can only activate one object from list at a time so 2 lists
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects1;
    public GameObject objectToPool1;
    public int amountToPool1;
    public List<GameObject> pooledObjects2;
    public GameObject objectToPool2;
    public int amountToPool2;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects1 = CreatePoolObjects(objectToPool1, amountToPool1);
        pooledObjects2 = CreatePoolObjects(objectToPool2, amountToPool2);
        
    }

    public GameObject GetPooledObject1(){
        return ReturnObject(pooledObjects1);
    }

    public GameObject GetPooledObject2(){
        return ReturnObject(pooledObjects2);
    }

    private List<GameObject> CreatePoolObjects(GameObject objectToPool, int amountToPool){
        List<GameObject> pooledObjects = new List<GameObject>();    // create temp list
        for (int i = 0; i < amountToPool1; i++){
            GameObject obj = (GameObject)Instantiate(objectToPool); // create object
            obj.SetActive(false);
            pooledObjects.Add(obj);                  // add to list
            obj.transform.SetParent(this.transform); // set as children of ObjectPooler
        }
        return pooledObjects;
    }

    private GameObject ReturnObject(List<GameObject> pooledObjects){
        // For as many objects as are in the pooledObjects list
        for (int i = 0; i < pooledObjects.Count; i++){
            // if the pooled objects is NOT active, return that object 
            if (!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        // otherwise, return null   
        return null;
    }
}
