using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get { return instance; } }  
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        
    }
}
