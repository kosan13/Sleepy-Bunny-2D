using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ResetLevel : MonoBehaviour
    {
        public static void ResetCurrentLevel()
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}