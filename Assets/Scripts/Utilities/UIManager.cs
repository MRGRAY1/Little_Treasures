using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject[] screens;

    public void ShowScreen(string screenName)
    {
        foreach (var screen in screens)
        {
            screen.SetActive(screen.name == screenName);
        }
    }
}

