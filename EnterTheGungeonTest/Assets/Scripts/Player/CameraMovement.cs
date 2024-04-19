using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference aimInputAction;
    [SerializeField] private float maxDistanceFromTargetY;
    [SerializeField] private float maxDistanceFromTargetX;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float directFollowDist = 0.05f;

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
        if(dist < directFollowDist)
		{
            smoothT = 0.01f;
        }
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetLocation.x, targetLocation.y, transform.position.z),
            ref velocity, smoothT);
    }
}