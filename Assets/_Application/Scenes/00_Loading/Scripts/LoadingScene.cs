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

            // Ad info
            public string IRONSOURCE_APP_KEY = "176a755f5";

            private void SdkInitializationCompletedEvent()
            {
                IronSource.Agent.displayBanner();
                SceneManager.LoadSceneAsync(NextSceneName);
            }

            private void Start()
            {
#if UNITY_EDITOR
                Observable.Timer(TimeSpan.FromSeconds(5))
                    .Subscribe(_ =>
                    {
                        SceneManager.LoadSceneAsync(NextSceneName);
                    })
                    .AddTo(this);
#elif UNITY_IOS
                IronSource.Agent.validateIntegration();
                IronSource.Agent.init(IRONSOURCE_APP_KEY);

/*
                IronSource.Agent.init(IRONSOURCE_APP_KEY, IronSourceAdUnits.REWARDED_VIDEO);
                IronSource.Agent.init(IRONSOURCE_APP_KEY, IronSourceAdUnits.INTERSTITIAL);
                IronSource.Agent.init(IRONSOURCE_APP_KEY, IronSourceAdUnits.BANNER);

                IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
                */
#endif
            }


            private void OnEnable()
            {
#if UNITY_EDITOR
#elif UNITY_IOS
                IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
#endif
            }
        }
    }
}
