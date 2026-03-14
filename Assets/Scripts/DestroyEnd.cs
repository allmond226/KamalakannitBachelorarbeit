using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnd : MonoBehaviour
{
    public GameObject destroyedAni;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyAni()
    {
        destroyedAni.SetActive(true);
        gameObject.SetActive(false);
    }
}
