using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Префаб игрока")]
    [SerializeField] private GameObject _playerPrefab; 
    [Tooltip("Префаб волка")]
    [SerializeField] private GameObject _wolfPrefab; 
    [Tooltip("Префаб лани")]
    [SerializeField] private GameObject _doePrefab; 
    [Tooltip("Префаб зайца")]
    [SerializeField] private GameObject _rabbitPrefab; 

    [Header("Arrays Units")]
    [Tooltip("Лист (массив) всей живых существ на сцене")]
    [SerializeField] internal List<Transform> alives; 
    [Tooltip("Лист (массив) всех живых существ, которые могут группироваться")]
    [SerializeField] internal List<GroupedAnimal> groupedAnimals;
    [Tooltip("Массив точек пути на сцене")]
    [SerializeField] private Transform[] _wayPoints; 

    [Header("Options")]
    [Tooltip("В этом объекте будут находится все животные, как дочерние")]
    [SerializeField] private Transform _spawnTransform; 
    [Tooltip("Количество генерируемых волков")]
    [SerializeField] private int _countWolfs = 1;
    [Tooltip("Количество генерируемых групп ланей")]
    [SerializeField] private int _countGroupsDoes = 1; 
    [Tooltip("Количество генерируемых зайцев")]
    [SerializeField] private int _countRabbits = 1;

    [SerializeField] private List<Vector2> _busyVectors; 

    private void Awake()
    {
        GeneratePlayer();
        GenerateAnimals(_countWolfs, _wolfPrefab);
        for (int i = 0; i < _countGroupsDoes; i++)
            GenerateAnimals(Random.Range(3, 10), _doePrefab, i); 
        GenerateAnimals(_countRabbits, _rabbitPrefab); 
    }

    private void GeneratePlayer()
    {
        Vector2 pos = GeneratePosition(); 
        _busyVectors.Add(pos); 
        GameObject player = Instantiate(_playerPrefab, pos, Quaternion.identity, _spawnTransform); 
        alives.Add(player.transform); 
        _busyVectors.Clear(); 
    }

    /// <param name="count"></param>
    /// <param name="prefab"></param>
    private void GenerateAnimals(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = GeneratePosition(); 
            _busyVectors.Add(pos); 
            GameObject animal = Instantiate(prefab, pos, Quaternion.identity, _spawnTransform); 
            animal.name = prefab.name; 
            alives.Add(animal.transform); 
            PatrolAnimal patrolAnimal = animal.GetComponent<PatrolAnimal>(); 
            patrolAnimal.wayPoints = _wayPoints; 
            patrolAnimal.gameManager = this;
        }
    }

    /// <param name="count"></param>
    /// <param name="prefab"></param>
    /// <param name="group"></param>
    private void GenerateAnimals(int count, GameObject prefab, int group)
    {
        Vector2 pos = GeneratePosition(); 
        _busyVectors.Add(pos); 
        Transform mainAnimal = null; 
        for (int i = 0; i < count; i++)
        {
            GameObject animal = Instantiate(prefab, pos, Quaternion.identity, _spawnTransform);
            animal.name = prefab.name; 
            alives.Add(animal.transform); 
            if (prefab.name == "GroupedAnimal") 
            {
                GroupedAnimal groupedAnimal = animal.GetComponent<GroupedAnimal>(); 
                groupedAnimal.groupID = "Group_" + group.ToString(); 
                groupedAnimals.Add(groupedAnimal); 
                groupedAnimal.wayPoints = _wayPoints; 
                groupedAnimal.gameManager = this;
                if (i == 0)
                {
                    mainAnimal = animal.transform;
                    groupedAnimal._mainAnimal = true; 
                }   
                groupedAnimal.mainAnimalTransform = mainAnimal; 
            }
        }
    }

    /// <returns></returns>
    private Vector2 GeneratePosition()
    {
        Vector2 pos = new Vector2(x: Random.Range(22, 34), y: Random.Range(-12.5f, -7.5f)); 
        if (_busyVectors.Count < 0) 
        {
            foreach (Vector2 busyVector in _busyVectors) 
            {
                while (pos == busyVector) 
                    pos = new Vector2(x: Random.Range(22, 34), y: Random.Range(-12.5f, -7.5f)); 
            }
        }
        return pos;
    }
}