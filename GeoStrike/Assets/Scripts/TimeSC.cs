using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Time.timeScale = 10;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }
}
