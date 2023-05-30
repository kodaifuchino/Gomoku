using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;



public class ItemManager : MonoBehaviour
{
    public Button itembutton;
    public bool meteor = false;//Meteor: 次に選択したマスに自分の碁石を置き、その周囲の8マスに置かれた碁石を破壊する
    public bool blackhole = false;//BlackHale: 次に選択したマスに自分の碁石を置き、そのマスとその周囲8マスに,相手は一ターンの間石が置けなくなる
    public bool zerogravity = false;//ZeroGravity: 次に選択したマスに自分の碁石を置き、その周囲8マスに置かれた碁石をランダムに配置しなおす(再配置はその8マスの中で行う)

    public bool item = false;//item: アイテムを使用したかどうか判定用
    public Button meteorButton;
    public Button blackHoleButton;
    public Button zeroGravityButton;
    
    public void PushMeteor ()
	{
		meteor = true;
        item = true;
        itembutton.interactable = false;
        meteorButton.interactable = false;
	}

    public void PushBlackHole ()
	{
		blackhole = true;
        item = true;
        itembutton.interactable = false;
        blackHoleButton.interactable = false;
	}

    public void PushZeroGravity()
    {
        zerogravity = true;
        item = true;
        itembutton.interactable = false;
        zeroGravityButton.interactable = false;
    }
}
