using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] Thruster[] thruster;

    [Tooltip("A multiplier to the input. Describes the maximum speed in degrees / second. To flip vertical rotation, set Y to a negative value")]
    [SerializeField] private Vector2 sensitivity = new Vector2(1000,1000);
    [Tooltip("The rotation acceleration, in degrees / second")]
    [SerializeField] private Vector2 acceleration = new Vector2(1000, 1000);
    [Tooltip("The maximum angle from the horizon the player can rotate, in degrees")]
    [SerializeField] private float maxVerticalAngleFromHorizon;
    [Tooltip("The period to wait until resetting the input value. Set this as low as possible, without encountering stuttering")]
    [SerializeField] private float inputLagPeriod;

    Transform myTransform;
    private Vector2 movementVector; // The current rotation velocity, in degrees
    private Vector2 thisFrameRotation; // The current rotation, in degrees
    private Vector2 lastInputEvent; // The last received non-zero input value
    private float inputLagTimer; // The time since the last received non-zero input value

    private Vector3 desiredRotation;
    private void Awake()
    {
        myTransform = transform;

        desiredRotation.x = transform.rotation.x + 50f;
        desiredRotation.y = transform.rotation.y;
        desiredRotation.z = transform.rotation.z;
    }

    void Update()
    {
        Thrust();
        Rotations();
    }

    //private void OnEnable()
    //{
    //    movementVector = Vector2.zero;
    //    inputLagTimer = 0;
    //    lastInputEvent = Vector2.zero;

    //    // Calculate the current rotation by getting the gameObject's local euler angles
    //    Vector3 euler = transform.localEulerAngles;
    //    // Euler angles range from [0, 360), but we want [-180, 180)
    //    if (euler.x >= 180)
    //    {
    //        euler.x -= 360;
    //    }

    //    transform.localEulerAngles = euler;
    //    // Rotation is stored as (horizontal, vertical), which corresponds to the euler angles
    //    // around the y (up) axis and the x (right) axis
    //    thisFrameRotation = new Vector2(euler.y, euler.x);
    //}

    void Rotations()
    {
        var wantedVelocity = GetInput() * sensitivity;
        //Debug.Log($"wantedVelocity: {wantedVelocity}");
        movementVector = wantedVelocity;

        //Debug.Log($"wanted velocity: {wantedVelocity}");

        // I dont think we want to use MoveTowards, we want to rotate and keep rotating. 
        //movementVector = new Vector2(
        //    Mathf.MoveTowards(movementVector.x, wantedVelocity.x, acceleration.x * Time.deltaTime),
        //    Mathf.MoveTowards(movementVector.y, wantedVelocity.y, acceleration.y * Time.deltaTime));


        thisFrameRotation += movementVector * Time.deltaTime;

        //float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        float pitch = turnSpeed * Time.deltaTime * Input.GetAxis("Pitch");
        float roll = turnSpeed * Time.deltaTime * Input.GetAxis("Roll");
        //myTransform.Rotate(pitch, yaw, -roll);

        //var final = new Vector3(0, myTransform.localEulerAngles.x + 1, 0);

        //var final = new Vector3(transform.localEulerAngles.x + 0.1f,0, 0);
        Debug.Log($"after: {transform.localEulerAngles.x}");

        //transform.localEulerAngles = final;



        transform.Rotate(desiredRotation * Time.deltaTime);

        //var currentX = transform.localEulerAngles.x;
        //float newX = currentX + 0.1f;
        //if (currentX > 90f)
        //{
        //    newX = (newX * -1f) +360f;
        //}

        //transform.localEulerAngles = new Vector3(newX, 0, 0);


    }
    void Thrust()
    {
        // Activate and deactivate trail rendereer. 
        if (Input.GetAxis("Vertical") > 0)
        {
            myTransform.position += myTransform.forward * movementSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        }
    }

    private Vector2 GetInput()
    {// TODO extend to include roll rotation

        // Add to the lag timer
        inputLagTimer += Time.deltaTime;

        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        if ((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || inputLagTimer >= inputLagPeriod)
        {
            lastInputEvent = input;
            inputLagTimer = 0;
        }
        return lastInputEvent; // dont like this being a global variable, TODO look into changing to be just here and only the timeout is used. 
    }
}
