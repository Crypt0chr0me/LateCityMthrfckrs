﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelselectScript : MonoBehaviour {


    public string selectedLevel;
    public GameObject LevelPreview;

    //Added changes to Title for level preview.
    public Text Title;

    public void SelectLevel(GameObject thisLevel)
    {
        selectedLevel = thisLevel.name;
        LevelPreview.transform.Find(selectedLevel).gameObject.SetActive(true);
        Title.text = selectedLevel;
        //load respective leaderboard
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene("Level_" + selectedLevel);
    }

    //Brady Jacobson
    //Used to get rid of the currently used preview.
    public void DeactivatePreview()
    {
        LevelPreview.transform.Find(selectedLevel).gameObject.SetActive(false);
    }

    public void HighlightButton(UnityEngine.UI.Button button)
    {
        button.Select();
    }
}
