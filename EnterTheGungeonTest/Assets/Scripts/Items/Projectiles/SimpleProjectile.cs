using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplest projectile, just fires forward and disables on trigger enter. 
/// This can be extended to affect enemies as well
/// </summary>
public class SimpleProjectile : ProjectileBase
{
	public override void Init()
	{
		rb.velocity = Vector2.zero;
		gameObject.SetActive(true);
	}

	public override void Fire(Vector2 position, Vector2 direction, float velocity)
	{
		transform.position = position;
		rb.velocity = direction * velocity;
	}
	public override void Destruct()
	{
		Destroy(this.gameObject, 1);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameObject.SetActive(false);
	}

}
