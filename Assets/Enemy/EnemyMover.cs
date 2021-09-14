using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(EnemyHealth))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    [SerializeField] Animator dragon;
    int dragon_fly_id;
    List<Node> path = new List<Node>();
    Enemy enemy;
    GridManager gridManager;
    PathFinder pathFinder;
    EnemyHealth health;
    bool isPauseByDog = false;

    void OnEnable()
    {
        dragon_fly_id = Animator.StringToHash("Fly");
        dragon.SetBool(dragon_fly_id, true);

        ReturnToStart();
        RecalculatePath(true);
    }

    void Awake() {
        enemy = GetComponent<Enemy>();
        health = GetComponent<EnemyHealth>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    IEnumerator TravelPath()
    {
        if (path.Count <= 1)
        {
            gameObject.SetActive(false);
        }

        // ***starting from i = 1 because dragons are already at their start position*** 
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition); // always faces the endPosition
            while (travelPercent < 1f) // it takes 1 sec to finish traveling if speed is 1
            {
                if (!isPauseByDog)
                {
                    travelPercent += Time.deltaTime * speed; // so the movement is frame rate independent
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    yield return new WaitForEndOfFrame();
                }
                else {
                    SetPauseByDog();
                    yield return new WaitForSeconds(health.WaitByDog);
                }
            }
        }
        FinishPath();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int currentCoordinates = new Vector2Int();
        if (resetPath)
        {
            currentCoordinates = pathFinder.StartCoordinates;
        }
        else
        {
            currentCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(currentCoordinates);
        
        // ***so that the coroutine will STOP before getting a new path***
        StartCoroutine(TravelPath()); 
    }

    public void SetPauseByDog()
    {
        isPauseByDog = !isPauseByDog;
    }
    
    void FinishPath()
    {
        gameObject.SetActive(false);
        enemy.StealGold();
    }
}
