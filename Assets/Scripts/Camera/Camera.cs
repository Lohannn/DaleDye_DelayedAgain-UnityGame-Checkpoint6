using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Player target;
    [SerializeField] private float leftLimit, rightLimit, topLimit, bottomLimit;


    private float x;
    private float y;

    void Update()
    {
        //CameraFollow();
    }

    private void FixedUpdate()
    {
        CameraFollow();

    }

    private void CameraFollow()
    {
        x = Mathf.Clamp(target.transform.position.x, leftLimit, rightLimit);
        y = Mathf.Clamp(target.transform.position.y, bottomLimit, topLimit);

        Vector3 cameraPosition = new(x, y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraPosition, 
            Time.deltaTime * (target.GetCurrentSpeed() - 1.0f));
    } 
}
