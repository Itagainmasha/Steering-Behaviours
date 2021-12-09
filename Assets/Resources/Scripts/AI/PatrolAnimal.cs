using System.Collections.Generic;
using UnityEngine;


public class PatrolAnimal : Flair
{
    [Header("WayPoints")]
    [Tooltip("Массив всевозможных путей")]
    [SerializeField] internal Transform[] wayPoints; 
    private Transform _nowPoint; 

    [Header("PatrolAnimal: Options")]
    [Tooltip("Радиус поиска доступных путей по близости")]
    [SerializeField] private float _radiusSearchWay = 3f;
    [Tooltip("Радиус в котором начнётся поиск нового пути")]
    [SerializeField] private float _radiusStopWay = 1f; 


    protected virtual void Start() => FindAndSetNewWay(); 

    protected virtual void Update()
    {
        CalculateOptimalTarget(); 
        if (_running && _target != null) 
            RunAnimal(); 
        if (_patrol) 
            MoveAnimal(); 
    }


    private void RunAnimal()
    {
        if (Vector2.Distance(transform.position, _target.position) > _radiusSearchPrey) 
        {
            Running(false); 
            return; 
        }
        transform.position = Vector2.MoveTowards(transform.position, -_target.position, _speed * Time.deltaTime); 
    }


    protected void FindAndSetNewWay()
    {
        _patrol = false;
        List<Transform> _wayPointsArround = new List<Transform>(); 
        foreach (Transform _wayPoint in wayPoints) 
        {
            if (Vector2.Distance(_wayPoint.position, transform.position) < _radiusSearchWay) 
                _wayPointsArround.Add(_wayPoint); 
        } 
        int rand = Random.Range(0, _wayPointsArround.Count - 1); 
        _nowPoint = _wayPointsArround[rand]; 
        _patrol = true; 
    }


    private void MoveAnimal()
    {
        transform.position = Vector2.MoveTowards(transform.position, _nowPoint.position, _speed * Time.deltaTime); 
        if (Vector2.Distance(transform.position, _nowPoint.position) < _radiusStopWay) 
            FindAndSetNewWay(); 
    }
}