using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private static GameInput instance;
    public static GameInput Instance=>instance;
    /// <summary>
    /// 按下E键就调用事件
    /// </summary>
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAltemateAction;
    //按下停止时间
    public event EventHandler OnPusePerformed;
    private const string PLAYERPREFS_BINDING = "PlayerPrefs_Binding";
    private const string PLAYERPREFS_REBINDING = "PlayerPrefs_ReBinding";

    private void Awake()
    {
        instance = this;
        //得到input脚本
        playerInputAction = new PlayerInputAction();
        PlayerPrefs.SetString(PLAYERPREFS_REBINDING,playerInputAction.SaveBindingOverridesAsJson());
        if (PlayerPrefs.HasKey(PLAYERPREFS_BINDING))
        {   playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYERPREFS_BINDING));
        }
       
        
        //开启脚本检测
        playerInputAction.Enable();
        //添加事件检测
        playerInputAction.Player.Interact.performed += Interact_Performed;
        playerInputAction.Player.InteractAltemate.performed += InteractAltemate_Performed;
        playerInputAction.Player.Puse.performed += Puse_Performed;
      
    }

   public enum Binding
    {
        MonveUp,
        MonveDown,
        MonveLeft,
        MonveRigth,
        Interact,
        InteractAlt,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlt,
        GamePad_Pause,
        
    }
    private void Puse_Performed(InputAction.CallbackContext obj)
    {
        OnPusePerformed?.Invoke(this,EventArgs.Empty);
    }
    private void InteractAltemate_Performed(InputAction.CallbackContext obj)
    {
        OnInteractAltemateAction?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    
    }

    public  Vector2 GetMoveVector()
    {
        Vector2 inputVector2 = playerInputAction.Player.Move.ReadValue<Vector2>();
         return inputVector2;
    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_Performed;
        playerInputAction.Player.InteractAltemate.performed -= InteractAltemate_Performed;
        playerInputAction.Player.Puse.performed -= Puse_Performed;
        playerInputAction.Dispose();
    }

    public string GetBinding(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
              return  playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt: 
                return   playerInputAction.Player.InteractAltemate.bindings[0].ToDisplayString();
            case Binding.MonveDown: 
                return   playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.MonveLeft:
                return  playerInputAction.Player.Move.bindings[3].ToDisplayString();

            case Binding.MonveRigth: 
                return   playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.MonveUp:
                return  playerInputAction.Player.Move.bindings[1].ToDisplayString(); 
            case Binding.Pause:
                return  playerInputAction.Player.Puse.bindings[0].ToDisplayString();
            case Binding.GamePad_Interact:
                return  playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause: 
                return   playerInputAction.Player.InteractAltemate.bindings[1].ToDisplayString(); 
            case Binding.GamePad_InteractAlt: 
                return   playerInputAction.Player.InteractAltemate.bindings[1].ToDisplayString();
        }
    }

    public void RebindingBinding(Binding binding , Action onCallback)
    {
        playerInputAction.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        
        switch (binding)
        {
            default:
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
               break;
            case Binding.InteractAlt: 
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.MonveDown: 
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.MonveLeft:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MonveRigth: 
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.MonveUp:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Puse;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Pause:
                inputAction = playerInputAction.Player.Puse;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlt: 
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;

        }
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete
            (callback =>
            {
                callback.Dispose();
                playerInputAction.Player.Enable();
                onCallback?.Invoke();
                PlayerPrefs.SetString(PLAYERPREFS_BINDING,
                    playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
              
            } ) .Start();
    }

    public void RestoreBlinding(Action action)
    {       
        playerInputAction.Player.Disable();
        if (PlayerPrefs.HasKey(PLAYERPREFS_REBINDING))
        { 
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYERPREFS_REBINDING));
            PlayerPrefs.SetString(PLAYERPREFS_BINDING, playerInputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }
        playerInputAction.Enable();
        action?.Invoke();
    }
}
