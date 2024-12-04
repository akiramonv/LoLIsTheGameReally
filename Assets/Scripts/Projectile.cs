using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;        // Скорость снаряда
    public float lifetime = 5f;     // Время жизни снаряда

    private Vector2 direction;      // Направление движения

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime); // Уничтожить снаряд через заданное время
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // Движение снаряда
        Destroy(gameObject, lifetime); // Уничтожить снаряд через заданное время
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage(); // Наносим урон цели
            Debug.Log("Player hit!");
            Destroy(gameObject); // Уничтожить снаряд
        }
    }
}
