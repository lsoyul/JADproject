using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameScene
{
    NONE,
    TITLE,
    VILLAGE,
    BATTLESTAGE,
    TEST
}

public class GameSceneManager : ManagerTemplate<GameSceneManager> {

    private EGameScene currentScene = EGameScene.NONE;
    public EGameScene CurrentScene { get { return currentScene; } }

    public void GotoScene(EGameScene scene)
    {
        switch(scene)
        {
            case EGameScene.NONE:
                break;
            case EGameScene.TITLE:
                break;
            case EGameScene.VILLAGE:
                break;
            case EGameScene.BATTLESTAGE:
                StartCoroutine("AsynchronousLoad", "scene_gaming");
                break;
            case EGameScene.TEST:
                break;
        }
    }
    
    IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while(!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (ao.progress == 0.9f)
            {
                //Debug.Log("Press a key to start");
                //if (Input.anyKeyDown)
                    ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
