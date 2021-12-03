using UnityEngine;

/// <summary>
/// Класс передвижения игрока
/// </summary>
public class PlayerInput : PlayerState
{
    [Header("PlayerInput: Options")]
    [Tooltip("Скорость передвижения игрока")]
    [SerializeField] private float _speed = 1f; 
    private Transform _playerTransform; 
    private Vector2 _direction; 
    private Rigidbody2D _rig; 

    /// <summary>
    /// Метод вызывается при первом кадре
    /// </summary>
    private void Start()
    {
        _playerTransform = GetComponent<Transform>(); 
        _rig = GetComponent<Rigidbody2D>(); 
    }

  
    protected override void Update()
    {
        base.Update();
        _direction = DirectionCalculate(); 
        RotatePlayer(); 
    }

    
    private void FixedUpdate() => MovePlayer(); // Вызываем метод перемещения игрока

    
    private void MovePlayer()
        => _rig.MovePosition(_rig.position + _direction * _speed * Time.fixedDeltaTime); 

    
    private void RotatePlayer()
    {
        if (_direction.x != 0 || _direction.y != 0)
            _playerTransform.rotation = new Quaternion(0f, FlipXPlayer(), 0f, _playerTransform.rotation.w); // Устанавливаем поворот по оси Х игрока
    }

        /// <summary>
        /// Вычисляем куда смотрит игрок
        /// Если пойти вправо, то игрок будет смотреть вправо
        /// Если пойти влево, то игрок будет смотреть влево
        /// </summary>
        /// <returns></returns>
        private float FlipXPlayer()
    {
        float flipX = 0f;
        if (_direction.x < 0) 
            flipX = 180f; 

        return flipX; 
    }

   
    private Vector2 DirectionCalculate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Высчитываем направление вектора
        return direction; 
    }
}