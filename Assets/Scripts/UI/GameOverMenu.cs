using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverMenu : Menu
    {
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
