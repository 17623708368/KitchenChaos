using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField]private Image imgTimer;

    private void Update()
    {
         imgTimer.fillAmount=GameMgr.Instance. GetPlayingTimerNormalize();
    }
}
 