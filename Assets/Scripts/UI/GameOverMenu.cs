using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverMenu : Menu
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject scoreInput;
        [SerializeField] private TMP_Text inputText;
        [SerializeField] private GameObject submitSuccessText;

        private bool _scoreSubmitted;
        
        public void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            scoreText.text = "Score: " + Indestructibles.PlayerData.CurrentScore;
        }
        public void PlayAgain()
        {
            ScoreManager.AddScore("Player", Indestructibles.PlayerData.CurrentScore);
            SceneManager.LoadScene(1);
        }
        
        public void Quit2MainMenu()
        {
            ScoreManager.AddScore("Player", Indestructibles.PlayerData.CurrentScore);
            SceneManager.LoadScene(0);
        }

        public void OnScoreSubmit()
        {
            _scoreSubmitted = true;
            scoreInput.SetActive(false);
            submitSuccessText.SetActive(true);
            if (inputText.text != "")
            {
                ScoreManager.AddScore(inputText.text, Indestructibles.PlayerData.CurrentScore);
            }
            else
            {
                ScoreManager.AddScore("Player", Indestructibles.PlayerData.CurrentScore);
            }
          
        }

        private void OnEnable()
        {
            submitSuccessText.SetActive(false);
            if (_scoreSubmitted)
            {
                scoreInput.SetActive(false);
            }
            else
            {
                scoreInput.SetActive(true);
            }
        }
    }
}
