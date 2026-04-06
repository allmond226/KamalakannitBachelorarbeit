using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelChanger : MonoBehaviour
{
    public Sprite[] hoverSprites; // Array für die Hover-Sprites, [0] = normal, [1] = hover oben, [2] = hover unten
    public GameObject newsScreen;
    public GameObject worldScreen;
    public GameObject nameBox;
    public GameObject destroyButton;
    public GameObject newsPicture;
    public bool stopSwitch;
    public bool isEnd;
    public bool isDestroyed;
    Collider2D noteCollider;
    Camera mainCamera;
    bool closedTextBox;
    bool closedChoice;
    // Start is called before the first frame update
    void Start()
    {
        noteCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckNoteHover();
        isEnd = GameManager.Instance.isEnd;
        if (worldScreen.activeSelf)
        {
            GameManager.Instance.choiceBox1.SetActive(false);
            GameManager.Instance.choiceBox2.SetActive(false);
            newsPicture.SetActive(false);
            GameManager.Instance.textBox.SetActive(false);
            nameBox.SetActive(false);
            if (!isDestroyed) destroyButton.SetActive(true);
        }
    }

    private void CheckNoteHover()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (stopSwitch) return;
        
        // Prüfen ob Maus über dem Objekt ist
        if (noteCollider.OverlapPoint(mouseWorldPos))
        {
            Bounds bounds = noteCollider.bounds;

            float middleY = bounds.center.y;

            if (mouseWorldPos.y > middleY)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = hoverSprites[1]; // Hover-Sprite für obere Hälfte
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchChannel(1);
                }
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = hoverSprites[2]; // Hover-Sprite für untere Hälfte
                if (Input.GetMouseButtonDown(0))
                {
                    SwitchChannel(2);
                }
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = hoverSprites[0]; // Normales Sprite
        }
    }
    public void BlockSwitch(float time)
    {
        stopSwitch = true;
        Invoke("Unblock", time);
    }
    void Unblock()
    {
        stopSwitch = false;
    }
    public void SwitchChannel(int channel)
    { 
        if (stopSwitch) return;
        if (channel == 1)
        {
            newsScreen.SetActive(true);
            worldScreen.SetActive(false);
            destroyButton.SetActive(false);
            if (closedTextBox && !isDestroyed && !isEnd)
            {
                GameManager.Instance.textBox.SetActive(true);
                nameBox.SetActive(true);
                newsPicture.SetActive(true);
                TextManager.Instance.ShowText(TextManager.Instance.currentDialogue.lines[TextManager.Instance.currentLine]);
                closedTextBox = false;
            }
            if (closedChoice && !isDestroyed && !isEnd)
            {
                GameManager.Instance.choiceBox1.SetActive(true);
                GameManager.Instance.choiceBox2.SetActive(true);
                closedChoice = false;
            }
        }
        else if (channel == 2)
        {
            newsScreen.SetActive(false);
            if (GameManager.Instance.textBox.activeSelf)
            {
                TextManager.Instance.textActive = false;
                TextManager.Instance.StopTextManager();
                closedTextBox = true;
            }
            if (GameManager.Instance.choiceBox1.activeSelf || GameManager.Instance.choiceBox2.activeSelf)
            {
                closedChoice = true;
            }
            worldScreen.SetActive(true);
        }
    }
}
