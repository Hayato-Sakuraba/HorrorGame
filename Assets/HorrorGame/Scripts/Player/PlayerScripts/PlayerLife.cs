using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxLife = 3;

    private int currentLife;

    public int CurrentLife => currentLife;
    public int MaxLife => maxLife;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        currentLife = maxLife;
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Damage(int damage)
    {
        currentLife -= damage;

        if (currentLife < 0)
        {
            currentLife = 0;
        }

        Debug.Log("現在のライフ：" + currentLife);

        if (currentLife <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentLife += amount;

        if (currentLife > maxLife)
        {
            currentLife = maxLife;
        }

        Debug.Log("回復 現在のライフ：" + currentLife);
    }

    private void Die()
    {
        Debug.Log("プレイヤー死亡");
        playerMovement.SetDead(true);
    }
}