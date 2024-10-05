using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heir : MonoBehaviour
{
    public int KilledEnemies = 3;
    public bool IsGrown = false;
    public Transform Destination;
    public float Speed;
    private void Start()
    {
        Destination = GameManager.Instance.King;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
    }
    public void AddKilledEnemy()
    {
        KilledEnemies -= 1;
        if (KilledEnemies <= 0)
        {
            IsGrown = true;
            // GameManager.Instance.Enemies.Add(gameObject);
            Destination = GameManager.Instance.King;
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
    }
}
