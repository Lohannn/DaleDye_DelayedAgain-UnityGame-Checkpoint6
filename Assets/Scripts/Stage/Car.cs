using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float speed;
    [SerializeField] private Transform powerUpPosition;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
    }

    public Transform GetPowerUpPosition()
    {
        return powerUpPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("VehicleLimit"))
        {
            gameObject.SetActive(false);
        }
    }
}
