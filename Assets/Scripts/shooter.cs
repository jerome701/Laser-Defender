using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVarience = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;

    public bool isFiring;

    Coroutine firingCorutine;
    AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    
    void Start()
    {
        if(useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
       Fire(); 
    }

    void Fire()
    {
        if (isFiring && firingCorutine == null)
        {
            firingCorutine = StartCoroutine(FireContinuously());
        }
        else if(!isFiring && firingCorutine != null)
        {
            StopCoroutine(firingCorutine);
            firingCorutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);

            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVarience,
                                            baseFiringRate + firingRateVarience);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }

}
