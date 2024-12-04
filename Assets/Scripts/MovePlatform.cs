using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboPatrol : EntityEngine
{
    public float moveDistance = 5.0f; // ���������� �������� � ���� �������
    public float speed = 2.0f; // �������� ��������

    private Vector3 startPosition; // ��������� �������
    private Vector3 leftLimit; // ����� �������
    private Vector3 rightLimit; // ������ �������
    private Vector3 dir = Vector3.right; // ����������� ��������
    private SpriteRenderer sprite;


    private void Start()
    {
        // ��������� ��������� �������
        startPosition = transform.position;

        // ���������� ������� ��������
        leftLimit = startPosition - Vector3.right * moveDistance;
        rightLimit = startPosition + Vector3.right * moveDistance;

        // �������� SpriteRenderer ��� ����� ����������� �������
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    private void Move()
    {
        // ������� ������
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        // ���������, ������ �� ������ ������
        if (transform.position.x >= rightLimit.x)
        {
            dir = Vector3.left; // ������ ����������� �� �����
        }
        else if (transform.position.x <= leftLimit.x)
        {
            dir = Vector3.right; // ������ ����������� �� ������
        }

        // ��������� ����������� ����������� �������
    }

    private void Update()
    {
        Move();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject == Hero.Instance.gameObject) Hero.Instance.GetDamage();
    //}
}
