using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 1.5f;
    public float HorizontalMovement;
    public float VerticalMovement;
    public Rigidbody Rigidbody;
    public float Health = 10;
    void Update()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal");
        VerticalMovement = Input.GetAxisRaw("Vertical");
    }
    private void FixedUpdate()
    {
        Rigidbody.velocity = new Vector3(HorizontalMovement * Speed, VerticalMovement * Speed, 0);
    }
    public void Hit()
    {
        Health -= 1;
        if (Health <= 0f) GameManager.Instance.SetState(State.Lose);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Viking>())
        {
            Debug.Log("VIKING DEATH");
            other.gameObject.GetComponent<Viking>().Death();
            Hit();
        }
    }
}