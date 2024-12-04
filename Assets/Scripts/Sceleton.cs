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

//    [SerializeField] private Transform pointA; // ������ ������������� ����
//    [SerializeField] private Transform pointB; // ����� ������������� ����
//    [SerializeField] private float detectionRange = 5.0f; // ������ ����������� ������

//    private bool isChasing = false;
//    private Transform targetPoint;

//    private void Start()
//    {
//        dir = transform.right;
//        lives = 5;
//        player = Hero.Instance.transform;
//        targetPoint = pointA; // �������� �������������� � ����� A
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
//            // ���� ����� � ���� ���������, ���� � ����
//            Vector3 targetDirection = (player.position - transform.position).normalized;
//            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
//            sprite.flipX = targetDirection.x < 0.0f;
//        }
//        else
//        {
//            // ���� ����� �� �����, ����������� ����� ������� A � B
//            float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

//            if (distanceToTarget < 1f) // �������� ����� ��� ��������
//            {
//                // ���� �������� ����� �� �����, ������ ���� �� ���������������
//                //targetPoint = targetPoint == pointA ? pointB : pointA;
//                if (targetPoint == pointA) targetPoint = pointB;
//                else if (targetPoint = pointB)targetPoint = pointA;
//            }
//            // ����������� � ������� �����
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
//        // ��������� ���������� �� ������
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

public class Sceleton : EntityEngine
{
    private float speed = 2.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Transform pointA; // ������ ������������� ����
    [SerializeField] private Transform pointB; // ����� ������������� ����
    [SerializeField] private float detectionRange = 5.0f; // ������ ����������� ����
    [SerializeField] private float attackRange = 1.5f; // ������ �����
    [SerializeField] private float attackCooldown = 1f; // �������� ����� �������
    [SerializeField] private float attackDuration = 1f; // �������� ����� �������

    [SerializeField] private GameObject[] potions; // ������ ��������� ������
    [SerializeField] private float dropChance = 1f; // ���� ��������� ����� (50%)

    [SerializeField] private GameObject target; // ������� ������ ��� �����

    public bool isRecharged = false;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isOnCooldown = false;                        // ��������� �� ���� �� �����������
    private Transform targetPoint;


    [SerializeField] private bool canShoot = false;           // ����� �� ���� ��������
    [SerializeField] private float shootCooldown = 2f;        // ����� ����������� ��������
    [SerializeField] private GameObject projectilePrefab;     // ������ �������
    [SerializeField] private Transform firePoint;             // ����� ��������


    private void Start()
    {
        dir = transform.right;
        lives = 5;

        if (target != null)
        {
            targetPoint = pointA; // �������� �������������� � ����� A
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
            Destroy(gameObject); // ���������� �����
        }
    }

    private void DropPotion()
    {
        if (Random.value <= dropChance) // ���������� ��������� ����� � ������
        {
            int potionIndex = Random.Range(0, potions.Length); // �������� ��������� �����
            Instantiate(potions[potionIndex], transform.position, Quaternion.identity); // ������ �����
        }
    }

    private void Move()
    {
        if (target == null) return; // ���� ���� �� ������, ������ �� ������

        Transform player = target.transform; // �������� ������� �������� �������
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;

            // ���� ���� ����� ��������, ��������� ���� �����������
            if (canShoot && !isOnCooldown)
            {
                ShootAtPlayer(player);
            }

            if (distanceToPlayer <= attackRange && isRecharged)
            {
                AttackPlayer(player);
            }

            else if (!isAttacking)
            {
                State = States.run;

                // ���������� ����
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                sprite.flipX = player.position.x < transform.position.x; // ������������ �����
            }
        }
        else
        {
            isChasing = false;
            State = States.run;

            // ���� ���� �� �����, ����������� ����� ������� A � B
            if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
            {
                targetPoint = targetPoint == pointA ? pointB : pointA; // ������ ����
            }

            // ��������� � ����� ��������������
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

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown); // �������� �� �����������
        isOnCooldown = false;
    }

    private void ShootAtPlayer(Transform player)
    {
        isOnCooldown = true;
        State = States.shoot; // ������� � �������� �����

        // ������� ������
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // ���������� ������ � ������� ������
        Vector2 direction = (player.position - firePoint.position).normalized;
        projectile.GetComponent<Projectile>().SetDirection(direction);
        

        StartCoroutine(ShootCooldown()); // ��������� �����������
    }



    private void AttackPlayer(Transform player)
    {
        State = States.attack;
        isAttacking = true;
        isRecharged = false;

        StartCoroutine(AttackAnim());

        // ���������, ��� ����� ��� ��� � ������� �����
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Hero.Instance.GetDamage(); // ������� ���� ������
        }

        StartCoroutine(AttackCoolDown());
    }


    private IEnumerator AttackAnim()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isRecharged = true;
    }

    private States State
    {
        get { return (States)anim.GetInteger("Skeleton_State"); }
        set { anim.SetInteger("Skeleton_State", (int)value); }
    }

    private void Update()
    {
        if (target == null) return; // ���� ���� �� ������, ������ �� ������

        // ��������� ���������� �� ����
        Transform player = target.transform;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isChasing = distanceToPlayer < detectionRange;

        if (!isAttacking) Move(); // ��������� ������ ���� �� �������
    }

    private void OnDrawGizmosSelected()
    {
        // ������ �����������
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // ������ �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
