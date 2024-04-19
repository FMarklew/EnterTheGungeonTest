using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for weapons - comes with firing logic, ammo handling, object pool
/// </summary>
public class Weapon : MonoBehaviour
{
	public string weaponId;
	[SerializeField] private ProjectileBase projectilePrefab;
	public Transform projectileSpawnPoint;
	[SerializeField] private GameObject gunSpriteParent;

	[Tooltip("Seconds between shots")][SerializeField] private float shotInterval = 0.2f;

	public float projectileVelocity;
	[Tooltip("Max ammo, set to -1 for infinite ammo")] [SerializeField] private int maxAmmo;
	public int currentAmmo;

	public ProjectileObjectPool projectileObjectPoolHandler = new ProjectileObjectPool();
	

	private void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		RefreshAmmo();
		projectileObjectPoolHandler.InitializePool(projectilePrefab);
	}

	public virtual void CleanUp()
	{
		// on weapon discard
		projectileObjectPoolHandler.DestroyPool();

		Destroy(this.gameObject, 10);
	}

    public virtual void FireWeapon(Vector2 direction)
	{
		if (maxAmmo == -1 || currentAmmo > 0)
		{
			ProjectileBase projectile = projectileObjectPoolHandler.GetProjectileFromPool();
			projectile.Init();
			projectile.Fire(projectileSpawnPoint.position, direction, projectileVelocity);
			currentAmmo--;
		}
	}

	public GameObject GetGunSprite()
	{
		return gunSpriteParent;
	}

	public int GetRemainingAmmo()
	{
		return maxAmmo == -1 ? 100 : currentAmmo;
	}

	public float GetShotInterval()
	{
		return shotInterval;
	}

	public void RefreshAmmo()
	{
		currentAmmo = maxAmmo;
	}
}
