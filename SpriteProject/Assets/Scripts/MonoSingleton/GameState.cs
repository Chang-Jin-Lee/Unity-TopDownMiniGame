using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}

public enum eSceneState
{
    MenuScene,
    PlayScene,
    EndScene,
    MAX
}

public enum eCharacterState
{
    Midori,
    Momoi,
    Max,
}

public enum eLive2DState
{
    Start,
    Idle,
    Maid_Start,
    Maid_Idle,
    Max
}

public class GameState : MonoSingleton<GameState>
{
    public string PlayerState { get; private set; } = "Idle";
    public int RemainEnemyCount { get; private set; } = 0;
    public int RemainTimeCount{ get; private set; } = 0;
    public int KillEnemyCount { get; private set; } = 0;
    public eSceneState curSceneState{ get; private set; } = eSceneState.MenuScene;
    public eCharacterState curCharacterState{ get; private set; } = eCharacterState.Midori;

    #region GameStateFunctions
    public static string GetSceneStateToString(eSceneState state)
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
    #endregion
    #region DelegateFunctions
    public Action<int> OnEnemyCount;
    public Action<int> OnTimeCount;
    public Action<int> OnKillCount;
    public int EnemyCount
    {
        get => RemainEnemyCount;
        set
        {
            RemainEnemyCount = value;
            OnEnemyCount?.Invoke(value);
        }
    }
    
    public int TimeCount
    {
        get => RemainTimeCount;
        set
        {
            RemainTimeCount = value;
            OnTimeCount?.Invoke(value);
        }
    }
    
    public int KillCount
    {
        get => KillEnemyCount;
        set
        {
            KillEnemyCount = value;
            OnKillCount?.Invoke(value);
        }
    }
    
    #endregion
    #region Setter
    public void SetPlayerState(string newState)
    {
        PlayerState = newState;
    }
    public void AddRemainEnemyCount(int newRemainEnemyCount)
    {
        RemainEnemyCount = newRemainEnemyCount;
    }
    
    public void AddKillEnemyCount(int newKillEnemyCount)
    {
        KillEnemyCount = newKillEnemyCount;
    }
    public void SetRemainTimeCount(int newRemainTimeCount)
    {
        RemainTimeCount = newRemainTimeCount;
    }
    public void SetGameState(eSceneState newState)
    {
        curSceneState = newState;
    }
    public void SetCharacterState(eCharacterState newState)
    {
        curCharacterState = newState;
    }
    #endregion
}
