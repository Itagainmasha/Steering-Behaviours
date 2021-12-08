using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PlayerInput: Options")]
    [Tooltip("Скорость передвижения игрока")]
    [SerializeField] private float _speed = 1f; 
    private Transform _playerTransform;  
    private Vector2 _direction; 
    private Rigidbody2D _rig; 


    private void Start()
    {
        _playerTransform = GetComponent<Transform>(); 
        _rig = GetComponent<Rigidbody2D>(); 
    }


    private void Update()
    {
        _direction = DirectionCalculate(); 
        RotatePlayer(); 
    }

    private void FixedUpdate() => MovePlayer(); 

    private void MovePlayer()
        => _rig.MovePosition(_rig.position + _direction * _speed * Time.fixedDeltaTime); 

    private void RotatePlayer()
    {
        if (_direction.x != 0 || _direction.y != 0) 
            _playerTransform.rotation = new Quaternion(0f, FlipXPlayer(), 0f, _playerTransform.rotation.w); 
    }

    private float FlipXPlayer()
    {
        float flipX = 0f; 
        if (_direction.x < 0) 
            flipX = 180f; 

        return flipX; 
    }

    private Vector2 DirectionCalculate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return direction; 
    }
}



