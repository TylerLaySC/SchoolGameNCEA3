using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopDownCarController : MonoBehaviour
{
    //public 

    // UI

    [SerializeField] private Text SpeedText;

    [Header("Car Settings")]                    // to edit within unity instead of here
    public float accelerationFactor = 30.0f;    // how fast the car will accelorate

    public float turnFactor = 3.5f;             // how fast the car will turn
    public float allowTurnFactor = 8.0f;        // how fast you have to be going to turn

    public float dragFactor = 2.0f;             // changes how quickly it slows down when you stop accelerating

    public float maxSpeed = 11.625f;              // maximum speed
    public float maxSpeedMultiplier = 5.6f;    // max speed multiplier
    

    // mins and maxes

    public float driftFactor = 0.92f;           // how much the car will drift
    float driftMinValue = 0.93f;                // minimum float value for drifting (this has to be the same as "driftFactor" as the clamp extension for Mathf needs 2 arguments)
    float driftMaxValue = 0.97f;                // maximum float value for drifting capibilities when using handbrake



    //Local variables

    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    

    //components
    Rigidbody2D carRigidbody2D;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //when pressing space it should make the car slide more, like using a handbrake
        if (Input.GetKey(KeyCode.Space))
        {
            driftFactor += Time.deltaTime;
            driftFactor = Mathf.Clamp(driftFactor, driftMinValue, driftMaxValue);
        }
        else
        {
            driftFactor -= Time.deltaTime;
            driftFactor = Mathf.Clamp(driftFactor, driftMinValue, driftMaxValue);
        }

        float kmph = GetComponent<Rigidbody2D>().velocity.magnitude * 9f;
        //Debug.Log(Mathf.Abs(GetLateralVelocity()));


        // Make the kmph show on the ui speedometer
        SpeedText.text = kmph.ToString("0");

    }

    // Frame-rate independent for physics calculations
    void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();

    }

    void ApplyEngineForce()
    {
        //calculates how much "forward" we are going in terms of the direction of the velocity
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        //slow the car down when using handbrake
        if (Input.GetKey(KeyCode.Space))
        {
            maxSpeed -= Time.deltaTime;
            maxSpeed = Mathf.Clamp(maxSpeed, maxSpeedMultiplier, 0);
        }
        else
        {
            maxSpeed = 11.625f;
        }
      

        //maximum speed calculations going forward (if speed is above max it will stop applying force)
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;


        //reverse max speed (half of forward max speed)
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        //max speed sideways
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        //adds drag if not accelerating
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, dragFactor, Time.fixedDeltaTime * 3);
        else carRigidbody2D.drag = 0;

        //makes the force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //applies the force made above
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);

        //driftFactor -= Time.deltaTime;
        //driftFactor = Mathf.Clamp(driftFactor, driftMinValue, driftMaxValue);

    }

    void ApplySteering()
    {
        //makes it so you cant turn when completely still (stops tank turning)
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.velocity.magnitude / allowTurnFactor);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //updates the rotation angle based on inputyeah
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        //applies steering by rotation the car
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()                       //orthogonal velocity is side velocity of the car (affects drifting)
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up); // calculates forward velocity
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right); //calculates right velocity

        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;

    }

    float GetLateralVelocity()
    {
        //returns how fast the car is moving sideways
        return Vector2.Dot(transform.right, carRigidbody2D.velocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        //see if moving forward and if the brakes are being pressed - if so tires should be screeching
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        //if drifting the tires should also be screeching
        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
            return true;

        return false;
        
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }


}
