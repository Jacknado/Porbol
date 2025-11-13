using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class UsedAssetsCollector : EditorWindow
{
    private string levelName = "Level1";
    private bool processScene = true; // Toggle between Scene and Prefab

    [MenuItem("Tools/Collect Used Assets")]
    public static void ShowWindow()
    {
        GetWindow<UsedAssetsCollector>("Used Assets Collector");
    }

    private void OnGUI()
    {
        GUILayout.Label("Used Assets Collector", EditorStyles.boldLabel);

        // Choose Level Folder
        levelName = EditorGUILayout.TextField("Level Name", levelName);

        EditorGUILayout.Space();

        // Toggle for source type
        processScene = EditorGUILayout.Toggle("Process Current Scene", processScene);

        if (!processScene)
        {
            EditorGUILayout.HelpBox("Select a prefab in the Project window before clicking Collect.", MessageType.Info);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Collect Used Assets"))
        {
            string targetFolder = $"Assets/UsedAssets/{levelName}";
            if (processScene)
                CollectFromScene(targetFolder);
            else
                CollectFromSelectedPrefab(targetFolder);
        }
    }

    private static void CollectFromScene(string targetFolder)
    {
        if (!Directory.Exists(targetFolder))
            Directory.CreateDirectory(targetFolder);

        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogError("No scene is loaded!");
            return;
        }

        string[] dependencies = AssetDatabase.GetDependencies(scene.path, true);
        CopyDependencies(dependencies, targetFolder);
        Debug.Log($"✅ Collected assets from scene '{scene.name}' into: {targetFolder}");
    }

    private static void CollectFromSelectedPrefab(string targetFolder)
    {
        var selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogWarning("Please select a prefab first!");
            return;
        }

        if (!Directory.Exists(targetFolder))
            Directory.CreateDirectory(targetFolder);

        string prefabPath = AssetDatabase.GetAssetPath(selected);
        string[] dependencies = AssetDatabase.GetDependencies(prefabPath, true);
        CopyDependencies(dependencies, targetFolder);

        Debug.Log($"✅ Collected assets from prefab '{selected.name}' into: {targetFolder}");
    }

    private static void CopyDependencies(string[] dependencies, string targetFolder)
    {
        foreach (var dep in dependencies)
        {
            if (!dep.StartsWith("Assets") || dep.EndsWith(".cs"))
                continue;

            string fileName = Path.GetFileName(dep);
            string targetPath = Path.Combine(targetFolder, fileName).Replace("\\", "/");

            if (dep == targetPath)
                continue;

            // Copy the asset
            if (AssetDatabase.CopyAsset(dep, targetPath))
                Debug.Log($"Copied: {dep} -> {targetPath}");
            else
                Debug.LogWarning($"⚠️ Failed to copy: {dep}");
        }

        AssetDatabase.Refresh();
    }
}
