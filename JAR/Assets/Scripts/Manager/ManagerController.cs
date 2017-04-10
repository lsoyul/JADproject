using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerController : MonoBehaviour {

    void Awake()
    {
        this.gameObject.name = "[System]ManagerController";
        DontDestroyOnLoad(this.gameObject);

        InitManager();
    }

    public void StartGame()
    {
        GameSceneManager.Instance.GotoScene(EGameScene.BATTLESTAGE);
    }
	

    private void InitManager()
    {
        GameSceneManager.Create();
    }
}
