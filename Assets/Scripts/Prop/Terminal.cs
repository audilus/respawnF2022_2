using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : Interactable
{
    [SerializeField]
    private Transform[] camPositions;
    private Vector3 tempCamPos;
    private Quaternion tempCamRot;

    private Camera cam;
    private bool isUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        camPositions = new Transform[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var g = gameObject.transform.GetChild(i);
            if (g.CompareTag("camPos"))
                camPositions[i] = g;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed)
        {
            cam.transform.SetPositionAndRotation(Vector3.Lerp(cam.transform.position, tempCamPos, Time.deltaTime * 2f), 
                Quaternion.Lerp(cam.transform.rotation, tempCamRot, Time.deltaTime * 2f));
        }
        else
        {
            cam.transform.SetPositionAndRotation(Vector3.Lerp(tempCamPos, cam.transform.position, Time.deltaTime * 2f),
                Quaternion.Lerp(tempCamRot, cam.transform.rotation, Time.deltaTime * 2f));
        }
    }

    new void Use()
    {
        cam = Camera.main;
        tempCamPos = cam.transform.position;
        tempCamRot = cam.transform.rotation;
        isUsed = true;
    }

    new void UnUse()
    {
        cam = null;
        cam.transform.position = tempCamPos;
        cam.transform.rotation = tempCamRot;
        isUsed = false;
    }
}
