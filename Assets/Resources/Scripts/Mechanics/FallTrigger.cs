using UnityEngine;

/// <summary>
/// Класс триггера обрыва
/// </summary>
public class FallTrigger : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Игровой менеджер, здесь хранится всё важное, он всего 1 на сцене")]
    [SerializeField] protected GameManager _gameManager; // Игровой менеджер

    /// <summary>
    /// Метод входа в триггер
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Animal")) // Проверка объекта с тегом "Animal"
            col.GetComponent<Animal>().Die(); // Убиваем животного

        if (col.CompareTag("Bullet") || col.CompareTag("Player")) // Проверка объекта с тегом "Bullet" или "Player"
            Destroy(col.gameObject); // Удаляем объект со сцены

        _gameManager.alives.Remove(col.transform); // Удаляем мёртвый объект

    }
}