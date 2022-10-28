using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PutStone(RaycastHit hit)　// 石を置く
    {
        // test
        Debug.Log(hit.collider.name);
        // 石を置けるかどうか
        // 実際に石を置く
    }
}
