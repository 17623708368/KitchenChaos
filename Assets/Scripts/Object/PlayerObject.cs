using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerObject : NetworkBehaviour,IkitchenObjectParent
{


    private static PlayerObject localInstance;
    public static PlayerObject LocalInstance => localInstance;

    public static EventHandler OnAnyPlayerSpwand;
    
    [SerializeField]    private int moveSpeed;
   [SerializeField] LayerMask collisionsLayerMask;
   [SerializeField] private List<Vector3> spwanVector3s = new List<Vector3>();

    private bool isWalking;
    private Vector3 lastInteractDir;

    public EventHandler OnPickedSomething;
    private BaseCounter selectCounter;

    //事件选择的橱柜发生切换就调用
    public event EventHandler<OnSelectedCounterChangedEventAgr> OnSelectedCounterChanged;
    /// <summary>
    /// 用于传递参数的类
    /// </summary>
    public class OnSelectedCounterChangedEventAgr:EventArgs
    {
        public BaseCounter selectCounter;
    }


    public static void ResetCallback()
    {
        OnAnyPlayerSpwand = null;
    }
public override void OnNetworkSpawn()
{
    if (IsOwner)
    {
        localInstance = this;
    }
      transform.position= spwanVector3s[(int)OwnerClientId];
    OnAnyPlayerSpwand?.Invoke(this,EventArgs.Empty);
    if (IsServer)
    {
        NetworkManager.Singleton.OnClientDisconnectCallback+=NetworkManager_OnClientDisconnectCallback;
    }
}

private void NetworkManager_OnClientDisconnectCallback(ulong obj)
{
    if (IsOwner&&HasKitchenObject())
    {
        GetKitchenObject().DestroySelf();
    }
}

private void Start()
{//添加事件
    GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    GameInput.Instance.OnInteractAltemateAction += GameInput_OnInteractAltemateAction;
}

private void GameInput_OnInteractAltemateAction(object sender, EventArgs e)
{
    //是否是在游戏
    if (!GameMgr.Instance.IsGamePlaying())
        return;
    //判断当前选择的橱柜是否为空
    if (selectCounter!=null)
    {
        selectCounter.InteractAltemate(this);
    }
}

/// <summary>
/// 按键交互事件
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void GameInput_OnInteractAction(object sender, EventArgs e)
{    //是否是在游戏
    if (!GameMgr.Instance.IsGamePlaying())
        return;
    //判断当前选择的橱柜是否为空
    if (selectCounter!=null)
    {
        selectCounter.Interact(this);
    }
    
}

private void Update()
    {
        //是否是本地玩家
        if (!IsOwner)
            return;
        HandleInteraction();
        HandleMovement();   
    }
   
    public void HandleInteraction()
    {
        Vector2 inputVector2 =  GameInput.Instance.GetMoveVector();
        float interactionDis=2f;
        Vector3 moveDir = new Vector3(inputVector2.x, 0, inputVector2.y);
        RaycastHit raycastHit ;
        if (moveDir!=Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        
        if ( Physics.Raycast(transform.position,lastInteractDir, out raycastHit,interactionDis,1<<LayerMask.NameToLayer("Counter")))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter!=selectCounter)
                {
                    SelectCounter(counter);
                }
               
            }
            else
            {
                SelectCounter(null);

            }
        }
        else
        {
            SelectCounter(null);
        }   
    }
/// <summary>
/// 设则当前橱柜并调用
/// </summary>
/// <param name="counter"></param>
    private void SelectCounter(BaseCounter counter)
    {
       this. selectCounter = counter;
        OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventAgr()
        {
            selectCounter =this. selectCounter
        });
    }
    private void HandleMovement()
    {
        Vector2 inputVector2 = GameInput.Instance.GetMoveVector();
        inputVector2 = inputVector2.normalized;
        Vector3 moveDir = new Vector3(inputVector2.x, 0, inputVector2.y);
        float rotateMove = 10f;
        float playerRadius = .7f;
        float moveDic = moveSpeed * Time.deltaTime;
        float playerHight = 2;
        isWalking = moveDir != Vector3.zero;

        /*射线检测是否前方有物体
        //参数一，从哪里开始画球
        //参数二，从哪里结束
         transform.position:
        这是胶囊体的起始位置（底部半球的中心点）。transform.position 是当前游戏对象（通常是玩家或其他需要检测碰撞的物体）在世界空间中的位置。
        transform.position + Vector3.up * playerHight:  
        这是胶囊体的终止位置（顶部半球的中心点）。这里，Vector3.up 是一个表示向上方向的向量，playerHight 应该是一个浮点数，
        表示胶囊体的高度。所以，Vector3.up * playerHight 会得到一个表示胶囊体高度向上方向的向量，与 transform.position 相加后得到胶囊体顶部半球的中心点位置。
        playerRadius:
        这是胶囊体的半径。它定义了胶囊体两端半球的大小以及圆柱体部分的直径。
        moveDir:
        这是胶囊体移动的方向向量。Physics.CapsuleCast 会沿着这个方向进行碰撞检测。
        如果胶囊体沿着这个方向移动会与场景中的其他物体发生碰撞，那么这个方法会返回碰撞信息。
       
         */
        bool canMove = !Physics.BoxCast(transform.position,
             Vector3.one*playerRadius,   moveDir,Quaternion.identity, moveDic,collisionsLayerMask);
        //如果前方有阻挡物就
        if (!canMove)
        {
            //判断X轴是否能走
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x<-.5||moveDir.x>+.5)&&!Physics.BoxCast(transform.position , 
                Vector3.one*playerRadius, moveDirX,Quaternion.identity, moveDic,collisionsLayerMask);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //判断Z轴是否有阻挡物
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove =  (moveDir.z<-.5||moveDir.z>+.5)&&!Physics.BoxCast(transform.position,
                    Vector3.one*playerRadius, moveDirZ,Quaternion.identity, moveDic,collisionsLayerMask);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        transform.forward = Vector3.Slerp(transform.forward, lastInteractDir,   rotateMove);
        if (canMove)
        {
            transform.position += moveDir * moveDic;
        }

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    #region 厨房交互接口

       [SerializeField]private Transform kitchenPoint;
       private  KitchenObject kitchenObj;


        public Transform GetKitchenObjectFollowTransform()
        {
            return kitchenPoint;
        }
        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObj = kitchenObject;
            if (kitchenObj!=null)
            {
                OnPickedSomething?.Invoke(this,EventArgs.Empty);
            }
        }
    
        public KitchenObject GetKitchenObject()
        {
            return kitchenObj;
    
        }

   
        public void ClearKitchenObject()
        {   
            kitchenObj = null;
        }
    
        public bool HasKitchenObject()
        {
            return kitchenObj != null;
        }

        public NetworkObject GetNetworkObject()
        {
        return NetworkObject;
        }

        #endregion

}
