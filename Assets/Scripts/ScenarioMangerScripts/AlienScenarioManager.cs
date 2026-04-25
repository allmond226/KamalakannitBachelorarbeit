using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScenarioManager : ScenarioManager
{
    public bool canDefend; // Speichert, ob der Spieler die Option hat, sich zu verteidigen. Diese Variable wird verwendet, um die Konsequenzen der Entscheidungen zu steuern.
    public GameObject deadTV;
    public bool religionAccepted; // Speichert, ob der Spieler die Religion der Aliens akzeptiert hat.
    int religionChoiceIndex = 5; // Entscheidung muss variabel sein, da sie  an unterschiedlichen Stellen im Szenario auftreten kann. 
    int damageCosequenceIndex = 99; // Verhindert das Konsequenz außerhlb der gewollten Rute aufgerufen wird.
    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (consequenceIndex == 0) //Entscheidung: Kontakt suchen/Aliens Misstrauen
        {
            if (decision)//Vertrauen
            {
                scenarioDialogues.RemoveAt(1);
            }
            else //Misstrauen
            {
                scenarioDialogues.RemoveAt(2);
                world.GetComponent<WorldScreen>().worldSpriteObject.GetComponent<SpriteRenderer>().sprite = world.GetComponent<WorldScreen>().worldSprites[1];
            }
        }
        
        if (consequenceIndex == 1) queuedAnimation = eventAnimations[0]; //AnimationsTrigger

        if (consequenceIndex == 4) //Entscheidung: Reich anschließen/Unabhängig bleiben
        {
            if (decision) //Reich anschließen
            {
                scenarioDialogues.RemoveRange(5,2);
                scenarioDialogues.RemoveAt(6);
                scenarioDialogues.Add(scenarioDialogues[5]);
                scenarioDialogues.RemoveAt(5);
                
            }
            else //Unabhängig bleiben
            {
                if (canDefend) //Unabhängig bleiben + Verteidigungsmöglichkeit
                {
                    damageCosequenceIndex = 5; // Aktiviert die Konsequenzabfrage
                    religionChoiceIndex = 99; // Deaktiviert die Entschidungsabfrage (andere Rute)
                    scenarioDialogues.RemoveAt(5);
                    scenarioDialogues.RemoveRange(8, 3);
                    GameManager.Instance.randomScenario = false;
                }
                else //Unabhängig bleiben + Keine Verteidigungsmöglichkeit
                {
                    religionChoiceIndex = 6;
                    scenarioDialogues.RemoveRange(6, 3);
                }
            }
        }
        if (consequenceIndex == damageCosequenceIndex) //Konsequenz der Entscheidung Unabhängig bleiben, wenn Verteidigungsmöglichkeit besteht
        {
            if (choicesMade[0]) //Unabhängig bleiben + Verteidigen + Vertraut
            {
                scenarioDialogues.RemoveAt(6);
                queuedAnimation = eventAnimations[1];
                GameManager.Instance.surfaceArea -= 30;
            }
            else if (!choicesMade[0]) //Unabhängig bleiben + Verteidigen + Misstraut
            {
                scenarioDialogues.RemoveAt(7);
                queuedAnimation = eventAnimations[2];
            }
        }
        if (consequenceIndex == religionChoiceIndex) //Religion annehmen/Ablehnen
        {
            if (decision) //Religion annehmen
            {
                if (religionChoiceIndex == 5) scenarioDialogues.RemoveRange(7, 2);
                else if (religionChoiceIndex == 6) scenarioDialogues.RemoveAt(8);
                GameManager.Instance.faith -= 30;
            }
            else //Religion ablehnen
            {
                GameManager.Instance.randomScenario = false;
                if (canDefend)
                {

                    if (choicesMade[0]) //Ablehnen + Verteidigen + Vertraut
                    {
                        scenarioDialogues.RemoveAt(6);
                        scenarioDialogues.RemoveAt(7);
                        GameManager.Instance.faith -= 15;
                        queuedAnimation = eventAnimations[1];
                        GameManager.Instance.surfaceArea -= 30;
                    }
                    else if (!choicesMade[0]) //Ablehnen + Verteidigen + Misstraut
                    {
                        scenarioDialogues.RemoveRange(6, 2);
                        queuedAnimation = eventAnimations[2];
                    }
                }
                else //Ablehnen + Keine Verteidigungsmöglichkeit
                {
                    mainCamera.GetComponent<EventCamera>().eventToPlay = eventAnimations[3];
                    mainCamera.GetComponent<EventCamera>().StartZoomIn();
                    WorldScreen.Instance.ufo.SetActive(false);
                    GameManager.Instance.isEnd = true;
                    Invoke("BadEnd", eventAnimations[3].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length + 1.5f);
                }

            }
        }
        if (consequenceIndex == religionChoiceIndex+1)
        {
            if (choicesMade[2])
            {
                WorldScreen.Instance.UpdateStatues(1);
                religionAccepted = true;
            }
            else if (choicesMade[0] && canDefend)
            {
                WorldScreen.Instance.UpdateStatues(0);
            }
        }
        scenarioLenght = scenarioDialogues.Count;

    }
    void BadEnd()
    {
        GameManager.Instance.alienBadEnd = true;
        GameManager.Instance.surfaceArea = 25;
        deadTV.SetActive(true);
        GameManager.Instance.note.GetComponent<ChannelChanger>().stopSwitch = false;
        GameManager.Instance.note.GetComponent<ChannelChanger>().isDestroyed = true;
        GameManager.Instance.SetGameOver();
    }
    public override void CheckScenarioChanges()
    {
        if (GameManager.Instance.presidentDead)
        {
            if (altTexts[0] != null)
            {
                scenarioDialogues[0] = altTexts[0];
                withChoices[0] = altTexts[0];
            } 
            if (altTexts[1] != null) scenarioDialogues[1] = altTexts[1];
            if (altTexts[2] != null)
            {
                scenarioDialogues[5] = altTexts[2];
                withChoices[1] = altTexts[2];
            }
            if (altTexts[3] != null) scenarioDialogues[7] = altTexts[3];
            altTexts.Clear();
        }
        currentDialogue = scenarioDialogues[scenarioProgress];
    }
    public override void CheckWorldChanges()
    {
        if (scenarioProgress == 2)
        {
            var ws = WorldScreen.Instance;
            if (ws == null)
            {
                Debug.LogWarning("WorldScreen.Instance is null in AlienScenarioManager.CheckWorldChanges (scenarioProgress==2)");
            }
            else if (ws.ufo == null)
            {
                Debug.LogWarning("WorldScreen.ufo is null (not assigned or destroyed) in AlienScenarioManager.CheckWorldChanges");
            }
            else
            {
                ws.ufo.SetActive(true);
            }
        } //UFO erscheint
        if (scenarioProgress == religionChoiceIndex + 1)
        {
            WorldScreen.Instance.ufo.SetActive(false);
            world.GetComponent<WorldScreen>().worldSpriteObject.GetComponent<SpriteRenderer>().sprite = world.GetComponent<WorldScreen>().worldSprites[0];
        }
        if (scenarioProgress == damageCosequenceIndex)
        {
            WorldScreen.Instance.ufo.SetActive(false);
            world.GetComponent<WorldScreen>().worldSpriteObject.GetComponent<SpriteRenderer>().sprite = world.GetComponent<WorldScreen>().worldSprites[0];
        }
    }
    
}
