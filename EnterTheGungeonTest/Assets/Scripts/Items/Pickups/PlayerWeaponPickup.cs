using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple weapon pickup system which shows the weapon's sprite, and fires pickup event for PlayerWeaponHandler.cs
/// </summary>
public class PlayerWeaponPickup : PickupBase
{
	[SerializeField] private Weapon weapon;

	public override void Init()
	{
		Instantiate(weapon.GetGunSprite(), transform);
	}
	public override string GetPickupInteractionTag()
	{
		return "Player";
	}

	public override void OnPickupTriggered(GameObject other)
	{
		PlayerWeaponHandler.AddWeaponEvent(weapon, true);
		Destroy(this.gameObject);
	}
}
