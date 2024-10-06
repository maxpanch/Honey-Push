using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public void TutorialEnd()
    {
        GameManager.Instance.SetState(State.Game);
    }
}
