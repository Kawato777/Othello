using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // �}�X�̏��
    const int EMPTY = 0;
    const int WHITE = -1;
    const int BLACK = 1;
    const int WALL = 2;

    // �{�[�h�T�C�Y
    const int BOARD_SIZE = 8;

    // �{�[�h�̔z��
    int[][] boardCondition = new int[BOARD_SIZE + 2][];

    // �Z�����̔z��֌W
    BoardCell[][] boardCells = new BoardCell[BOARD_SIZE][];
    [SerializeField] GameObject cellsObj;

    // Start is called before the first frame update
    void Start()
    {
        FirstSetting();
    }

    private void FirstSetting()
    {
        // �Ֆʂ̐ݒ�
        // �ǂ̐ݒ�
        for(int m = 0;m < boardCondition.Length; m++)
        {
            boardCondition[m] = new int[BOARD_SIZE + 2];
        }
        Array.Fill(boardCondition[0], WALL);
        Array.Fill(boardCondition[BOARD_SIZE + 1], WALL);
        for (int m = 0; m < boardCondition.Length; m++)
        {
            for(int n = 0;n < boardCondition[m].Length; n++)
            {
                if(n == 0 || n == BOARD_SIZE + 1){
                    boardCondition[m][n] = WALL;
                }
            }
        }
        // �����z�u
        boardCondition[4][4] = WHITE;
        boardCondition[5][5] = WHITE;
        boardCondition[4][5] = BLACK;
        boardCondition[5][4] = BLACK;

        // �Z�����̎擾
        
    }

    public void PutStone(RaycastHit hit)�@// �΂�u��
    {
        // test
        // Debug.Log(hit.collider.GetComponent<BoardCell>().cellPlace);
        // �΂�u���邩�ǂ���
        // ���ۂɐ΂�u��
    }
}
