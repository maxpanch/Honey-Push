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
    public Transform[] VikingSpawnPoints;
    public Transform SortingGroup;
    [Header("Intro Cutscene")]
    public GameObject Queen;
    public GameObject QueenSprite;
    public GameObject Bubble;
    public CanvasGroup CanvasGroup;
    public GameObject Title;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        SetState(State.Menu);
    }
    private void Update()
    {
        if (GameState == State.Intro || GameState == State.Tutorial || GameState == State.Lose || GameState == State.Win || GameState == State.Menu) return;

        Timer -= Time.deltaTime;
        TimerUI.GetComponent<TMP_Text>().text = Mathf.Ceil(Timer) + "";

        if (Timer <= 0) SetState(State.Win);
    }
    public IEnumerator WaveSpawnRoutine()
    {
        while (IsSpawning)
        {
            GameObject viking = Instantiate(Viking, VikingSpawnPoints[Random.Range(0, VikingSpawnPoints.Length)].transform.position + new Vector3(0, Random.Range(-2, 2), 0), Quaternion.identity);
            viking.GetComponent<Viking>().Destination = King;
            viking.transform.parent = SortingGroup;
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
        GameState = state;
        if (GameState == State.Intro)
        {
            MouseAimSprite.SetActive(false);
            CanvasGroup.alpha = 0f;
            Bubble.SetActive(true);
            IntroUI.enabled = true;
            ShowIntro();
        }
        if (GameState == State.Tutorial) ShowTutorial();
        if (GameState == State.Game)
        {
            Bubble.SetActive(false);
            IntroUI.enabled = false;
            TimerUI.SetActive(true);
            MouseAimSprite.SetActive(true);
            CanvasGroup.alpha = 1f;
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
        yield return new WaitForSeconds(dialogueLength / 4);
        Bubble.SetActive(true);
        StartCoroutine(FadeRoutine(0.5f));
        IntroUI.text = "WE'RE GETTING RAIDED!";
        yield return new WaitForSeconds(dialogueLength * 1.5f);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "THIS RAIDERS ARE MERCILESS AND VIOLENT!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "BUT I... I WANT TO LIVE MY LIFE!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "SO, MY QUEEN, WE DOING IT!";
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";

        float queenMoveSeconds = 2f;
        while (queenMoveSeconds > 0)
        {
            queenMoveSeconds -= Time.deltaTime;
            QueenSprite.transform.position = Vector3.MoveTowards(QueenSprite.transform.position, King.transform.position + new Vector3(0.6f, 0, 0), 1f * Time.deltaTime);
            yield return null;
        }
        QueenSprite.SetActive(false);
        Queen.SetActive(true);
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "HONEY, YOU NEED TO PUSH!";
        yield return new WaitForSeconds(dialogueLength * 2);
        StartCoroutine(FadeRoutine(-0.5f));
        IntroUI.text = "";
        Bubble.SetActive(false);
        Title.SetActive(false);
        yield return new WaitForSeconds(dialoguePause);
        SetState(State.Game);
    }
    private IEnumerator FadeRoutine(float value)
    {
        yield return new WaitForSeconds(0.1f);
        float fadeSeconds = 2f;
        while (fadeSeconds > 0)
        {
            fadeSeconds -= Time.deltaTime;
            CanvasGroup.alpha += value * Time.deltaTime;
            yield return null;
        }
    }
    public void ShowTutorial()
    {

    }
}
public enum State
{
    Menu,
    Intro,
    Tutorial,
    Game,
    Win,
    Lose
}
