using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScript : MonoBehaviour
    {
        // serializing this as it is the nicest way to fetch a deactivated game object
        [SerializeField] private GameObject highScoreTextPanel;
        
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
            var index = 1;

            ClearHighScores(scoreTextBoxes);
            
            //since the list of scores is sorted from smallest to biggest, we have to pull a good old switcheroo
            for (var i = highScores.Count - 1; i >= 0; i--)
            {
                scoreTextBoxes[(int) HighScoreColumns.Positions].GetComponent<TextMeshProUGUI>()
                    .text += index++ + "\n";

                var format = new NumberFormatInfo {NumberGroupSeparator = " "};
                
                scoreTextBoxes[(int) HighScoreColumns.HighScores].GetComponent<TextMeshProUGUI>()
                    .text += highScores[i].score.ToString("N0", format) + "\n";

                scoreTextBoxes[(int) HighScoreColumns.PlayerNames].GetComponent<TextMeshProUGUI>()
                    .text += highScores[i].name + "\n";

                scoreTextBoxes[(int) HighScoreColumns.Dates].GetComponent<TextMeshProUGUI>()
                    .text += highScores[i].date.ToString("d") + "\n";
            }
        }

        private static void ClearHighScores(IEnumerable<TextMeshProUGUI> columns)
        {
            foreach (var column in columns)
            {
                column.text = "";
            }
        }

        private enum HighScoreColumns
        {
            Positions = 0,
            HighScores = 1,
            PlayerNames = 2,
            Dates = 3
        }
    }
}