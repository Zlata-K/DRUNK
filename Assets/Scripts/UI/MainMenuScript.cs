using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuScript : Menu
    {
        public void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void PlayGame()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }

    }
}