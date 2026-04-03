using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
// Dises Skript managed/speichert Szenarien-Daten, wie welche Texte angezeigt und welche Animationen und Sounds abgespielt werden. Diese werden dann vom GameManager abgerufen.
{
    public int scenarioProgress; // Speichert den Fortschritt des Spiels, um zu wissen, welche Teile bereits durchlaufen wurden.
    public int scenarioLenght;
    public GameObject world;
    public Camera mainCamera; // Referenz zur Hauptkamera, um Kamerafahrten oder -effekte zu steuern.
    public Dialogue currentDialogue; // Das aktuelle Dialog-Objekt, das die Texte für das aktuelle Szenario enthält.
    public List<Dialogue> scenarioDialogues;
    public GameObject[] eventAnimations; // Speichert die Animationen, die im Szenario abgespielt werden sollen.
    public GameObject queuedAnimation;
    public List<Dialogue> withChoices;
    public List<Dialogue> altTexts;
    public Dialogue[] choices;
    public bool isChoice;
    public bool skipScenario;
    public List<bool> choicesMade; // Speichert welche Entscheidungen getroffen wurden
    // Start is called before the first frame update
    void Start()
    {
       mainCamera = Camera.main;
       scenarioProgress = 0;
       currentDialogue = scenarioDialogues[scenarioProgress];
       scenarioLenght = scenarioDialogues.Count;
    }
    
     public virtual void SetProgress()
     {
        print("Checking for choice consequences...");
        if (choicesMade.Count != 0) ApplyChoiceConsequence(choicesMade[choicesMade.Count-1],scenarioProgress);
        scenarioProgress++;
        if (scenarioProgress >= scenarioLenght)
        {
            GameManager.Instance.scenarios.Remove(gameObject);
            GameManager.Instance.randomScenario = true;
            if (GameManager.Instance.scenarios.Count < 3 && !GameManager.Instance.addedfScenario)
            {
                GameManager.Instance.scenarios.Add(GameManager.Instance.fanaticScenario);
                GameManager.Instance.addedfScenario = true;
            }
            return;
        }
        else currentDialogue = scenarioDialogues[scenarioProgress];

        if (withChoices.Contains(currentDialogue))
        {
            isChoice = true;
        }       
     }
    public virtual void ApplyChoiceConsequence(bool decision,int consequenceIndex)
    {
       if (consequenceIndex == 2)
       {
           if (decision)
           {
               queuedAnimation = eventAnimations[0];
               scenarioDialogues.RemoveAt(4);
               scenarioLenght = scenarioDialogues.Count;
            }
           else
           {
               queuedAnimation = eventAnimations[1];
               scenarioDialogues.RemoveAt(3);
               scenarioLenght = scenarioDialogues.Count;
               GameManager.Instance.presidentDead = true;
               GameManager.Instance.CallScenarioCheck();
               print("President is dead, calling scenario check...");
            }
        }
       if (consequenceIndex == 3)
       {
           if (decision)
           {
                GameManager.Instance.scenarios.Add(GameManager.Instance.dictatorScenario);
           }

        }
    }
    public virtual void CheckScenarioChanges()
    {

    }
}
