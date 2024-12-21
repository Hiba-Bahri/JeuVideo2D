using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Level Start Text")]
    [SerializeField] private Text levelStartText;


    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);

        if (levelStartText != null)
        {
            StartCoroutine(ShowLevelStartText("Level " + (SceneManager.GetActiveScene().buildIndex)));
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            PauseGame(true);
        }
    }

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        Time.timeScale = status ? 0 : 1;
    }

    public void SFXVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
    #region Level Start Text
    private IEnumerator ShowLevelStartText(string message)
    {
        levelStartText.text = message;
        levelStartText.color = new Color(levelStartText.color.r, levelStartText.color.g, levelStartText.color.b, 1); // Fully visible

        yield return new WaitForSeconds(1f); // Show for 1 second

        // Fade out over 1 second
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Color originalColor = levelStartText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // Calculate alpha value
            levelStartText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully invisible
        levelStartText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
    #endregion
}
