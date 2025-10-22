using System.Collections;
using UnityEngine;

public class SodaSplash : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(SplashRoutine(0.5f));
    }

    private IEnumerator SplashRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.eulerAngles = Vector3.zero;
        transform.parent = null;
        gameObject.SetActive(false);
    }
}
