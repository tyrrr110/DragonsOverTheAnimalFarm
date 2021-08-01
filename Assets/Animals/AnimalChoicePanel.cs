using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalChoicePanel : MonoBehaviour
{
    public bool isChicken;
    public bool isCat;
    public bool isDog;
    [SerializeField] int costChicken = 75;
    [SerializeField] int costCat = 50;
    [SerializeField] int costDog = 25;
    [SerializeField] Animal chicken;
    [SerializeField] Animal cat;
    [SerializeField] Animal dog;
    Bank bank;
    [SerializeField] List<Waypoint> placedPoints;
    
    void Start() 
    {
        bank = FindObjectOfType<Bank>();
        placedPoints = new List<Waypoint>();

        // // obj attached should follow singleton pattern
        // int numAnimChoicePanel = FindObjectsOfType<AnimalChoicePanel>().Length;

        // if (numAnimChoicePanel > 1) 
        // {
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     DontDestroyOnLoad(gameObject);
        // }
    }

    public bool PlaceAnimal(Waypoint waypoint)
    {
        if (bank == null) return false;

        if (isChicken && bank.CurrentBalance >= costChicken)
        {
            Instantiate(chicken, waypoint.transform.position, Quaternion.identity);
            bank.Withdraw(costChicken);
            placedPoints.Add(waypoint);
            return true;
        }
        else if (isCat && bank.CurrentBalance >= costCat)
        {
            Instantiate(cat, waypoint.transform.position, Quaternion.identity);
            bank.Withdraw(costCat);
            placedPoints.Add(waypoint);
            return true;
        }
        else if (isDog && bank.CurrentBalance >= costDog)
        {
            Instantiate(dog, waypoint.transform.position, Quaternion.identity);
            bank.Withdraw(costDog);
            placedPoints.Add(waypoint);
            return true;
        }
        else return false;
    }

    public Waypoint GetPlacedPoint(Vector3 targetPosition)
    {
        foreach(Waypoint waypoint in placedPoints)
        {
            if (waypoint.transform.position == targetPosition)
            {
                placedPoints.Remove(waypoint);
                return waypoint;
            }
        }
        return null;
    }
}
