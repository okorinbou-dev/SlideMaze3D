using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Panel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLevel;

    [SerializeField] public int level;

    float x = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        txtLevel.text = "LEVEL " + (level + 1).ToString();        
    }
}
