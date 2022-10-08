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



    private Holdable holdable = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (showDebug && holdable != null)
        {
            Gizmos.DrawSphere(holdable.gameObject.transform.position, 0.2f);
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
        if (holdable != null)
        {

            var _dir = (transform.position - holdable.transform.position).normalized;
            
            if (showDebug)
            {
                Debug.DrawLine(holdable.transform.position, holdable.transform.position + holdable.uprightVector, Color.green);
                Debug.DrawLine(holdable.transform.position, holdable.transform.position + _dir, Color.red);
            }


            holdable.transform.eulerAngles = new Vector3(_dir.x, 1, _dir.z);

            Vector3 x = transform.position + transform.forward * grabDist;
            Rigidbody rb = holdable.GetComponent<Rigidbody>();

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
                holdable.GetComponent<Rigidbody>().useGravity = false;
                holdable.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                holdable.GetComponent<Rigidbody>().MovePosition(Vector3.Slerp(holdable.transform.position, x, Time.deltaTime * grabStrength));
            }
            else
            {
                holdable.transform.position = Vector3.Slerp(holdable.transform.position, x, Time.deltaTime * grabStrength);
            }
        }
        if (Input.GetButtonDown("Fire1") && grabCooldownClock <= 0)
        {
            if (holdable != null)
            {
                Rigidbody rb = holdable.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = transform.forward * 10f;
                holdable.gameObject.layer = 0;
                holdable = null;
            }
        }
        if (Input.GetButtonDown("Use") && grabCooldownClock <= 0)
        {
            if (holdable != null)
            {
                Rigidbody rb = holdable.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.velocity = transform.parent.GetComponent<Rigidbody>().velocity;
                holdable.gameObject.layer = 0;
                holdable = null;
            }
            else if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDist))
            {
                if (showDebug)
                    Debug.DrawLine(transform.position, hit.point);

                if (showDebug)
                    Debug.Log(hit.transform.gameObject.name);

                if (hit.collider.tag == "Prop")
                {
                    Holdable p = hit.collider.GetComponent<Holdable>();

                    holdable = p;
                    holdable.gameObject.layer = 2;

                }
                
                
            }
            grabCooldownClock = grabCooldownTime;
        }

        if (Input.GetButtonDown("Use") && interactionClock <= 0)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDist))
            {
                if (hit.collider.isTrigger)
                {
                    hit.collider.SendMessageUpwards("Use"); //TODO: Triggers
                }

            }

        }

        grabCooldownClock -= Time.deltaTime;
        interactionClock -= Time.deltaTime;
    }
}
