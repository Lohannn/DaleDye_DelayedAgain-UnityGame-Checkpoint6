using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [Header("Item Images")]
    [SerializeField] private Sprite sugarImage;
    [SerializeField] private Sprite beerImage;
    [SerializeField] private Sprite sodaImage;

    public const string SUGAR = "Sugar";
    public const string BEER = "Beer";
    public const string SODA = "Soda";

    private Image currentItemImage;

    private void Start()
    {
        currentItemImage = GetComponent<Image>();
        currentItemImage.sprite = null;
        currentItemImage.color = new Color32(255, 255, 255, 0);
    }

    public string DisplayItem(string itemName)
    {
        switch (itemName)
        {
            case "Sugar":
                currentItemImage.sprite = sugarImage;
                currentItemImage.color = new Color32(255, 255, 255, 255);
                return SUGAR;
            case "Beer":
                currentItemImage.sprite = beerImage;
                currentItemImage.color = new Color32(255, 255, 255, 255);
                return BEER;
            case "Soda":
                currentItemImage.sprite = sodaImage;
                currentItemImage.color = new Color32(255, 255, 255, 255);
                return SODA;
            default:
                return null;
        }
    }

    public string ClearItem()
    {
        currentItemImage.sprite = null;
        currentItemImage.color = new Color32(255, 255, 255, 0);
        return null;
    }
}
