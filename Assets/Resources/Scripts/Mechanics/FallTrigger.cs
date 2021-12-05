using UnityEngine;

/// <summary>
/// Класс триггера обрыва
/// </summary>
public class FallTrigger : MonoBehaviour
{
    /// <summary>
    /// Метод входа в триггер
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet") || col.CompareTag("AI") || col.CompareTag("Player")) // Проверяем, входил ли в триггер объект с указанными тегами
            Destroy(col.gameObject); 
    }
}