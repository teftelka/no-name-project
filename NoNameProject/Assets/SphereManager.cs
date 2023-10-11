using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    private int counter = 0;
    public Transform sphereObject;

    [SerializeField] private GameObject[] spawnPoints;

    private void Start()
    {
        SphereInstantiate();
    }

    private void SphereInstantiate()
    {
        if (counter < 1)
        {
            foreach (var spawnPoint in spawnPoints)
            {
                Instantiate(sphereObject, spawnPoint.transform.position, Quaternion.identity);
            }
        }
        counter++;
    }
    
}
