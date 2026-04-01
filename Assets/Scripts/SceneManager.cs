using UnityEngine;
//Build error: Namespace 'UnityEditor' is not allowed to be used in runtime scripts.
using TMPro;
public class SceneManager : MonoBehaviour
{

    [SerializeField] private GameObject MainScreen;
    [SerializeField] private GameObject RaceCreatorScreen;
    
    public void ShowHideMainScreen()
    {
        //compiler error: syntax error
        MainScreen.SetActive(!MainScreen.activeSelf);
    }
    public void ShowHideRaceCreatorScreen()
    {
        //compiler error: syntax error
        RaceCreatorScreen.SetActive(!RaceCreatorScreen.activeSelf);
    }
}
