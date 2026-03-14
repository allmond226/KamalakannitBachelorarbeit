using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScreen : MonoBehaviour
{
    public Sprite[] worldSprites;
    public GameObject destroyAnimation;
    public GameObject worldSpriteObject;
    public float faith;
    public float surfaceSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        surfaceSize = GameManager.Instance.surfaceArea;
        faith = GameManager.Instance.faith;
        if (surfaceSize < 60)
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
    }
}
