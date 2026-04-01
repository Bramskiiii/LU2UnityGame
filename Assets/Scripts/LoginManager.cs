using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField userInput;
    public TMP_InputField passInput;
    public TextMeshProUGUI displayText;

public string password = "Password"; // Example password

    public void Inlog()
    {
        displayText.text = "Welcome, " + userInput.text + "!";
        displayText.gameObject.SetActive(true);
        Debug.Log("Inlog Success");
    }

public void CheckPassword()
{
    if (passInput.text == password)
    {
        Inlog();
    }
    else
    {
        displayText.text = "Incorrect password. Please try again.";
        displayText.gameObject.SetActive(true);
        Debug.Log("Wachtwoord klopt niet");
    }
}


// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
{

}

// Update is called once per frame
void Update()
{

}
}
