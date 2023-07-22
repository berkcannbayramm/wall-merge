using BBGameStudios.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelStates
{
    Game,
    Finish
}
public class LevelManager : MonoBehaviour
{
    [SerializeField] private Panel panel;
    [SerializeField] private bool isMultipleScene;
    [SerializeField] private List<String> levelsName;

    private LevelStates levelStates;

    private void Start()
    {
        if (!isMultipleScene) return;
        for(int i = 1; i <= SceneManager.sceneCountInBuildSettings; i++)
        {
            levelsName.Add("Level_"+i.ToString());
        }
    }

    private int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void ReloadScene()
    {
        if (GameManager.instance.CanClick) return;
        GameManager.instance.CanClick = true;
        panel.DisapperPanel();
        StartCoroutine(ChangeSceneWDelay());
        IEnumerator ChangeSceneWDelay()
        {
            yield return new WaitForSeconds(.5f);
            SceneManager.LoadScene(GetSceneIndex());
        }
        
    }
    public void NextLevel()
    {
        if (levelStates == LevelStates.Finish) return;

        levelStates = LevelStates.Finish;

        DataHandler.Level++;

        int currLevel = DataHandler.Level % levelsName.Count;

        ChangeScene(levelsName[currLevel]);
    }
    public void ChangeScene(string sceneName)
    {
        panel.DisapperPanel();
        StartCoroutine(ChangeSceneWDelay());
        IEnumerator ChangeSceneWDelay()
        {
            yield return new WaitForSeconds(.5f);
            SceneManager.LoadScene(sceneName);
            levelStates = LevelStates.Game;
        }
    }
}
