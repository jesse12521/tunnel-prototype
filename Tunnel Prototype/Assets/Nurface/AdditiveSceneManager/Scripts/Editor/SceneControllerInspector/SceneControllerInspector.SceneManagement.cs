using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public partial class SceneControllerInspector
{
    void UnloadScene(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        if (Application.isPlaying)
        {
            // Tell SceneManager to unload the scene
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
            SceneManager.UnloadScene(sceneName);
#endif
        }
        else
        {
            EditorSceneManager.CloseScene(scene, true);
        }
    }

    void LoadScene(string sceneName)
    {        
        if (Application.isPlaying)
        {
            sceneController.LoadScene(sceneName, true);
        }
        else
        {
            if (String.IsNullOrEmpty(sceneName)) return;
            UnityEngine.Object scene = new UnityEngine.Object();
            string sceneGUID = "";
            string[] sceneGUIDs = AssetDatabase.FindAssets(String.Format("{0} t:scene", sceneName));
            foreach (string guid in sceneGUIDs) {
                scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guid));
                if (scene.name == sceneName) {
                    sceneGUID = guid;
                }
            }
            if (String.IsNullOrEmpty(sceneGUID)) return;
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        }
    }
}