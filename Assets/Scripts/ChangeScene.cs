using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] private string loadScene="Gomoku";
    [SerializeField] private Color fadeColor = Color.black;
    [SerializeField] private float fadeSpeedMultiplier = 1.0f;

    public void PushButton()
    {
        Initiate.Fade(loadScene, fadeColor, fadeSpeedMultiplier);
    }

}