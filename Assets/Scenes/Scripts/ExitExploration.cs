using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ExitExploration : MonoBehaviour
{
    bool hasEnded;

    void Start()
    {
        hasEnded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hasEnded = true;
            StartCoroutine(Quit.WaitQuit(6));
        }
    }

    public bool exploreHasEnded()
    {
        return hasEnded;
    }
}
