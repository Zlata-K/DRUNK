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
        
        private int _score;
        private bool _scoreSubmitted;
        
        public void Awake()
        {
            _scoreSubmitted = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Indestructibles.PlayerData != null)
            {
                _score = Indestructibles.PlayerData.CurrentScore;
            }
            
            scoreText.text = "Score: " + _score;
        }
        public void PlayAgain()
        {
            if (!_scoreSubmitted)
            {
                ScoreManager.AddScore("Player", _score);
            }
            SceneManager.LoadScene(1);
        }
        
        public void Quit2MainMenu()
        {
            if (!_scoreSubmitted)
            {
                ScoreManager.AddScore("Player", _score);
            }
            SceneManager.LoadScene(0);
        }

        public void OnScoreSubmit()
        {
            _scoreSubmitted = true;
            scoreInput.SetActive(false);
            submitSuccessText.SetActive(true);
            if (inputText.text != "")
            {
                ScoreManager.AddScore(inputText.text, _score);
            }
            else
            {
                ScoreManager.AddScore("Player", _score);
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
