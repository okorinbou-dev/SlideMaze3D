using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PanelStageNo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtStageNo;

    private int stageno = 1;

    public void SetStageNo(int no)
    {
        stageno = no;
        txtStageNo.text = stageno.ToString();
    }

    public int GetStageNo()
    {
        return stageno;
    }
}
