﻿using UnityEngine;
using System.Collections;

public class IncrementScoreOnDestroy : MonoBehaviour {

    public int incrementAmount;
    public ScoreManager scoreManager;

    void Start ()
    {
        // Find score manager by tag, if not referenced already
        if (scoreManager == null)
        {
            this.scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        }
    }

    // Increment player score when destroyed
    void OnDestroy()
    {
        this.scoreManager.score += this.incrementAmount;
    }
}
