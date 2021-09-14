using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } } // set IsPlaceable as a property
    Vector2Int coordinates = new Vector2Int();
    public Vector2Int Coordinates { get {return coordinates;} }
    AnimalChoicePanel animalToPlace;
    GridManager gridManager;
    PathFinder pathFinder;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void Start() 
    {
        animalToPlace = FindObjectOfType<AnimalChoicePanel>();

        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (isPlaceable)
            {
                gridManager.UnblockNode(coordinates);
            }
        }
    } 

    void OnMouseDown() {
        if (gridManager == null) return;

        if (gridManager.GetNode(coordinates).isWalkable) 
        {
            bool[] isPlacedOrDog = animalToPlace.PlaceAnimal(this);
            if (isPlacedOrDog[0] && !isPlacedOrDog[1]) 
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }

    // TODO: UNBLOCK NODE/TILE
    public void UnblockTile()
    {
        if (gridManager == null) return;

        gridManager.UnblockNode(coordinates);
    }
}
