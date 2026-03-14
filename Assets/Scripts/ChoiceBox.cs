using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceBox : MonoBehaviour
{
    public TextMeshProUGUI choiceText;
    public bool leftChoice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowChoice(Dialogue dialogueChoice)
    {
        if (leftChoice)
        {
            // Flavor Text,  Leerzeile, und Entscheidung (mit größerer Größe und dunklem Orange)
            choiceText.text = dialogueChoice.lines[0] + "\n\n<size=40><color=#FF8C00>" + dialogueChoice.lines[1] + "</color></size>";
        }
        else
        {
            choiceText.text = dialogueChoice.lines[2] + "\n\n<size=40><color=#FF8C00>" + dialogueChoice.lines[3] + "</color></size>";
        }
    }
    public void SendChoice()
    {
        if (leftChoice)
        {
            GameManager.Instance.ConfirmChoice(true);
        }
        else
        {
            GameManager.Instance.ConfirmChoice(false);
        }
        
    }
}
