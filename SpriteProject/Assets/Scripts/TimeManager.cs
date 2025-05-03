using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TimeManager : MonoSingleton<TimeManager>
{
    public int MaxTimeCount = 30;
    [FormerlySerializedAs("TimeCount")] public double TimeNext = 0;
    [FormerlySerializedAs("TimeCount")] public int TimeCur = 0;

    [SerializeField] private GameState gameState;
    [SerializeField] private MainCanvas mainCanvas;

    private void Start()
    {
        TimeCur = MaxTimeCount;
        TimeNext = Time.timeAsDouble;
    }

    private void FixedUpdate()
    {
        if (Time.timeAsDouble - TimeNext > 1.0)
        {
            TimeCur -= 1;
            gameState.TimeCount = TimeCur;
            if (TimeCur <= 0)
            {
                if (gameState.curSceneState == eSceneState.PlayScene)
                {
                    mainCanvas.ChangeScene(eSceneState.EndScene);
                }
            }
            TimeNext = Time.timeAsDouble;
        }
    }
}
