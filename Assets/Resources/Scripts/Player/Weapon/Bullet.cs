using UnityEngine;

/// <summary>
/// Класс физики пули
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Скорость полёта пули")]
    [SerializeField] private float _speed = 1f; // Скорость полёта пули
    private Rigidbody2D _rig;

    /// <summary>
    /// Метод вызывается при первом кадре
    /// </summary>
    private void Start()
    {
        _rig = GetComponent<Rigidbody2D>(); // Инициализируем компонент физики Rigidbody2D пули
        _rig.velocity = transform.right * _speed; // Создаём силу полёта пули
    }
}