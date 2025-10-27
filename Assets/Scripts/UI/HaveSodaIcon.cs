using UnityEngine;
using UnityEngine.UI;

public class HaveSodaIcon : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Sprite isCarryingSoda;
    [SerializeField] private Sprite isntCarryingSoda;

    // Update is called once per frame
    void Update()
    {
        if (player.GetIsCarryingSoda())
        {
            GetComponent<Image>().sprite = isCarryingSoda;
        }
        else
        {
            GetComponent<Image>().sprite = isntCarryingSoda;
        }
    }
}
