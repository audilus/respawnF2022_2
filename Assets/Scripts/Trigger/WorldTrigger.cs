using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldTrigger : MonoBehaviour
{
    private Collider trigger;
    private void Awake()
    {
        if (!trigger.isTrigger)
        {
            trigger.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
