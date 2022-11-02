using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UniRx;

public class Top : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => SceneManager.LoadSceneAsync("StageSelect"))
            .AddTo(this);
    }
}