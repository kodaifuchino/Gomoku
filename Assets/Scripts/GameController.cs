using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
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

    [SerializeField] AudioClip[] clips;
    [SerializeField] float pitchRange = 0.1f;
    public AudioClip sound;//石を置くときの音
    public AudioClip sound2;//勝った時の音
    AudioSource audioSource;

    public GameObject particleObject;//石を置くときのエフェクト
    public GameObject particleObject2;//勝った時のエフェクト
    public GameObject particleObject3;//勝った時のエフェクト
    public GameObject particleObject4;//勝った時のエフェクト
    private Vector3 pos;
    public int finish_flg =0;
    public string message="White's Turn!";
    public Text textObject;
    [SerializeField] private string loadScene="Menu 3D 1";
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeSpeedMultiplier = 1.5f;
    public Button itembutton;
    public Button meteorbutton;
    public Button blackholebutton;
    public Button zerogravitybutton;
    public ItemManager itemManager;
    private bool whiteMeteor = false;
    private bool whiteBlackHole = false;
    private bool whiteZeroGravity = false;
    private bool blackMeteor = false;
    private bool blackBlackHole = false;
    private bool blackZeroGravity = false;




    

    // Start is called before the first frame update
    void Start()
    {
        //カメラ情報を取得
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();

        //配列を初期化
        InitializeArray();
        finish_flg =0;

        //アイテムを初期化
        whiteMeteor = false;
        whiteBlackHole = true;//白は初めからblackholeを使っている扱い
        whiteZeroGravity = false;
        blackMeteor = false;
        blackBlackHole = false;
        blackZeroGravity = false;

        blackholebutton.interactable = false;//一ターン目は必ず白なので最初はブラックホールをfalseに

        //デバッグ用メソッド
        //DebugArray();

        audioSource = GetComponent<AudioSource>();

        textObject.text = "White's Turn!";
    }

    // Update is called once per frame
    void Update()
    {
        if(finish_flg == 0){
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

                            if(itemManager.meteor == true){
                                //Meteorモード
                                whiteMeteor = true;
                                //??????????????????????????????????????????????????????
                                GameObject stone = Instantiate(whiteStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成

                                //周囲の碁石を全破壊
                                //周囲の碁石の座標取得
                                List<string> aroundList = new List<string>();
                                int meteorCounter = 0;
                                if(z != 0){
                                    aroundList.Add((z-1) + "" + (x));
                                    meteorCounter++;
                                }
                                if(z != 9){
                                    aroundList.Add((z+1) + "" + (x));
                                    meteorCounter++;
                                    
                                }
                                if(x != 0){
                                    aroundList.Add((z) + "" + (x-1));
                                    meteorCounter++;

                                }
                                if(x != 9){
                                    aroundList.Add((z) + "" + (x+1));
                                    meteorCounter++;

                                }
                                if( z != 0 && x != 0){
                                    aroundList.Add((z-1) + "" + (x-1));
                                    meteorCounter++;
                                }
                                if(z != 0 && x != 9){
                                    aroundList.Add((z-1) + "" + (x+1));
                                    meteorCounter++;
                                }
                                if(z != 9 && x != 0){
                                    aroundList.Add((z+1) + "" + (x-1));
                                    meteorCounter++;
                                }
                                if(z != 9 && x != 9){
                                    aroundList.Add((z+1) + "" + (x+1));
                                    meteorCounter++;

                                }
                                
                                for(int i=0; i<meteorCounter ; i++){
                                    GameObject aroundStone = GameObject.FindWithTag(aroundList[i]);
                                    if(aroundStone != null){
                                        string Coordinate = aroundList[i];
                                        char firstCharacter = Coordinate[0];
                                        int aroundZ = int.Parse(firstCharacter.ToString());//１文字目がZ座標
                                        char secondCharacter = Coordinate[1];
                                        int aroundX = int.Parse(secondCharacter.ToString());//2文字目がX座標
        
                                        squares[aroundZ, aroundX] = EMPTY;
                                        Destroy(aroundStone);
                                    }
                                    
                                }




                            }else if(itemManager.zerogravity == true){
                                //ZeroGravityモード
                                whiteZeroGravity = true;
                                //???????????????????????????????????????????????????????
                                GameObject stone = Instantiate(whiteStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成
                            }else{
                                //通常モード
                                //Stoneを出力
                                GameObject stone = Instantiate(whiteStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成
                            }

                            //Playerを交代
                            currentPlayer = BLACK;
                            message = "Black's Turn!";
                            textObject.text = "Black's Turn!";
                            textObject.color = Color.black;


                            //Itemを黒用に変える
                            if(blackMeteor == true){
                                meteorbutton.interactable = false;
                            }else{
                                meteorbutton.interactable = true;
                            }

                            if(blackBlackHole == true){
                                blackholebutton.interactable = false;
                            }else{
                                blackholebutton.interactable = true;

                            }

                            if(blackZeroGravity == true){
                                zerogravitybutton.interactable = false;
                            }else{
                                zerogravitybutton.interactable = true;
                            }


                            //Itemボタン使用可に変更し、Itemモードを終了する
                            itembutton.interactable = true;
                            itemManager.item = false;
                            itemManager.meteor = false;
                            itemManager.blackhole = false;
                            itemManager.zerogravity = false;
                        }
                        //黒のターンのとき
                        else if (currentPlayer == BLACK)
                        {
                            
                            //Squaresの値を更新
                            squares[z, x] = BLACK;


                            if(itemManager.meteor == true){
                                //Meteorモード
                                blackMeteor = true;
                                //??????????????????????????????????????????????????????
                                GameObject stone = Instantiate(blackStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成

                                //周囲の碁石を全破壊
                                //周囲の碁石の座標取得
                                List<string> aroundList = new List<string>();
                                int meteorCounter = 0;
                                if(z != 0){
                                    aroundList.Add((z-1) + "" + (x));
                                    meteorCounter++;
                                }
                                if(z != 9){
                                    aroundList.Add((z+1) + "" + (x));
                                    meteorCounter++;
                                    
                                }
                                if(x != 0){
                                    aroundList.Add((z) + "" + (x-1));
                                    meteorCounter++;

                                }
                                if(x != 9){
                                    aroundList.Add((z) + "" + (x+1));
                                    meteorCounter++;

                                }
                                if( z != 0 && x != 0){
                                    aroundList.Add((z-1) + "" + (x-1));
                                    meteorCounter++;
                                }
                                if(z != 0 && x != 9){
                                    aroundList.Add((z-1) + "" + (x+1));
                                    meteorCounter++;
                                }
                                if(z != 9 && x != 0){
                                    aroundList.Add((z+1) + "" + (x-1));
                                    meteorCounter++;
                                }
                                if(z != 9 && x != 9){
                                    aroundList.Add((z+1) + "" + (x+1));
                                    meteorCounter++;

                                }
                                
                                for(int i=0; i<meteorCounter ; i++){
                                    GameObject aroundStone = GameObject.FindWithTag(aroundList[i]);
                                    if(aroundStone != null){
                                        string Coordinate = aroundList[i];
                                        char firstCharacter = Coordinate[0];
                                        int aroundZ = int.Parse(firstCharacter.ToString());//１文字目がZ座標
                                        char secondCharacter = Coordinate[1];
                                        int aroundX = int.Parse(secondCharacter.ToString());//2文字目がX座標
        
                                        squares[aroundZ, aroundX] = EMPTY;
                                        Destroy(aroundStone);
                                    }
                                    
                                }

                            }else if(itemManager.zerogravity == true){
                                //ZeroGravityモード
                                whiteZeroGravity = true;
                                //???????????????????????????????????????????????????????
                                GameObject stone = Instantiate(blackStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成
                            }else{
                                //通常モード
                                //Stoneを出力
                                GameObject stone = Instantiate(blackStone);
                                stone.transform.position = hit.collider.gameObject.transform.position;
                                stone.tag = z + "" + x; //生成した碁石に座標情報をタグ付け
                                Debug.Log(stone.tag);
                                audioSource.PlayOneShot(sound);
                                pos = stone.transform.position;
                                pos.y += 1.0f;
                                Instantiate(particleObject, pos, Quaternion.identity); //エフェクト生成
                            }    

                            //Playerを交代
                            currentPlayer = WHITE;
                            message = "White's Turn!";
                            textObject.text = "White's Turn!";
                            textObject.color = Color.white;

                            //Itemを白用に変える
                            if(whiteMeteor == true){
                                meteorbutton.interactable = false;
                            }else{
                                meteorbutton.interactable = true;
                            }

                            if(whiteBlackHole == true){
                                blackholebutton.interactable = false;
                            }else{
                                blackholebutton.interactable = true;
                            }

                            if(whiteZeroGravity == true){
                                zerogravitybutton.interactable = false;
                            }else{
                                zerogravitybutton.interactable = true;

                            }

                            //Itemボタン使用可に変更し、Itemモードを終了する
                            itembutton.interactable = true;
                            itemManager.item = false;
                            itemManager.meteor = false;
                            itemManager.blackhole = false;
                            itemManager.zerogravity = false;
                        }
                    }
                }
            }

            //碁石が揃っているかどうか確認する
            if (CheckStone(WHITE) || CheckStone(BLACK)){
                if(CheckStone(WHITE)){
                    Debug.Log("白の勝ち！！！");
                    message = "White Win!!";
                    textObject.text = "White Win!";
                }
                if(CheckStone(BLACK)){
                    Debug.Log("黒の勝ち！！！");
                    message = "Black Win!!";
                    textObject.text = "Black Win!";
                }

                //os.y -= 1.0f;
                //Instantiate(particleObject2, pos, Quaternion.identity);
                //Instantiate(particleObject3, pos, Quaternion.identity);
                Quaternion rotation = Quaternion.Euler(270, 0, 0);  // Y軸周りに45度回転
                Instantiate(particleObject4, pos, rotation);
                audioSource.PlayOneShot(sound2);

                finish_flg = 1;//ゲーム終了を示す

                return;
            }
            

            //マウスがクリックされたとき
            if (Input.GetMouseButtonDown(0))
            {
                //マウスのポジションを取得してRayに代入
                Ray ray = camera_object.ScreenPointToRay(Input.mousePosition);
            }
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
         private void DebugArray()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Debug.Log("(i,j) = (" + i + "," + j + ") = " + squares[i, j]);
                }
            }
        } 

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
                        //Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        //Debug.Log("黒の勝ち！！！");
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
                        //Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        //Debug.Log("黒の勝ち！！！");
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
                        //Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        //Debug.Log("黒の勝ち！！！");
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
                        //Debug.Log("白の勝ち！！！");
                    }
                    //黒のとき
                    else
                    {
                        //Debug.Log("黒の勝ち！！！");
                    }

                    return true;
                }

                down--;
            }
        }

        //まだ判定がついていないとき
        return false;
    }

    private void Finish(){

    }

    public void PushNewGame()
    {
        Initiate.Fade("Gomoku", fadeColor, fadeSpeedMultiplier);
    }

    public void PushQuit()
    {
        Initiate.Fade(loadScene, fadeColor, fadeSpeedMultiplier);
    }



}