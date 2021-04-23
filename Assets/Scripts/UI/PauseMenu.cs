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

                if (!pausePanel.activeSelf && GameObject.Find("DrinkChoiceCanvas").GetComponent<DrinkChoice>().wasActive)
                {
                    GameObject.Find("DrinkChoiceCanvas").transform.Find("Menu").gameObject.SetActive(true);
                }
            }
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            if (GameObject.Find("DrinkChoiceCanvas").GetComponent<DrinkChoice>().wasActive)
            {
                GameObject.Find("DrinkChoiceCanvas").transform.Find("Menu").gameObject.SetActive(true);
            }
        }

        public void Quit2MainMenu()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}
