using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMgr : NetworkBehaviour
{
    private static GameMgr instance;
    public static GameMgr Instance=>instance;
    public EventHandler OnStateChenged;
    private bool isPuse=false;

    private NetworkVariable<bool> isPauseGame=new NetworkVariable<bool>(false);
    //显示暂停UI
    public event EventHandler OnLocalPauseGameUI;
    public event EventHandler OnUnLocalPauseGameUI;
    public event EventHandler OnLocalPlayerReadyChanged;
    public Action OnCloseReadyUI;
    //网络显示暂停界面UI
    public Action OnWaitingAllPlayer;
    //关闭暂停UI
    public Action OnUnWaitingAllPlayer;
    private NetworkVariable<State>  state=new NetworkVariable<State>(State.WaitingToStart);

     //等待加载场景时间
    //  private float waitingToSartTimer=1;
    //等待开始
    private NetworkVariable<float>  countdowToStartTimer=new NetworkVariable<float>(3);
    //游戏进行时间
    private float  gamePlayingTimerMax =50 ;

    //游戏时间
    private NetworkVariable<float> gamePlayingTimer=new NetworkVariable<float>(0);
    private bool isLocalPlayerReady=false;
    //记录每个客户端是否准备好
    private Dictionary<float, bool> playerReadyDic;
    private Dictionary<float, bool>  playerPauseGameDic;

    private bool autoTestGamePause;
    enum State
    {
        WaitingToStart,
        CountdowToStart,
        GamePlaying,
        GameOver,
    }   
    


    private void Awake()
    {
        instance = this;
        OnStateChenged?.Invoke(this, EventArgs.Empty);
        playerReadyDic = new Dictionary<float, bool>();
        playerPauseGameDic = new Dictionary<float, bool>();

    }

    private void Start()
    {
        GameInput.Instance.OnPusePerformed += GameInput_OnpusePerformed;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
 
        state.OnValueChanged += state_OnValueChanged;
        isPauseGame.OnValueChanged += isPauseGame_OnValueChanged;
        //如果有人掉线就交给服务端处理
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback+=NetworkManager_OnClientDisconnectCallback;
        }
        
    }
/// <summary>
/// 监听客户端掉线
/// </summary>
/// <param name="obj"></param>
    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        autoTestGamePause = true;
    }

    private void isPauseGame_OnValueChanged(bool previousvalue, bool newvalue)
    {
        if (isPauseGame.Value)
        {
            Time.timeScale = 0;
            OnWaitingAllPlayer?.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            OnUnWaitingAllPlayer?.Invoke();
        }
    }

    private void state_OnValueChanged(State previousvalue, State newvalue)
    {
        OnStateChenged?.Invoke(this,EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state.Value==State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this,EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CloseServerRpc()
    {
        CloseClientRpc();
    }
    [ClientRpc]
    private void CloseClientRpc()
    {
        OnCloseReadyUI?.Invoke();
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams=default)
    {
        playerReadyDic[serverRpcParams.Receive.SenderClientId] = true;
        bool allClientReady = true;
        foreach (int clientsId   in NetworkManager.Singleton.ConnectedClientsIds)     
        {
            if ( ! playerReadyDic.ContainsKey(clientsId) || !playerReadyDic[clientsId])
            {
                allClientReady = false;
                break;
            }
        }
        //所有玩家准备好
        if (allClientReady)
        {
            CloseServerRpc();   
            state.Value = State.CountdowToStart; 
        }
     
    }
    private void GameInput_OnpusePerformed(object sender, EventArgs e)
    {
        PuseGame();
    }



    private void Update()
    {
        if (!IsServer)
      return;
        switch (state.Value)
        {
            case State.WaitingToStart:
                gamePlayingTimer.Value = 0;
                break;
            case State.CountdowToStart: 
                countdowToStartTimer.Value -= Time.deltaTime;
                if (countdowToStartTimer.Value<0)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value =  gamePlayingTimerMax ;

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer .Value-= Time.deltaTime;
                if (gamePlayingTimer.Value<=0)
                {
                    state .Value= State.GameOver;
                }
                break;
            case State.GameOver: 
                break;
        }
       
    }

    private void LateUpdate()
    {
        if (autoTestGamePause)
        {
            autoTestGamePause = false;
            IsAllClientPause();
        }
    }

    public void PuseGame()
    {
        isPuse = !isPuse;
        if (isPuse)
        {
            OnLocalPauseGameUI?.Invoke(this,EventArgs.Empty);
            OnPauseGameServerRpc();
        }
        else
        {
            OnUnLocalPauseGameUI?.Invoke(this,EventArgs.Empty);
            UnPauseGameServerRpc();
        }
    }
   [ServerRpc(RequireOwnership = false)]
    private void OnPauseGameServerRpc(ServerRpcParams serverRpcParams=default)
    {
        playerPauseGameDic[serverRpcParams.Receive.SenderClientId] = true;
        IsAllClientPause();
    }
    /// <summary>
    /// 服务端调用方法
    /// </summary>
    /// <param name="serverRpcParams"></param>
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams=default)
    { 
        playerPauseGameDic[serverRpcParams.Receive.SenderClientId] = false;
        IsAllClientPause();
    }
/// <summary>
/// 检测是否是所有玩家都取消了暂停
/// </summary>
    private void IsAllClientPause( )
    {      
        foreach (int  clientsId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPauseGameDic.ContainsKey(clientsId)&&playerPauseGameDic[clientsId])
            {
                isPauseGame.Value = true;
                return;
            }
        }

        isPauseGame.Value = false;
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsLocalPlayerReadey()
    {
        return isLocalPlayerReady;
    }
    public bool IsCountdowToStart()
    {
        return state.Value == State.CountdowToStart;
    }

    public float CountdowToStartTimer()
    {
        return countdowToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public float GetPlayingTimerNormalize()
    {
        return 1- (gamePlayingTimer.Value / gamePlayingTimerMax) ;
    }
}
