using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mainCanvas : MonoBehaviour
{
    public static mainCanvas Instance { get; private set; }
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �Ѿ�� �ı����� �ʰ�
        }
        else
        {
            Destroy(gameObject); // �̱��� ����
        }
    }

    private void Update()
    {
        //scoreText = GetComponentInChildren<TextMeshProUGUI>();
        print(enemySpawner.transform.name);
        print(scoreText.transform.name);
        scoreText.text = enemySpawner.GetComponent<EnemySpawner>().enemies.Count.ToString();
    }

    void AddScore(int value)
    {
        TextMeshPro scoreText = GetComponentInChildren<TextMeshPro>();
        scoreText.text = value.ToString();
    }
}
