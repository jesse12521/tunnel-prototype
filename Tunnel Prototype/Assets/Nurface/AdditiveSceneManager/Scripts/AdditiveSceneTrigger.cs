using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AdditiveSceneTrigger : MonoBehaviour {
    public string[] visibleScenes;
    [HideInInspector]
    public List<string> visibleSceneNames = new List<string>();
    private bool visibleSceneNamesPopulated = false;

    void OnTriggerEnter(Collider other) {        
        if (!other.gameObject.CompareTag("Player")) return;
        if (!visibleSceneNamesPopulated || visibleSceneNames.Count == 0) PopulateVisibleSceneNames();       
        foreach (string sceneName in visibleSceneNames) {
            SceneController.control.LoadScene(sceneName);
        }
        SceneController.control.UnloadAllScenes(visibleSceneNames.ToArray());
    }    
    
    void PopulateVisibleSceneNames() {        
        visibleSceneNames = visibleScenes.Where(sceneName => ProjectSceneList.SceneExists(sceneName)).ToList();
        visibleSceneNames.Add(gameObject.scene.name);
        visibleSceneNamesPopulated = true;
    }    

    void OnValidate()
    {             
        PopulateVisibleSceneNames(); 
    }    
}
