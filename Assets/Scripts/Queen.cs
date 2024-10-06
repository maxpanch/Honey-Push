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
    public LayerMask EnemyLayerMask;
    public SpriteRenderer QueenSprite;
    private void Start()
    {
        PowerBase = Power;
        ShootCooldownBase = ShootCooldown;
        ShootCooldown = 1;
    }
    void Update()
    {
        if (GameManager.Instance.GameState == State.Intro || GameManager.Instance.GameState == State.Tutorial || GameManager.Instance.GameState == State.Lose || GameManager.Instance.GameState == State.Win || GameManager.Instance.GameState == State.Menu) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        ShootCooldown -= Time.deltaTime;
        if (ShootCooldown <= 0)
        {
            if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)) Power += Time.deltaTime / 4;
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Animator.SetTrigger("Shoot");
                GameObject bullet = Instantiate(Heir, SpawnPoint.position, quaternion.identity);
                Color color = QueenSprite.color;
                color.a = 0.2f;
                QueenSprite.color = color;
                StartCoroutine(ReturnColorAlpha(ShootCooldownBase));
                AudioManager.Instance.Play(SoundEnum.hp_baby);

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 40f, EnemyLayerMask);
                float closestEnemyDistance = 999f;
                foreach (var hitCollider in hitColliders)
                {
                    // Debug.Log(hitCollider.gameObject.name + " - " + Vector3.Distance(transform.position, hitCollider.transform.position) + ", closest is " + closestEnemyDistance);
                    if (Vector3.Distance(transform.position, hitCollider.transform.position) < closestEnemyDistance)
                    {
                        bullet.GetComponent<Heir>().Destination = hitCollider.transform;
                        closestEnemyDistance = Vector3.Distance(transform.position, hitCollider.transform.position);
                    }
                }
                if (bullet.GetComponent<Heir>().Destination == null) bullet.GetComponent<Heir>().Destination = GameManager.Instance.King;
                // Debug.Log(bullet.GetComponent<Heir>().Destination);
                bullet.transform.parent = GameManager.Instance.SortingGroup;
                // Debug.Log(Aim.transform.position);
                bullet.GetComponent<Heir>().Rigidbody.AddForce(bullet.GetComponent<Heir>().Destination.position * Power, ForceMode.Impulse);
                Power = PowerBase;
                ShootCooldown = ShootCooldownBase;
            }
        }
    }
    private IEnumerator ReturnColorAlpha(float cooldown)
    {
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            Color color = QueenSprite.color;
            color.a += 0.25f * Time.deltaTime;
            QueenSprite.color = color;
            yield return null;
        }
        yield return null;
    }
}
