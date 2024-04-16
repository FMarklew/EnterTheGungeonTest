using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
public class DoorAreaScanner : EditorWindow
{
	private Tilemap tilemap;
	private DoorConfigObject doorConfig;
	private DoorController doorController;
	private string doorConfigPath = "Assets/ScriptableObjects/DoorConfig.asset"; // Path to the DoorConfigObject asset

	[MenuItem("Tools/Door Area Scanner")]
	private static void Init()
	{
		GetWindow<DoorAreaScanner>("Door Area Scanner").Show();
	}

	private void OnEnable()
	{
		doorConfig = AssetDatabase.LoadAssetAtPath<DoorConfigObject>(doorConfigPath);
		if (doorConfig == null)
		{
			GUILayout.Label("No asset found at: " + doorConfigPath, EditorStyles.boldLabel);
		}

		if (tilemap == null)
		{
			tilemap = FindObjectOfType<Tilemap>();
		}

		if (doorController == null)
		{
			doorController = FindObjectOfType<DoorController>();
		}
	}

	void OnGUI()
	{
		GUILayout.Label("Scan for Doors in Area", EditorStyles.boldLabel);

		tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", tilemap, typeof(Tilemap), true);
		doorController = (DoorController)EditorGUILayout.ObjectField("Door Controller", doorController, typeof(DoorController), true);
		if (doorConfig == null)
		{
			GUILayout.Label("No asset found at: " + doorConfigPath, EditorStyles.boldLabel);
		}
		if (tilemap != null && doorController != null)
		{
			if (GUILayout.Button("Scan for Doors"))
			{
				List<DoorTilePair> foundDoors = ScanForDoors(tilemap, doorConfig.doorTiles);

				Undo.RecordObject(doorController, "Update Door Tile Pairs");

				doorController.doorTilePairs.Clear();
				foreach (var doorPair in foundDoors)
				{
					doorController.doorTilePairs.Add(doorPair);
				}

				EditorUtility.SetDirty(doorController);

				PrefabUtility.RecordPrefabInstancePropertyModifications(doorController);

				if (!Application.isPlaying)
				{
					EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
				}
			}
		}
		else
		{
			GUILayout.Label("Tilemap and DoorController must be set.", EditorStyles.boldLabel);
		}
	}

	private List<DoorTilePair> ScanForDoors(Tilemap tilemap, List<DoorOpenCloseTilePair> doorTiles)
	{
		List<DoorTilePair> foundDoors = new List<DoorTilePair>();

		foreach (var pos in tilemap.cellBounds.allPositionsWithin)
		{
			TileBase tile = tilemap.GetTile(pos);
			foreach (var pair in doorTiles)
			{
				if (tile == pair.doorOpen || tile == pair.doorClosed)
				{
					foundDoors.Add(new DoorTilePair
					{
						position = pos,
						doorOpen = pair.doorOpen,
						doorClosed = pair.doorClosed
					});
					break;
				}
			}
		}

		BoxCollider2D[] childObjects = doorController.transform.GetComponentsInChildren<BoxCollider2D>();
		foreach(BoxCollider2D col in childObjects)
		{
			if(col.gameObject.name == "DoorCollider")
			{
				DestroyImmediate(col.gameObject);
			}
		}
		foreach (var foundDoor in foundDoors)
		{
			GameObject colliderObj = new GameObject("DoorCollider");
			colliderObj.transform.parent = doorController.transform;
			colliderObj.transform.position = tilemap.CellToWorld(foundDoor.position) + tilemap.tileAnchor;
			BoxCollider2D boxCollider = colliderObj.AddComponent<BoxCollider2D>();
			boxCollider.isTrigger = true;
			foundDoor.doorCollider = boxCollider;
		}

		return foundDoors;
	}
}
#endif