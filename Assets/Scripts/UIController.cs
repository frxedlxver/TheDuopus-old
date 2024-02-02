/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public UIDocument uiDocument;
    private void Start()
    {
        Getting_Head gettingHeadScript = GetComponent<Getting_Head>();

        // Subscribe to the custom event
        gettingHeadScript.gameOverEvent.AddListener(GameOver);
    }

    private void GameOver()
    {
        //pause game
        Time.timeScale = 0f;
        
        //make the leaderboard show up
        if (uiDocument != null)
        {
            VisualElement uiTree = uiDocument.rootVisualElement;
            uiTree.visible = true;
        }
    }
}*/