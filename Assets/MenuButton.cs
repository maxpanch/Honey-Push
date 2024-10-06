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
        CanvasGroup.alpha -= 0.1f;
        if (Slider.value == 10)
        {
            GameManager.Instance.SetState(State.Intro);
            Title.SetActive(false);
            Slider.gameObject.SetActive(false);
            gameObject.SetActive(false);
            CanvasGroup.alpha = 1f;
        }
    }
}
