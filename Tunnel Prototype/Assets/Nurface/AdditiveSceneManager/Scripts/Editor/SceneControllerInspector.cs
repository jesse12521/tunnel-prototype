using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(SceneController))]
public partial class SceneControllerInspector : Editor
{
    private SceneController sceneController;    
    private SerializedObject targetSerializedObject;
    private SerializedProperty settings;
    private SerializedProperty loadAtStartScenes;
    private SerializedProperty persistentScenes;
    private SerializedObject settingsObject;
    private int previousSceneManagerCount;
    
    void OnEnable()
    {
        EditorApplication.update += Update;
        sceneController = (SceneController)target;
        string scriptableObjectPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SceneControllerSettings t: scriptableObject")[0]);// Load the settings file
        sceneController.settings = (SceneControllerSettingsObject)AssetDatabase.LoadAssetAtPath(scriptableObjectPath, typeof(SceneControllerSettingsObject));
        targetSerializedObject = new SerializedObject(target);
        settings = targetSerializedObject.FindProperty("settings");
        settingsObject = new SerializedObject(settings.objectReferenceValue);
        loadAtStartScenes = settingsObject.FindProperty("loadAtStartScenes");
        persistentScenes = settingsObject.FindProperty("persistentScenes");

        GenerateSceneList();
    }

    void OnDisable() {
        EditorApplication.update -= Update;
    }

    void Update() {
        int inspectorSceneCount = 0;
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            inspectorSceneCount++;
        }
        if (previousSceneManagerCount != inspectorSceneCount) {
            previousSceneManagerCount = inspectorSceneCount;
            Repaint();
        }
    }

    public override void OnInspectorGUI()
    {
        targetSerializedObject.Update();
        settingsObject.Update();
        //EditorGUILayout.PropertyField(settings, new GUIContent("Settings:"), true);
        RenderLoadAtStartScenes();
        RenderSeparator();
        RenderPersistentScenes();
        RenderSeparator();
        RenderCurrentlyLoadedScenes();
        RenderLightmapOptions();
        if (targetSerializedObject.targetObject != null) targetSerializedObject.ApplyModifiedProperties();
        if (settingsObject.targetObject != null) settingsObject.ApplyModifiedProperties();
    }

    void RenderSeparator()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    void RenderSectionHeader(string header, string tooltip = "")
    {
        EditorGUILayout.LabelField(new GUIContent(header, tooltip), EditorStyles.largeLabel, GUILayout.Height(20f));
    }

    public void GenerateSceneList() {
        var scenes = AssetDatabase.FindAssets("t:scene")
                                  .Select(s => AssetDatabase.GUIDToAssetPath(s))
                                  .Select(s => AssetDatabase.LoadAssetAtPath<SceneAsset>(s))
                                  .Select(s => s.name)
                                  .ToList();

        sceneController.settings.scenes = scenes;
    }
}