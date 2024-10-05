using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heir : MonoBehaviour
{
    public int KilledEnemies = 4;
    public bool IsGrown = false;
    public Vector3 Destination;
    public float Speed;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody Rigidbody;
    public bool IsMovingRight;
    public Animator HeirSwordAnimator;
    public bool IsDead = false;
    public BoxCollider BoxCollider;
    public GameObject Sword;
    public bool IsAttacking = false;
    private void Update()
    {
        if (!IsDead && !IsAttacking)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 direction = (Destination - transform.position).normalized;
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

            if (IsGrown) Destination = GameManager.Instance.King.position;
            transform.position = Vector3.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
            // Debug.Log(Vector3.Distance(transform.position, Destination));
            if (Vector3.Distance(transform.position, Destination) < 0.5f && !IsGrown) FindEnemy();
        }
    }
    public void FindEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 15f);
        float closestEnemyDistance = 999f;
        foreach (var hitCollider in hitColliders)
        {
            // Debug.Log(hitCollider.gameObject.name);
            if (Vector3.Distance(transform.position, hitCollider.transform.position) < closestEnemyDistance) Destination = hitCollider.transform.position;
        }
        if (Destination == null) Destination = GameManager.Instance.King.position;
    }
    public void AddKilledEnemy()
    {
        KilledEnemies -= 1;
        if (KilledEnemies <= 0)
        {
            IsGrown = true;
            // GameManager.Instance.Enemies.Add(gameObject);
            SpriteRenderer.color = Color.red;
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
        Vector3 scaleChange = new Vector3(0.0005f, 0.0005f, 0);
        while (timeForScale > 0)
        {
            transform.localScale += scaleChange;
            timeForScale -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Viking>() && !IsGrown)
        {
            if (!IsMovingRight) HeirSwordAnimator.SetTrigger("AttackLeft");
            else HeirSwordAnimator.SetTrigger("AttackRight");
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
                other.gameObject.GetComponent<Heir>().Death();
                StartCoroutine(AttackRoutine());
                AddKilledEnemy();
            }
        }
        if (other.gameObject.GetComponent<Character>() && IsGrown)
        {
            if (!IsMovingRight) HeirSwordAnimator.SetTrigger("AttackLeft");
            else HeirSwordAnimator.SetTrigger("AttackRight");
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
        Destroy(gameObject, 1.25f);
        BoxCollider.enabled = false;
    }
}
