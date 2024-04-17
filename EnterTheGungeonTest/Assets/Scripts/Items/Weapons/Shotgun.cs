using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
	[SerializeField] private float shotArc;
	[SerializeField] private int totalShotsOverArc;

	public override void FireWeapon(Vector2 direction)
	{
		float halfSpread = shotArc / 2;
		Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction.normalized);

		for (int i = 0; i < totalShotsOverArc; i++)
		{
			float angle = ((float)i / (totalShotsOverArc - 1) * shotArc) - halfSpread;
			Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward) * rotation;

			ProjectileBase projectile = projectileObjectPoolHandler.GetProjectileFromPool();
			if (projectile != null)
			{
				projectile.Fire(projectileSpawnPoint.position, bulletRotation * Vector2.up, projectileVelocity);
			}
		}
		currentAmmo--;
	}
}
