using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScript : Menu
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        public void SetHighScoreText()
        {
            var highScores = ScoreManager.GetScores();
            var scoreTextBoxes = highScoreTextPanel.GetComponentsInChildren<TextMeshProUGUI>();
            
            SetHighScoreText(highScores, scoreTextBoxes);
        }
    }
}