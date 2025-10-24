using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("VehicleLimit"))
        {
            gameObject.SetActive(false);
        }
    }
}
