using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Update()
    {
        GetComponent<Text>().text = "x" + player.GetLifes().ToString();
    }
}
