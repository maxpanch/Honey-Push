using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : MonoBehaviour
{
    public Transform Destination;
    public float Speed;
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
    }
    public void Death()
    {
        // GameManager.Instance.Enemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
