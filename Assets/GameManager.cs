using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform King;
    public float Timer = 180;
    public State GameState = State.Intro;
    [Header("Wave Managing")]
    // public List<GameObject> Enemies;
    public bool IsSpawning = true;
    public GameObject Viking;
    public int WaveNumber = 1;
    public float WaveScaler = 1.2f;
    public int WaveEnemiesCount = 5;
    public float WaveTimeBetweenEnemies = 0.5f;
    private void Start()
    {
        StartCoroutine(WaveSpawnRoutine());
    }
    public IEnumerator WaveSpawnRoutine()
    {
        while (IsSpawning)
        {
            GameObject viking = Instantiate(Viking, new Vector3(Random.Range(-7, 7), Random.Range(-4, 4), 0), Quaternion.identity);
            viking.GetComponent<Viking>().Destination = King;
            // Enemies.Add(viking);
            yield return new WaitForSeconds(WaveTimeBetweenEnemies);
        }
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetState(State state)
    {
        Debug.Log(state);
        GameState = state;
        if (GameState == State.Lose) Debug.Log("Lose");
    }
}
public enum State
{
    Intro,
    Game,
    Win,
    Lose
}
