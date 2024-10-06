using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Slider Slider;
    public CanvasGroup CanvasGroup;
    public void PushButton()
    {
        Slider.value += 1;
        AudioManager.Instance.Play(SoundEnum.hp_menu_push_baby);
        if (Slider.value == 10)
        {
            GetComponent<Button>().interactable = false;
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
        Slider.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
