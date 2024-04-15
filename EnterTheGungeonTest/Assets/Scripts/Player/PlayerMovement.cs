using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private InputActionReference movementInputAction;

	[SerializeField] private Rigidbody2D rb;
	void Update()
	{
		Vector2 moveDir = movementInputAction.action.ReadValue<Vector2>().normalized;

		moveDir *= moveSpeed * Time.deltaTime;

		rb.MovePosition(transform.position + new Vector3(moveDir.x, moveDir.y, 0));
	}
}
