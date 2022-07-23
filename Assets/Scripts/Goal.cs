using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.black;//olum parcaları default renk
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        GameManager.singleton.NextLevel();
    }
}
