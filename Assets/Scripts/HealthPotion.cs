using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 1; // ���������� ����������������� ������

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)   // ���������, ��� ������������ � �������
        {
            Hero.Instance.RestoreHealth(healAmount); // ��������������� ��������
            Destroy(gameObject); // ���������� �����
        }
    }
}
