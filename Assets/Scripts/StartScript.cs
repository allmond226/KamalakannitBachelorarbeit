using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    public GameObject tvLight;
    public GameObject tvScreen;
    public GameObject Blackscreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TVIsOn ()
    {
        tvLight.SetActive(true);
        tvScreen.SetActive(true);
        Blackscreen.SetActive(false);
        GameManager.Instance.Invoke("OpenTextBox", 1f);
        GameManager.Instance.note.GetComponent<ChannelChanger>().BlockSwitch(2.5f);
        gameObject.SetActive(false);
    }
}
