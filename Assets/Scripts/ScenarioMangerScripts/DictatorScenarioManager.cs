using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictatorScenarioManager : ScenarioManager
{
    public GameObject rebellScenario;
    public GameObject priestScenario;
    public GameObject alienScenario;
    public GameObject fanaticScenario;
    public List<Dialogue> deathDialogues;
    public Dialogue addedDialogue;
    bool hasAddedDeathDialogues = false;
    private void Update()
    {
        if (rebellScenario.GetComponent<RevolutionScenarioManager>().fightStarted && !hasAddedDeathDialogues) //Rebellentod hinzufügen
        {
            scenarioDialogues.Add(deathDialogues[1]);
            addedDialogue = deathDialogues[1];
            hasAddedDeathDialogues = true;
            skipScenario = false;
        }
        if (fanaticScenario.GetComponent<FanaticsScenarioManager>().warStarted)
        {
            if (hasAddedDeathDialogues) //Der FanatikerTod hat Priorität vor dem Rebellentod, um das verzweigte Narrativ besser zu zeigen
            {
                if (addedDialogue = deathDialogues[1]) // Wenn der Rebellentod bereits hinzugefügt wurde, soll dieser durch den FanatikerTod ersetzt werden.
                {
                    scenarioDialogues.Remove(deathDialogues[1]);
                    scenarioDialogues.Add(deathDialogues[2]);
                    addedDialogue = deathDialogues[2];
                }
            }
            else //FanatikerTod hinzufügen
            {
                scenarioDialogues.Add(deathDialogues[2]);
                addedDialogue = deathDialogues[2];
                hasAddedDeathDialogues = true;
                skipScenario = false;
            }
        }
        else //Wenn der Krieg nicht mehr stattfindet, soll die entsprechende Todesfolge nicht mehr angezeigt werden, da sie sonst auch nach dem Ende von diesem noch auftaucht, was keinen Sinn ergibt.
        {
            if (addedDialogue == deathDialogues[2])
            {
                scenarioDialogues.Remove(deathDialogues[2]);
                addedDialogue = null;
                hasAddedDeathDialogues = false;
                skipScenario = true; 
            }
        }
        if (alienScenario.GetComponent<AlienScenarioManager>().religionAccepted) //AlienTod hat höchste Priorität
        {
            if (hasAddedDeathDialogues && addedDialogue != deathDialogues[0]) //andere Todesfolge durch Alientod austauschen
            {
                scenarioDialogues.Remove(addedDialogue);
                scenarioDialogues.Add(deathDialogues[0]);
                addedDialogue = deathDialogues[0];
                if (scenarioProgress >= 7)
                {
                    queuedAnimation = eventAnimations[2];
                }
            }
            else if (!hasAddedDeathDialogues) //Alientod hinzufügen
            {
                scenarioDialogues.Add(deathDialogues[0]);
                addedDialogue = deathDialogues[0];
                if (scenarioProgress >= 7)
                {
                    queuedAnimation = eventAnimations[2];
                }
                hasAddedDeathDialogues = true;
                skipScenario = false;
            }
        }
    }
    public override void SetProgress()
    {
        print("Checking for choice consequences...");
        ApplyChoiceConsequence(true, scenarioProgress);
        scenarioProgress++;
        if (scenarioProgress >= scenarioLenght)
        {
            if (hasAddedDeathDialogues == false)
            {
                skipScenario = true;
            }
            else
            {
                GameManager.Instance.scenarios.Remove(gameObject);
                GameManager.Instance.randomScenario = true;
            } 
            
            return;
        }
        else currentDialogue = scenarioDialogues[scenarioProgress];

        if (withChoices.Contains(currentDialogue))
        {
            isChoice = true;
        }
    }
    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (scenarioProgress == 1)
        {
            GameManager.Instance.scenarios.Add(rebellScenario);
        }
        if (scenarioProgress == 2)
        {
            GameManager.Instance.scenarios.Add(priestScenario);
            queuedAnimation = eventAnimations[0];
        }
        if (scenarioProgress == 5)
        {
            queuedAnimation = eventAnimations[1];
        }
        if (scenarioProgress == 7 && alienScenario.GetComponent<AlienScenarioManager>().religionAccepted)
        {
            queuedAnimation = eventAnimations[2];
        }
        scenarioLenght = scenarioDialogues.Count;
    }
}
