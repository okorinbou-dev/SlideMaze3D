using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YCLib.Utility;

public class GameInfo : MonoBehaviour
{
    public int SelectLevel { get; set; } = 1;
    public int SelectStage { get; set; } = 1;

    public int StageNum { get; private set; }

    public enum OBJECTPOOL
    {
        STAGEPANEL,
        BLOCK,
        PLAYER,
        GOAL,

        NUM
    };

    // Start is called before the first frame update
    void Start()
    {
        ObjectPool.Initialize((int)OBJECTPOOL.NUM);

        ObjectPool.SetPrefab((int)OBJECTPOOL.STAGEPANEL, (GameObject)Resources.Load("Prefabs/3D/PanelStageNo"));
        ObjectPool.SetPrefab((int)OBJECTPOOL.BLOCK, (GameObject)Resources.Load("Prefabs/3D/Block"));
        ObjectPool.SetPrefab((int)OBJECTPOOL.PLAYER, (GameObject)Resources.Load("Prefabs/3D/Player"));
        ObjectPool.SetPrefab((int)OBJECTPOOL.GOAL, (GameObject)Resources.Load("Prefabs/3D/Goal"));

        DontDestroyOnLoad(this);  
    }

    private void Update()
    {
        SetStageNum();
    }

    void SetStageNum()
    {
        int[] stagenum = 
        {
            60,
            60,
            60,
            60,
            60,
        };

        StageNum = stagenum[SelectLevel]; 
    }
}
