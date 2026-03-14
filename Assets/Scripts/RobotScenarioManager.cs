using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotScenarioManager : ScenarioManager
{
    public Dialogue robotOvertake;
    public Dialogue humanRetake;
    public GameObject[] newsscreens;
    public GameObject retakeAnimation;
    public Animator robotTalk;
    public Animator humanTalk;
    public AudioClip robotAudio;
    public AudioClip humanAudio;
    public TextMeshProUGUI speakerText;
    bool hasOvertaken;
    bool hasRetaken;
    public void Update()
    {
       if (GameManager.Instance.activeScenario.GetComponent<ScenarioManager>().currentDialogue == robotOvertake)
       {
           if (TextManager.Instance.currentLine == 1 && !hasOvertaken)
           {
               TextManager.Instance.textActive = false;
               hasOvertaken = true;
               Invoke("Overtake", 2f);
           }
           else if (TextManager.Instance.currentLine == 2)
           {
                speakerText.text = "X-3602";
                speakerText.color = new Color32(185, 85, 35, 255);
                TextManager.Instance.nameColor = speakerText.color;
                newsscreens[1].SetActive(true);
                newsscreens[2].SetActive(false);
            }
           else return;
        }
        if (GameManager.Instance.activeScenario.GetComponent<ScenarioManager>().currentDialogue == humanRetake)
        {
            if (TextManager.Instance.currentLine == 0 && !hasRetaken)
            {
                TextManager.Instance.textActive = false;
                hasRetaken = true;
                StartCoroutine(Retake());
            }
            else if (TextManager.Instance.currentLine == 1)
            {
                speakerText.text = "Chap";
                speakerText.color = new Color32(36, 77, 147, 255);
                TextManager.Instance.nameColor = speakerText.color;
            }
            else return;

        }

    }
    public override void ApplyChoiceConsequence(bool decision, int consequenceIndex)
    {
        if (consequenceIndex == 0)
        {
            if (decision)
            {
                scenarioDialogues.RemoveAt(1);
                scenarioDialogues.RemoveRange(2, 7);
            }
            else
            {
                scenarioDialogues.RemoveAt(2);
                scenarioDialogues.RemoveRange(9, 10);
                queuedAnimation = eventAnimations[0];
            }
        }
        if (consequenceIndex == 3)
        {
            if (choicesMade[0])
            {
                queuedAnimation = eventAnimations[1];
                return;
            }
            if (decision)
            {
                scenarioDialogues.RemoveAt(5);
            }
            else
            {
                scenarioDialogues.RemoveAt(4);
                scenarioDialogues.RemoveRange(5, 3);
            }
        }
        if (consequenceIndex == 5)
        {
            if (!choicesMade[0])
            {
                return;
            }
            else
            {
                if (decision)
                {
                    scenarioDialogues.RemoveAt(6);
                    queuedAnimation = eventAnimations[2];
                }
                else
                {
                    scenarioDialogues.RemoveAt(7);
                    queuedAnimation = eventAnimations[2];
                }
            }
        }
        if (consequenceIndex == 7 && !choicesMade[0] && choicesMade[1])
        {
            GameManager.Instance.SetGameOver();
            newsscreens[3].SetActive(true);
        }
        scenarioLenght = scenarioDialogues.Count;
    }
    void Overtake()
    {
        newsscreens[2].SetActive(true);
        newsscreens[0].SetActive(false);
        TextManager.Instance.talkAnimation = robotTalk;
        TextManager.Instance.GetComponent<AudioSource>().Stop();
        TextManager.Instance.GetComponent<AudioSource>().clip = robotAudio;
        TextManager.Instance.textActive = true;
    }
    IEnumerator Retake()
    {
        yield return new WaitForSeconds(4.5f); 
        newsscreens[1].SetActive(false);
        newsscreens[0].SetActive(false);
        newsscreens[2].SetActive(false);
        retakeAnimation.SetActive(true);
        yield return new WaitForSeconds(retakeAnimation.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
        retakeAnimation.SetActive(false);
        newsscreens[0].SetActive(true);
        TextManager.Instance.textActive = true;
        TextManager.Instance.talkAnimation = humanTalk;
        TextManager.Instance.GetComponent<AudioSource>().Stop();
        TextManager.Instance.GetComponent<AudioSource>().clip = humanAudio;
    }
    public override void CheckScenarioChanges()
    {
        if (GameManager.Instance.presidentDead)
        {
            if (altTexts[0] != null) scenarioDialogues[3] = altTexts[0];
            if (altTexts[1] != null) scenarioDialogues[5] = altTexts[1];
            altTexts.Clear();
        }
    }
}