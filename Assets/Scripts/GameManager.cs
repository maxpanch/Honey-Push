using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject TutorialUI;
    public GameObject TutorialButtonUI;
    public TMP_Text WinLoseUIText;
    public GameObject RetryUI;
    public GameObject Slider;
    public GameObject Button;
    public GameObject HelpTutorial;
    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        AudioManager.Instance.Play(SoundEnum.hp_amb_tone, 1, true);
        AudioManager.Instance.Play(SoundEnum.GamejamSoulsHoneyPushMenu);
        SetState(State.Menu);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushGame, 1);
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushLose, 1);
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushWin, 1);
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushMenu, 1);
            AudioManager.Instance.Stop(SoundEnum.hp_amb_tone, 1);
            SceneManager.LoadSceneAsync(0);
        }
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
            Title.SetActive(true);
            MouseAimSprite.SetActive(false);
            CanvasGroup.alpha = 0f;
            Bubble.SetActive(true);
            IntroUI.enabled = true;
            ShowIntro();
        }
        if (GameState == State.Tutorial) ShowTutorial();
        if (GameState == State.Game)
        {
            TutorialButtonUI.SetActive(false);
            TutorialUI.SetActive(false);
            Bubble.SetActive(false);
            IntroUI.enabled = false;
            TimerUI.SetActive(true);
            RetryUI.SetActive(true);
            MouseAimSprite.SetActive(true);
            // HelpTutorial.SetActive(true);
            CanvasGroup.alpha = 1f;
            AudioManager.Instance.Play(SoundEnum.GamejamSoulsHoneyPushGame, 1);
            StartCoroutine(WaveSpawnRoutine());
        }
        if (GameState == State.Lose)
        {
            StopCoroutine(WaveSpawnRoutine());
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushGame, 1);
            AudioManager.Instance.Play(SoundEnum.GamejamSoulsHoneyPushLose);
            MouseAimSprite.SetActive(false);
            WinLoseUIText.gameObject.SetActive(true);
            WinLoseUIText.enabled = true;
            WinLoseUIText.text = "NOOOOO, MY LIFE ;C";
            King.GetComponent<Character>().HorizontalMovement = 0;
            King.GetComponent<Character>().VerticalMovement = 0;
            StartCoroutine(RestartWinGame(12));
        }
        if (GameState == State.Win)
        {
            StopCoroutine(WaveSpawnRoutine());
            AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushGame, 1);
            AudioManager.Instance.Play(SoundEnum.GamejamSoulsHoneyPushWin);
            MouseAimSprite.SetActive(false);
            WinLoseUIText.gameObject.SetActive(true);
            WinLoseUIText.enabled = true;
            WinLoseUIText.text = "MY LIFE IS IN MY HANDS! MY WIFE'S TOO!";
            King.GetComponent<Character>().HorizontalMovement = 0;
            King.GetComponent<Character>().VerticalMovement = 0;
            StartCoroutine(RestartWinGame(16));
        }
    }
    private IEnumerator RestartWinGame(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushGame, 1);
        AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushLose, 1);
        AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushWin, 1);
        AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushMenu, 1);
        AudioManager.Instance.Stop(SoundEnum.hp_amb_tone, 1);
        SceneManager.LoadSceneAsync(0);
        yield return null;
    }
    public void ShowIntro()
    {
        AudioManager.Instance.Stop(SoundEnum.GamejamSoulsHoneyPushMenu, 3f);
        StartCoroutine(IntroRoutine());
    }
    private IEnumerator IntroRoutine()
    {
        float dialogueLength = 3.5f;
        float dialoguePause = 0.6f;
        yield return new WaitForSeconds(dialogueLength / 4);
        Bubble.SetActive(true);
        StartCoroutine(FadeRoutine(0.5f));
        IntroUI.text = "WE'RE GETTING RAIDED!";
        AudioManager.Instance.Play(SoundEnum.hp_dialogue_1);
        yield return new WaitForSeconds(dialogueLength * 1.2f);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "THIS RAIDERS ARE MERCILESS AND VIOLENT!";
        AudioManager.Instance.Play(SoundEnum.hp_dialogue_2);
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "BUT I... I WANT TO LIVE MY LIFE!";
        AudioManager.Instance.Play(SoundEnum.hp_dialogue_3);
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        yield return new WaitForSeconds(dialoguePause);
        IntroUI.text = "SO, MY QUEEN, WE'RE DOIN IT!";
        AudioManager.Instance.Play(SoundEnum.hp_dialogue_4);
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";

        AudioManager.Instance.Play(SoundEnum.hp_queen_heels);
        float queenMoveSeconds = 3f;
        while (queenMoveSeconds > 0)
        {
            queenMoveSeconds -= Time.deltaTime;
            QueenSprite.transform.position = Vector3.MoveTowards(QueenSprite.transform.position, King.transform.position + new Vector3(0.6f, 0, 0), 1f * Time.deltaTime);
            yield return null;
        }
        AudioManager.Instance.Stop(SoundEnum.hp_queen_heels, 1f);
        AudioManager.Instance.Play(SoundEnum.hp_king_take_queen);
        QueenSprite.SetActive(false);
        Queen.SetActive(true);
        yield return new WaitForSeconds(dialoguePause * 2);
        IntroUI.text = "HONEY, YOU NEED TO PUSH!";
        AudioManager.Instance.Play(SoundEnum.hp_dialogue_5);
        yield return new WaitForSeconds(dialogueLength * 1.3f);
        StartCoroutine(FadeRoutine(-0.5f));
        yield return new WaitForSeconds(dialogueLength);
        IntroUI.text = "";
        Bubble.SetActive(false);
        Title.SetActive(false);
        TutorialUI.SetActive(true);
        TutorialButtonUI.SetActive(true);
        SetState(State.Tutorial);
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
        StartCoroutine(FadeRoutine(1f));
        TutorialUI.SetActive(true);
        TutorialButtonUI.SetActive(true);
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
