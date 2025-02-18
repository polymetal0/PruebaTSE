#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();      
        CubeSpawner cubeSpawner = (CubeSpawner)target;

        EditorGUILayout.Space();
        GUILayout.Label("Custom Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        /* Slider */
        cubeSpawner.cubeNr = EditorGUILayout.IntSlider("Cubes spawned at once", 
            cubeSpawner.cubeNr, 1, Mathf.Max(1, cubeSpawner.maxCubes)); /* Spawn at least 1 cube */     
        EditorGUILayout.Space();
        /* Buttons */
        if (GUILayout.Button("Spawn cube(s)"))
        {
            cubeSpawner.SpawnCube();
            cubeSpawner.UpdateUI();
        }      
        EditorGUILayout.Space();        
        if (GUILayout.Button("Remove all cubes"))
        {
            cubeSpawner.DestroyCubes();
            cubeSpawner.UpdateUI();
        }
    }
}
#endif