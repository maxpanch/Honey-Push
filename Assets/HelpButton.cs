using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public GameObject Tutorial;
    public void ShowTutorial()
    {
        Tutorial.SetActive(true);
    }
}
