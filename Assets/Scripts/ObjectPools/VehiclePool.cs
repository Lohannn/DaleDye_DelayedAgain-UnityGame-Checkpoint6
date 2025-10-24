using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehiclePool : MonoBehaviour
{
    [Header("Pool Stock")]
    [SerializeField] private GameObject ambulancePrefab;
    private GameObject[] ambulancePool = new GameObject[20];
    [SerializeField] private GameObject truckPrefab;
    private GameObject[] truckPool = new GameObject[20];
    [SerializeField] private GameObject taxiPrefab;
    private GameObject[] taxiPool = new GameObject[20];
    [SerializeField] private GameObject vanPrefab;
    private GameObject[] vanPool = new GameObject[20];
    [SerializeField] private GameObject carPrefab;
    private GameObject[] carPool = new GameObject[20];
    [SerializeField] private GameObject deliverPrefab;
    private GameObject[] deliverPool = new GameObject[20];

    private void Start()
    {
        InitializeAmbulancePool();
        InitializeTruckPool();
        InitializeTaxiPool();
        InitializeVanPool();
        InitializeCarPool();
        InitializeDeliverPool();
    }

    public GameObject GetRandomVehicle(float xPosition)
    {
        int vehicleType = Random.Range(0, 6);
        switch (vehicleType)
        {
            case 0:
                return GetPooledAmbulance(xPosition);
            case 1:
                return GetPooledTruck(xPosition);
            case 2:
                return GetPooledTaxi(xPosition);
            case 3:
                return GetPooledVan(xPosition);
            case 4:
                return GetPooledCar(xPosition);
            case 5:
                return GetPooledDeliver(xPosition);
            default:
                return GetPooledCar(xPosition);
        }
    }

    #region Initializers
    private void InitializeAmbulancePool()
    {
        for (int i = 0; i < ambulancePool.Length; i++)
        {
            ambulancePool[i] = Instantiate(ambulancePrefab);
            ambulancePool[i].SetActive(false);
        }
    }

    private void InitializeTruckPool()
    {
        for (int i = 0; i < truckPool.Length; i++)
        {
            truckPool[i] = Instantiate(truckPrefab);
            truckPool[i].SetActive(false);
        }
    }

    private void InitializeTaxiPool()
    {
        for (int i = 0; i < taxiPool.Length; i++)
        {
            taxiPool[i] = Instantiate(taxiPrefab);
            taxiPool[i].SetActive(false);
        }
    }

    private void InitializeVanPool()
    {
        for (int i = 0; i < vanPool.Length; i++)
        {
            vanPool[i] = Instantiate(vanPrefab);
            vanPool[i].SetActive(false);
        }
    }

    private void InitializeCarPool()
    {
        for (int i = 0; i < carPool.Length; i++)
        {
            carPool[i] = Instantiate(carPrefab);
            carPool[i].SetActive(false);
        }
    }

    private void InitializeDeliverPool()
    {
        for (int i = 0; i < deliverPool.Length; i++)
        {
            deliverPool[i] = Instantiate(deliverPrefab);
            deliverPool[i].SetActive(false);
        }
    }
    #endregion

    #region Getters
    public GameObject GetPooledAmbulance(float xPosition)
    {
        foreach (var ambulance in ambulancePool)
        {
            if (!ambulance.activeInHierarchy)
            {
                ambulance.transform.position = new Vector3(xPosition, ambulance.transform.position.y, ambulance.transform.position.z);
                ambulance.SetActive(true);
                return ambulance;
            }
        }

        print("Instantiating new ambulance");
        return Instantiate(ambulancePrefab);
    }

    public GameObject GetPooledTruck(float xPosition)
    {
        foreach (var truck in truckPool)
        {
            if (!truck.activeInHierarchy)
            {
                truck.transform.position = new Vector3(xPosition, truck.transform.position.y, truck.transform.position.z);
                truck.SetActive(true);
                return truck;
            }
        }
        print("Instantiating new truck");
        return Instantiate(truckPrefab);
    }

    public GameObject GetPooledTaxi(float xPosition)
    {
        foreach (var taxi in taxiPool)
        {
            if (!taxi.activeInHierarchy)
            {
                taxi.transform.position = new Vector3(xPosition, taxi.transform.position.y, taxi.transform.position.z);
                taxi.SetActive(true);
                return taxi;
            }
        }
        print("Instantiating new taxi");
        return Instantiate(taxiPrefab);
    }

    public GameObject GetPooledVan(float xPosition)
    {
        foreach (var van in vanPool)
        {
            if (!van.activeInHierarchy)
            {
                van.transform.position = new Vector3(xPosition, van.transform.position.y, van.transform.position.z);
                van.SetActive(true);
                return van;
            }
        }
        print("Instantiating new van");
        return Instantiate(vanPrefab);
    }

    public GameObject GetPooledCar(float xPosition)
    {
        foreach (var car in carPool)
        {
            if (!car.activeInHierarchy)
            {
                car.transform.position = new Vector3(xPosition, car.transform.position.y, car.transform.position.z);
                car.SetActive(true);
                return car;
            }
        }
        print("Instantiating new car");
        return Instantiate(carPrefab);
    }

    public GameObject GetPooledDeliver(float xPosition)
    {
        foreach (var deliver in deliverPool)
        {
            if (!deliver.activeInHierarchy)
            {
                deliver.transform.position = new Vector3(xPosition, deliver.transform.position.y, deliver.transform.position.z);
                deliver.SetActive(true);
                return deliver;
            }
        }
        print("Instantiating new deliver");
        return Instantiate(deliverPrefab);
    }
    #endregion
}