using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class test
{
    public test()
    {
        abc = 1;
        ddd = 5;
    }

    public int abc;
    public int ddd;
}


public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        test test = new test() { abc = 100};


        //test test1 = new test();
        //test1.abc = 10;

        Debug.Log(test.abc);
        Debug.Log(test.ddd);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
