using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("Progress Bar Settings")]
    [SerializeField] private float finalXPosition;
    [SerializeField] private Transform player;

    private float currentYPosition;

    private Slider progress;

    private void Start()
    {
        progress = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentYPosition = player.position.x;
        progress.value = currentYPosition / finalXPosition;
    }
}
