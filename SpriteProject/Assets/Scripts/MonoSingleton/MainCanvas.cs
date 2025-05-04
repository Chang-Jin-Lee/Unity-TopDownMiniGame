using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Live2DStateGroup
{
    public GameObject[] live2DAnimations = new GameObject[(int)eLive2DState.Max];
}

public class MainCanvas : MonoSingleton<MainCanvas>
{
    [SerializeField] private GameObject MenuScene;
    [SerializeField] private GameObject PlayScene;
    [SerializeField] private GameObject EndScene;
    [SerializeField] private GameObject OptionMenu;

    [SerializeField] private TimeManager timeManager;

    [SerializeField] private GameState gameState;
    private TextMeshProUGUI RemainEnemyText;
    private TextMeshProUGUI RemainTimeText;
    private TextMeshProUGUI KillEnemyCountText;
    
    [SerializeField]
    private Live2DStateGroup[] live2DGroups = new Live2DStateGroup[(int)eCharacterState.Max];

    [SerializeField]
    private Texture2D[] cursors = new Texture2D[(int)eCharacterState.Max];

    #region OnValidate()

#if UNITY_EDITOR
    private void OnValidate()
    {
        int targetSize = (int)eCharacterState.Max;

        if (live2DGroups == null || live2DGroups.Length != targetSize)
        {
            Array.Resize(ref live2DGroups, targetSize);

            // 배열 안의 null 요소 초기화
            for (int i = 0; i < live2DGroups.Length; i++)
            {
                if (live2DGroups[i] == null)
                    live2DGroups[i] = new Live2DStateGroup();
            }

            // 변경 감지를 위해 에디터에 알림
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    #endregion
    private void Start()
    {
        Initialize_MenuScene();
        Initialize_PlayScene();
        Initialize_EndScene();
        Initialize_OptionMenu();
        OnVisibleUI(eSceneState.MenuScene);
        ShowLive2D(gameState.curCharacterState, eLive2DState.Start);
        UpdateCursor();
    }

    #region InitializeUI
    void Initialize_MenuScene()
    {
        Transform startButton_MenuSceneObj = MenuScene.transform.Find("StartButton");
        startButton_MenuSceneObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.PlayScene));
        Transform OptionButton_MenuSceneObj = MenuScene.transform.Find("OptionButton");
        OptionButton_MenuSceneObj.GetComponent<Button>().onClick.AddListener(() => ShowOption());
        Transform ExitButton_MenuSceneObj = MenuScene.transform.Find("ExitButton");
        ExitButton_MenuSceneObj.GetComponent<Button>().onClick.AddListener(() => ShutDown());
        
        Transform MidoriButton_MenuSceneObj = MenuScene.transform.Find("MidoriButton");
        MidoriButton_MenuSceneObj.GetComponent<Button>().onClick.AddListener(() => CharacterSelected(eCharacterState.Midori));
        Transform MomoiButton_MenuSceneObj = MenuScene.transform.Find("MomoiButton");
        MomoiButton_MenuSceneObj.GetComponent<Button>().onClick.AddListener(() => CharacterSelected(eCharacterState.Momoi));
    }
    
    void Initialize_PlayScene()
    {
        Transform OptionButton_PlaySceneObj = PlayScene.transform.Find("TopUI").transform.Find("OptionButton");
        OptionButton_PlaySceneObj.GetComponent<Button>().onClick.AddListener(() => ShowOption());

        Transform RemainEnemyText_PlaySceneObj = PlayScene.transform.Find("TopUI").transform.Find("TotalEnemyText");
        RemainEnemyText = RemainEnemyText_PlaySceneObj.GetComponent<TextMeshProUGUI>();
        RemainEnemyText.SetText("");
        gameState.OnEnemyCount += SetEnemyCountUI;
        
        Transform RemainTimeText_PlaySceneObj = PlayScene.transform.Find("TopUI").transform.Find("RemainTimeText");
        RemainTimeText = RemainTimeText_PlaySceneObj.GetComponent<TextMeshProUGUI>();
        RemainTimeText.SetText("");
        gameState.OnTimeCount += SetTimeCountUI;
    }
    
