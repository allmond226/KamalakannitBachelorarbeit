using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool randomScenario;
    public GameObject textBox;
    public GameObject note;
    public GameObject startAnimation;
    public GameObject endAnimation;
    public GameObject startScreen;
    public GameObject activeScenario;
    public GameObject dictatorScenario;
    public GameObject fanaticScenario;
    public GameObject gameOverMenu;
    public GameObject staticScreen;
    public GameObject blackTV;
    public GameObject tvLight;
    public GameObject startNewsscreen;
    public GameObject rewindAni;
    public GameObject[] destroyAnimations;
    public List<GameObject> scenarios;
    public Animator textboxAnimation;
    public Animator fadeToBlack;
    public Animator normalTalk;
    public GameObject choiceBox1;
    public GameObject choiceBox2;
    public bool isEnd;
    public bool addedfScenario;
    public float surfaceArea;
    public GameObject surfaceGameOver;
    public float faith;
    public bool presidentDead;
    public bool alienBadEnd;
    public bool scenarioFight;
    bool isStart;
    bool surfaceGOtriggered;
    Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
        textBox.SetActive(false);
        randomScenario = false;
        activeScenario = scenarios[0];
        note.GetComponent<ChannelChanger>().stopSwitch = true;
        isStart = true;
        surfaceArea = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStart)
        {
            startScreen.SetActive(false);
            startAnimation.SetActive(true);
            startNewsscreen.SetActive(true);
            isStart = false;
        }
        if (Input.GetKeyDown("1")) note.GetComponent<ChannelChanger>().SwitchChannel(1);
        if (Input.GetKeyDown("2")) note.GetComponent<ChannelChanger>().SwitchChannel(2);
        if (surfaceArea < 30 && !surfaceGOtriggered)
        {
            if (!alienBadEnd) ExplodeWorld();
            surfaceGOtriggered = true;
        }
    }
    void ChooseScenario()
    {
        if (isEnd) return;
        activeScenario.GetComponent<ScenarioManager>().SetProgress();
        if (surfaceArea < 30) return;
        bool isDone = CheckIfDone();
        if (randomScenario && !isEnd && !isDone)
        {
            List<GameObject> remainingScenarios = scenarios.FindAll(scenario => !scenario.GetComponent<ScenarioManager>().skipScenario);
            int randomIndex = UnityEngine.Random.Range(0,remainingScenarios.Count);
            activeScenario = remainingScenarios[randomIndex];
        }
        print("Choosing new scenario...");
        if (activeScenario.GetComponent<ScenarioManager>().queuedAnimation != null) 
        {
            Camera.main.GetComponent<EventCamera>().eventToPlay = activeScenario.GetComponent<ScenarioManager>().queuedAnimation;
            Camera.main.GetComponent<EventCamera>().StartZoomIn();
            activeScenario.GetComponent<ScenarioManager>().queuedAnimation = null;
        } 
        else 
        { 
            if (!isEnd && !isDone)
            {
                fadeToBlack.SetTrigger("Blackscreen");
                Invoke("OpenTextBox", 2);
                note.GetComponent<ChannelChanger>().BlockSwitch(3.5f);
            }
        }
        CheckWorldStatus();
    }

    private void CheckWorldStatus()
    {
        activeScenario.GetComponent<ScenarioManager>().CheckWorldChanges();
        bool currentFight = false;
        foreach (var item in scenarios)
        {
            if (item.GetComponent<ScenarioManager>().fight)
            {
                scenarioFight = true;
                currentFight = true;
            }
        }
        if (!currentFight) scenarioFight = false;
    }

    public void OpenTextBox()
    {   
        textBox.SetActive(true);
        textboxAnimation.SetTrigger("PopUp");
    }   
    public void CheckEndConditions()
    {
        print("Checking end conditions...");
        if (activeScenario.GetComponent<ScenarioManager>().isChoice)
        {
            Debug.Log("Choice should be called");
            ShowChoice();
            activeScenario.GetComponent<ScenarioManager>().isChoice = false;
        }
        else
        {
            ChooseScenario();
        }
    }
    public bool CheckIfDone()
    {
        if (scenarios.Count == 0 && !isEnd)
        {
            blackTV.SetActive(true);
            endAnimation.SetActive(true);
            tvLight.SetActive(false);
            note.GetComponent<ChannelChanger>().stopSwitch = true;
            Invoke("TvOff", endAnimation.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
            return true;
        }
        return false;
    }
    void TvOff()
    { 
        endAnimation.SetActive(false);
        SetGameOver();
    }

    private void ShowChoice()
    {
        for (int i = 0; i < activeScenario.GetComponent<ScenarioManager>().withChoices.Count; i++)
        {
            if (TextManager.Instance.currentDialogue == activeScenario.GetComponent<ScenarioManager>().withChoices[i])
            {
                choiceBox1.SetActive(true);
                choiceBox2.SetActive(true);
                choiceBox1.GetComponent<ChoiceBox>().ShowChoice(activeScenario.GetComponent<ScenarioManager>().choices[i]);
                choiceBox2.GetComponent<ChoiceBox>().ShowChoice(activeScenario.GetComponent<ScenarioManager>().choices[i]);
            }
        }
    }
    public void ConfirmChoice(bool choice)
    {
        activeScenario.GetComponent<ScenarioManager>().choicesMade.Add(choice);
        choiceBox2.SetActive(false);
        choiceBox1.SetActive(false);
        CheckEndConditions();
    }
    public void DestroyWorld(int i)
    {
        destroyAnimations[i].SetActive(true);
        isEnd = true;
        note.GetComponent<ChannelChanger>().isDestroyed = true;
        note.GetComponent<ChannelChanger>().destroyButton.SetActive(false);
        Invoke("SetGameOver", destroyAnimations[i].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }
    public void ExplodeWorld()
    { 
        textBox.SetActive(false);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        isEnd = true;
        mainCamera.GetComponent<EventCamera>().eventToPlay = surfaceGameOver;
        note.GetComponent<ChannelChanger>().isDestroyed = true;
        note.GetComponent<ChannelChanger>().destroyButton.SetActive(false);
        mainCamera.GetComponent<EventCamera>().StartZoomIn();
        Invoke("SetGameOver", 6);
    }
    public void SetGameOver()
    {
        gameOverMenu.SetActive(true);
        choiceBox1.SetActive(false);
        choiceBox2.SetActive(false);
        textBox.SetActive(false);
        textBox.GetComponent<Textbox>().nameBoxObject.SetActive(false);
    }
    public void Rewind()
    {
        StartCoroutine(RewindGame());
    }
    IEnumerator RewindGame() //Spielt Animation abhängig von den vorherigen Screens ab
    {
        //Phase 1 Spiele Animation auf aktiven Screen ab
        TextManager.Instance.GetComponent<AudioSource>().clip = null;
        tvLight.SetActive(true);
        isEnd = false;
        gameOverMenu.SetActive(false);
        rewindAni.SetActive(true);
        note.GetComponent<ChannelChanger>().stopSwitch = true;
        staticScreen.GetComponent<AudioSource>().clip = null;
        yield return new WaitForSeconds(1f);
        //Phase 2 wechsle den Screen, für den Fall das Rewind im World Screen aktiviert wurde
        note.GetComponent<ChannelChanger>().stopSwitch = false;
        note.GetComponent<ChannelChanger>().SwitchChannel(1);
        note.GetComponent<ChannelChanger>().stopSwitch = true;
        yield return new WaitForSeconds(0.5f);
        //Phase 3 da der schwarze oder Störungs-Bildschirm nur über den vorherigen Bildschirm gelegt wurde können wir diese deaktiveren um den vorherigen Bildschirm erneut anzuzeigen
        if (staticScreen.activeSelf) staticScreen.SetActive(false);
        if (blackTV.activeSelf) blackTV.SetActive(false);
        TextManager.Instance.talkAnimation.SetBool("talk",true);
        staticScreen.SetActive(false);
        if (!startNewsscreen.activeSelf)  //Phase 4 Wenn der normale Newsscreen nicht aktiv ist, wird dieser nach einer Zeit aktiviert
        {
            print("Called Phase4...");
            yield return new WaitForSeconds(1f);
            var parent = startNewsscreen.transform.parent;
            if (parent != null)
            {
                int childCount = parent.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    parent.GetChild(i).gameObject.SetActive(false);
                }
            }
            startNewsscreen.SetActive(true);
            TextManager.Instance.talkAnimation = normalTalk;
            TextManager.Instance.talkAnimation.SetBool("talk", true);
            yield return new WaitForSeconds(2.5f);
        }
        else
        {
            print("Skip Phase4...");
            yield return new WaitForSeconds(3.5f); 
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void CallScenarioCheck()
    {
        foreach (var item in scenarios)
        {
            item.GetComponent<ScenarioManager>().CheckScenarioChanges();
        }
    }
}
