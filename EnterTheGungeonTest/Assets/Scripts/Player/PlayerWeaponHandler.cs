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

		// equip last weapon if added to list in editor (pistol is the current baseline and should be at index 0)
		if(currentWeapons.Count > 0)
		{
			EquipWeapon(currentWeapons[currentWeapons.Count-1]);
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
		Weapon knownWeapon = currentWeapons.Find(weap => weap.weaponId.Equals(weapon.weaponId));
		if (knownWeapon == null)
		{
			knownWeapon = Instantiate(weapon, weaponParent);
			currentWeapons.Add(knownWeapon);
			knownWeapon.gameObject.SetActive(false);
			if (equip)
			{
				EquipWeapon(knownWeapon);
			}
		} else
		{
			knownWeapon.RefreshAmmo();
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
		nextShotTimeAvailable = 0f;
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
		if(equippedWeapon != null && equippedWeapon.GetRemainingAmmo() > 0)
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
