using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartStage()
    {
        SceneManager.LoadScene("CityStage");
    }
}
