using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceBox : MonoBehaviour
{
    public TextMeshProUGUI choiceText;
    public TextMeshProUGUI flavorText;
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
            flavorText.text = dialogueChoice.lines[0];
            choiceText.text = dialogueChoice.lines[1];
        }
        else
        {
            flavorText.text = dialogueChoice.lines[2];
            choiceText.text = dialogueChoice.lines[3];
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
