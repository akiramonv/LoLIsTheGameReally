using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;        // �������� �������
    public float lifetime = 5f;     // ����� ����� �������

    private Vector2 direction;      // ����������� ��������

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime); // ���������� ������ ����� �������� �����
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // �������� �������
        Destroy(gameObject, lifetime); // ���������� ������ ����� �������� �����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage(); // ������� ���� ����
            Debug.Log("Player hit!");
            Destroy(gameObject); // ���������� ������
        }
    }
}
