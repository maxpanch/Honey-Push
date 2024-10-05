using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heir : MonoBehaviour
{
    public int KilledEnemies = 3;
    public bool IsGrown = false;
    public Vector3 Destination;
    public float Speed;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody Rigidbody;
    private void Update()
    {
        // Debug.Log(Destination);
        if (IsGrown) Destination = GameManager.Instance.King.position;
        transform.position = Vector3.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        // Debug.Log(Vector3.Distance(transform.position, Destination));
        if (Vector3.Distance(transform.position, Destination) < 0.5f && !IsGrown) FindEnemy();
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
            // Destination = GameManager.Instance.Enemies[Random.Range(0, GameManager.Instance.Enemies.Count)].transform;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Viking>() && !IsGrown)
        {
            other.gameObject.GetComponent<Viking>().Death();
            AddKilledEnemy();
        }
        if (other.gameObject.GetComponent<Heir>() && !IsGrown)
        {
            if (other.gameObject.GetComponent<Heir>().IsGrown)
            {
                other.gameObject.GetComponent<Heir>().Death();
                AddKilledEnemy();
            }
        }
        if (other.gameObject.GetComponent<Character>() && IsGrown)
        {
            other.gameObject.GetComponent<Character>().Hit();
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
