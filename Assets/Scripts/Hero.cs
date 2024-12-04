using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : EntityEngine
{
    [SerializeField] private float speed = 3f; // �������� ��������
    [SerializeField] private int health; // ����� ������
    [SerializeField] private float jumpForse = 15f; // ���� ������

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public bool isAttcking = false;
    public bool isRecharged = false;


    [SerializeField] private Vector3 attackOffsetRight; // ��������, ����� ����� ������� ������
    [SerializeField] private Vector3 attackOffsetLeft;  // ��������, ����� ����� ������� �����

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    [SerializeField] private GameObject LosePanel;  // ������ lose
   

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
            Die(); 
        }
    }

    public override void Die()
    {
        //Time.timeScale = 0;
        StartCoroutine(RespawnWithDelay()); // ��������� �������� ��� ��������

        // ����� �������� �������, ����� ��� ������������ ����� ��� �������� ������
        Debug.Log("Player has died and will respawn.");

    }

    private IEnumerator RespawnWithDelay()
    {
        LosePanel.SetActive(true);
        yield return new WaitForSeconds(2f); // �������� 0.3 �������
        LosePanel.SetActive(false);

        Respawn(); // ��������� ������
    }

    public void Respawn()
    {
        // ��������� ���������� ������
        SaveData data = SaveSystem.LoadProgress();
        string activeCheckpointID = data.lastActiveCheckpointID;

        if (string.IsNullOrEmpty(activeCheckpointID))
        {
            Debug.LogWarning("No active checkpoint found! Respawning at default position.");
            transform.position = Vector3.zero; // ���������� ������ �� ��������� �������, ���� ��� �������� �����

            return;
        }

        // ���� �������� �������� � �����
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.checkpointID == activeCheckpointID)
            {
                transform.position = checkpoint.transform.position; // ���������� ������ � �����
                Debug.Log($"Player respawned at checkpoint: {activeCheckpointID}");

                return;
            }
        }
        // ���� �� ����� �����, ���������� ������ �� ��������� �������
        Debug.LogWarning("Active checkpoint not found in scene. Respawning at default position.");
        transform.position = Vector3.zero;

    }


    private void Awake()
    {
        health = 5;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();   
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged= true;  
        LosePanel.SetActive (false);
    }



    private void Run()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.005f) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position,transform.position + dir, speed * Time.deltaTime); // ������������

        sprite.flipX = dir.x < 0.0f; // �������� ������ �����
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
        yield return new WaitForSeconds(0.3f);
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
        // ��������� ������� AttackPos � ����������� �� �����������
        attackPos.localPosition = sprite.flipX ? attackOffsetLeft : attackOffsetRight;

        if (Input.GetButton("Horizontal") && !isAttcking) Run();
        if (Mathf.Abs(rb.velocity.y) < 0.005f && Input.GetButtonDown("Jump") && !isAttcking) Jump();
        if (Mathf.Abs(rb.velocity.y) < 0.005f && !Input.GetButton("Horizontal") && !isAttcking) State = States.stay;
        if (Input.GetButton("Fire1") && !isAttcking) Attack();

        // ��������� ����������� ������
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].sprite = aliveHeart;
            else hearts[i].sprite = deadHeart;
        }
    }

    public void RestoreHealth(int amount)
    {
        health = Mathf.Min(health + amount, hearts.Length); // ����������� ��������, �� �� ��������� ��������
        Debug.Log($"Health restored by {amount}. Current health: {health}");
    }


}




public enum States
{
    stay,
    run,
    jump,
    attack,
    shoot
}