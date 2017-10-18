using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Make a static reference to this gamecontroller
    public static SceneController control;
    
    // Settings file
    [SerializeField]
    public SceneControllerSettingsObject settings;
    
    // The AsyncOperation for loading a level
    public Dictionary<string, AsyncOperation> asyncOperations = new Dictionary<string, AsyncOperation>();
   
    // Awake and ensure this gameobject is persisent and the only gamecontroller
    void Awake()
    {        
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        //Application.targetFrameRate = 60;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        foreach (var sceneName in settings.loadAtStartScenes)
        {
            LoadScene(sceneName, true);
        }

        // Just some test code to show loading multiple scenes simultaneously
        /*
        UnloadScenes();
        LoadScene("RoomA", true);
        LoadScene("RoomB", false);
        LoadScene("RoomC", true);
        LoadScene("RoomD", false);
        LoadScene("RoomE", true);
        */

        /*
        UnloadAllScenes();
        string[] levels = new string[5] { "RoomA", "RoomB", "RoomC", "RoomD", "RoomE" };
        LoadScenes(levels, true);
        */
        

    }

    void Update() {
        // Here you can see exactly what is happening with the Async Operations
        /*
        foreach (KeyValuePair<string, AsyncOperation> i in asyncOperations) {
            Debug.Log(i.Key + " progress: " + i.Value.progress);
        }
        */
        // Testing Unload and Reloading
        /*
        if (Input.GetKeyDown(KeyCode.U)) {
            string[] levels = new string[5] { "RoomA", "RoomB", "RoomC", "RoomD", "RoomE" };
            UnloadScenes(levels);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            string[] levels = new string[5] { "RoomA", "RoomB", "RoomC", "RoomD", "RoomE" };
            ReloadScenes(levels);
        }*/
    }

    //****************************************************************
    // SceneExists (string sceneName)
    // Check if a scene exists in the project
    //****************************************************************
    public bool SceneExists(string name) {
        return settings.scenes.Contains(name, StringComparer.OrdinalIgnoreCase);
    }

    //****************************************************************
    // LoadScene (string sceneName)
    // Load a single scene additively and asynchronously. allowSceneActivation will be set to true.
    //****************************************************************
    public void LoadScene(string sceneName) {
        StartCoroutine(ILoadScene(sceneName));
    }
    private IEnumerator ILoadScene(string sceneName) {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();
        // Look at all currently opened scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            // If the scene we are trying to load is already open
            if (sceneName == SceneManager.GetSceneAt(i).name) {
                // Stop now
                yield break;
            }
        }
        // Load the new scene additively
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // Set the async allowSceneActivation bool to bool passed into this function
        async.allowSceneActivation = true;
        // Add this async operation to the list of current operations (manages loading multiple levels simultaneously)
        asyncOperations.Add(sceneName, async);
        // Yield the async operation;
        yield return async;
        // Scene is now fully loaded
        StartCoroutine(SetLoadedScene(sceneName));
    }
    //****************************************************************
    // LoadScene (string sceneName, bool allowSceneActivation)
    // Load a scene additively and asynchronously, with control of allowSceneActivation boolean.
    //****************************************************************
    public void LoadScene(string sceneName, bool allowSceneActivation) {
        StartCoroutine(ILoadScene(sceneName, allowSceneActivation));
    }
    private IEnumerator ILoadScene(string sceneName, bool allowSceneActivation) {
        // Wait until the end of the current frame
        yield return new WaitForEndOfFrame();
        // Look at all currently opened scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            // If the scene we are trying to load is already open
            if (sceneName == SceneManager.GetSceneAt(i).name) {
                // Stop now
                yield break;
            }
        }
        // Load the new scene additively
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // Set the async allowSceneActivation bool to bool passed into this function
        async.allowSceneActivation = allowSceneActivation;
        // Add this async operation to the list of current operations (manages loading multiple levels simultaneously)
        asyncOperations.Add(sceneName, async);
        // Yield the async operation;
        yield return async;
        // Scene is now fully loaded
        StartCoroutine(SetLoadedScene(sceneName));
    }

    //****************************************************************
    // LoadScenes (string[] sceneNames)
    // Load multiple scenes, they will be loaded in the order of the array
    // allowSceneActivation is true for all scenes
    //****************************************************************
    public void LoadScenes(string[] sceneNames) {
        foreach (string sceneName in sceneNames) {
            LoadScene(sceneName);
        }
    }

    //****************************************************************
    // Load Scenes (string[] sceneNames, bool allowSceneActivation)
    // Load an array of scenes, they will be loaded in the order of the array.
    // The allowSceneActivation boolean will be assigned to all scenes.
    //****************************************************************
    public void LoadScenes(string[] sceneNames, bool allowSceneActivation) {
        foreach (string sceneName in sceneNames) {
            LoadScene(sceneName, allowSceneActivation);
        }
    }

    //****************************************************************
    // SetLoadedScene (sceneName)
    // Private script, don't use. This removes the async operation after a scene is loaded.
    //****************************************************************
    private IEnumerator SetLoadedScene(string sceneName) {
        yield return new WaitForEndOfFrame();
        asyncOperations[sceneName] = null;
        asyncOperations.Remove(sceneName);
    }

    //****************************************************************
    // UnloadScene(string SceneName)
    // Unload a scene. The scene will NOT be unloaded if it’s a Persistent Scene.
    //****************************************************************
    public void UnloadScene(string sceneName) {
        StartCoroutine(IUnloadScene(sceneName));
    }
    private IEnumerator IUnloadScene(string sceneName) {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();
        if (settings.persistentScenes.Contains(SceneManager.GetSceneByName(sceneName).name) == false) {
#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(sceneName);
#else
            SceneManager.UnloadScene(sceneName);
#endif
        }
    }

    //****************************************************************
    // UnloadScenes(string[] sceneNames)
    // Unload an array of scenes. Persistent Scenes will not be unloaded.
    //****************************************************************
    public void UnloadScenes(string[] sceneNames) {
        StartCoroutine(IUnloadScenes(sceneNames));
    }
    private IEnumerator IUnloadScenes(string[] sceneNames) {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();
        foreach (string sceneName in sceneNames) {
            if (settings.persistentScenes.Contains(SceneManager.GetSceneByName(sceneName).name) == false) {
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }
    }

    //****************************************************************
    // UnloadAllScenes()
    // Unload all scenes. Persistent Scenes will not be unloaded.
    //****************************************************************
    public void UnloadAllScenes() {
        StartCoroutine(IUnloadAllScenes());
    }
    private IEnumerator IUnloadAllScenes() {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();
        // Scenes marked to be unloaded
        List<string> scenesToUnload = new List<string>();
        // For all of the currently loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            // Make sure the scene is not a persistent scene
            if (settings.persistentScenes.Contains(SceneManager.GetSceneAt(i).name) == false) {
                // Add this scene's name to a list of scenes to be unloaded (or index 'i' will change during this for loop)
                scenesToUnload.Add((string)SceneManager.GetSceneAt(i).name);
            }
        }
        // Now that we're done iterating through each of the Scenes, we can safely unload all the levels
        if (scenesToUnload.Count != 0) {
            foreach (string sceneName in scenesToUnload) {
                // Tell SceneManager to unload the scene
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }
    }
    //****************************************************************
    // UnloadAllScenes(string excpetion)
    // Unload all scenes except ‘exception’. Persistent Scenes will not be unloaded.
    //****************************************************************
    public void UnloadAllScenes(string exception) {
        StartCoroutine(IUnloadAllScenes(exception));
    }
    private IEnumerator IUnloadAllScenes(string exception) {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();
        // Scenes marked to be unloaded
        List<string> scenesToUnload = new List<string>();
        // For all of the currently loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            // Make sure the scene is not a persistent scene
            if (settings.persistentScenes.Contains(SceneManager.GetSceneAt(i).name) == false) {
                // Make sure this scene is not in our list of exceptions
                if (exception != SceneManager.GetSceneAt(i).name == false) {
                    // Add this scene's name to a list of scenes to be unloaded (or index 'i' will change during this for loop)
                    scenesToUnload.Add((string)SceneManager.GetSceneAt(i).name);
                }
            }
        }
        // Now that we're done iterating through each of the Scenes, we can safely unload all the levels
        if (scenesToUnload.Count != 0) {
            foreach (string sceneName in scenesToUnload) {
                // Tell SceneManager to unload the scene
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }
    }
    //****************************************************************
    // UnloadAllScenes(string[] excpetions)
    // Unload all scenes. An array of exceptions will not be unloaded. Persistent Scenes will not be unloaded.
    //****************************************************************
    public void UnloadAllScenes(string[] exceptions) {
        StartCoroutine(IUnloadAllScenes(exceptions));
    }
    private IEnumerator IUnloadAllScenes(string[] exceptions)
    {
        // Wait until the end of the frame
        yield return new WaitForEndOfFrame();
        // Scenes marked to be unloaded
        List<string> scenesToUnload = new List<string>();
        // For all of the currently loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            // Make sure the scene is not a persistent scene
            if (settings.persistentScenes.Contains(SceneManager.GetSceneAt(i).name) == false)
            {
                // Make sure this scene is not in our list of exceptions
                if (exceptions.Contains(SceneManager.GetSceneAt(i).name) == false)
                {
                    // Add this scene's name to a list of scenes to be unloaded (or index 'i' will change during this for loop)
                    scenesToUnload.Add((string)SceneManager.GetSceneAt(i).name);
                }
            }
        }
        // Now that we're done iterating through each of the Scenes, we can safely unload all the levels
        if (scenesToUnload.Count != 0)
        {
            foreach (string sceneName in scenesToUnload)
            {
                // Tell SceneManager to unload the scene
#if UNITY_5_5_OR_NEWER
                SceneManager.UnloadSceneAsync(sceneName);
#else
                SceneManager.UnloadScene(sceneName);
#endif
            }
        }
    }

    //****************************************************************
    // ReloadScene(string sceneName)
    // Reload a scene. Persistent Scenes will not be reloaded
    //****************************************************************
    public void ReloadScene(string sceneName) {
        UnloadScene(sceneName);
        StartCoroutine(IReloadScene(sceneName));
    }
    private IEnumerator IReloadScene(string sceneName) {
        yield return new WaitForEndOfFrame();
        LoadScene(sceneName);
    }

    //****************************************************************
    // ReloadScenes(string[] sceneNames)
    // Reload an array of scenes. Persistent scenes will not be reloaded
    //****************************************************************
    public void ReloadScenes(string[] sceneNames) {
        UnloadScenes(sceneNames);
        StartCoroutine(IReloadScenes(sceneNames));
    }
    private IEnumerator IReloadScenes(string[] sceneNames) {
        yield return new WaitForEndOfFrame();
        LoadScenes(sceneNames);
    }
}