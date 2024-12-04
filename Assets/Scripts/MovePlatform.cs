using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboPatrol : EntityEngine
{
    public float moveDistance = 5.0f; // Расстояние движения в одну сторону
    public float speed = 2.0f; // Скорость движения

    private Vector3 startPosition; // Начальная позиция
    private Vector3 leftLimit; // Левая граница
    private Vector3 rightLimit; // Правая граница
    private Vector3 dir = Vector3.right; // Направление движения
    private SpriteRenderer sprite;


    private void Start()
    {
        // Сохраняем начальную позицию
        startPosition = transform.position;

        // Определяем границы движения
        leftLimit = startPosition - Vector3.right * moveDistance;
        rightLimit = startPosition + Vector3.right * moveDistance;

        // Получаем SpriteRenderer для смены направления спрайта
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    private void Move()
    {
        // Двигаем объект
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        // Проверяем, достиг ли объект границ
        if (transform.position.x >= rightLimit.x)
        {
            dir = Vector3.left; // Меняем направление на левое
        }
        else if (transform.position.x <= leftLimit.x)
        {
            dir = Vector3.right; // Меняем направление на правое
        }

        // Обновляем направление отображения спрайта
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
