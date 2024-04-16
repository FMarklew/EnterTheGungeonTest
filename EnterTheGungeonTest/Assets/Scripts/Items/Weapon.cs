using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
	[SerializeField] private ProjectileBase projectilePrefab;
	[SerializeField] private Transform projectileSpawnPoint;
	[SerializeField] private GameObject gunSpriteParent;

	[Tooltip("Seconds between shots")][SerializeField] private float shotInterval = 0.2f;

	[Tooltip("Max ammo, set to -1 for infinite ammo")] [SerializeField] private int maxAmmo;
	private int currentAmmo;

	private List<ProjectileBase> projectilePool = new List<ProjectileBase>();

	private void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		currentAmmo = maxAmmo;
	}

	public virtual void CleanUp()
	{
		foreach(ProjectileBase p in projectilePool)
		{
			Destroy(p.gameObject, 1);
		}

		Destroy(this.gameObject, 10);
	}

    public virtual void FireWeapon(Vector2 direction)
	{
		if (maxAmmo == -1 || currentAmmo > 0)
		{
			ProjectileBase projectile = GetProjectileFromPool();
			projectile.Init();
			projectile.Fire(projectileSpawnPoint.position, direction);
			currentAmmo--;
		}
	}

	private ProjectileBase GetProjectileFromPool()
	{
		for(int i = 0; i < projectilePool.Count; i++)
		{
			if (!projectilePool[i].gameObject.activeInHierarchy)
			{
				projectilePool[i].gameObject.SetActive(true);
				return projectilePool[i];
			}
		}
		ProjectileBase projectile = Instantiate<ProjectileBase>(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
		projectilePool.Add(projectile);
		return projectile;
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
}
