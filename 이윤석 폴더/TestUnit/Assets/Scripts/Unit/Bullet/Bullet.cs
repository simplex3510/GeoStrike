using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int movespeed;

    public void OnCollisionEnter2D(Collision2D enemy)
    {
        
    }
}
