using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //10*10のint型２次元配列を定義
    private int[,] squares = new int[10, 10];

    //WHITE=1,BLACK=-1で定義
    private const int EMPTY = 0;
    private const int WHITE = 1;
    private const int BLACK = -1;

    //現在のプレイヤー(初期プレイヤーは白)
    private int currentPlayer = WHITE;

    //カメラ情報
    private Camera camera_object;
    private RaycastHit hit;

    //prefabs
    public GameObject whiteStone;
    public GameObject blackStone;

    // Start is called before the first frame update
    void Start()
    {
        //カメラ情報を取得
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();

        //配列を初期化
        InitializeArray();

        //デバッグ用メソッド
        //DebugArray();
    }

    // Update is called once per frame
    void Update()
    {
        //マウスがクリックされたとき
        if (Input.GetMouseButtonDown(0))
        {
            //マウスのポジションを取得してRayに代入
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition);

            //マウスのポジションからRayを投げて何かに当たったらhitに入れる
            if (Physics.Raycast(ray, out hit))
            {
                //x,zの値を取得
                int x = (int)hit.collider.gameObject.transform.position.x;
                int z = (int)hit.collider.gameObject.transform.position.z;

                //マスが空のとき
                if (squares[z, x] == EMPTY)
                {
                    //白のターンのとき
                    if (currentPlayer == WHITE)
                    {
                        //Squaresの値を更新
                        squares[z, x] = WHITE;

                        //Stoneを出力
                        GameObject stone = Instantiate(whiteStone);
                        stone.transform.position = hit.collider.gameObject.transform.position;

                        //Playerを交代
                        currentPlayer = BLACK;
                    }
                    //黒のターンのとき
                    else if (currentPlayer == BLACK)
                    {
                        //Squaresの値を更新
                        squares[z, x] = BLACK;

                        //Stoneを出力
                        GameObject stone = Instantiate(blackStone);
                        stone.transform.position = hit.collider.gameObject.transform.position;

                        //Playerを交代
                        currentPlayer = WHITE;
                    }
                }
            }
        }

        //碁石が揃っているかどうか確認する
        if (CheckStone(WHITE) || CheckStone(BLACK))
        {
            return;
        }

        //マウスがクリックされたとき
        if (Input.GetMouseButtonDown(0))
        {
            //マウスのポジションを取得してRayに代入
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition);
        }
    }

    //配列情報を初期化する
    private void InitializeArray()
    {
        //for文を利用して配列にアクセスする
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //配列を空（値を０）にする
                squares[i, j] = EMPTY;
            }
        }
    }

    //配列情報を確認する（デバッグ用）
    /*     private void DebugArray()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Debug.Log("(i,j) = (" + i + "," + j + ") = " + squares[i, j]);
                }
            }
        } */

    //5個連続で碁石が置かれているか確認する(colorに判定する色を渡す)
    private bool CheckStone(int color)
    {
        //碁石の数をカウントする
        int count = 0;

        //横向き
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //squaresの値が空のとき
                if (squares[i, j] == EMPTY || squares[i, j] != color)
                {
                    //countを初期化する
                    count = 0;
                }
                else
                {
                    //countにsquaresの値を格納する
                    count++;
                }

                //countの値が5になったとき
                if (count == 5)
                {
                    //白のとき
                    if (color == WHITE)
                    {
                        Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        Debug.Log("黒の勝ち！！！");
                    }

                    return true;
                }
            }
        }

        //countの値を初期化
        count = 0;

        //縦向き
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //squaresの値が空のとき
                if (squares[j, i] == EMPTY || squares[j, i] != color)
                {
                    //countを初期化する
                    count = 0;
                }
                else
                {
                    //countにsquaresの値を格納する
                    count++;
                }

                //countの値が5になったとき
                if (count == 5)
                {
                    //白のとき
                    if (color == WHITE)
                    {
                        Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        Debug.Log("黒の勝ち！！！");
                    }

                    return true;
                }
            }
        }

        //countの値を初期化
        count = 0;

        //斜め(右上がり)
        for (int i = 0; i < 10; i++)
        {
            //上移動用
            int up = 0;

            for (int j = i; j < 10; j++)
            {
                //squaresの値が空のとき
                if (squares[j, up] == EMPTY || squares[j, up] != color)
                {
                    //countを初期化する
                    count = 0;
                }
                else
                {
                    count++;
                }

                //countの値が5になったとき
                if (count == 5)
                {
                    //白のとき
                    if (color == WHITE)
                    {
                        Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        Debug.Log("黒の勝ち！！！");
                    }

                    return true;
                }

                up++;
            }
        }

        //countの値を初期化
        count = 0;

        //斜め(右下がり)
        for (int i = 0; i < 10; i++)
        {
            //下移動用
            int down = 9;

            for (int j = i; j < 10; j++)
            {
                //squaresの値が空のとき
                if (squares[j, down] == EMPTY || squares[j, down] != color)
                {
                    //countを初期化する
                    count = 0;
                }
                else
                {
                    count++;
                }

                //countの値が5になったとき
                if (count == 5)
                {
                    //白のとき
                    if (color == WHITE)
                    {
                        Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        Debug.Log("黒の勝ち！！！");
                    }

                    return true;
                }

                down--;
            }
        }

        //まだ判定がついていないとき
        return false;
    }



}