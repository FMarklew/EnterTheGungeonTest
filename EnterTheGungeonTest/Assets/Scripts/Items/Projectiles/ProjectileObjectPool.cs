using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectPool
{
	private List<ProjectileBase> projectilePool = new List<ProjectileBase>();
	private ProjectileBase projectilePrefab;
	public void InitializePool(ProjectileBase pooledProjectile)
	{
		projectilePrefab = pooledProjectile;
	}
	public ProjectileBase GetProjectileFromPool()
	{
		for (int i = 0; i < projectilePool.Count; i++)
		{
			if (!projectilePool[i].gameObject.activeInHierarchy)
			{
				projectilePool[i].gameObject.SetActive(true);
				return projectilePool[i];
			}
		}
		ProjectileBase projectile = GameObject.Instantiate<ProjectileBase>(projectilePrefab);
		projectilePool.Add(projectile);
		return projectile;
	}

	public void DestroyPool()
	{
		foreach(ProjectileBase proj in projectilePool)
		{
			proj.Destruct();
		}
	}
}
