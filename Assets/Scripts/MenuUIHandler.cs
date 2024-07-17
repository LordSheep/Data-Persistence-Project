using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public Text BestScoreTextMain;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.LoadBestScore();
            BestScoreTextMain.text = $"Best Score : {MainManager.Instance.bestScorePlayerName} : {MainManager.Instance.bestScore}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // Based on this video (at 2:08): https://www.youtube.com/watch?v=zahrwl1125k
    public void GrabFromInputField(string input)
    {
        MainManager.Instance.playerName = input;
    }
}
