using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PutStone(RaycastHit hit)�@// �΂�u��
    {
        // test
        Debug.Log(hit.collider.GetComponent<BoardCell>().cellPlace);
        // �΂�u���邩�ǂ���
        // ���ۂɐ΂�u��
    }
}
