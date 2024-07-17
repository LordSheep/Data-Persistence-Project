using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Renamed script from MainManager to GameManager following Yuzu Tamaki's comment advice
// (I was already aware that the MainManager script acted more like a GameManager before reading his comment)
public class GameManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.LoadBestScore();
            BestScoreText.text = $"Best Score : {MainManager.Instance.bestScorePlayerName} : {MainManager.Instance.bestScore}";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > MainManager.Instance.bestScore)
        {
            UpdateBestScore();

            // Update the best score variables saved in MainManager before using SaveBestScore(),
            // as SaveBestScore() uses what is saved in the best score variables to fill
            // the new instance of the SaveData's class members.
            MainManager.Instance.bestScorePlayerName = MainManager.Instance.playerName;
            MainManager.Instance.bestScore = m_Points;

            MainManager.Instance.SaveBestScore();
        }
    }

    public void UpdateBestScore()
    {
        BestScoreText.text = $"Best Score : {MainManager.Instance.playerName} : {m_Points}";
    }
}
