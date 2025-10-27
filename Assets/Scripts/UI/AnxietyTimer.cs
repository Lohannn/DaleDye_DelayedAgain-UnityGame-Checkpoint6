using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnxietyTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float startMinutes;
    [SerializeField] private float startSeconds;

    private MainCanvas mainCanvas;

    private float minutes;
    private float seconds = 0;
    private float miliseconds = 0;

    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<MainCanvas>();

        minutes = startMinutes;
        seconds = startSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds);
        transform.Find("TextTimer").GetComponent<Text>().text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds);
    }

    public IEnumerator StartTimer()
    {
        while (minutes > 0 || seconds > 0 || miliseconds > 0)
        {
            if (miliseconds <= 0)
            {
                miliseconds = 99;

                if (seconds <= 0)
                {
                    seconds = 59;

                    if (minutes > 0)
                    {
                        minutes--;
                    }
                }
                else
                {
                    seconds--;
                }
            }
            else
            {
                miliseconds--;
            }

            yield return new WaitForSeconds(0.01f);
        }

        if (minutes == 0 && seconds == 0 && miliseconds == 0)
        {
            mainCanvas.StageLose();
        }
    }
}
