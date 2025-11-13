using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelFunctionsLibrary
{
    public class LevelFunctions : MonoBehaviour
    {
        public static void ResetCurrentLevel()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
        
        public static void LoadNewLevel(string scene) => SceneManager.LoadScene(scene);
        public static void LoadNewLevel(int scene) => SceneManager.LoadScene(scene);
        public static void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}