using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool junang = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (junang)
        {
        Debug.Log("Áß¾Ó"+ this.transform.position);
        }
        else
        {
            Debug.Log("¿ÞÂÊ¾Æ·¡" + this.transform.position);
        }
        
    }
}
