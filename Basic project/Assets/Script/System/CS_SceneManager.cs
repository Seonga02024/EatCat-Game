using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_SceneManager : SingleTon<CS_SceneManager>
{
/* States */
    private SceneData.SceneType curScene = SceneData.SceneType.None;

    public void LoadScene(SceneData.SceneType type) {
        if(curScene != type) {
            StartCoroutine(unloadScene(SceneData.GetSceneName(curScene), ()=>{
                StartCoroutine(loadScene(SceneData.GetSceneName(type), ()=>{
                    curScene = type;
                    SwitchActiveScene(type);
                }));
            }));
        }
    }

    IEnumerator loadScene(string sceneName, Action callback) {
        if(sceneName != SceneData.GetSceneName(SceneData.SceneType.None)) {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!asyncOp.isDone) {
                yield return null;
            }
        }
        callback();
    }

    IEnumerator unloadScene(string sceneName, Action callback) {
        if(sceneName != SceneData.GetSceneName(SceneData.SceneType.None)) {
            AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(sceneName);
            while(!asyncOp.isDone) {
                yield return null;
            }
        }
        callback();
    }

    public void SwitchActiveScene(SceneData.SceneType type)
    {
        SceneManager.SetActiveScene(
            SceneManager.GetSceneByName(SceneData.GetSceneName(type))
        );
    }
}
