using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionScenarioManager : ScenarioManager
{
    public bool fightStarted;

    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (consequenceIndex == 1)
        {
            if (decision)
            {
                scenarioDialogues.RemoveAt(3);
            }
            else
            {
                scenarioDialogues.RemoveAt(2);
                scenarioDialogues.RemoveAt(3);
            }
        }
        if (consequenceIndex == 3)
        {
            fightStarted = true;
        }
        scenarioLenght = scenarioDialogues.Count;
    }
}
