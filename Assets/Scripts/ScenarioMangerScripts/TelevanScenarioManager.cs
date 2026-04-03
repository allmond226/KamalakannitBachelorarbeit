using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelevanScenarioManager : ScenarioManager
{
    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (consequenceIndex == 1) //Entscheidung: Fernseher reparieren/Abschalten
        {
            if (decision)
            {
                scenarioDialogues.RemoveAt(3);
            }
            else
            {
                scenarioDialogues.RemoveAt(2);
                scenarioDialogues.RemoveAt(3);
                queuedAnimation = eventAnimations[0];
            }
        }
        scenarioLenght = scenarioDialogues.Count;
    }
}
