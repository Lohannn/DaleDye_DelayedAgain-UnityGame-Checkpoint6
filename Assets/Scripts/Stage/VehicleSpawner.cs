using System.Collections;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Vehicle Spawn Settings")]
    [SerializeField] private float spawnInterval;

    private VehiclePool pool;
    private PowerUpPool powerUpPool;

    private void Start()
    {
        pool = GameObject.Find("VehiclePoolManager").GetComponent<VehiclePool>();
        powerUpPool = GameObject.Find("PowerUpPoolManager").GetComponent<PowerUpPool>();

        StartCoroutine(SpawnVehicleCoroutine());
    }

    private IEnumerator SpawnVehicleCoroutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        GameObject vehicle = pool.GetRandomVehicle(transform.position.x);
        if (Random.Range(0, 3) == 0)
        {
            powerUpPool.GetRandomPowerUp(vehicle.GetComponent<Car>().GetPowerUpPosition());
        }

        StartCoroutine(SpawnVehicleCoroutine());
    }
}
