using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float acceleration = 10f;
    public float maxFloorVelocity = 90f;
    public float maxAirVelocity = 30f;
    public float floorVelocityDampening = 0.7f;
    public float jumpPower = 4f;
    public float gravityMult = 4f;
    public float airAccel = 0.1f;
    public float airTimeLimit = 2f;
    public float jumpSecondDelay = 0.12f;

    public float shakeAmt;
    public float shakeThreshold = 25f;

    new private Rigidbody rigidbody;
    private bool isGrounded = true;
    private float jumpClock = 0;
    private float airTime = 0;
    private float shakeClock = 0;
    //private CameraShake cameraShake;

    private void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 300, 30), "Player Velocity: " + rigidbody.velocity.ToString());
        GUI.Label(new Rect(25, 50, 300, 30), "Player Position: " + transform.position.ToString());
    }


    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        Cursor.visible = false;
        //cameraShake = GetComponentInChildren<CameraShake>();
    }

    // Fixed update is called every physics tick (locked at 60Hz), so I don't need to multiply by time.deltatime here
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.magnitude > shakeThreshold)
        {
            shakeClock = shakeAmt;
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}


        //cameraShake.Shake(Mathf.Max(0, shakeClock -= Time.deltaTime * shakeAmt * 15));
        // Create a new vector with the velocity of the previous frame.
        Vector3 final = rigidbody.velocity;

        //If the player is on the ground, multiply the X and Z velocity by a number less than 1 to slow them down over time.
        if(isGrounded){
            final = new Vector3(rigidbody.velocity.x * floorVelocityDampening, rigidbody.velocity.y, rigidbody.velocity.z * floorVelocityDampening);
        }
        
        //If the player's previous velocity isn't higher than the speed limit and the player is on the floor, move based on the input.
        if ((final.magnitude <= maxFloorVelocity) && isGrounded)
        {
            final += Input.GetAxisRaw("Vertical") * transform.forward * acceleration;
            final += Input.GetAxisRaw("Horizontal") * transform.right * acceleration;
        }

        //air control code.

         else if ((final.magnitude <= maxAirVelocity) && airTime > 0)
        {
            final += Input.GetAxis("Vertical") * transform.forward * (airAccel * airTime + 1);
            final += Input.GetAxis("Horizontal") * transform.right * (airAccel * airTime);
        }

        airTime -= Time.deltaTime;


        final += Vector3.down * gravityMult * Time.deltaTime; //Add extra gravity (multiply the "down" direction by gravity multiplier)
        rigidbody.velocity = final;                           //Set the velocity to the processed output.

        //Decrement the clock in real time by how long the previous frame took to render.
        jumpClock -= Time.deltaTime;

        //If we press the jump button, the player is on the ground, and the jump timer is already reset, add a physics impulse (jump!).
        if (Input.GetButton("Jump") && isGrounded && (jumpClock <= 0))
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpClock = jumpSecondDelay; //Reset the delay after a jump
        }
        else
        {   //If one of the above isn't true, check if the ground is actually below the player.
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
            {
                if (hit.collider.enabled){
                    isGrounded = true;
                    airTime = airTimeLimit;
                }   
                else{
                    isGrounded = false;
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        Debug.Log(isGrounded);
    }
}
