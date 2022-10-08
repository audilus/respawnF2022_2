using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IChangeMat : Interactable
{
    public Material newMat;
    public GameObject otherObject;

    private MeshRenderer otherMeshRenderer;
    private Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        otherMeshRenderer = otherObject.GetComponent<MeshRenderer>();
        mat = otherMeshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new void Use()
    {
        if (otherMeshRenderer.material == mat)
        {
            otherMeshRenderer.material = newMat;
        }
        else
        {
            otherMeshRenderer.material = mat;
        }

        
    }
}
