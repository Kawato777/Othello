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
    bool[][] movablePos = new bool[BOARD_SIZE + 2][];
    int[][] movableDir = new int[BOARD_SIZE + 2][];

    // ����(2�i��)
    const int NONE = 0;
    const int LEFT = 1;
    const int UPPER_LEFT = 2;
    const int UPPER = 4;
    const int UPPER_RIGHT = 8;
    const int RIGHT = 16;
    const int LOWER_RIGHT = 32;
    const int LOWER = 64;
    const int LOWER_LEFT = 128;

    // �Z�����̔z��֌W
    BoardCell[][] boardCells = new BoardCell[BOARD_SIZE][];
    [SerializeField] GameObject cellsObj;

    // �������
    int currentColor;

    // Start is called before the first frame update
    void Start()
    {
        FirstSetting();
    }

    private void FirstSetting()
    {
        // �Ֆʂ̐ݒ�
        // �������̐ݒ�
        currentColor = BLACK;

        // �Z�����̎擾
        BoardCell[] boardCells_getting = cellsObj.GetComponentsInChildren<BoardCell>();
        for (int m = 0; m < boardCells.Length; m++)
        {
            BoardCell[] copied = new BoardCell[BOARD_SIZE];
            Array.Copy(boardCells_getting, m * 8, copied, 0, BOARD_SIZE);
            boardCells[m] = copied;
        }

        // �ǂ̐ݒ�
        for (int m = 0;m < boardCondition.Length; m++)
        {
            boardCondition[m] = new int[BOARD_SIZE + 2];
            movablePos[m] = new bool[BOARD_SIZE + 2];    // movablePos�̏�����
            Array.Fill(movablePos[m], false);
            movableDir[m] = new int[BOARD_SIZE + 2];    // movableDir�̏�����
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
        StatusUpdate(); // �����ڂ�ύX
        SetMovablePosAndDir();    // ���̓�����ꏊ�𔻒�
    }

    private void StatusUpdate()
    {
        for (int m = 1; m < boardCondition.Length; m++)
        {
            if(!(m == BOARD_SIZE + 1))
            {
                for (int n = 1; n < boardCondition[m].Length; n++)
                {
                    if(!(n == BOARD_SIZE + 1))
                    {
                        boardCells[m - 1][n - 1].ViewUpdate(boardCondition[m][n]);
                    }
                }
            }
        }
    }

    public void PutStone(RaycastHit hit)�@// �΂�u��
    {
        // test
        // Debug.Log(hit.collider.GetComponent<BoardCell>().cellPlace);
        if (hit.collider.CompareTag("Cell"))
        {
            BoardCell hitCell = hit.collider.GetComponent<BoardCell>();
            // �΂�u���邩�ǂ���
            if (CheckPuttingStone(hitCell))
            {
                // ���ۂɐ΂�u��(�Ђ�����Ԃ�����)
                if (movableDir[hitCell.cellPlace.x][hitCell.cellPlace.y] != 0)
                {
                    FlipStone(hitCell);
                }
                else
                {
                    Debug.Log("�v���Ă��铮���ł͂Ȃ�");
                }
            }
            else
            {
                // �΂��u���Ȃ�
                Debug.Log("�����ɂ͒u���܂���B");
            }
        }
        else
        {
            // �}�X��I�����Ă��Ȃ�
            Debug.Log("�}�X��I�����Ă��������B");
        }
    }

    private void FlipStone(BoardCell hitCell)
    {
        // �΂�u��
        boardCondition[hitCell.cellPlace.x][hitCell.cellPlace.y] = currentColor;
        // �΂𗠕Ԃ�
        // �u�����΂�movableDir���Z�b�g
        int dir = movableDir[hitCell.cellPlace.x][hitCell.cellPlace.y];
        if(dir == 0)
        {
            Debug.Log(movablePos[hitCell.cellPlace.x][hitCell.cellPlace.y]);
            Debug.Log("��������");
            return;
        }

        int x = hitCell.cellPlace.x;
        int y = hitCell.cellPlace.y;

        // ��
        if((dir & LEFT) == LEFT)
        {
            int x_tmp = x + 1;
            while(boardCondition[x_tmp][y] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y] = currentColor;
                x_tmp++;
            }
        }

        // ����
        if ((dir & UPPER_LEFT) == UPPER_LEFT)
        {
            int x_tmp = x + 1;
            int y_tmp = y - 1;

            while (boardCondition[x_tmp][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y_tmp] = currentColor;
                x_tmp++;
                y_tmp--;
            }
        }

        // ��
        if ((dir & UPPER) == UPPER)
        {
            int y_tmp = y - 1;

            while (boardCondition[x][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x][y_tmp] = currentColor;
                y_tmp--;
            }
        }

        // �E��
        if ((dir & UPPER_RIGHT) == UPPER_RIGHT)
        {
            int x_tmp = x - 1;
            int y_tmp = y - 1;

            while (boardCondition[x_tmp][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y_tmp] = currentColor;
                x_tmp--;
                y_tmp--;
            }
        }

        // �E
        if ((dir & RIGHT) == RIGHT)
        {
            int x_tmp = x - 1;
            while (boardCondition[x_tmp][y] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y] = currentColor;
                x_tmp--;
            }
        }

        // �E��
        if ((dir & LOWER_RIGHT) == LOWER_RIGHT)
        {
            int x_tmp = x - 1;
            int y_tmp = y + 1;

            while (boardCondition[x_tmp][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y_tmp] = currentColor;
                x_tmp--;
                y_tmp++;
            }
        }

        // ��
        if ((dir & LOWER) == LOWER)
        {
            int y_tmp = y + 1;

            while (boardCondition[x][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x][y_tmp] = currentColor;
                y_tmp++;
            }
        }

        // ����
        if ((dir & LOWER_LEFT) == LOWER_LEFT)
        {
            int x_tmp = x + 1;
            int y_tmp = y + 1;

            while (boardCondition[x_tmp][y_tmp] == -currentColor)    // �Ђ�����Ԃ�������
            {
                boardCondition[x_tmp][y_tmp] = currentColor;
                x_tmp++;
                y_tmp++;
            }
        }

        StatusUpdate();
        currentColor *= -1;
        SetMovablePosAndDir();    // ���̎�Ԃ̓�����ꏊ���m�F
    }

    // �΂�u���邩�ǂ������m���߂�֐�
    private bool CheckPuttingStone(BoardCell hitCell)
    {
        // �I�������}�X��EMPTY�ł��邩
        if (hitCell.status != EMPTY)
        {
            return false;
        }

        // �I�������}�X���Ђ�����Ԃ��邩
        return movablePos[hitCell.cellPlace.x][hitCell.cellPlace.y];
    }

    // ���ꂼ��̂�����}�X�̕����m�F
    private void SetMovablePosAndDir()
    {
        for(int m = 1; m < BOARD_SIZE + 1; m++)
        {
            for(int n = 1; n < BOARD_SIZE + 1; n++)
            {
                // �����̒l��ۑ�
                int dir = 0;
                if(boardCondition[m][n] != EMPTY)
                {
                    movableDir[m][n] = dir;
                    continue;
                }
                // �E
                if(boardCondition[m - 1][n] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m - 2;
                    int y_tmp = n;

                    // ����̐΂������Ă��邾�����[�v
                    while(boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if(boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | RIGHT;
                    }
                }

                // �E��
                if (boardCondition[m - 1][n - 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m - 2;
                    int y_tmp = n - 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                        y_tmp--;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | UPPER_RIGHT;
                    }
                }

                // ��
                if (boardCondition[m][n - 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m;
                    int y_tmp = n - 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        y_tmp--;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | UPPER;
                    }
                }

                // ����
                if (boardCondition[m + 1][n - 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m + 2;
                    int y_tmp = n - 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                        y_tmp--;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= UPPER_LEFT;
                    }
                }

                // ��
                if (boardCondition[m + 1][n] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m + 2;
                    int y_tmp = n;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LEFT;
                    }
                }

                // ����
                if (boardCondition[m + 1][n + 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m + 2;
                    int y_tmp = n + 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                        y_tmp++;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER_LEFT;
                    }
                }

                // ��
                if (boardCondition[m][n + 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m;
                    int y_tmp = n + 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        y_tmp++;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER;
                    }
                }

                // �E��
                if (boardCondition[m - 1][n + 1] == -currentColor)   // ���̕����ɑ���̐΂�����Ƃ�
                {
                    int x_tmp = m - 2;
                    int y_tmp = n + 2;

                    // ����̐΂������Ă��邾�����[�v
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                        y_tmp++;
                    }

                    // ����̐΂�����Ŏ����̐΂������dir���X�V
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER_RIGHT;
                    }
                }

                movableDir[m][n] = dir;
                if(dir != 0)
                {
                    movablePos[m][n] = true;
                }
            }
        }
    }


}
