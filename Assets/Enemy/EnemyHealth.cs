using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 1500;
    int currentHitPoints = 0;
    [Tooltip("Adds amount to max hit points when the enemy dies")]
    [SerializeField] int difficultyRamp = 500;
    [SerializeField] int damagePerHitByChicken;
    [SerializeField] int damagePerHitByCat;
    [SerializeField] int damagePerHitByDog;
    [SerializeField] [Range(0f, 10f)] float waitByDog = 3f;
    public float WaitByDog { get{ return waitByDog; } }
    Enemy enemy;
    EnemyMover mover;
    AnimalChoicePanel panel;

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void Start() {
        enemy = FindObjectOfType<Enemy>();
        mover = GetComponent<EnemyMover>();
        panel = FindObjectOfType<AnimalChoicePanel>();
    }

    void OnParticleCollision(GameObject other) {
        // Debug.Log($"{name} is hit by {other.name}");
        ProcessHit(other);    
    }

    void ProcessHit(GameObject hitBy)
    {
        switch (hitBy.name)
        {
            case "FireBall": // by chicken
                currentHitPoints -= damagePerHitByChicken;
                break;

            case "Bubble":
                currentHitPoints -= damagePerHitByCat;
                break;

            case "CloudShield":
                currentHitPoints -= damagePerHitByDog;
                mover.SetPauseByDog();
                Transform dogToDestroy = hitBy.transform.parent;
                if (isActiveAndEnabled)
                    StartCoroutine(WaitAndRemove(dogToDestroy));
                else
                    Debug.Log("Detected.");
                break;

            default:
                break;
        }

        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHitPoints += difficultyRamp;
        }
    }

    /*
    * Mainly used to wait for some time and then remove dogs
    */
    IEnumerator WaitAndRemove(Transform toRemove)
    {
        Debug.Log("Starting coroutine");
        yield return new WaitForSeconds(waitByDog);
        if (toRemove != null)
        {
            Destroy(toRemove.gameObject);
            // resets isPlaceable
            Waypoint waypoint = panel.GetPlacedPoint(toRemove.position);
            waypoint.ResetPlaceable();
        }
        Debug.Log("Returning from WaitAndRemove");
    }
}
