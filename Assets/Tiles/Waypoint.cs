using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } } // set IsPlaceable as a property
    AnimalChoicePanel animalToPlace;

    void Start() 
    {
        animalToPlace = FindObjectOfType<AnimalChoicePanel>();
    } 

    void OnMouseDown() {
        if (isPlaceable) 
        {
            bool isPlaced = animalToPlace.PlaceAnimal(this);
            isPlaceable = !isPlaced;
        }
    }

    public void ResetPlaceable()
    {
        isPlaceable = true;
    }

}
