using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCGameManager : MonoBehaviour
{
    private int[,] chess_state = new int[3, 3];  // 棋盘落子状态，0: empty, 1: playerA, 2: playerB
    private int turn = 1;  // 回合，1: playerA, 2: playerB
    private bool start_state = false;  // 用于标定游戏是否开始
    private int game_state = 0;  // 游戏状态0: 棋盘未满，1: playerA, 2: playerB, 3: 平局

    // Start is called before the first frame update
    void Start()
    {
        start_state = false;
        newGame();
    }

    // Update is called once per frame
    void Update()
    {

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

    // 判断是否获胜
    private int getGameState()
    {
        for (int i = 0; i < 3; i++)
        {
            // 判断竖向是否有相同的棋子
            if (chess_state[i, 0] != 0 && chess_state[i, 0] == chess_state[i, 1] && chess_state[i, 0] == chess_state[i, 2])
                return chess_state[i, 0];
            // 判断横向是否有相同的棋子
            if (chess_state[0, i] != 0 && chess_state[0, i] == chess_state[1, i] && chess_state[0, i] == chess_state[2, i])
                return chess_state[0, i];
        }

        // 判断对角线是否有相同的棋子
        if (chess_state[0, 0] != 0 && chess_state[0, 0] == chess_state[1, 1] && chess_state[0, 0] == chess_state[2, 2]) return chess_state[1, 1];
        if (chess_state[0, 2] != 0 && chess_state[0, 2] == chess_state[1, 1] && chess_state[0, 2] == chess_state[2, 0]) return chess_state[1, 1];

        // 判断棋盘是否已满，如果没有，则返回0
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (chess_state[i, j] == 0)
                    return 0;
        return 3;
    }

    // OnGUI每帧都会被调用，用于绘制UI
    void OnGUI()
    {
        GUI.skin.button.fontSize = 20;
        GUI.skin.button.fontStyle = FontStyle.Bold;
        GUI.skin.button.normal.background = null;
        GUI.skin.button.hover.background = null;
        GUI.skin.button.active.background = null;
        GUI.skin.label.fontSize = 20;
        GUI.skin.label.normal.textColor = Color.red;


        if (!start_state)
        {
            // 开始游戏按钮
            if (GUI.Button(new Rect(Screen.width * 1 / 5, Screen.height * 2 / 5, 200, 50), "新的对局"))
                start_state = true;
            // 游戏标题
            GUI.skin.label.fontSize = 32;
            GUI.Label(new Rect(Screen.width * 1 / 5, Screen.height * 1 / 5, 200, 100), "井字棋1.0");
            GUI.Label(new Rect(Screen.width * 3 / 5, Screen.height * 3 / 10, 200, 100), "轻触按钮\n开始游戏");
        }
        else
        {
            // 重新开始按钮
            if (GUI.Button(new Rect(Screen.width * 1 / 5, Screen.height * 2 / 5, 200, 50), "重新开始"))
                newGame();
            // 根据当前回合情况决定，game_state为0棋盘未满，1玩家A获胜，2玩家B获胜，3棋盘已满，平局
            switch (game_state)
            {
                case 1:
                    GUI.Label(new Rect(Screen.width * 1 / 5, Screen.height * 1 / 5, 200, 50), "恭喜玩家A获胜！");
                    break;
                case 2:
                    GUI.Label(new Rect(Screen.width * 1 / 5, Screen.height * 1 / 5, 200, 50), "恭喜玩家B获胜！");
                    break;
                case 3:
                    GUI.Label(new Rect(Screen.width * 1 / 5, Screen.height * 1 / 5, 200, 50), "无人获胜，平局");
                    break;
                default:
                    GUI.Label(new Rect(Screen.width * 1 / 5, Screen.height * 1 / 5, 200, 50), "当前回合：" + (turn == 1 ? "玩家A" : "玩家B"));
                    break;
            }
            // 绘制棋盘
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GUI.Button(new Rect(Screen.width * 1 / 2 + 60 * i, Screen.height * 1 / 5 + 60 * j, 60, 60), chess_state[i, j] == 0 ? "" : chess_state[i, j] == 1 ? "A" : "B"))
                    {
                        // 如果被按下，且当前回合未结束，且当前位置未落子，则落子
                        if (chess_state[i, j] == 0 && game_state == 0)
                        {
                            chess_state[i, j] = turn;
                            game_state = getGameState();
                            turn = turn == 1 ? 2 : 1;
                        }
                    }
                }
            }
        }
    }
}