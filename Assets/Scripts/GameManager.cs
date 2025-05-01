using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BallController ballController;
    PlayerController playerController;
    int highScore;

    public bool RoundStarted { get; set; }

    public GameObject bricksParent;
    public TextMeshProUGUI livesDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public TextMeshProUGUI scoreDisplay;

    void Start()
    {
        ballController = FindFirstObjectByType<BallController>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        SetDisplaysText();
        SetHighScore();
        ResetGame();
    }

    private void SetDisplaysText()
    {
        livesDisplay.text = $"Lives: {playerController.Lives}";
        highScoreDisplay.text = $"High score: {highScore}";
        scoreDisplay.text = $"Score: {playerController.Score}";
    }

    public void ResetRound()
    {
        playerController.Lives--;
        RoundStarted = false;
    }

    private void ResetGame()
    {
        if (playerController.Lives < 0)
        {
            ballController.ResetProperties();
            playerController.ResetStats();
            playerController.ResetProperties();

            ReenableBricks();
        }
    }

    private void ReenableBricks()
    {
        for (int i = 0; i < bricksParent.transform.childCount; i++)
        {
            Transform row = bricksParent.transform.GetChild(i);

            for (int j = 0; j < row.childCount; j++)
            {
                GameObject brick = row.GetChild(j).gameObject;
                if (!brick.activeSelf) brick.SetActive(true);
            }
        }
    }

    private void SetHighScore()
    {
        if (playerController.Score > highScore) highScore = playerController.Score;
    }

}
