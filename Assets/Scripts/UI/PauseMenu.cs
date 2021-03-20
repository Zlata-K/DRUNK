using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : Menu
    {
        [SerializeField] private GameObject pausePanel;

        // Update is called once per frame
        private void Update()
        {
            PauseGame();
        }

        public void SetHighScoreText()
        {
            var highScores = ScoreManager.GetScores();
            var scoreTextBoxes = highScoreTextPanel.GetComponentsInChildren<TextMeshProUGUI>();
            
            SetHighScoreText(highScores, scoreTextBoxes);
        }
        
        private void PauseGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
            }
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

        public void QuitGame()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}
