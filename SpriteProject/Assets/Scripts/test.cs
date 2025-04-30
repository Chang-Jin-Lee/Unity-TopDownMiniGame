using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(SceneManager.GetActiveScene().name == "PlayScene")
            {
                SceneManager.LoadScene("EndScene");
            }
            else if (SceneManager.GetActiveScene().name == "EndScene")
            {
                SceneManager.LoadScene("PlayScene");
            }
        }
    }
}
