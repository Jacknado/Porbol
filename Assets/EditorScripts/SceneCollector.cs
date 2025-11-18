using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class SceneCollector
{
    [MenuItem("Tools/Collect Current Scene Assets (Full Self-Contained + Sorted + Deep Relink)")]
    static void CollectAndRelink()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            Debug.LogError("❌ No scene loaded.");
            return;
        }

        string sceneName = scene.name;
        string scenePath = scene.path;
        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("❌ Please save the scene before collecting.");
            return;
        }

        string outputFolder = $"Assets/UsedAssets/{sceneName}";
        if (Directory.Exists(outputFolder))
            Directory.Delete(outputFolder, true);
        Directory.CreateDirectory(outputFolder);

        string[] deps = AssetDatabase.GetDependencies(scenePath, true);

        int copied = 0;
        Dictionary<string, string> guidMap = new Dictionary<string, string>();

        // === Copy assets and map GUIDs ===
        foreach (string path in deps)
        {
            if (!path.StartsWith("Assets/"))
                continue;
            if (path.Contains("/Editor/"))
                continue;
            if (path == scenePath)
                continue;

            string category = GetCategoryFolder(path);
            string categoryFolder = Path.Combine(outputFolder, category).Replace("\\", "/");
            if (!Directory.Exists(categoryFolder))
                Directory.CreateDirectory(categoryFolder);

            string fileName = Path.GetFileName(path);
            string destPath = Path.Combine(categoryFolder, fileName).Replace("\\", "/");

            if (AssetDatabase.CopyAsset(path, destPath))
            {
                copied++;
                string oldGuid = AssetDatabase.AssetPathToGUID(path);
                string newGuid = AssetDatabase.AssetPathToGUID(destPath);
                if (!string.IsNullOrEmpty(oldGuid) && !string.IsNullOrEmpty(newGuid))
                    guidMap[oldGuid] = newGuid;
            }
        }

        // === Copy the scene ===
        string sceneDest = Path.Combine(outputFolder, $"{sceneName}.unity").Replace("\\", "/");
        AssetDatabase.CopyAsset(scenePath, sceneDest);
        AssetDatabase.Refresh();

        // === Fix GUIDs in all YAML-based files ===
        string[] yamlFiles = Directory.GetFiles(outputFolder, "*.*", SearchOption.AllDirectories);
        foreach (string file in yamlFiles)
        {
            if (IsTextAsset(file))
            {
                string text = File.ReadAllText(file);
                bool modified = false;

                foreach (var kvp in guidMap)
                {
                    if (text.Contains(kvp.Key))
                    {
                        text = text.Replace(kvp.Key, kvp.Value);
                        modified = true;
                    }
                }

                if (modified)
                    File.WriteAllText(file, text);
            }
        }

        AssetDatabase.Refresh();

        Debug.Log($"✅ Scene '{sceneName}' fully collected and deep-relinked into '{outputFolder}'.");
        Debug.Log($"📦 {copied} dependent assets copied, including materials and textures.");
    }

    // === Helpers ===

    static bool IsTextAsset(string path)
    {
        string ext = Path.GetExtension(path).ToLower();
        return IsIn(ext, ".unity", ".mat", ".prefab", ".anim", ".controller", ".overridecontroller", ".asset");
    }

    static string GetCategoryFolder(string assetPath)
    {
        string ext = Path.GetExtension(assetPath).ToLower();

        if (IsIn(ext, ".png", ".jpg", ".jpeg", ".tga", ".psd", ".tif", ".tiff", ".bmp", ".hdr", ".exr"))
            return "Textures";
        if (IsIn(ext, ".mat"))
            return "Materials";
        if (IsIn(ext, ".fbx", ".obj", ".blend", ".dae", ".3ds", ".stl"))
            return "Meshes";
        if (IsIn(ext, ".prefab"))
            return "Prefabs";
        if (IsIn(ext, ".anim", ".controller", ".overridecontroller"))
            return "Animations";
        if (IsIn(ext, ".cs", ".shader", ".cginc", ".compute"))
            return "Scripts";

        return "Misc";
    }

    static bool IsIn(string ext, params string[] list)
    {
        foreach (var l in list)
            if (ext == l) return true;
        return false;
    }
}