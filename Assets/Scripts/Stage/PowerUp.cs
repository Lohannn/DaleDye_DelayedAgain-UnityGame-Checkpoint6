using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeToInvert;
    
    void Start()
    {
        StartCoroutine(InvertCoroutine());
    }
    
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private IEnumerator InvertCoroutine()
    {
        yield return new WaitForSeconds(timeToInvert);
        speed = -speed;

        StartCoroutine(InvertCoroutine());
    }
}
