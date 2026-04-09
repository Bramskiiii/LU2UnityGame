using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;
    [SerializeField] private GameObject[] screens;
    [SerializeField] private GameObject[] noReturnScreens;
    public int currentScreen;
    private Stack<int> history = new Stack<int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < screens.Length; i++)
            screens[i].SetActive(i == currentScreen);
    }

    public void SwitchTo(int index)
    {
        screens[currentScreen].SetActive(false);
        history.Push(currentScreen);
        currentScreen = index;
        screens[currentScreen].SetActive(true);
    }

    public void GoBack()
    {
        if (history.Count > 0)
        {
            int previous = history.Peek();
            if (noReturnScreens.Contains(screens[previous]))
                return;

            screens[currentScreen].SetActive(false);
            currentScreen = history.Pop();
            screens[currentScreen].SetActive(true);
        }
    }
}