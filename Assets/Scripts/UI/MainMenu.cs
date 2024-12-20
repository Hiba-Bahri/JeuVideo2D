using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;

    private void Awake()
    {
        mainMenuScreen.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
