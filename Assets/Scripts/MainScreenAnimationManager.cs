using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenAnimationManager : MonoBehaviour
{
    public bool doNotPlayAni;
    bool checkDoNotPlay;
    bool isPlayingAni;
    public AudioClip eatAudio;
    public AudioClip drinkAudio;
    public AudioClip changeAudio;
    // Start is called before the first frame update
    void Start()
    {
        doNotPlayAni = true;
        checkDoNotPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doNotPlayAni != checkDoNotPlay)
        {
            checkDoNotPlay = doNotPlayAni;
            if (!doNotPlayAni)
            {
                StartCoroutine(PlayEatAnimation());
                StartCoroutine(PlayDrinkAnimation());
                StartCoroutine(PlayChangeAnimation());
            }
            else StopAllCoroutines();
        }
    }
    private IEnumerator PlayEatAnimation()
    {
        float randomTime = Random.Range(35, 95);
        yield return new WaitForSeconds(randomTime);
        if (!isPlayingAni)
        {
            isPlayingAni = true;
            gameObject.GetComponent<Animator>().SetBool("getWings", true);
            gameObject.GetComponent<AudioSource>().PlayOneShot(eatAudio);
            yield return new WaitForSeconds(gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
            gameObject.GetComponent<Animator>().SetBool("getWings", false);
            isPlayingAni = false;
        }
        StartCoroutine(PlayEatAnimation());
    }
    private IEnumerator PlayDrinkAnimation()
    {
        float randomTime = Random.Range(10, 60);
        yield return new WaitForSeconds(randomTime);
        if (!isPlayingAni)
        {
            isPlayingAni = true;
            gameObject.GetComponent<Animator>().SetBool("getBeer", true);
            gameObject.GetComponent<AudioSource>().PlayOneShot(drinkAudio);
            yield return new WaitForSeconds(gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
            gameObject.GetComponent<Animator>().SetBool("getBeer", false);
            isPlayingAni = false;
        }
        StartCoroutine(PlayDrinkAnimation());
    }
    private IEnumerator PlayChangeAnimation()
    {
        float randomTime = Random.Range(100, 200);
        yield return new WaitForSeconds(randomTime);
        if (!isPlayingAni)
        {
            isPlayingAni = true;
            gameObject.GetComponent<Animator>().SetBool("changeBeer", true);
            gameObject.GetComponent<AudioSource>().PlayOneShot(changeAudio);
            yield return new WaitForSeconds(gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
            gameObject.GetComponent<Animator>().SetBool("changeBeer", false);
            isPlayingAni = false;
        }
        StartCoroutine(PlayChangeAnimation());
    }
}
