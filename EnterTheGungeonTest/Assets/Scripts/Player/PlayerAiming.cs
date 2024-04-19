using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// handles player aiming, sprite flipping for character and weapon in-hand
/// </summary>
public class PlayerAiming : MonoBehaviour
{
	[SerializeField] private Transform aimObject;
	[SerializeField] private Transform playerSprite;
	[SerializeField] private InputActionReference aimInputAction;
	[SerializeField] private Transform weaponsParent;
	private bool isFlipped = true;

	private float playerSpriteScale = 1.5f;
	private void Awake()
	{
		playerSpriteScale = playerSprite.localScale.x;
	}

	void Update()
	{
		Vector2 aimDirection = aimInputAction.action.ReadValue<Vector2>().normalized;

		if (aimDirection != Vector2.zero)
		{
			aimObject.eulerAngles = new Vector3(aimObject.eulerAngles.x, aimObject.eulerAngles.y, -Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg);
			float currentAimRot = aimObject.eulerAngles.z;
			if(currentAimRot >= 0 && currentAimRot < 180)
			{
				if (isFlipped)
				{
					isFlipped = false;
					weaponsParent.localScale = new Vector3(-1, 1, 1);
					playerSprite.localScale = new Vector3(playerSpriteScale * - 1, playerSpriteScale, playerSpriteScale);
				}
			} else
			{
				if (!isFlipped)
				{
					isFlipped = true;
					weaponsParent.localScale = new Vector3(1, 1, 1);
					playerSprite.localScale = new Vector3(playerSpriteScale, playerSpriteScale, playerSpriteScale);
				}
			}
		}
	}
}