    void Initialize_EndScene()
    {
        Transform TitleButton_EndSceneObj = EndScene.transform.Find("TitleButton");
        TitleButton_EndSceneObj.GetComponent<Button>().onClick.AddListener(() => ChangeScene(eSceneState.MenuScene));
        
        Transform KillEnemyCountText_PlaySceneObj = EndScene.transform.Find("KillEnemyText");
        KillEnemyCountText = KillEnemyCountText_PlaySceneObj.GetComponent<TextMeshProUGUI>();
        KillEnemyCountText.SetText("죽인 적 수 : 0");
        gameState.OnKillCount += SetKillCountUI;
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
    
    private void SetEnemyCountUI(int obj)
    {
        RemainEnemyText.text = $"{obj}";
    }
    
    private void SetTimeCountUI(int obj)
    {
        RemainTimeText.text = $"{obj}";
    }
    
    private void SetKillCountUI(int obj)
    {
        KillEnemyCountText.text = $"죽인 적 수 : {obj}";
    }
    #endregion 

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

    void OnVisibleUI(eSceneState state) // UI 전환 
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
                gameState.EnemyCount = 0;
                gameState.KillCount = 0;
                ShowLive2D(gameState.curCharacterState, eLive2DState.Start);
                break;
            case eSceneState.PlayScene:
                PlayScene.SetActive(true);
                gameState.EnemyCount = 0;
                gameState.KillCount = 0;
                break;
            case eSceneState.EndScene:
                Time.timeScale = 1f;
                EndScene.SetActive(true);
                ShowLive2D(gameState.curCharacterState, eLive2DState.Maid_Start);
                break;
            default:
                break;
        }

        gameState.SetGameState(state);
    }

    void AddScore(int value)
    {
        TextMeshPro scoreText = GetComponentInChildren<TextMeshPro>();
        scoreText.text = value.ToString();
    }

    public void ChangeScene(eSceneState state)  // 씬 전환 
    {
        OnVisibleUI(state);
        SceneManager.LoadScene(GameState.GetSceneStateToString(state));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // 씬이 로드된 다음에 실행 
    {
        // 씬이 로드된 뒤에 실행되는 콜백
        if (scene.name == "PlayScene")
        {
            timeManager.Initialize(); // 초기화
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // 콜백 해제 (중복 방지)
    }

    void ShowOption()
    {
        GetSceneStateToGameObject(gameState.curSceneState).SetActive(false);
        Time.timeScale = 0f;
        OptionMenu.SetActive(true);
    }

    void HideOption()
    {
        GetSceneStateToGameObject(gameState.curSceneState).SetActive(true);
        Time.timeScale = 1f;
        OptionMenu.SetActive(false);
    }

    void ShutDown()
    {
        Application.Quit();
    }

    void CharacterSelected(eCharacterState newState)    // 캐릭터 선택했을 때 실행될 함수 
    {
        switch (newState)
        {
            case eCharacterState.Midori:
                gameState.SetCharacterState(eCharacterState.Midori);
                ShowLive2D(eCharacterState.Midori, eLive2DState.Start);
                break;
            case eCharacterState.Momoi:
                gameState.SetCharacterState(eCharacterState.Momoi);
                ShowLive2D(eCharacterState.Momoi, eLive2DState.Start);
                break;
            default:
                break;
        }

        UpdateCursor();
    }

    void ShowLive2D(eCharacterState characterState, eLive2DState live2DState)   // 캐릭터, AnimationState에 맞는 Live2D 재생 
    {
        foreach (var live2DGroup in live2DGroups)
        {
            if(live2DGroup == null) continue;
            foreach (var animation in live2DGroup.live2DAnimations)
            {
                if (animation != null)
                {
                    animation.SetActive(false);
                }
            }
        }
        live2DGroups[(int)characterState].live2DAnimations[(int)live2DState].SetActive(true);
    }
    
    void UpdateCursor() // 커서 이미지를 업데이트 
    {
        Texture2D cursorTex = cursors[(int)gameState.curCharacterState];
        Vector2 hotspot = new Vector2(cursorTex.width / 2f, cursorTex.height / 2f);
        if (cursorTex != null)
        {
            Cursor.SetCursor(cursorTex, hotspot, CursorMode.Auto);
            Cursor.visible = true;
        }
    }
}
