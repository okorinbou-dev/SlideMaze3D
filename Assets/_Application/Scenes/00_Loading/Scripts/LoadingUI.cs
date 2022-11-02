using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
//    [SerializeField] private RectTransform _loadingImage;

    private void Update()
    {
        var angle = GetComponent<RectTransform>().rotation.eulerAngles;
//        var angle = _loadingImage.rotation.eulerAngles;
        angle.z -= 180f * Time.deltaTime;
        GetComponent<RectTransform>().rotation = Quaternion.Euler(angle);
//        _loadingImage.rotation = Quaternion.Euler(angle);
    }
}

