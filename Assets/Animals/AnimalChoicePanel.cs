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
    [SerializeField] List<Tile> placedPoints;
    [SerializeField] ParticleSystem buildAnimVFX;
    [SerializeField] float buildAnimWait = 2f;
    [SerializeField] float SpawnOffset_Y = 1f;

    Bank bank;
    PathFinder pathFinder;
    
    void Start() 
    {
        bank = FindObjectOfType<Bank>();
        placedPoints = new List<Tile>();
        pathFinder = FindObjectOfType<PathFinder>();

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

/*
 * Returns a bool array whose index
 * - [0]: true if an animal is placed
 * - [1]: true if a dog is placed
 */
    public bool[] PlaceAnimal(Tile tile)
    {
        Vector3 spawnPosition = tile.transform.position + new Vector3(0f, SpawnOffset_Y, 0f);

        if (bank == null) return new bool[2]{false, false};

        if (isChicken && bank.CurrentBalance >= costChicken && !pathFinder.WillBlockPath(tile.Coordinates))
        {
            StartCoroutine(BuildAnimalEffect(spawnPosition));
            Instantiate(chicken, spawnPosition, Quaternion.identity);
            bank.Withdraw(costChicken);
            placedPoints.Add(tile);
            return new bool[2]{true, false};
        }
        else if (isCat && bank.CurrentBalance >= costCat && !pathFinder.WillBlockPath(tile.Coordinates))
        {
            StartCoroutine(BuildAnimalEffect(spawnPosition));
            Instantiate(cat, spawnPosition, Quaternion.identity);
            bank.Withdraw(costCat);
            placedPoints.Add(tile);
            return new bool[2]{true, false};
        }
        else if (isDog && bank.CurrentBalance >= costDog)
        {
            StartCoroutine(BuildAnimalEffect(spawnPosition));
            Instantiate(dog, spawnPosition, Quaternion.identity);
            bank.Withdraw(costDog);
            placedPoints.Add(tile);
            return new bool[2]{true, true};
        }
        else return new bool[2]{false, false};
    }

    public Tile GetPlacedPoint(Vector3 targetPosition)
    {
        foreach(Tile tile in placedPoints)
        {
            if (tile.transform.position == targetPosition)
            {
                placedPoints.Remove(tile);
                return tile;
            }
        }
        return null;
    }

    IEnumerator BuildAnimalEffect(Vector3 spawnPosition)
    {
        ParticleSystem builtVFX;
        do 
        {
            builtVFX = Instantiate(buildAnimVFX, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(buildAnimWait);
        } while (false);
        
        Destroy(builtVFX.gameObject);
    }
}
