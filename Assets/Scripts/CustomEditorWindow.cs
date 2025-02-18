#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CustomEditorWindow : EditorWindow
{
    private List<string> sceneList;

    [MenuItem("Window/Custom Editor Window")]
    public static void ShowWindow()
    {
        GetWindow<CustomEditorWindow>("Custom Editor Window");
    }

    private void OnEnable()
    {
        ListScenes();
        /* Refresh when adding or removing a scene from build */
        EditorBuildSettings.sceneListChanged += ListScenes;
    }

    private void ListScenes()
    {
        sceneList = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            sceneList.Add(scene.path);
        }
        Debug.Log("Scene list loaded");
    }

    private void OpenScene(string scenePath)
    {
        try
        {
            /* Can't call OpenScene(path) while in play mode */
            if (EditorApplication.isPlaying)
            {
                SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(scenePath));
            }
            else
            {
                EditorSceneManager.OpenScene(scenePath);
            }
            Debug.Log("Loaded scene " + scenePath);
        }
        catch (System.Exception)
        {
            throw;
        }
        
    }

    private void AddSceneToBuild(string path)
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        var scene = new EditorBuildSettingsScene(path, true);
        foreach (var _scene in EditorBuildSettings.scenes)
        {
            editorBuildSettingsScenes.Add(_scene);
            if (scene.path == _scene.path)
            {
                Debug.LogWarning("Scene already in build!");
                return;
            }
        }
        editorBuildSettingsScenes.Add(scene);
        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        Debug.Log("Scene added");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scenes in build", EditorStyles.boldLabel);

        foreach (var scene in sceneList)
        {
            var _scene = System.IO.Path.GetFileNameWithoutExtension(scene);
            if (GUILayout.Button(_scene))
            {
                OpenScene(scene);
            }
        }
        
        /* Drag & drop area */
        Rect dragNDrop = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        Event currentEvent = Event.current;
        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
                /* Hovering over the window while dragging file(s) */
                if (dragNDrop.Contains(currentEvent.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    currentEvent.Use();
                }
                break;
            case EventType.DragPerform:
                /* Dropping the file(s) on the window */
                if (dragNDrop.Contains(currentEvent.mousePosition))
                {
                    string[] draggedFiles = DragAndDrop.paths;
                    foreach (string path in draggedFiles)
                    {
                        if (path.EndsWith(".unity"))
                        {
                            AddSceneToBuild(path);
                        }
                        else
                        {
                            Debug.LogWarning("Not a scene file!");
                        }
                    }
                    DragAndDrop.AcceptDrag();
                    currentEvent.Use();
                }
                break;
        }
        GUI.Box(dragNDrop, "Drag & drop a scene here");
    }
}
#endif