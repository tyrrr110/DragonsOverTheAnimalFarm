using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy), typeof(EnemyHealth))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    [SerializeField] Animator dragon;
    int dragon_fly_id;
    Enemy enemy;
    EnemyHealth health;
    bool isPauseByDog = false;

    void OnEnable()
    {
        dragon_fly_id = Animator.StringToHash("Fly");
        dragon.SetBool(dragon_fly_id, true);

        FindPath();
        ReturnToStart();
        StartCoroutine(TravelPath());
    }

    void Start() {
        enemy = GetComponent<Enemy>();
        health = GetComponent<EnemyHealth>();
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator TravelPath()
    {
        foreach(Waypoint waypoint in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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

    void FindPath()
    {
        path.Clear();

        GameObject pathObj = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in pathObj.transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null)
                path.Add(waypoint);
        }
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
