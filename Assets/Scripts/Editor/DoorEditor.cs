using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Door door = (Door)target;

        EditorGUILayout.BeginHorizontal();
        door.overridePlayerPositionOnLoad = EditorGUILayout.Toggle("Override Player Location", door.overridePlayerPositionOnLoad);
        if (door.overridePlayerPositionOnLoad)
        {
            door.playerPositionOnLoad = EditorGUILayout.Vector2Field("", door.playerPositionOnLoad);
        }
        EditorGUILayout.EndHorizontal();
    }
}