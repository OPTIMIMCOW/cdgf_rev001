using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform ship;
    [SerializeField] Vector3 cameraDistance = new Vector3 (0f, 2f, -10f);
    [SerializeField] float cameraInertia = 0.3f;
    public Vector3 velocity = Vector3.one;

    Transform myTransform;
   
    private void Awake()
    {
        myTransform = transform; // This is the camera not the player
    }

    private void LateUpdate()
    {
        var toPosition = ship.position + (ship.rotation * cameraDistance);
        //var curPosition = Vector3.Lerp(myTransform.position, toPosition, cameraInertia * Time.deltaTime);
        var curPosition = Vector3.SmoothDamp(myTransform.position, toPosition, ref velocity, cameraInertia);
        myTransform.position = curPosition;

        var toRotation = Quaternion.LookRotation(ship.position - myTransform.position, ship.up);
        var curRotation = Quaternion.Slerp(myTransform.rotation, toRotation, cameraInertia * Time.deltaTime);
        //myTransform.rotation = curRotation;
        myTransform.LookAt(ship,ship.up);
    }
}
