using System.Collections;
using UnityEngine;

public class LightningWarning : MonoBehaviour
{
    [SerializeField] [Range(1f, 5f)] private float strikeDelayTime;
    [SerializeField] private GameObject lightningGameObject;

    void Start()
    {
        StartCoroutine(StrikeDelay());
    }
    
    private IEnumerator StrikeDelay()
    {
        yield return new WaitForSeconds(strikeDelayTime);
        Instantiate(lightningGameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
