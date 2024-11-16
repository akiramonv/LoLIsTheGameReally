using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : EntityEngine
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int health; // колво жизней
    [SerializeField] private float jumpForse = 15f; // сила прыжка

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public bool isAttcking = false;
    public bool isRecharged = false;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;


    public static Hero Instance { get; set; }


    public override void GetDamage()
    {
        health -= 1;
        Debug.Log(health);

        if (health == 0)
        {
            foreach (var item in hearts)
            {
                item.sprite = deadHeart;
            }
            //Die(); временно отключил для тестов
        }
    }





    private void Awake()
    {
        health = 5;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();   
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged= true;  
    }



    private void Run()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position,transform.position + dir, speed * Time.deltaTime); // передвижение

        sprite.flipX = dir.x < 0.0f; // повороты вправо влево
    }



    private void Jump()
    {
        rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
        State = States.jump;
    }

    public void Attack()
    {
        if(Mathf.Abs(rb.velocity.y) < 0.005f && isRecharged)
        {
            State = States.attack;
            isAttcking = true;
            isRecharged = false;

            StartCoroutine(AttackAnim());
            StartCoroutine(AttackCoolDown());

        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<EntityEngine>().GetDamage();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPos.position, attackRange);

    }


    private IEnumerator AttackAnim()
    {
        yield return new WaitForSeconds(0.4f);
        isAttcking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }
   
    private void Update()
    {
        if (Input.GetButton("Horizontal") && !isAttcking) Run();
        if (Mathf.Abs(rb.velocity.y) < 0.005f && Input.GetButtonDown("Jump") && !isAttcking) Jump();
        if (Mathf.Abs(rb.velocity.y) < 0.005f && !Input.GetButton("Horizontal") && !isAttcking) State = States.stay;
        if (Input.GetButton("Fire1") || Input.GetButton("x") && !isAttcking) Attack();


        for (int i = 0; i < hearts.Length; i++)
        {
            if(i<health) hearts[i].sprite = aliveHeart;
            else hearts[i].sprite = deadHeart;
        }

    }
}


public enum States
{
    stay,
    run,
    jump,
    attack
}