using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private List<Weapon> currentWeapons = new List<Weapon>();
	public static System.Action<Weapon, bool> AddWeaponEvent;

	[SerializeField] private Transform weaponParent;

	private Weapon equippedWeapon;

	[SerializeField] private InputActionReference aimInputAction;
	private float nextShotTimeAvailable;

	private void OnEnable()
	{
		AddWeaponEvent += AddWeapon;

		// equip first weapon if added to list in editor
		if(currentWeapons.Count > 0)
		{
			EquipWeapon(currentWeapons[0]);
		}
	}

	private void Update()
	{
		// because its mobile, shoot as long as there is an input detected by the aim stick
		if (aimInputAction.action.ReadValue<Vector2>() != Vector2.zero)
		{
			TryShooting();
		}
	}

	private void OnDisable()
	{
		AddWeaponEvent -= AddWeapon;
	}

	public void AddWeapon(Weapon weapon, bool equip)
	{
		if (!currentWeapons.Contains(weapon))
		{
			Weapon weap = Instantiate(weapon, weaponParent);
			currentWeapons.Add(weap);
			weap.gameObject.SetActive(false);
			if (equip)
			{
				EquipWeapon(weap);
			}
		}
	}

	private void EquipWeapon(Weapon weapon)
	{
		equippedWeapon = weapon;
		foreach(Weapon weap in currentWeapons)
		{
			weap.gameObject.SetActive(false);
		}
		equippedWeapon.gameObject.SetActive(true);

	}

	private void UnequipWeapon()
	{
		equippedWeapon.CleanUp();
		equippedWeapon.transform.parent = null;

		currentWeapons.Remove(equippedWeapon);
		if (currentWeapons.Count > 0) {
			EquipWeapon(currentWeapons[currentWeapons.Count - 1]);
		}
	}

	private void TryShooting()
	{
		if(equippedWeapon.GetRemainingAmmo() > 0)
		{
			if(nextShotTimeAvailable <= Time.time)
			{
				Vector2 dir = aimInputAction.action.ReadValue<Vector2>().normalized;
				equippedWeapon.FireWeapon(dir);
				nextShotTimeAvailable = Time.time + equippedWeapon.GetShotInterval();
			}
		} else
		{
			UnequipWeapon();
		}
	}

}
