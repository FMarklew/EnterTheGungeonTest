#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapColliderEditor : EditorWindow
{
    private Tilemap tilemap;
    private GameObject tilemapGameObject;

    [MenuItem("Tools/Tilemap Collider Editor")]
    public static void ShowWindow()
    {
        GetWindow<TilemapColliderEditor>("Tilemap Collider Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Tilemap Collider Configuration", EditorStyles.boldLabel);

        tilemapGameObject = EditorGUILayout.ObjectField("Tilemap GameObject", tilemapGameObject, typeof(GameObject), true) as GameObject;

        if (tilemapGameObject != null)
        {
            tilemap = tilemapGameObject.GetComponent<Tilemap>();
            if (tilemap == null)
            {
                EditorGUILayout.HelpBox("No Tilemap component found on the selected GameObject.", MessageType.Warning);
            }
            else
            {
                if (GUILayout.Button("Refresh Box Colliders"))
                {
                    ClearBoxColliders();
                    AddBoxCollidersToTilemap();
                    EditorUtility.SetDirty(tilemapGameObject); // Mark the GameObject as changed
                }
            }
        }
    }

    private void ClearBoxColliders()
    {
        // Gather all children named "TileCollider" and destroy them
        var children = new List<GameObject>();
        foreach (Transform child in tilemap.transform)
        {
            if (child.name.StartsWith("TileCollider"))
            {
                children.Add(child.gameObject);
            }
        }

        foreach (GameObject child in children)
        {
            Undo.DestroyObjectImmediate(child); // Use Undo to allow revert of the operation
        }
    }

    private void AddBoxCollidersToTilemap()
    {
        if (tilemap != null)
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(position.x, position.y, position.z);
                if (tilemap.HasTile(localPlace))
                {
                    GameObject go = new GameObject("TileCollider_" + localPlace);
                    Undo.RegisterCreatedObjectUndo(go, "Create TileCollider");
                    go.transform.SetParent(tilemap.transform);
                    go.transform.position = tilemap.CellToWorld(localPlace) + new Vector3(0.5f, 0.5f, 0); // Center the collider
                    BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(1, 1); // Assuming tiles are 1x1 units
                }
            }
        }
    }
}
#endif