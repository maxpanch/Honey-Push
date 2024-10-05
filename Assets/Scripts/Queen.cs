using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Queen : MonoBehaviour
{
    public GameObject Heir;
    public MouseAimSprite Aim;
    public Transform SpawnPoint;
    public float Power = 0.2f;
    private float PowerBase;
    public float ShootCooldown = 0.5f;
    private float ShootCooldownBase;
    public Animator Animator;
    private void Start()
    {
        PowerBase = Power;
        ShootCooldownBase = ShootCooldown;
    }
    void Update()
    {
        if (GameManager.Instance.GameState == State.Intro || GameManager.Instance.GameState == State.Tutorial || GameManager.Instance.GameState == State.Lose || GameManager.Instance.GameState == State.Win) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        ShootCooldown -= Time.deltaTime;
        if (ShootCooldown <= 0)
        {
            if (Input.GetMouseButton(0)) Power += Time.deltaTime / 4;
            if (Input.GetMouseButtonUp(0))
            {
                Animator.SetTrigger("Shoot");
                GameObject bullet = Instantiate(Heir, SpawnPoint.position, quaternion.identity);
                bullet.GetComponent<Heir>().Destination = Aim.transform.position;
                bullet.transform.parent = GameManager.Instance.SortingGroup;
                // Debug.Log(Aim.transform.position);
                bullet.GetComponent<Heir>().Rigidbody.AddForce(bullet.GetComponent<Heir>().Destination * Power, ForceMode.Impulse);
                Power = PowerBase;
                ShootCooldown = ShootCooldownBase;
            }
        }
    }
}
