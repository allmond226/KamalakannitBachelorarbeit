using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanaticsScenarioManager : ScenarioManager
{
    public bool warStarted;
    // Start is called before the first frame update
    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (scenarioProgress == 0)
        {
            if (decision)
            {
                scenarioDialogues.RemoveAt(2);
                scenarioDialogues.RemoveRange(4, scenarioDialogues.Count-4);
                queuedAnimation = eventAnimations[0];
            }
            else
            {
                scenarioDialogues.RemoveAt(1);
                scenarioDialogues.RemoveAt(2);
                scenarioDialogues.RemoveAt(2);
                queuedAnimation = eventAnimations[1];
            }
        }
        if (scenarioProgress == 4) 
        {
            warStarted = true;
            GameManager.Instance.surfaceArea -= 10;
            print("Surface area decreased by war - 10. Current surface area: " + GameManager.Instance.surfaceArea);

        }
        if (scenarioProgress == 5)
        {
            GameManager.Instance.surfaceArea -= 15;
            print("Surface area decreased by war - 15. Current surface area: " + GameManager.Instance.surfaceArea);
            if (!decision)
            { 
                GameManager.Instance.surfaceArea -= 20;
                print("Surface area decreased by Attack - 20. Current surface area: " + GameManager.Instance.surfaceArea);
            }
        }
        if (scenarioProgress == 6)
        {
            if (decision)
            {
                GameManager.Instance.surfaceArea += 15;
                print("Area increased by rebuild +15. Current surface area: " + GameManager.Instance.surfaceArea);
            }
        }
        if (scenarioProgress == 7)
        {
                GameManager.Instance.surfaceArea -= 10;
                print("Area decreased by war -10. Current surface area: " + GameManager.Instance.surfaceArea);
        }
        if (scenarioProgress == 8)
        {
            if (decision)
            {
                GameManager.Instance.surfaceArea -= 20;
                queuedAnimation = eventAnimations[2];
                withChoices.Remove(scenarioDialogues[10]);
                scenarioDialogues.RemoveAt(10);
                print("Area decreased by terror -20. Current surface area: " + GameManager.Instance.surfaceArea);
            }
            else
            {
                GameManager.Instance.surfaceArea -= 10;
                queuedAnimation = eventAnimations[3];
                withChoices.Remove(scenarioDialogues[9]);
                scenarioDialogues.RemoveAt(9);
                print("Area decreased by singular terror -10. Current surface area: " + GameManager.Instance.surfaceArea);
            }
        }
        if (scenarioProgress == 9)
        { 
            if (decision)
            {
              warStarted = false;
            }
            else
            {
                scenarioDialogues.RemoveAt(10);
                GameManager.Instance.surfaceArea -= 50;
                print("Area decreased by end -50. Current surface area: " + GameManager.Instance.surfaceArea);
            }
        }
        scenarioLenght = scenarioDialogues.Count;
    }

}
