using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("PlayerState: Options")]
    [Tooltip("Здоровье игрока")]
    [SerializeField] private int _health = 5; // Здоровье игрока

    protected virtual void Update()
    {
        
    }
}