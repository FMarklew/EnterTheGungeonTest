using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference movementInputAction;

    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private InputActionReference dodgeInputAction;

    [SerializeField] private Rigidbody2D rb;

    private bool isDodging = false;
    private Vector2 dodgeDirection = new Vector2();

    private void Start()
    {
        dodgeInputAction.action.performed += Dodge;
    }

    void Update()
    {
        Vector2 moveDir = movementInputAction.action.ReadValue<Vector2>().normalized;
        moveDir *= moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + new Vector3(moveDir.x, moveDir.y, 0));
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

    float CalculateDodgeDistance(Vector2 direction, float maxDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, LayerMask.GetMask("CollisionObjects"));
        if (hit.collider != null)
        {
            return hit.distance - 0.05f;
        }
        return maxDistance;
    }

    IEnumerator DodgeMovement(float distance)
    {
        isDodging = true;
        float elapsedTime = 0;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + dodgeDirection * distance;

        while (elapsedTime < distance / dodgeSpeed)
        {
            rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / (distance / dodgeSpeed)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);  // Ensure final position is exact
        isDodging = false;
    }
}