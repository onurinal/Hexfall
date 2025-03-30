using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexfall.Manager
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadSameScene()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}