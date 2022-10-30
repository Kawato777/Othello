using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // マスの状態
    const int EMPTY = 0;
    const int WHITE = -1;
    const int BLACK = 1;
    const int WALL = 2;

    // ボードサイズ
    const int BOARD_SIZE = 8;

    // ボードの配列
    int[][] boardCondition = new int[BOARD_SIZE + 2][];
    bool[][] movablePos = new bool[BOARD_SIZE + 2][];
    int[][] movableDir = new int[BOARD_SIZE + 2][];

    // 方向(2進数)
    const int NONE = 0;
    const int LEFT = 1;
    const int UPPER_LEFT = 2;
    const int UPPER = 4;
    const int UPPER_RIGHT = 8;
    const int RIGHT = 16;
    const int LOWER_RIGHT = 32;
    const int LOWER = 64;
    const int LOWER_LEFT = 128;

    // セル情報の配列関係
    BoardCell[][] boardCells = new BoardCell[BOARD_SIZE][];
    [SerializeField] GameObject cellsObj;

    // 試合情報
    int currentColor;

    // Start is called before the first frame update
    void Start()
    {
        FirstSetting();
    }

    private void FirstSetting()
    {
        // 盤面の設定
        // 試合情報の設定
        currentColor = BLACK;

        // セル情報の取得
        BoardCell[] boardCells_getting = cellsObj.GetComponentsInChildren<BoardCell>();
        for (int m = 0; m < boardCells.Length; m++)
        {
            BoardCell[] copied = new BoardCell[BOARD_SIZE];
            Array.Copy(boardCells_getting, m * 8, copied, 0, BOARD_SIZE);
            boardCells[m] = copied;
        }

        // 壁の設定
        for (int m = 0;m < boardCondition.Length; m++)
        {
            boardCondition[m] = new int[BOARD_SIZE + 2];
            movablePos[m] = new bool[BOARD_SIZE + 2];    // movablePosの初期化
            Array.Fill(movablePos[m], false);
            movableDir[m] = new int[BOARD_SIZE + 2];    // movableDirの初期化
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
        // 初期配置
        boardCondition[4][4] = WHITE;
        boardCondition[5][5] = WHITE;
        boardCondition[4][5] = BLACK;
        boardCondition[5][4] = BLACK;
        StatusUpdate(); // 見た目を変更
        SetMovablePosAndDir();    // 黒の動ける場所を判定
        
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

    public void PutStone(RaycastHit hit)　// 石を置く
    {
        // test
        // Debug.Log(hit.collider.GetComponent<BoardCell>().cellPlace);
        if (hit.collider.CompareTag("Cell"))
        {
            BoardCell hitCell = hit.collider.GetComponent<BoardCell>();
            // 石を置けるかどうか
            if (CheckPuttingStone(hitCell))
            {
                // 実際に石を置く(ひっくり返せるやつも)
                FlipStone(hitCell);
            }
            else
            {
                // 石が置けない
                Debug.Log("そこには置けません。");
            }
        }
        else
        {
            // マスを選択していない
            Debug.Log("マスを選択してください。");
        }
    }

    private void FlipStone(BoardCell hitCell)
    {
        // 石を置く
        boardCondition[hitCell.cellPlace.x][hitCell.cellPlace.y] = currentColor;

        // 石を裏返す

        StatusUpdate();
        currentColor *= -1;
        SetMovablePosAndDir();    // 次の手番の動ける場所を確認
    }

    // 石を置けるかどうかを確かめる関数
    private bool CheckPuttingStone(BoardCell hitCell)
    {
        Debug.Log(movableDir[hitCell.cellPlace.x][hitCell.cellPlace.y]);
        // 選択したマスがEMPTYであるか
        if (hitCell.status != EMPTY)
        {
            return false;
        }

        // 選択したマスをひっくり返せるか
        return movablePos[hitCell.cellPlace.x][hitCell.cellPlace.y];
    }

    // それぞれのおけるマスの方向確認
    private void SetMovablePosAndDir()
    {
        for(int m = 1; m < BOARD_SIZE + 1; m++)
        {
            for(int n = 1; n < BOARD_SIZE + 1; n++)
            {
                // 方向の値を保存
                int dir = 0;
                if(boardCondition[m][n] != EMPTY)
                {
                    movableDir[m][n] = dir;
                    continue;
                }
                // 左
                if(boardCondition[m - 1][n] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m - 2;
                    int y_tmp = n;

                    // 相手の石が続いているだけループ
                    while(boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if(boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | LEFT;
                    }
                }

                // 左上
                if (boardCondition[m - 1][n - 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m - 2;
                    int y_tmp = n - 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                        y_tmp--;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | UPPER_LEFT;
                    }
                }

                // 上
                if (boardCondition[m][n - 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m;
                    int y_tmp = n - 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        y_tmp--;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir = dir | UPPER;
                    }
                }

                // 右上
                if (boardCondition[m + 1][n - 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m + 2;
                    int y_tmp = n - 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                        y_tmp--;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= UPPER_RIGHT;
                    }
                }

                // 右
                if (boardCondition[m + 1][n] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m + 2;
                    int y_tmp = n;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= RIGHT;
                    }
                }

                // 右下
                if (boardCondition[m + 1][n + 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m + 2;
                    int y_tmp = n + 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp++;
                        y_tmp++;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER_RIGHT;
                    }
                }

                // 下
                if (boardCondition[m][n + 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m;
                    int y_tmp = n + 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        y_tmp++;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER;
                    }
                }

                // 左下
                if (boardCondition[m - 1][n + 1] == -currentColor)   // その方向に相手の石があるとき
                {
                    int x_tmp = m - 2;
                    int y_tmp = n + 2;

                    // 相手の石が続いているだけループ
                    while (boardCondition[x_tmp][y_tmp] == -currentColor)
                    {
                        x_tmp--;
                        y_tmp++;
                    }

                    // 相手の石を挟んで自分の石があればdirを更新
                    if (boardCondition[x_tmp][y_tmp] == currentColor)
                    {
                        dir |= LOWER_LEFT;
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
