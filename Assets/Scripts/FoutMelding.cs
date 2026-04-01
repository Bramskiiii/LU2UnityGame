using TMPro;
using UnityEngine;

public class FoutMelding : MonoBehaviour
{
    public string message = "Dit is een foutmelding.";
    public int errorCode = 404;
    public bool isCritical = true;



    public GameObject relatedObject;
    public TMP_InputField userInput;
    public TextMeshProUGUI displayText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Er is een fout opgetreden bij het laden van de gegevens.");
        displayText.text = message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
