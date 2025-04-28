using System.Collections;
using System.Collections.Generic;
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
public class GameManager : MonoSingleton<GameManager>
{
    public int Score { get; private set; } = 0;
    public int Rank { get; private set; } = 0;
    public string PlayerState { get; private set; } = "Idle";


    public void AddScore(int amount)
    {
        Score += amount;
    }
    public void UpdateRank(int newRank)
    {
        Rank = newRank;
    }
    public void SetPlayerState(string newState)
    {
        PlayerState = newState;
    }
}
