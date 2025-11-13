using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneCollector
{
    [MenuItem("Tools/Collect Current Scene Assets (Self-Contained)")]
    static void CollectCurrentSceneAssets()
    {
        // Get the currently open scene
        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogError("❌ No scene is currently loaded.");
            return;
        }

        string sceneName = scene.name;
        string scenePath = scene.path;

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("❌ Current scene has not been saved yet. Please save it before collecting assets.");
            return;
        }

        // Prepare the output folder
        string outputFolder = $"Assets/UsedAssets/{sceneName}";
        if (Directory.Exists(outputFolder))
        {
            Debug.Log($"🧹 Cleaning existing folder: {outputFolder}");
            Directory.Delete(outputFolder, true);
        }
        Directory.CreateDirectory(outputFolder);

        // Load the scene as an asset
        Object sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(scenePath);
        if (sceneAsset == null)
        {
            Debug.LogError($"❌ Could not load scene asset at path: {scenePath}");
            return;
        }

        // Collect all dependencies
        Object[] deps = EditorUtility.CollectDependencies(new Object[] { sceneAsset });

        int copiedCount = 0;

        foreach (var obj in deps)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path) || !path.StartsWith("Assets/"))
                continue;

            // Skip editor-only content
            if (path.Contains("/Editor/"))
                continue;

            string dest = Path.Combine(outputFolder, Path.GetFileName(path));

            // Copy asset and its .meta file
            if (!File.Exists(dest))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(path, dest, true);
                if (File.Exists(path + ".meta"))
                    File.Copy(path + ".meta", dest + ".meta", true);
                copiedCount++;
            }
        }

        // Copy the scene itself and meta
        string sceneDest = Path.Combine(outputFolder, $"{sceneName}.unity");
        File.Copy(scenePath, sceneDest, true);
        if (File.Exists(scenePath + ".meta"))
            File.Copy(scenePath + ".meta", sceneDest + ".meta", true);

        AssetDatabase.Refresh();
        Debug.Log($"✅ Successfully collected scene '{sceneName}' and {copiedCount} assets into '{outputFolder}'.");
        Debug.Log("📦 You can now commit that folder and others can open the scene with no missing assets.");
    }
}
