using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Slider Slider;
    public CanvasGroup CanvasGroup;
    public GameObject Title;
    public void PushButton()
    {
        Slider.value += 1;
        if (Slider.value == 10)
        {
            StartCoroutine(MenuFadeRoutine());
        }
    }
    private IEnumerator MenuFadeRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        float fadeSeconds = 2f;
        while (fadeSeconds > 0)
        {
            fadeSeconds -= Time.deltaTime;
            CanvasGroup.alpha -= 0.5f * Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.SetState(State.Intro);
        Title.SetActive(false);
        Slider.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
