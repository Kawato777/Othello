using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PutStone(RaycastHit hit)�@// �΂�u��
    {
        // test
        Debug.Log(hit.collider.name);
        // �΂�u���邩�ǂ���
        // ���ۂɐ΂�u��
    }
}
