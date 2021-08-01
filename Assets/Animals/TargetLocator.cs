using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] ParticleSystem attackVFX;
    Transform target;

    void Start() 
    {
        attackVFX = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        FindClosestTarget();
        AimAnimal();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach(Enemy target in enemies)
        {
            float targetDistance = Vector3.Distance(target.transform.position, transform.position);
            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                closestTarget = target.transform;
            }
        }
        target = closestTarget;
    }

    void AimAnimal()
    {
        if (target == null)
        {
            Attack(false);
            return;
        }

        float targetDistance = Vector3.Distance(target.transform.position, transform.position); 
        transform.LookAt(target);
    
        if (targetDistance <= range)
        {
            Attack(true);
        }
        else 
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
    //     if (!attackVFX.isPlaying && isActive)
    //         attackVFX.Play();
    //     else if (attackVFX.isPlaying && !isActive)
    //         attackVFX.Stop();

    // Use Emission Module instead
        var emissionModule = attackVFX.emission;
        emissionModule.enabled = isActive;
    }
}
