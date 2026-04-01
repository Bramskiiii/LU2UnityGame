using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateTrackManager : MonoBehaviour
{
    //Compiler/Build error: ontbrekende namespaces
    public TMP_InputField trackNameInputField;
    public TMP_InputField trackAuthorInputField;

    ////Compiler error: onbekende variabele/ type mismatch
    public TMP_InputField trackDescriptionInputField;
    public List<GameObject> tracks = new();


    public void CreateTrack()
    {
        string trackName = trackNameInputField.text;
        string trackAuthor = trackAuthorInputField.text;
        string trackDescription = trackDescriptionInputField.text;
        Debug.Log("Track Created: " + trackName + " by " + trackAuthor + ". Description: " + trackDescription);
    }

    
}
