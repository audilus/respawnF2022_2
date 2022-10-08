using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Use()
    {
        // Template; implement in inherited classes
        Debug.Log("Interactable: (" + this.name + ") used! ");
    }
}
