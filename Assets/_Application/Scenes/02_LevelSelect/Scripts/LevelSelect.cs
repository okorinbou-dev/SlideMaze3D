using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UniRx;

public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameInfo gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                RaycastHit _hit;

                var _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit))
                {
                    gameInfo.SelectLevel = _hit.collider.gameObject.GetComponent<Panel>().level;
                    SceneManager.LoadSceneAsync("StageSelect");
                }
            })
            .AddTo(this);
    }
}
