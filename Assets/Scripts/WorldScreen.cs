using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreen : MonoBehaviour
{
    public static WorldScreen Instance { get; private set; }
    public Sprite[] worldSprites;
    public GameObject destroyAnimation;
    public GameObject worldSpriteObject;
    public GameObject[] statues;
    public GameObject war;
    public GameObject ufo;
    public float faith;
    public float surfaceSize;
    bool displayWar;
    void Awake()
    {
        // Ensure we don't overwrite an existing instance with a destroyed or duplicate one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        surfaceSize = GameManager.Instance.surfaceArea;
        faith = GameManager.Instance.faith;
        displayWar = GameManager.Instance.scenarioFight;
        if (surfaceSize <= 70)
        {
            if (surfaceSize < 30)
            {
                worldSpriteObject.SetActive(false);
                destroyAnimation.SetActive(true);
            }
            else
            {
                worldSpriteObject.SetActive(true);
                destroyAnimation.SetActive(false);
                worldSpriteObject.GetComponent<SpriteRenderer>().sprite = worldSprites[2];
            }
        }
        else
        {
            worldSpriteObject.SetActive(true);
            destroyAnimation.SetActive(false);
        }
        if (displayWar) war.SetActive(true);
        else war.SetActive(false);

    }
    public void UpdateStatues(int i)
    {
        foreach (GameObject item in statues)
        {
            item.SetActive(false);
        }
        if (i < statues.Length) statues[i].SetActive(true);
    }
}
