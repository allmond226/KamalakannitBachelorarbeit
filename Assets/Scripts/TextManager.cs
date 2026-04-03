using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    public Dialogue currentDialogue;
    public GameObject textBoxObject;
    public GameObject picture;
    public TMP_Text textBox;
    public GameObject nameBoxObject;
    public AudioClip normalTalk;
    public AudioClip robotTalk;
    public TMP_Text nameBox;
    public Color nameColor;
    public Animator talkAnimation;
    public float letterDelay = 0.05f;
    public int currentLine = 0;
    public bool textActive;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        nameBoxObject.SetActive(false);
        textActive = false;
    }
    private void Update()
    {
        if (textActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.isEnd) return;
                if (textBox.text == currentDialogue.lines[currentLine])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textBox.text = currentDialogue.lines[currentLine];
                    talkAnimation.SetBool("talk", false);
                }
            }
        }
        if (talkAnimation.GetBool("talk") && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
        else if (!talkAnimation.GetBool("talk") && GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }
    }
    public void StartDialogue()
    {
        textBoxObject.SetActive(true);
        currentDialogue = GameManager.Instance.activeScenario.GetComponent<ScenarioManager>().currentDialogue;
        currentLine = 0;
        picture.SetActive(true);
        picture.GetComponent<SpriteRenderer>().sprite = currentDialogue.NewsImage;
        ShowText(currentDialogue.lines[currentLine]);
    }
    public void ShowText(string fullText)
    {
        StopAllCoroutines();
        textActive = true;
        talkAnimation.SetBool("talk", true);
        StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        nameBox.text = currentDialogue.speakerName;
        nameBox.color = nameColor;
        textBox.text = "";

        foreach (char letter in fullText)
        {
            talkAnimation.SetBool("talk", true);
            textBox.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
        talkAnimation.SetBool("talk", false);
    }
    public void NextLine()
    {
        currentLine++;
        if (currentLine < currentDialogue.lines.Length)
        {
            ShowText(currentDialogue.lines[currentLine]);
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        textBox.text = "";
        picture.GetComponent<SpriteRenderer>().sprite = null;
        picture.SetActive(false);
        textBoxObject.SetActive(false);
        nameBoxObject.SetActive(false);
        textActive = false;
        GameManager.Instance.CheckEndConditions();
    }
    public void StopTextManager()
    {         
        StopAllCoroutines();
        textBox.text = "";
        print("Stopping Text Manager...");
    }
}
