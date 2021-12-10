using UnityEngine;


/// Чутье животных

public class Flair : Animal
{
    [Header("Flair: Options")]
    [Tooltip("Скорость передвижения животного")]
    [SerializeField] protected float _speed = 1f; 
    [Tooltip("Радиус поиска добычи")]
    [SerializeField] protected float _radiusSearchPrey = 2f; 
    #region Conditions
    [Tooltip("Ведётся ли преследование (true - да, false - нет)")]
    [SerializeField] protected bool _chased = false; // Ведётся ли преследование (true - да, false - нет)
    [Tooltip("Состояние животного (true - патрулирует, false - занят чем-то другим)")]
    [SerializeField] protected bool _patrol = true; // Состояние животного (true - патрулирует, false - занят чем-то другим)
    [Tooltip("Состояние животного (true - бегство, false - занят чем-то другим)")]
    [SerializeField] protected bool _running = false; // Состояние животного (true - бегство, false - занят чем-то другим)
    #endregion
    [Tooltip("Опция, является ли животное вражеским")]
    [SerializeField] private bool _enemy = false; 

    protected Transform _target; 
    protected float _timeReset = 0f; 

    
    protected void CalculateOptimalTarget()
    {
        float minDistance = 0f; 
        foreach (Transform alive in gameManager.alives) 
        {
            if (alive != null && alive.gameObject.name == gameObject.name) continue; 

            if (_enemy && _target == null) 
                _timeReset = Time.time; 
            float dist = Vector2.Distance(transform.position, alive.position); 
            if (dist <= _radiusSearchPrey) 
            {
                if (minDistance == 0f) 
                {
                    minDistance = dist; 
                    if (_enemy) 
                        _timeReset = Time.time; 
                }

                if (dist <= minDistance) 
                {
                    minDistance = dist; 
                    _target = alive; 
                    if (_enemy) 
                        Chase(true);  
                    else
                    {
                        if ((gameObject.name == "GroupedAnimal" && _target.name == "IdleAnimal") || (gameObject.name == "GroupedAnimal" && _target.name != "EnemyAnimal")) continue;
                        Running(true); 
                    }  
                }
            }
        }
    }


   
    protected void Chase(bool chase)
    {
        if (!chase) 
            _target = null; 
        _patrol = !chase; 
        _chased = chase;
    }


  
    protected void Running(bool running)
    {
        if (!running) 
            _target = null; 
        _patrol = !running;
        _running = running; 
    }
}