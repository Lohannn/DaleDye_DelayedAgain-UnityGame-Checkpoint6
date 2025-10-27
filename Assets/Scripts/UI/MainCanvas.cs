using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;

    private float currentTimeScale;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (Time.timeScale != 0 || pausePanel.activeSelf))
        {
            if (Time.timeScale != 0)
            {
                currentTimeScale = Time.timeScale;
            }

            pausePanel.SetActive(!pausePanel.activeSelf);
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = currentTimeScale;
            }
        }
    }

    public void StageWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void StageLose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = currentTimeScale;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CityStage");
    }

    public void MainTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainTitle");
    }
}
