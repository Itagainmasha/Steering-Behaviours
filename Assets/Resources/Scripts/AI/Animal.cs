using System.Collections.Generic;
using UnityEngine;


public class Animal : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Игровой менеджер, здесь хранится всё важное, он всего 1 на сцене")]
    [SerializeField] internal GameManager gameManager;

    [Header("Animal: Options")]
    [Tooltip("Здоровье животного")]
    [SerializeField] internal int health = 1; 
    [Tooltip("Является ли животное главным")]
    [SerializeField] internal bool _mainAnimal = true;

   
    internal void GetDamage(int damage)
    {
        health -= damage; 
        if (health == 0) 
            Die(); 
    }

   
    internal void Die()
    {
        gameManager.alives.Remove(transform); 
        if (_mainAnimal) 
            SetNewMainAnimal(); 
        
        Destroy(gameObject); 
    }

    private void SetNewMainAnimal()
    {
        if (GetComponent<GroupedAnimal>() != null) 
        {
            GroupedAnimal thisGroupedAnimal = GetComponent<GroupedAnimal>(); 
            List<GroupedAnimal> _groupedAnimals = new List<GroupedAnimal>(); 
            for (int i = 0; i < gameManager.groupedAnimals.Count; i++) 
            {
                GroupedAnimal groupedAnimal = gameManager.groupedAnimals[i]; 
                if (groupedAnimal != null && groupedAnimal.groupID == thisGroupedAnimal.groupID && gameObject != groupedAnimal.gameObject)
                    _groupedAnimals.Add(groupedAnimal);
            }
            if (_groupedAnimals.Count > 0)
            {
                int rand = Random.Range(0, _groupedAnimals.Count - 1);
                if (_groupedAnimals[rand] != null)
                    _groupedAnimals[rand]._mainAnimal = true;
            }
        }
    }

    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet")) 
            GetDamage(col.GetComponent<Bullet>().damage); 
    }
}