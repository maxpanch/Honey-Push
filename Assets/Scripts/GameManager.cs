using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform King;
    public float Timer = 180;
    public GameObject TimerUI;
    public TMP_Text IntroUI;
    public GameObject MouseAimSprite;
    public State GameState = State.Intro;
    [Header("Wave Managing")]
    // public List<GameObject> Enemies;
    public bool IsSpawning = true;
    public GameObject Viking;
    public int WaveNumber = 1;
    public float WaveScaler = 3f;
    public int WaveEnemiesCount = 5;
    public float WaveTimeBetweenEnemies = 0.5f;
    public float WavePauseBetweenWaves = 5f;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        SetState(State.Game);
    }
    private void Update()
    {
        if (GameState == State.Intro || GameState == State.Tutorial || GameState == State.Lose || GameState == State.Win) return;

        Timer -= Time.deltaTime;
        TimerUI.GetComponent<TMP_Text>().text = Mathf.Ceil(Timer) + "";

        if (Timer <= 0) SetState(State.Win);
    }
    public IEnumerator WaveSpawnRoutine()
    {
        while (IsSpawning)
        {
            GameObject viking = Instantiate(Viking, new Vector3(Random.Range(-7, 7), Random.Range(-4, 4), 0), Quaternion.identity);
            viking.GetComponent<Viking>().Destination = King;
            WaveEnemiesCount -= 1;
            // Enemies.Add(viking);
            if (WaveEnemiesCount <= 0)
            {
                WaveNumber += 1;
                WaveEnemiesCount = (int)(WaveNumber * WaveScaler);
                yield return new WaitForSeconds(WavePauseBetweenWaves);
            }
            yield return new WaitForSeconds(WaveTimeBetweenEnemies);
        }
    }

    public void SetState(State state)
    {
        Debug.Log(state);
        GameState = state;
        if (GameState == State.Intro)
        {
            MouseAimSprite.SetActive(false);
            ShowIntro();
        }
        if (GameState == State.Tutorial) ShowTutorial();
        if (GameState == State.Game)
        {
            TimerUI.SetActive(true);
            MouseAimSprite.SetActive(true);
            StartCoroutine(WaveSpawnRoutine());
        }
        if (GameState == State.Lose)
        {
            MouseAimSprite.SetActive(false);
            Debug.Log("Lose");
        }
        if (GameState == State.Win)
        {
            MouseAimSprite.SetActive(false);
            Debug.Log("Lose");
        }
    }
    public void ShowIntro()
    {
        StartCoroutine(IntroRoutine());
    }
    private IEnumerator IntroRoutine()
    {
        float dialogueLength = 2f;
        float dialoguePause = 0.6f;
        IntroUI.text = "WE'RE GETTING RAIDED!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "THIS RAIDERS ARE MERCILESS AND VIOLENT!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "BUT I... I WANT TO LIVE ALL OF MY LIFE!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "SO, MY QUEEN, WE GOING TO DO IT!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "HONEY, PUSH! PUSH! PUSH!";
        yield return new WaitForSeconds(dialogueLength * 2);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        SetState(State.Game);
    }
    public void ShowTutorial()
    {

    }
}
public enum State
{
    Intro,
    Tutorial,
    Game,
    Win,
    Lose
}
