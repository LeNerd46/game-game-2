using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Slider enemyHealthSlider;
    public EnemyProfile enemyProfile;

    public float enemyHealth;

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        enemyHealthSlider.value = enemyHealth;

        // GetComponent<EnemyAI>().playerFound = true;
        // GetComponent<EnemyAI>().personallyFound = true;
        // GetComponent<EnemyAI>().lastPlayerLocation.playerSpotted = true;
        // GetComponent<EnemyAI>().alert = true;
        // transform.LookAt(GetComponent<EnemyAI>().player.position);

        if (enemyHealth <= 0f)
            Die();
    }

    public void Die()
    {
        if (GameManager.instance.onEnemyDeathCallback != null)
            GameManager.instance.onEnemyDeathCallback.Invoke(enemyProfile);

        LevelingSystem.xp += 500f;

        Destroy(gameObject);
    }
}