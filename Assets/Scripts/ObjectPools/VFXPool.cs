using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    [Header("Pool Stock")]
    [SerializeField] private GameObject sodaSplashPrefab;
    private GameObject[] sodaSplashPool = new GameObject[2];

    private void Start()
    {
        InitializeSodaSplashPool();
    }

    private void InitializeSodaSplashPool()
    {
        for (int i = 0; i < sodaSplashPool.Length; i++)
        {
            sodaSplashPool[i] = Instantiate(sodaSplashPrefab);
            sodaSplashPool[i].SetActive(false);
        }
    }

    public GameObject GetPooledSodaSplash(Vector3 position, float rotation)
    {
        foreach (var splash in sodaSplashPool)
        {
            splash.transform.position = position;
            splash.transform.eulerAngles = new Vector3(0, 0, rotation);
            splash.SetActive(true);
            return splash;
        }

        return Instantiate(sodaSplashPrefab);
    }

    public GameObject GetPooledSodaSplash(Transform parent)
    {
        foreach (var splash in sodaSplashPool)
        {
            splash.transform.parent = parent;
            splash.transform.localPosition = Vector3.zero;
            splash.transform.eulerAngles = new Vector3(0, 0, 90);
            splash.SetActive(true);
            return splash;
        }

        return Instantiate(sodaSplashPrefab);
    }
}
