using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}
