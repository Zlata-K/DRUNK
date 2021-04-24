using System;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool debugEnabled = true;
    public void OnPlayerDeath()
    {
        ScoreManager.AddScore("Player", Indestructibles.PlayerData.CurrentScore);
        Invoke(nameof(LoadGameOverScene), 3.0f);
    }

    private void Update()
    {
        Indestructibles.DebugEnabled = debugEnabled;
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverMenu");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            Indestructibles.SetDefaultValues();
        }
    }
    
    /*private void OnDrawGizmos()
    {
        var graph = NavigationGraph.graph;

        foreach (var cluster in graph) {
            foreach (var node in cluster.Value) {

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.position, 0.5F);
                foreach (var link in node.links) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(node.position, link.node.position);
                }
            }
        }
    }*/
}