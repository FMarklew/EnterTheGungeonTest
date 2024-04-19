using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// class for handling player movement, including dodge
/// this can be improved by adding iframes for the dodge once a PlayerHealth system is implemented
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference movementInputAction;
    [SerializeField] private Rigidbody2D rb;

    [Header("Dodging")]
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private InputActionReference dodgeInputAction;
    
    [SerializeField] private Animator playerAnim;
    private const string DODGE_ANIM_CONDITION_NAME = "Dodging";
    private const string COLLISION_LAYER_NAME = "CollisionObjects"; // if required this can be changed into a LayerMask serialized reference

    private bool isDodging = false;
    private Vector2 dodgeDirection = new Vector2();

    private void OnEnable()
    {
        dodgeInputAction.action.performed += Dodge;
    }

    void Update()
    {
        Vector2 moveDir = movementInputAction.action.ReadValue<Vector2>().normalized;
        moveDir *= moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + new Vector3(moveDir.x, moveDir.y, 0));
    }

	private void OnDisable()
	{
        dodgeInputAction.action.performed -= Dodge;
    }

	void Dodge(InputAction.CallbackContext context)
    {
        if (isDodging) return;

        dodgeDirection = movementInputAction.action.ReadValue<Vector2>().normalized;
        if (dodgeDirection != Vector2.zero)
        {
            float actualDodgeDistance = CalculateDodgeDistance(dodgeDirection, dodgeDistance);
            if (actualDodgeDistance > 0)
            {
                StartCoroutine(DodgeMovement(actualDodgeDistance));
            }
        }
    }

    // handle dodge use case for rolling into walls
    float CalculateDodgeDistance(Vector2 direction, float maxDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, LayerMask.GetMask(COLLISION_LAYER_NAME));
        if (hit.collider != null)
        {
            return hit.distance - 0.05f;
        }
        return maxDistance;
    }

    IEnumerator DodgeMovement(float distance)
    {
        isDodging = true;
        playerAnim.SetBool(DODGE_ANIM_CONDITION_NAME, true);
        float elapsedTime = 0;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + dodgeDirection * distance;

        while (elapsedTime < distance / dodgeSpeed)
        {
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / (distance / dodgeSpeed)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);
        dodgeDirection = Vector2.zero;
        playerAnim.SetBool(DODGE_ANIM_CONDITION_NAME, false);
        isDodging = false;
    }
}