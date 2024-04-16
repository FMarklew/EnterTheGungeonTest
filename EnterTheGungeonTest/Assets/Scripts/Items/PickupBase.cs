using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
	public abstract void Init();
	public abstract void OnPickup(GameObject other);
	public abstract string GetPickupInteractionTag();

	private void Awake()
	{
		Init();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(GetPickupInteractionTag()))
		{
			OnPickup(collision.gameObject);
		}
	}
}
