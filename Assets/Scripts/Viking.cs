using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : MonoBehaviour
{
    public Transform Destination;
    public float Speed;
    public BoxCollider BoxCollider;
    public bool IsDead = false;
    private void Update()
    {
        if (IsDead) return;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
    }
    public void Death()
    {
        // GameManager.Instance.Enemies.Remove(gameObject);
        Destroy(gameObject, 1f);
        BoxCollider.enabled = false;
        IsDead = true;
    }
}
