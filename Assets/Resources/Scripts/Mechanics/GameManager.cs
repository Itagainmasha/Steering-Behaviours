using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("UI")]
    #region TextsForSliders
    [Tooltip("Текст, который показывает какое количество пуль выбрал игрок, передвигая слайдер _sliderCountBullets")]
    [SerializeField] private Text _textCountBullets; 
    [Tooltip("Текст, который показывает какое количество волков выбрал игрок, передвигая слайдер _sliderCountWolfs")]
    [SerializeField] private Text _textCountWolfs; 
    [Tooltip("Текст, который показывает какое количество групп ланей выбрал игрок, передвигая слайдер _sliderCountGroupsDoes")]
    [SerializeField] private Text _textCountGroupsDoes; 
    [Tooltip("Текст, который показывает какое количество зайцев выбрал игрок, передвигая слайдер _sliderCountRabbits")]
    [SerializeField] private Text _textCountRabbits; 
    #endregion

    #region Sliders
    [Tooltip("Слайдер, устанавливающий количество патронов игрока")]
    [SerializeField] private Slider _sliderCountBullets; 
    [Tooltip("Слайдер, устанавливающий количество генерируемых волков")]
    [SerializeField] private Slider _sliderCountWolfs; 
    [Tooltip("Слайдер, устанавливающий количество генерируемых ланей")]
    [SerializeField] private Slider _sliderCountGroupsDoes; 
    [Tooltip("Слайдер, устанавливающий количество генерируемых зайцев")]
    [SerializeField] private Slider _sliderCountRabbits; 
    #endregion

    [Tooltip("Меню настроек генерации")]
    [SerializeField] private GameObject _menuSettings;

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

    private void Awake() => GeneratePlayer();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
            RestartLevel(); 
        SlidersValueGet(); 
    }

    private void RestartLevel() => SceneManager.LoadScene(0);

    private void SlidersValueGet()
    {
        _textCountBullets.text = _sliderCountBullets.value.ToString(); 
        _textCountWolfs.text = _sliderCountWolfs.value.ToString(); 
        _textCountGroupsDoes.text = _sliderCountGroupsDoes.value.ToString(); 
        _textCountRabbits.text = _sliderCountRabbits.value.ToString(); 
    }

    public void ApplyGenerationSettings()
    {
        GameObject.FindWithTag("Player").GetComponent<Weapon>().bulletCount = (int)_sliderCountBullets.value; 
        _countWolfs = (int)_sliderCountWolfs.value; 
        _countGroupsDoes = (int)_sliderCountGroupsDoes.value; 
        _countRabbits = (int)_sliderCountRabbits.value; 
        _menuSettings.SetActive(false); 

        Generate(); 
    }

    private void Generate()
    {
        GenerateAnimals(_countWolfs, _wolfPrefab); 
        for (int i = 0; i < _countGroupsDoes; i++)
            GenerateAnimals(Random.Range(4, 10), _doePrefab, i); 
        GenerateAnimals(_countRabbits, _rabbitPrefab); 
        _busyVectors.Clear(); 
    }

    private void GeneratePlayer()
    {
        Vector2 generatedPos = GeneratePosition();
        Vector3 pos = new Vector3(x: generatedPos.x, y: generatedPos.y, z: -30f); 
        _busyVectors.Add(generatedPos); 
        GameObject player = Instantiate(_playerPrefab, pos, Quaternion.identity); 
        alives.Add(player.transform); 
    }


    private void GenerateAnimals(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 generatedPos = GeneratePosition();
            Vector3 pos = new Vector3(x: generatedPos.x, y: generatedPos.y, z: -30f);
            _busyVectors.Add(generatedPos); 
            GameObject animal = Instantiate(prefab, pos, Quaternion.identity); 
            animal.name = prefab.name; 
            alives.Add(animal.transform); 
            PatrolAnimal patrolAnimal = animal.GetComponent<PatrolAnimal>(); 
            patrolAnimal.wayPoints = _wayPoints; 
            patrolAnimal.gameManager = this;
        }
    }

    private void GenerateAnimals(int count, GameObject prefab, int group)
    {
        Vector2 generatedPos = GeneratePosition();
        Vector3 pos = new Vector3(x: generatedPos.x, y: generatedPos.y, z: -30f);
        _busyVectors.Add(generatedPos); 
        Transform mainAnimal = null; 
        for (int i = 0; i < count-1; i++)
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