using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace YCLib
{
    namespace Utility
    {
        public class LoadingScene : MonoBehaviour
        {
            [SerializeField] string NextSceneName = "";

            private void Start()
            {
                LoadAsync();
            }

            private async void LoadAsync()
            {
                await Task.Delay(5000);
                SceneManager.LoadSceneAsync(NextSceneName);
            }
        }
    }
}
