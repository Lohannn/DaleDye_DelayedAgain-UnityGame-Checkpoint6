using System.Collections;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Vehicle Spawn Settings")]
    [SerializeField] private float spawnInterval;

    private VehiclePool pool;

    private void Start()
    {
        pool = GameObject.Find("VehiclePoolManager").GetComponent<VehiclePool>();

        StartCoroutine(SpawnVehicleCoroutine());
    }

    private IEnumerator SpawnVehicleCoroutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        pool.GetRandomVehicle(transform.position.x);

        StartCoroutine(SpawnVehicleCoroutine());
    }
}
