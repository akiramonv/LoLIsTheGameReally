using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 1; // Количество восстанавливаемых жизней

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)   // Проверяем, что столкновение с игроком
        {
            Hero.Instance.RestoreHealth(healAmount); // Восстанавливаем здоровье
            Destroy(gameObject); // Уничтожаем зелье
        }
    }
}
