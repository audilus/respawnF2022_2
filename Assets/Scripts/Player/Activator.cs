using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public float grabDist = 1.5f;
    public float grabStrength = 10f;
    public float rotateStrength = 90f;
    public bool showDebug = false;

    public float grabCooldownTime = 0.5f;
    private float grabCooldownClock = 0;

    public float interactionCooldownTime = 0.2f;
    private float interactionClock = 0;



    private Interactable interactable = null;

    public float triggerInteractDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (showDebug && interactable != null)
        {
            Gizmos.DrawSphere(interactable.gameObject.transform.position, 0.2f);
        }

        if (showDebug)
        {
            if (Physics.Raycast(transform.position, transform.forward * grabDist, out RaycastHit hit, grabDist))
            {
                if (hit.distance < grabDist)
                {
                    Gizmos.DrawWireSphere(hit.point, 0.2f);
                }
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position + transform.forward * grabDist, 0.2f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable != null)
        {

            var _dir = (transform.position - interactable.transform.position).normalized;
            
            if (showDebug)
            {
                Debug.DrawLine(interactable.transform.position, interactable.transform.position + Vector3.up, Color.green);
                Debug.DrawLine(interactable.transform.position, interactable.transform.position + _dir, Color.red);
            }


            interactable.transform.eulerAngles = new Vector3(_dir.x, 1, _dir.z);

            Vector3 x = transform.position + transform.forward * grabDist;
            Rigidbody rb = interactable.GetComponent<Rigidbody>();
            //holdable.transform.position = Vector3.Slerp(holdable.transform.position, transform.position + transform.forward * grabDist, Time.deltaTime * grabStrength);
            if (Physics.Raycast(transform.position, transform.forward * grabDist, out RaycastHit hit, grabDist))
            {
                if (hit.distance < grabDist)
                {
                    x = hit.point;
                }
            }

            if (rb != null)
            {
                interactable.GetComponent<Rigidbody>().useGravity = false;
                interactable.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                interactable.GetComponent<Rigidbody>().MovePosition(Vector3.Slerp(interactable.transform.position, x, Time.deltaTime * grabStrength));
            }
            else
            {
                interactable.transform.position = Vector3.Slerp(interactable.transform.position, x, Time.deltaTime * grabStrength);
            }
        }
        if (Input.GetButtonDown("Fire1") && grabCooldownClock <= 0)
        {
            if (interactable != null)
            {
                Rigidbody rb = interactable.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = transform.forward * 10f;
                interactable.gameObject.layer = 0;
                interactable = null;
            }
        }
        if (Input.GetButtonDown("Use") && grabCooldownClock <= 0)
        {
            if (interactable != null)
            {
                Rigidbody rb = interactable.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = transform.parent.GetComponent<Rigidbody>().velocity;
                interactable.gameObject.layer = 0;
                interactable.SendMessage("UnUse");
                interactable = null;

            }
            else if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDist))
            {
                if (showDebug)
                    Debug.DrawLine(transform.position, hit.point);

                if (showDebug)
                    Debug.Log(hit.transform.gameObject.name);

                if (hit.collider.tag == "Prop")
                {
                    Interactable p = hit.collider.GetComponent<Interactable>();
                    Rigidbody rb = p.GetComponent<Rigidbody>();

                    rb.interpolation = RigidbodyInterpolation.None;

                    interactable = p;
                    interactable.gameObject.layer = 2;

                }
                
                
            }
            grabCooldownClock = grabCooldownTime;
        }

        if (Input.GetButtonDown("Use") && interactionClock <= 0)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, triggerInteractDistance))
            {
                if (hit.collider.isTrigger)
                {
                    //if (hit.collider.GetComponent<Interactable>())
                    hit.collider.SendMessageUpwards("Use"); //TODO: Triggers
                }
            }
            interactionClock = interactionCooldownTime;
        }

        grabCooldownClock -= Time.deltaTime;
        interactionClock -= Time.deltaTime;
    }
}
