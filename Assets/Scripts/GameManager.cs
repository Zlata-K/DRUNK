using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void OnPlayerDeath()
    {
        ScoreManager.AddScore("Player", Indestructibles.PlayerData.CurrentScore);
        Invoke(nameof(LoadGameOverScene), 3.0f);
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverMenu");
    }
}