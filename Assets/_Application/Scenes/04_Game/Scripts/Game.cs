using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YCLib.Utility;

using UniRx;
using UniRx.Triggers;

public class Game : MonoBehaviour
{
    enum GamePhase
    {
        INITIALIZE,
        INGAME,


    };

    GamePhase gamePhase = GamePhase.INITIALIZE;

    List<GameObject> listFloor = new List<GameObject>();
    List<GameObject> listBlock = new List<GameObject>();

    GameInfo gameInfo;

    private void Initialize()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        string[] packname = {
                    "01_OPAL",
                    "02_AMETHYST",
                    "03_GARNET",
                    "04_AQUAMARINE",
                    "05_QUARTZ"
                };

        StageData.Instance.Initialize();
        StageData.Instance.Load(packname[gameInfo.SelectLevel], gameInfo.SelectStage + 1);

        for (int y = 0; y < StageData.Instance.data.Height; y++)
        {
            for (int x = 0; x < StageData.Instance.data.Width; x++)
            {
                // Floor配置
                GameObject objFloor = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.FLOOR);

                objFloor.transform.SetParent(GameObject.Find("Game/Stage").transform);
                objFloor.transform.localPosition = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0.5f);
                objFloor.transform.localRotation = Quaternion.Euler(0, 0, 0);
                objFloor.SetActive(true);

                listFloor.Add(objFloor);

                // 固定ブロック配置
                switch (StageData.Instance.data.Grid[y, x])
                {
                    case Data.GRIDTYPE_NEEDBLOCK:
                    case Data.GRIDTYPE_NEEDLESSBLOCK:
                    case Data.GRIDTYPE_SWITCHBLOCK:

                        GameObject obj = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.BLOCK);

                        obj.transform.SetParent(GameObject.Find("Game/Stage").transform);
                        obj.transform.localPosition = new Vector3(x - (StageData.Instance.data.Width / 2.0f), y - (StageData.Instance.data.Height / 2.0f), 0);
                        obj.transform.localRotation = Quaternion.Euler(0,0,0);
                        obj.SetActive(true);

                        listBlock.Add(obj);
                        break;
                }
            }
        }

        // プレイヤー
        CurrentPlayerPos = new Position(StageData.Instance.data.AnswerRoute[0].x, StageData.Instance.data.AnswerRoute[0].y);

        objPlayer = ObjectPool.Rent((int)GameInfo.OBJECTPOOL.PLAYER);
        objPlayer.transform.SetParent(GameObject.Find("Game/Stage").transform);
        objPlayer.transform.localPosition = new Vector3(CurrentPlayerPos.x - (StageData.Instance.data.Width / 2.0f), CurrentPlayerPos.y - (StageData.Instance.data.Height / 2.0f), 0);
        objPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        objPlayer.SetActive(true);

        playerBlock = new Block(objPlayer, CurrentPlayerPos.x, CurrentPlayerPos.y);
        playerBlock.SetPlayer();

        gamePhase = GamePhase.INGAME;
    }

    Position CurrentPlayerPos;

    GameObject objPlayer;
    Block playerBlock;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // ゲーム中
        this.UpdateAsObservable()
            .Where(_ => gamePhase == GamePhase.INGAME)
            .Subscribe(_ =>
            {
            })
            .AddTo(this);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
