using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SceneCollector
{
    [MenuItem("Tools/Collect Current Scene Assets (Self-Contained + ReLink)")]
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

        Object sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(scenePath);
        Object[] deps = EditorUtility.CollectDependencies(new Object[] { sceneAsset });

        // Track GUID remapping
        Dictionary<string, string> guidMap = new Dictionary<string, string>();
        int copied = 0;

        foreach (var obj in deps)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path) || !path.StartsWith("Assets/"))
                continue;

            if (path.Contains("/Editor/"))
                continue;

            string dest = Path.Combine(outputFolder, Path.GetFileName(path));
            if (!File.Exists(dest))
            {
                File.Copy(path, dest, true);
                if (File.Exists(path + ".meta"))
                {
                    File.Copy(path + ".meta", dest + ".meta", true);

                    // Map old GUID to new GUID
                    string oldGuid = File.ReadAllText(path + ".meta");
                    string newGuid = File.ReadAllText(dest + ".meta");

                    var oldMatch = Regex.Match(oldGuid, @"guid:\s*([a-f0-9]+)");
                    var newMatch = Regex.Match(newGuid, @"guid:\s*([a-f0-9]+)");

                    if (oldMatch.Success && newMatch.Success)
                        guidMap[oldMatch.Groups[1].Value] = newMatch.Groups[1].Value;
                }
                copied++;
            }
        }

        // Copy scene and meta
        string sceneDest = Path.Combine(outputFolder, $"{sceneName}.unity");
        File.Copy(scenePath, sceneDest, true);
        if (File.Exists(scenePath + ".meta"))
            File.Copy(scenePath + ".meta", sceneDest + ".meta", true);

        // Relink GUIDs in the new scene file
        string sceneText = File.ReadAllText(sceneDest);
        foreach (var kvp in guidMap)
        {
            sceneText = sceneText.Replace(kvp.Key, kvp.Value);
        }
        File.WriteAllText(sceneDest, sceneText);

        AssetDatabase.Refresh();
        Debug.Log($"✅ Scene '{sceneName}' collected, relinked, and made fully self-contained in '{outputFolder}'.");
        Debug.Log($"📦 {copied} assets copied and remapped.");
    }
}
