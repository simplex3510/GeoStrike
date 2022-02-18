using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTTTT : MonoBehaviour
{
    public Collider2D cd;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         cd = Physics2D.OverlapCircle(this.transform.position, 10);
    }
}
