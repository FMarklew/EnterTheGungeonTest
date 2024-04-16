using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : ProjectileBase
{
	[SerializeField] private float shotVelocity;
	[SerializeField] private Rigidbody2D rb;

	public override void Init()
	{
		rb.velocity = Vector2.zero;
		gameObject.SetActive(true);
	}

	public override void Fire(Vector2 position, Vector2 direction)
	{
		transform.position = position;
		rb.velocity = direction * shotVelocity;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		gameObject.SetActive(false);
	}
}
