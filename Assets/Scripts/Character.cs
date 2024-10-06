using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Speed = 1.5f;
    public float HorizontalMovement;
    public float VerticalMovement;
    public Rigidbody Rigidbody;
    public float Health = 10;
    public Animator Animator;
    public TMP_Text HealthUI;
    void Update()
    {
        if (GameManager.Instance.GameState == State.Intro || GameManager.Instance.GameState == State.Tutorial || GameManager.Instance.GameState == State.Lose || GameManager.Instance.GameState == State.Win || GameManager.Instance.GameState == State.Menu) return;
        HealthUI.gameObject.SetActive(true);
        HealthUI.text = Health + " <3";
        HorizontalMovement = Input.GetAxisRaw("Horizontal");
        VerticalMovement = Input.GetAxisRaw("Vertical");
        if (HorizontalMovement != 0 || VerticalMovement != 0) Animator.SetBool("IsWalking", true);
        else Animator.SetBool("IsWalking", false);
    }
    private void FixedUpdate()
    {
        Rigidbody.velocity = new Vector3(HorizontalMovement * Speed, VerticalMovement * Speed, 0);
    }
    public void Hit()
    {
        Health -= 1;
        AudioManager.Instance.Play(SoundEnum.hp_king_hit);
        if (Health <= 0f) GameManager.Instance.SetState(State.Lose);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Viking>())
        {
            other.gameObject.GetComponent<Viking>().Death();
            Hit();
        }
    }
}