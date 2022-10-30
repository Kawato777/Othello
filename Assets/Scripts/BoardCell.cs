using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour
{
    public Vector2 cellPlace;
    public int status = 0;

    // マスの状態
    const int EMPTY = 0;
    const int WHITE = -1;
    const int BLACK = 1;
    const int WALL = 2;

    // 石オブジェクト
    [SerializeField] GameObject stoneObjPrefab;
    GameObject stoneObj;

    // Start is called before the first frame update
    void Awake()
    {
        stoneObj = (GameObject)Instantiate(stoneObjPrefab,this.transform);
        stoneObj.transform.localPosition = new Vector3(0, 0.6f, 0);
        stoneObj.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        stoneObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 見た目の変更
    public void ViewUpdate(int sentStatus)
    {
        if (!(sentStatus == EMPTY || sentStatus == BLACK || sentStatus == WHITE))
        {
            if(sentStatus == WALL)
            {
                Debug.Log("壁のステータスが送られた");
            }
            else
            {
                Debug.LogError("無効なステータス");
            }
            return;
        }

        status = sentStatus;
        if(status == EMPTY)
        {
            stoneObj.SetActive(false);
        }
        else
        {
            stoneObj.SetActive(true);
            if(status == WHITE)
            {
                stoneObj.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                stoneObj.transform.localEulerAngles = new Vector3(180f, 0, 0);
            }
        }
    }
}
