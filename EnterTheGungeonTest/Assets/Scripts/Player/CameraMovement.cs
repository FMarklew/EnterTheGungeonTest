using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// handles camera movement for game. The system responds to virtual stick movement within specified horizontal and vertical limits
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference aimInputAction;
    [SerializeField] private float maxDistanceFromTargetY;
    [SerializeField] private float maxDistanceFromTargetX;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float directFollowMinDist = 0.05f; // minimum distance for camera to smooth less ie. follow more directly

    private Transform followTarget;
    private Vector3 targetLocation;
    private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
        followTarget = GameObject.Find("Player").transform;
        transform.position = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
	}

    void Update()
    {
        Vector2 moveDirection = aimInputAction.action.ReadValue<Vector2>().normalized;

        targetLocation = followTarget.position + new Vector3(moveDirection.x * maxDistanceFromTargetX, moveDirection.y * maxDistanceFromTargetY, 0);

        float dist = Vector2.Distance(targetLocation, transform.position);

        float smoothT = smoothTime;
        if(dist < directFollowMinDist)
		{
            smoothT = 0.01f;
        }
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetLocation.x, targetLocation.y, transform.position.z),
            ref velocity, smoothT);
    }
}