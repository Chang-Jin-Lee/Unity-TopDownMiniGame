using UnityEngine;
using UnityEngine.Serialization;

public class TimeManager : MonoSingleton<TimeManager>
{
    public int MaxTimeCount = 30;
    [FormerlySerializedAs("TimeCount")] public double TimeNext = 0;
    
    [SerializeField] private GameState gameState;
    [SerializeField] private MainCanvas mainCanvas;

    public void Initialize()
    {
        gameState.TimeCount = MaxTimeCount;
        TimeNext = Time.timeAsDouble;
    }
    private void FixedUpdate()
    {
        if (Time.timeAsDouble - TimeNext > 1.0)
        {
            gameState.TimeCount--;
            if (gameState.TimeCount <= 0)
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
