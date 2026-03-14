using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCamera : MonoBehaviour
{
    // Start is called before the first frame update
    float startSize;
    public float resetTime;
    public GameObject eventToPlay;
    public Animator eventAnimation;
    public GameObject tvLight;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
   
    }
    public void StartZoomIn()
    {
        eventAnimation = eventToPlay.GetComponent<Animator>();
        gameObject.GetComponent<Animator>().SetTrigger("ZoomIn");
        GameManager.Instance.note.GetComponent<ChannelChanger>().stopSwitch = true;
    }

    public void PlayEventAnimation()
    {
        eventToPlay.SetActive(true);
        tvLight.SetActive(false);
        Invoke("EndEventAnimation", eventAnimation.runtimeAnimatorController.animationClips[0].length);
    }
    public void EndEventAnimation()
    {
        GameManager.Instance.fadeToBlack.SetTrigger("Blackscreen");
        Invoke("ResetCamera", resetTime);

    }
    public void ResetCamera()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Reset");
        if (!GameManager.Instance.isEnd)
        {
            GameManager.Instance.Invoke("OpenTextBox", 2);
            GameManager.Instance.note.GetComponent<ChannelChanger>().BlockSwitch(2);
        }
        eventToPlay.SetActive(false);
        tvLight.SetActive(true);
    }

}
