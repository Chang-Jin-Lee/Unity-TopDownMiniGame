using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainCanvas : MonoSingleton<mainCanvas>
{
    [SerializeField] private GameObject MenuScene;
    [SerializeField] private GameObject PlayScene;
    [SerializeField] private GameObject EndScene;
    [SerializeField] private GameObject OptionMenu;

    [SerializeField] private GameState gameState;
    private TextMeshProUGUI RemainEnemyText;

    public enum eSceneState
    {
        MenuScene,
        PlayScene,
        EndScene
    }
    public eSceneState curSceneState;

    private void Start()
    {
        Initialize_MenuScene();
        Initialize_PlayScene();
        Initialize_EndScene();
        Initialize_OptionMenu();
        OnVisibleUI(eSceneState.MenuScene);
    }

    #region InitializeUI
    void Initialize_MenuScene()
    {
        Transform startButtonObj = MenuScene.transform.Find("StartButton");
        startButtonObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.PlayScene));
        Transform OptionButtonObj = MenuScene.transform.Find("OptionButton");
        OptionButtonObj.GetComponent<Button>().onClick.AddListener(() => ShowOption());
        Transform ExitButtonObj = MenuScene.transform.Find("ExitButton");
        ExitButtonObj.GetComponent<Button>().onClick.AddListener(() => ShutDown());
    }
    
    void Initialize_PlayScene()
    {
        Transform OptionButton_PlaySceneObj = PlayScene.transform.Find("TopUI").transform.Find("OptionButton");
        OptionButton_PlaySceneObj.GetComponent<Button>().onClick.AddListener(() => ShowOption());

        Transform RemainEnemyText_PlaySceneObj = PlayScene.transform.Find("TopUI").transform.Find("TotalEnemyText");
        RemainEnemyText = RemainEnemyText_PlaySceneObj.GetComponent<TextMeshProUGUI>();
        RemainEnemyText.SetText("");
        gameState.UIEnemyCount += SetEnemyCountUI;
    }
    
    void Initialize_EndScene()
    {
        Transform TitleButton_EndSceneObj = EndScene.transform.Find("TitleButton");
        TitleButton_EndSceneObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.MenuScene));
    }

    void Initialize_OptionMenu()
    {
        Transform ReturnButton_OptionObj = OptionMenu.transform.Find("ReturnButton");
        ReturnButton_OptionObj.GetComponent<Button>().onClick.AddListener(() => HideOption());
        Transform ExitButton_OptionObj = OptionMenu.transform.Find("ExitButton");
        ExitButton_OptionObj.GetComponent<Button>().onClick.AddListener(() => ShutDown());
        Transform TitleButton_OptionObj = OptionMenu.transform.Find("TitleButton");
        TitleButton_OptionObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.MenuScene));
        Transform EndButton_OptionObj = OptionMenu.transform.Find("EndButton");
        EndButton_OptionObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.EndScene));
    }
    #endregion 

    private void SetEnemyCountUI(int obj)
    {
        RemainEnemyText.text = $"{obj}";
    }

    string GetSceneStateToString(eSceneState state)
    {
        switch (state)
        {
            case eSceneState.MenuScene:
                return "MenuScene";
            case eSceneState.PlayScene:
                return "PlayScene";
            case eSceneState.EndScene:
                return "EndScene";
            default:
                return "default";
        }
    }

    GameObject GetSceneStateToGameObject(eSceneState state)
    {
        switch (state)
        {
            case eSceneState.MenuScene:
                return MenuScene;
            case eSceneState.PlayScene:
                return PlayScene;
            case eSceneState.EndScene:
                return EndScene;
            default:
                return null;
        }
    }

    void OnVisibleUI(eSceneState state)
    {
        MenuScene.SetActive(false);
        PlayScene.SetActive(false);
        EndScene.SetActive(false);
        OptionMenu.SetActive(false);

        switch (state)
        {
            case eSceneState.MenuScene:
                Time.timeScale = 1f;
                MenuScene.SetActive(true);
                break;
            case eSceneState.PlayScene:
                PlayScene.SetActive(true);
                break;
            case eSceneState.EndScene:
                Time.timeScale = 1f;
                EndScene.SetActive(true);
                break;
            default:
                break;
        }

        curSceneState = state;
    }

    void AddScore(int value)
    {
        TextMeshPro scoreText = GetComponentInChildren<TextMeshPro>();
        scoreText.text = value.ToString();
    }

    void ChangeScene(eSceneState state)
    {
        OnVisibleUI(state);
        SceneManager.LoadScene(GetSceneStateToString(state));
    }

    void ShowOption()
    {
        GetSceneStateToGameObject(curSceneState).SetActive(false);
        Time.timeScale = 0f;
        OptionMenu.SetActive(true);
    }

    void HideOption()
    {
        GetSceneStateToGameObject(curSceneState).SetActive(true);
        Time.timeScale = 1f;
        OptionMenu.SetActive(false);
    }

    void ShutDown()
    {
        print("Quit");
        Application.Quit();
    }
}
