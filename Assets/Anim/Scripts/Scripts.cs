using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAnimInEditor : MonoBehaviour
{

    public UnityAction myAction;
    public UnityEvent myEvent;
    private int[,] chess_state = new int[3, 3];  // 棋盘落子状态，0: empty, 1: playerA, 2: playerB
    private int game_state = 0;  // 游戏状态0: 棋盘未满，1: playerA, 2: playerB, 3: 平局

    private int turn = 1;  // 回合，1: playerA, 2: playerB
    private bool start_state = false;  // 用于标定游戏是否开始
    

    private void Awake()
    {
        myAction = new UnityAction(test02);
        myEvent.AddListener(myAction);
        myEvent.AddListener(getGameState);
        start_state = false;
    }

    // 开始新的对局
    private void newGame()
    {
        game_state = 0;
        turn = 1;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                chess_state[i, j] = 0;
    }
    public void test02()
    {
        Debug.Log("test for 02");
    }

    public void ClickBtn()
    {
        Debug.Log("test for ClickBtnTest");
        myEvent.Invoke();

    }

    private void getGameState()
    {
        for (int i = 0; i < 3; i++)
        {
            // 判断竖向是否有相同的棋子
            if (chess_state[i, 0] != 0 && chess_state[i, 0] == chess_state[i, 1] && chess_state[i, 0] == chess_state[i, 2])
                //return chess_state[i, 0];
                gameWin();

            // 判断横向是否有相同的棋子
            if (chess_state[0, i] != 0 && chess_state[0, i] == chess_state[1, i] && chess_state[0, i] == chess_state[2, i])
                //return chess_state[0, i];
                gameWin();
        }

        // 判断对角线是否有相同的棋子
        if (chess_state[0, 0] != 0 && chess_state[0, 0] == chess_state[1, 1] && chess_state[0, 0] == chess_state[2, 2])
            //return chess_state[1, 1];
            gameWin();
        if (chess_state[0, 2] != 0 && chess_state[0, 2] == chess_state[1, 1] && chess_state[0, 2] == chess_state[2, 0])
            //return chess_state[1, 1];
            gameWin();

        // 判断棋盘是否已满，如果没有，则返回0
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (chess_state[i, j] == 0)
                    gameWin();
                    //return 0;
        //return 3;
        gameLose();
    }
    private void gameWin()
    {

    }
    private void gameLose()
    {

    }

}

