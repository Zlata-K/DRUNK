using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : Menu
    {
        [SerializeField] private GameObject pausePanel;

        private void Update()
        {
            UpdatePauseStatus();
        }

        private void UpdatePauseStatus()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1 - Time.timeScale;
                pausePanel.SetActive(!pausePanel.activeSelf);
            }
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

        public void Quit2MainMenu()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}
