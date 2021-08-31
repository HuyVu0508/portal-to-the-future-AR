using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Transform playerPortal;
    public Transform cityViewPortal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Match cameras positions
        Vector3 playerOffsetFromPortal = playerCamera.position - playerPortal.position;
        transform.position = cityViewPortal.position + playerOffsetFromPortal;

        // Match cameras rotations
        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(playerPortal.rotation, cityViewPortal.rotation);
        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
