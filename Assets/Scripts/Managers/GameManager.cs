using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    SFXManager sfxManager;

    public bool GameStarted { get; set; }
    public bool RoundStarted { get; set; }

    public GameObject bricksParent;
    public GameObject startMenu;
    public GameObject gameEndMenu;
    public GameObject pauseMenu;
    public TextMeshProUGUI livesDisplay;
    public TextMeshProUGUI highScoreDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI gameEndMessage;

    void Start()
    {
        Time.timeScale = 1;

        RetrieveComponents();
    }

    void Update()
    {
        SetDisplaysText();
        SetHighScore();
        InterruptGame();
        FinishGame();
        HideStartMenu();
    }

    private void RetrieveComponents()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        sfxManager = FindFirstObjectByType<SFXManager>();
    }

    private void SetDisplaysText()
    {
        if (playerController.Lives >= 0) livesDisplay.text = $"Lives: {playerController.Lives}";
        highScoreDisplay.text = $"Hi-score: {DataManager.instance.HighScore}";
        scoreDisplay.text = $"Score: {playerController.Score}";
    }

    public void ResetRound()
    {
        playerController.Lives--;
        RoundStarted = false;
    }

    private void SetHighScore()
    {
        if (playerController.Score > DataManager.instance.HighScore)
        {
            DataManager.instance.HighScore = playerController.Score;
        }
    }

    private void InterruptGame()
    {
        if (GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.P)) Pause();
            else if (Input.GetKeyDown(KeyCode.R)) ResetGame();
            else if (Input.GetKeyDown(KeyCode.Escape)) Quit();
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void ResetGame()
    {
        if (Time.timeScale == 0)
        {
            SceneManager.LoadScene("Stage");
        }
    } 

    public void Quit()
    {
        if (Time.timeScale == 0)
        {
            Application.Quit();
        }
    }

    private bool HasBlocksRemaining()
    {
        for (int i = 0; i < bricksParent.transform.childCount; i++)
        {
            Transform row = bricksParent.transform.GetChild(i);

            for (int j = 0; j < row.childCount; j++)
            {
                GameObject brick = row.GetChild(j).gameObject;
                if (brick.activeSelf) return true;
            }
        }

        return false;
    }

    private void FinishGame()
    {
        if (!gameEndMenu.activeSelf && (playerController.Lives < 0 || !HasBlocksRemaining()))
        {
            Time.timeScale = 0;

            if (playerController.Lives < 0)
            {
                sfxManager.PlayClip(sfxManager.gameOver);
                gameEndMessage.text = "Game over :(";
            }
            else if (!HasBlocksRemaining())
            {
                sfxManager.PlayClip(sfxManager.win);
                gameEndMessage.text = "You win! :)";
            }

            gameEndMenu.SetActive(true);
        }
    }

    private void HideStartMenu()
    {
        if (GameStarted)
        {
            startMenu.SetActive(false);
        }
    }

}
