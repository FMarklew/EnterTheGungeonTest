using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

/// <summary>
/// Door Controller script responsible for opening and closing doors when player is near. Can be improved by adding a lock system which waits until enemies are defeated for example.
/// </summary>
public class DoorController : MonoBehaviour
{
	public Tilemap tilemap;
	public List<DoorTilePair> doorTilePairs = new List<DoorTilePair>();

	public void ToggleDoor(DoorTilePair door, bool open)
	{
		Tile tileToSet = open ? door.doorOpen : door.doorClosed;
		tilemap.SetTile(door.position, tileToSet);

		if (door.doorCollider != null)
		{
			door.doorCollider.enabled = !open;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			foreach (var door in doorTilePairs)
			{
				ToggleDoor(door, true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			foreach (var door in doorTilePairs)
			{
				ToggleDoor(door, false);
			}
		}
	}
}

[System.Serializable]
public class DoorTilePair
{
	public Vector3Int position;
	public Tile doorOpen;
	public Tile doorClosed;
	public BoxCollider2D doorCollider;
}