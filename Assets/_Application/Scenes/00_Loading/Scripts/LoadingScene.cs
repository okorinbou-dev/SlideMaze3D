using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

using UniRx;

namespace YCLib
{
    namespace Utility
    {
        public class LoadingScene : MonoBehaviour
        {
            [SerializeField] string NextSceneName = "";

            private void Start()
            {
                Observable.Timer(TimeSpan.FromSeconds(5))
                    .Subscribe(_ =>
                    {
                        SceneManager.LoadSceneAsync(NextSceneName);
                    })
                    .AddTo(this);
            }
        }
    }
}
