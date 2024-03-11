using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lightning : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 2f)]
    private float lifetime;

    public static event Action<Vector3> LightningStrikeEvent;
    
    void Start()
    {
        LightningStrikeEvent?.Invoke(transform.position);
        StartCoroutine(Expire());
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entityScript = other.GetComponent<Entity>();
        if (entityScript != null)
        {
            entityScript.Die();
        }
    }
}
