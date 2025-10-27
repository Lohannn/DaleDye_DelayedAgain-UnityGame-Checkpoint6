using UnityEngine;

public class PowerUpPool : MonoBehaviour
{
    [Header("Pool Stock")]
    [SerializeField] private GameObject sodaPrefab;
    private GameObject[] sodaPool = new GameObject[20];
    [SerializeField] private GameObject beerPrefab;
    private GameObject[] beerPool = new GameObject[20];
    [SerializeField] private GameObject sugarPrefab;
    private GameObject[] sugarPool = new GameObject[20];

    private void Start()
    {
        InitializeSodaPool();
        InitializeBeerPool();
        InitializeSugarPool();
    }

    public GameObject GetRandomPowerUp(Transform parent)
    {
        int powerUp = Random.Range(0, 3);
        switch (powerUp)
        {
            case 0:
                return GetPooledSoda(parent);
            case 1:
                return GetPooledBeer(parent);
            case 2:
                return GetPooledSugar(parent);
            default:
                return GetPooledSoda(parent);
        }
    }

    #region Initializers
    private void InitializeSodaPool()
    {
        for (int i = 0; i < sodaPool.Length; i++)
        {
            sodaPool[i] = Instantiate(sodaPrefab);
            sodaPool[i].SetActive(false);
        }
    }

    private void InitializeBeerPool()
    {
        for (int i = 0; i < beerPool.Length; i++)
        {
            beerPool[i] = Instantiate(beerPrefab);
            beerPool[i].SetActive(false);
        }
    }

    private void InitializeSugarPool()
    {
        for (int i = 0; i < sugarPool.Length; i++)
        {
            sugarPool[i] = Instantiate(sugarPrefab);
            sugarPool[i].SetActive(false);
        }
    }
    #endregion

    #region Getters
    public GameObject GetPooledSoda(Transform parent)
    {
        foreach (var soda in sodaPool)
        {
            if (!soda.activeInHierarchy)
            {
                soda.transform.SetParent(parent);
                soda.transform.position = parent.position;
                soda.SetActive(true);
                return soda;
            }
        }

        print("Instantiating new soda");
        return Instantiate(sodaPrefab);
    }

    public GameObject GetPooledBeer(Transform parent)
    {
        foreach (var beer in beerPool)
        {
            if (!beer.activeInHierarchy)
            {
                beer.transform.SetParent(parent);
                beer.transform.position = parent.position;
                beer.SetActive(true);
                return beer;
            }
        }
        print("Instantiating new beer");
        return Instantiate(beerPrefab);
    }

    public GameObject GetPooledSugar(Transform parent)
    {
        foreach (var sugar in sugarPool)
        {
            if (!sugar.activeInHierarchy)
            {
                sugar.transform.SetParent(parent);
                sugar.transform.position = parent.position;
                sugar.SetActive(true);
                return sugar;
            }
        }
        print("Instantiating new sugar");
        return Instantiate(sugarPrefab);
    }
    #endregion
}
