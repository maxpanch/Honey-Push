using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heir : MonoBehaviour
{
    public int KilledEnemies = 4;
    public bool IsGrown = false;
    public Transform Destination;
    public float Speed;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody Rigidbody;
    public bool IsMovingRight;
    public Animator HeirSwordAnimator;
    public bool IsDead = false;
    public BoxCollider BoxCollider;
    public GameObject Sword;
    public bool IsAttacking = false;
    public Sprite HeirEnemy;
    public LayerMask EnemyLayerMask;
    public AudioSource FootstepAudioSource;
    public AudioSource DeathAudioSource;
    public AudioSource SwordAudioSource;
    public float FootstepCooldown = 0.5f;
    public float FootstepCooldownBase;
    private void Start()
    {
        FootstepCooldownBase = FootstepCooldown;
    }
    private void Update()
    {
        if (!IsDead && !IsAttacking)
        {
            if (GameManager.Instance.GameState == State.Intro || GameManager.Instance.GameState == State.Tutorial || GameManager.Instance.GameState == State.Lose || GameManager.Instance.GameState == State.Win || GameManager.Instance.GameState == State.Menu) return;

            FootstepCooldown -= Time.deltaTime;
            if (FootstepCooldown < 0)
            {
                FootstepCooldown = FootstepCooldownBase;
                if (Random.Range(0, 10) > 6) AudioManager.Instance.Play(SoundEnum.hp_footstep, FootstepAudioSource);
            }

            if (Destination == null) FindEnemy();
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 direction = (Destination.position - transform.position).normalized;
            if (direction.x >= 0) IsMovingRight = true;
            else IsMovingRight = false;
            if (IsMovingRight)
            {
                Sword.transform.localPosition = new Vector3(0.65f, 0.4f, 0);
                Sword.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                Sword.transform.localPosition = new Vector3(-0.65f, 0.4f, 0);
                Sword.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (IsGrown) Destination = GameManager.Instance.King;
            transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
            // Debug.Log(Vector3.Distance(transform.position, Destination));
        }
        if (GameManager.Instance.GameState == State.Win && IsGrown) Death();
    }
    public void FindEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 40f, EnemyLayerMask);
        float closestEnemyDistance = 999f;
        foreach (var hitCollider in hitColliders)
        {
            // Debug.Log(hitCollider.gameObject.name + " - " + Vector3.Distance(transform.position, hitCollider.transform.position) + ", closest is " + closestEnemyDistance);
            if (Vector3.Distance(transform.position, hitCollider.transform.position) < closestEnemyDistance)
            {
                Destination = hitCollider.transform;
                closestEnemyDistance = Vector3.Distance(transform.position, hitCollider.transform.position);
            }
        }
        if (Destination == null) Destination.position = GameManager.Instance.King.position;
    }
    public void AddKilledEnemy()
    {
        KilledEnemies -= 1;
        if (KilledEnemies <= 0)
        {
            IsGrown = true;
            // GameManager.Instance.Enemies.Add(gameObject);
            SpriteRenderer.sprite = HeirEnemy;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            StartCoroutine(IncreaseScaleRoutine());
            // Destination = GameManager.Instance.Enemies[Random.Range(0, GameManager.Instance.Enemies.Count)].transform;
        }
    }
    private IEnumerator IncreaseScaleRoutine()
    {
        float timeForScale = 0.3f;
        Vector3 scaleChange = new Vector3(0.003f, 0.003f, 0);
        while (timeForScale > 0)
        {
            transform.localScale += scaleChange;
            timeForScale -= Time.deltaTime;
            yield return null;
        }
        FindEnemy();
        yield return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Viking>() && !IsGrown)
        {
            if (!IsMovingRight) HeirSwordAnimator.SetTrigger("AttackLeft");
            else HeirSwordAnimator.SetTrigger("AttackRight");
            AudioManager.Instance.Play(SoundEnum.hp_sword, SwordAudioSource);
            other.gameObject.GetComponent<Viking>().Death();
            StartCoroutine(AttackRoutine());
            AddKilledEnemy();
        }
        if (other.gameObject.GetComponent<Heir>() && !IsGrown)
        {
            if (other.gameObject.GetComponent<Heir>().IsGrown)
            {
                if (!IsMovingRight) HeirSwordAnimator.SetTrigger("AttackLeft");
                else HeirSwordAnimator.SetTrigger("AttackRight");
                AudioManager.Instance.Play(SoundEnum.hp_sword, SwordAudioSource);
                other.gameObject.GetComponent<Heir>().Death();
                StartCoroutine(AttackRoutine());
                AddKilledEnemy();
            }
        }
        if (other.gameObject.GetComponent<Character>() && IsGrown)
        {
            if (!IsMovingRight) HeirSwordAnimator.SetTrigger("AttackLeft");
            else HeirSwordAnimator.SetTrigger("AttackRight");
            AudioManager.Instance.Play(SoundEnum.hp_sword, SwordAudioSource);
            other.gameObject.GetComponent<Character>().Hit();
            StartCoroutine(AttackRoutine());
            Death();
        }
    }
    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        yield return new WaitForSeconds(1f);
        IsAttacking = false;
    }
    public void Death()
    {
        StartCoroutine(DeathRoutine());
    }
    private IEnumerator DeathRoutine()
    {
        BoxCollider.enabled = false;
        DeathAudioSource.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
