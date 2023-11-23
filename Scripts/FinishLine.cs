using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public bool Finished;
   private void OnTriggerEnter2D(Collider2D collision)
    {
        Timer timer = GameObject.Find("Text (TMP)").GetComponent<Timer>();
        if (collision.tag == "Player")
        {
            
            timer.timeIsRunning = false;
            Finished = true;
        }

        
    }
    void Start()
    {
        Finished = false;
    }
}
