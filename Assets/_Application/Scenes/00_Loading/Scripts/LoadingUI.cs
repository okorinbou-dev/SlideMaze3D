using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class LoadingUI : MonoBehaviour
{
    void Start()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                var angle = GetComponent<RectTransform>().rotation.eulerAngles;
                angle.z -= 180f * Time.deltaTime;
                GetComponent<RectTransform>().rotation = Quaternion.Euler(angle);
            })
            .AddTo(this);
    }
}

