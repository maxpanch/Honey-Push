using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : MonoBehaviour
{
    public Transform Destination;
    public float Speed;
    public BoxCollider BoxCollider;
    public bool IsDead = false;
    public AudioSource FootstepAudioSource;
    public AudioSource DeathAudioSource;
    public float FootstepCooldown = 0.5f;
    public float FootstepCooldownBase;
    private void Start()
    {
        FootstepCooldownBase = FootstepCooldown;
    }
    private void Update()
    {
        if (IsDead) return;

        FootstepCooldown -= Time.deltaTime;
        if (FootstepCooldown < 0)
        {
            if (Random.Range(0, 10) > 6) AudioManager.Instance.Play(SoundEnum.hp_footstep, FootstepAudioSource);
            FootstepCooldown = FootstepCooldownBase;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
        if (GameManager.Instance.GameState == State.Win) Death();
    }
    public void Death()
    {
        StartCoroutine(DeathRoutine());
    }
    private IEnumerator DeathRoutine()
    {
        IsDead = true;
        BoxCollider.enabled = false;
        // AudioManager.Instance.Play(SoundEnum.)
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
