using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UniRx;
using UniRx.Triggers;

using YCLib.Utility;

public class StageSelect : MonoBehaviour
{
    [SerializeField] GameObject prefabPanel;

    List<GameObject> listPanel = new List<GameObject>();

    int center = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameInfo gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        for (int i = 0; i < gameInfo.StageNum; i++)
        {
            GameObject obj = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.STAGEPANEL);
            obj.name = "Stage" + (i + 1).ToString();
            obj.transform.SetParent(GameObject.Find("StageSelect/StageNo").transform);
            obj.transform.localPosition = new Vector3(i % 5, -(i / 5), 0) + new Vector3(-2.0f, 2.5f, 0);
            obj.GetComponent<PanelStageNo>().SetStageNo(i+1);
            listPanel.Add(obj);
        }

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                ObjectPool.ReturnAll((int)GameInfo.OBJECTPOOL.STAGEPANEL);
                SceneManager.LoadSceneAsync("Game");
            })
            .AddTo(this);
    }
}
