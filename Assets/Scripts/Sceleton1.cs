//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Sceleton : EntityEngine
//{
//    private float speed = 2.5f;
//    private Vector3 dir;
//    private SpriteRenderer sprite;
//    private Transform player;
//    private Rigidbody2D rb;
//    private Animator anim;

//    [SerializeField] private Transform pointA; // Начало патрулируемой зоны
//    [SerializeField] private Transform pointB; // Конец патрулируемой зоны
//    [SerializeField] private float detectionRange = 5.0f; // Радиус обнаружения игрока

//    private bool isChasing = false;
//    private Transform targetPoint;

//    private void Start()
//    {
//        dir = transform.right;
//        lives = 5;
//        player = Hero.Instance.transform;
//        targetPoint = pointA; // Начинаем патрулирование с точки A
//    }

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        anim = GetComponent<Animator>();
//        sprite = GetComponentInChildren<SpriteRenderer>();
//    }

//    private void Move()
//    {
//        if (Mathf.Abs(rb.velocity.y) < 0.005f) State = States.run;

//        if (isChasing)
//        {
//            // Если игрок в зоне видимости, идем к нему
//            Vector3 targetDirection = (player.position - transform.position).normalized;
//            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
//            sprite.flipX = targetDirection.x < 0.0f;
//        }
//        else
//        {
//            // Если игрок не виден, патрулируем между точками A и B
//            float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

//            if (distanceToTarget < 1f) // Увеличил порог для проверки
//            {
//                // Если достигли одной из точек, меняем цель на противоположную
//                //targetPoint = targetPoint == pointA ? pointB : pointA;
//                if (targetPoint == pointA) targetPoint = pointB;
//                else if (targetPoint = pointB)targetPoint = pointA;
//            }
//            // Направление к целевой точке
//            Vector3 patrolDirection = (targetPoint.position - transform.position).normalized;
//            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
//            sprite.flipX = patrolDirection.x < 0.0f;
//        }
//    }


//    private States State
//    {
//        get { return (States)anim.GetInteger("Skeleton_State"); }
//        set { anim.SetInteger("Skeleton_State", (int)value); }
//    }

//    private void Update()
//    {
//        // Проверяем расстояние до игрока
//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//        isChasing = distanceToPlayer < detectionRange;
//        Move();

//        // Run();
//        //if (Mathf.Abs(rb.velocity.y) < 0.005f && !Input.GetButton("Horizontal")) State = States.stay;
//        Debug.Log("Current target: " + targetPoint.name);


//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject == Hero.Instance.gameObject)
//        {
//            Hero.Instance.GetDamage();
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sceleton_old : EntityEngine
{
    private float speed = 2.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Transform pointA; // Начало патрулируемой зоны
    [SerializeField] private Transform pointB; // Конец патрулируемой зоны
    [SerializeField] private float detectionRange = 5.0f; // Радиус обнаружения цели
    [SerializeField] private float attackRange = 1.5f; // Радиус атаки
    [SerializeField] private float attackCooldown = 1f; // Задержка между атаками
    [SerializeField] private float attackDuration = 1f; // Задержка между атаками

    [SerializeField] private GameObject[] potions; // Массив возможных зельев
    [SerializeField] private float dropChance = 1f; // Шанс выпадения зелья (50%)

    [SerializeField] private GameObject target; // Целевой объект для атаки

    public bool isRecharged = false;

    private bool isChasing = false;
    private bool isAttacking = false;
    private Transform targetPoint;


    [SerializeField] public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint;         // Точка, из которой выпускается снаряд
    public float fireRate = 2f;         // Частота стрельбы (раз в секунду)
    private float nextFireTime = 0f;    // Время до следующего выстрела


    private void Start()
    {
        dir = transform.right;
        lives = 5;

        if (target != null)
        {
            targetPoint = pointA; // Начинаем патрулирование с точки A
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }

    public override void GetDamage()
    {
        lives -= 1;

        if (lives <= 0)
        {
            DropPotion();
            Destroy(gameObject); // Уничтожаем врага
        }
    }

    private void DropPotion()
    {
        if (Random.value <= dropChance) // Сравниваем случайное число с шансом
        {
            int potionIndex = Random.Range(0, potions.Length); // Выбираем случайное зелье
            Instantiate(potions[potionIndex], transform.position, Quaternion.identity); // Создаём зелье
        }
    }

    private void Move()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f) State = States.run;

        if (target == null) return; // Если цель не задана, ничего не делаем

        Transform player = target.transform; // Получаем позицию целевого объекта
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackPlayer(player)); // Если цель в радиусе атаки, атакуем
            }
            else if (!isAttacking)
            {
                // Преследуем цель
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                sprite.flipX = player.position.x < transform.position.x; // Поворачиваем врага
            }
        }
        else
        {
            isChasing = false;

            // Если цель не видна, патрулируем между точками A и B
            if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA; // Меняем цель
            }

            // Двигаемся к точке патрулирования
            Vector3 patrolPosition = new Vector3(targetPoint.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);
            sprite.flipX = targetPoint.position.x < transform.position.x;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointA.position, 0.2f);
        Gizmos.DrawSphere(pointB.position, 0.2f);
    }


    private IEnumerator AttackPlayer(Transform player)
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f && isRecharged)
        {
            isAttacking = true;
            isRecharged = false;
            State = States.attack; // Переход в анимацию атаки

            yield return new WaitForSeconds(attackDuration); // Ожидаем завершения анимации
            isAttacking = false;

            // Проверяем расстояние до цели ещё раз (на случай, если цель успела убежать)
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                
                    Hero.Instance.GetDamage(); // Наносим урон цели
               
            }

            yield return new WaitForSeconds(attackCooldown); // Ожидаем перезарядку
            isRecharged = true;
        }
    }

    private States State
    {
        get { return (States)anim.GetInteger("Skeleton_State"); }
        set { anim.SetInteger("Skeleton_State", (int)value); }
    }

    private void Update()
    {
        if (target == null) return; // Если цель не задана, ничего не делаем

        // Проверяем расстояние до цели
        Transform player = target.transform;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isChasing = distanceToPlayer < detectionRange;

        if (!isAttacking) Move(); // Двигаемся только если не атакуем
    }

    private void OnDrawGizmosSelected()
    {
        // Радиус обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Радиус атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
