using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverMenu : Menu
    {
        [SerializeField] private TMP_Text scoreText;
        public void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            scoreText.text = "Score: " + Indestructibles.PlayerData.CurrentScore;
        }
        public void PlayAgain()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Quit2MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
