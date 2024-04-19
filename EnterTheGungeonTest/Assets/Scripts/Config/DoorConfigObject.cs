using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

/// <summary>
/// Scriptable object responsible for pairing of open and closed door sprites
/// </summary>

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "DoorScannerConfig", menuName = "ScriptableObjects/DoorConfigObject", order = 1)]
public class DoorConfigObject : ScriptableObject
{
    public List<DoorOpenCloseTilePair> doorTiles = new List<DoorOpenCloseTilePair>();
}

[System.Serializable]
public class DoorOpenCloseTilePair
{
    public Tile doorOpen;
    public Tile doorClosed;
}

[CustomPropertyDrawer(typeof(DoorOpenCloseTilePair))]
public class DoorOpenCloseTilePairDrawer : PropertyDrawer
{
    private const float padding = 2f; 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int originalIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float fieldWidth = (position.width - EditorGUIUtility.labelWidth) / 2 - padding * 3;
        float labelWidth = EditorGUIUtility.labelWidth / 2 - padding;

        Rect openLabelRect = new Rect(position.x + padding, position.y, labelWidth, position.height);
        Rect openFieldRect = new Rect(openLabelRect.xMax + padding, position.y, fieldWidth, position.height);
        Rect closedLabelRect = new Rect(openFieldRect.xMax + padding * 2, position.y, labelWidth, position.height);
        Rect closedFieldRect = new Rect(closedLabelRect.xMax + padding, position.y, fieldWidth, position.height);

        EditorGUI.LabelField(openLabelRect, new GUIContent("Open"));
        EditorGUI.LabelField(closedLabelRect, new GUIContent("Closed"));
        EditorGUI.PropertyField(openFieldRect, property.FindPropertyRelative("doorOpen"), GUIContent.none); 
        EditorGUI.PropertyField(closedFieldRect, property.FindPropertyRelative("doorClosed"), GUIContent.none);

        EditorGUI.indentLevel = originalIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}

#endif