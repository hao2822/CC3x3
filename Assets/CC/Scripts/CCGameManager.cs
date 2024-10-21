using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CCGameManager : MonoBehaviour
{
    private int[,] chess_state = new int[3, 3];  // 棋盘落子状态，0: empty, 1: playerA, 2: playerB
    private int turn = 0;  // 回合
    private bool start_state = false;  // 用于标定游戏是否开始

    private bool isSingle;// 单机对战
    private int initTurn;  // 先后手
    private int game_state = 0;  // 游戏状态0: 棋盘未满，1: playerA, 2: playerB, 3: 平局

    public Texture2D O;
    public Texture2D X;

    //public Texture2D GameBG;
    public Texture2D ChessBG;

    public GameObject PrefabParent;
    public GameObject WinPrefab;
    public GameObject LosPrefab;
    public bool EasyLevel;
    private bool ifWin;


    // Start is called before the first frame update
    void Start()
    {
        start_state = false;
        Init();
    }

    // 新对局开始
    private void Init()
    {
        EasyLevel = true;
        game_state = 0;
        turn = initTurn;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                chess_state[i, j] = 0;
    }

    // 判断是否获胜
    private int checkState()
    {

        for (int i = 0; i < 3; i++) {
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

    private void SettlementPlay(bool ifWin)
    {
        if (ifWin && !WinPrefab) {
            Debug.Log("Instantiate");
            WinPrefab.SetActive(true);
        }
        else if (!ifWin && !LosPrefab) {
            Debug.Log("Instantiate");
            LosPrefab.SetActive(true);
        }


    }

    void OnGUI()
    {
        //小字体初始化
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.normal.background = null;
        style.fontSize = 35;

        //大字体初始化
        GUIStyle bigStyle = new GUIStyle();
        bigStyle.normal.textColor = Color.white;
        bigStyle.normal.background = null;
        bigStyle.fontSize = 80;

        //Pos
        //重置按钮位置
        Rect btnPos = new Rect(1300, 60, 200, 50);
        Rect titlePos = new Rect(740, 50, 0, 0);
        Rect postPos = new Rect(300, 50, 200, 50);
        Rect WLpostPos = new Rect(400, 540, 200, 50);
        Rect p1Pos = new Rect(800, 860, 150, 50);
        Rect p1Pos_ = new Rect(800, 920, 150, 50);
        Rect p2Pos = new Rect(1020, 860, 150, 50);
        Rect p2Pos_ = new Rect(1020, 920, 150, 50);


        //加载游戏状态
        int state = checkState();
        switch (state) {
            case 0:
                GUI.Label(postPos, " Player " + (turn + 1) + " Round", style);
                break;
            case 1:
                GUI.Label(WLpostPos, "[ Player1  WIN!! ]", style);

                SettlementPlay(true);
                state = 0;

                break;
            case 2:
                GUI.Label(WLpostPos, "[ Player2  WIN!! ]", style);

                SettlementPlay(true);
                state = 0;
                break;
            case 3:
                GUI.Label(WLpostPos, "[ Draw Game... ]", style);

                SettlementPlay(false);
                state = 0;

                break;
            case 4:
                //isPlaying = true;
                break;

        }

        //加载标题
        GUI.Label(titlePos, "Tic-Tac-Toe", bigStyle);

        //加载重置按钮
        if (GUI.Button(btnPos, "Reset"))
            Init();

        //加载玩家1选择先手与后手的选项及实现功能
        if (GUI.Button(p1Pos, "Player 1: Play First")) {
            initTurn = 0;
            Init();
        }
        if (GUI.Button(p1Pos_, "Player 1: Play Next")) {
            initTurn = 1;
            Init();
        }

        //加载选择与人或与AI对战模式的选项及实现其功能
        if (GUI.Button(p2Pos, "Player 2: Player")) {

            isSingle = true;
            Init();
        }
        if (GUI.Button(p2Pos_, "Player 2:  AI")) {
            isSingle = false;
            Init();
        }

        //游戏运行逻辑
        //mode1: 人vs人
        //mode2: 人vsAI
        if (isSingle)
            PvP();
        else
            PvE();
    }
    public void ChangeGmaeLevel() {
        EasyLevel = !EasyLevel;

    }

    float2 chessPos = new float2(650, 200);
    float length = 200;
    //玩家对战游戏逻辑
    void PvP()
    {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                switch (chess_state[i, j]) {
                    case 0:
                        //玩家已落子，更新棋盘
                        if (GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), ChessBG) && checkState() == 0) {
                            chess_state[i, j] = turn + 1;
                            turn = 1 - turn;
                        }
                        break;
                    case 1:
                        GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), O);
                        break;
                    case 2:
                        GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), X);
                        break;
                }
            }
        }
    }

    //人机对战游戏逻辑
    void PvE()
    {
        //遍历所有棋格，在玩家回合判断玩家落子位置，在AI回合唤醒AI进行落子
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                switch (chess_state[i, j]) {
                    case 0:
                        if (turn == 0) {
                            //玩家回合: 
                            if (GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), ChessBG) && checkState() == 0) {
                                chess_state[i, j] = 1;
                                turn = 1;
                            }
                        }
                        else {
                            //AI回合
                            AITurn(EasyLevel);
                            turn = 0;
                        }
                        break;
                    case 1:
                        GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), O);
                        break;
                    case 2:
                        GUI.Button(new Rect(chessPos.x + i * length, chessPos.y + j * length, length, length), X);
                        break;
                }
            }
        }
    }

    //AI回合，AI视棋盘情况进行落子
    void AITurn(bool gameLevel)
    {
        //游戏不在进行中，不进行落子
        if (checkState() != 0)
            return;

        /*
         * (tarLoseX, tarLoseY)  玩家下一回合如下此处，玩家将胜利
         * cnt                   棋盘空闲格数量
         * mp                    储存棋盘空闲格位置
         */
        int tarLoseX, tarLoseY, cnt;
        int[] mp = new int[9];
        cnt = 0;
        tarLoseX = tarLoseY = -1;

        switch (gameLevel) {
            // easy模式
            case false:
                for (int temp1 = 0; temp1 < 3; temp1++) {
                    for (int temp2 = 0; temp2 < 3; temp2++) {
                        if (chess_state[temp1, temp2] == 0){
                            chess_state[temp1, temp2] = 2;
                            return;
                        }
                    }
                }
                break;

            // hard模式
            case true:
                //遍历棋盘，计算下一次落子位置
                for (int temp1 = 0; temp1 < 3; temp1++) {
                    for (int temp2 = 0; temp2 < 3; temp2++) {
                        if (chess_state[temp1, temp2] == 0) {
                            //判断AI落子此处是否会胜利，若胜利，则落子
                            chess_state[temp1, temp2] = 2;
                            if (checkState() == 2)
                                return;

                            //判断玩家落子此处是否会胜利，若胜利，则记下当前位置(玩家已将军)
                            chess_state[temp1, temp2] = 1;
                            if (checkState() == 1) {
                                tarLoseX = temp1;
                                tarLoseY = temp2;
                            }

                            //恢复棋盘，记下当前空白格位置
                            chess_state[temp1, temp2] = 0;
                            mp[cnt++] = temp1 * 3 + temp2;
                        }
                    }
                }

                //若存在玩家将军，则落子
                if (tarLoseX != -1) {
                    chess_state[tarLoseX, tarLoseY] = 2;
                    return;
                }

                //AI落子后既不会胜利，也不存在玩家将军，则随机选择一个空白格落子
                int rd = (int)UnityEngine.Random.Range(0, cnt);
                chess_state[mp[rd] / 3, mp[rd] % 3] = 2;

                break;

        }

    }
}