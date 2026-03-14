using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textbox : MonoBehaviour
{
    public GameObject nameBoxObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartTypwriter()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        nameBoxObject.SetActive(true);
        TextManager.Instance.Invoke("StartDialogue",0.5f);
    }
}
