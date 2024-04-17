using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
