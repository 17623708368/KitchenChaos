using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
   [SerializeField ] private Slider sdrSound;
    [SerializeField]private Slider sdrMusic;
    [SerializeField]private Button btnClose;
    private static OptionsUI instance;
    public static OptionsUI Instance=>instance;
    //各个按键组件
    [SerializeField] private Button btnMoveUp;
    [SerializeField] private Button btnMoveDown;
    [SerializeField] private Button btnMoveLeft;
    [SerializeField] private Button btnMoveRight;
    [SerializeField] private Button btnInteract;
    [SerializeField] private Button btnInteractAlt;
    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnGamePad_Interact;
    [SerializeField] private Button btnGamePad_InteractAlt;
    [SerializeField] private Button btnGamePad_Pause;
    [SerializeField] private TextMeshProUGUI txtPause;
    [SerializeField] private TextMeshProUGUI txtMoveUp;
    [SerializeField] private TextMeshProUGUI txtMoveDown;
    [SerializeField] private TextMeshProUGUI txtMoveLeft;
    [SerializeField] private TextMeshProUGUI txtMoveRight;
    [SerializeField] private TextMeshProUGUI txtInteract;
    [SerializeField] private TextMeshProUGUI txtInteractAlt;
    [SerializeField] private TextMeshProUGUI txtGame_PadInteract;
    [SerializeField] private TextMeshProUGUI txtGame_PadInteractAlt;
    [SerializeField] private TextMeshProUGUI txtGame_PadPause;
    [SerializeField]private Transform rebindingUI;
    [SerializeField] private Button btnRestor;
    private Action action;
    private void Awake()
    {
        instance = this;
    }
      
    private void Start()
    {  sdrSound.onValueChanged.AddListener((v) =>
         {
             SoundManager.Instance.SetSoundVolume(v);
           
         });
         sdrMusic.onValueChanged.AddListener((v) =>
         {
             MusicMgr.Instance.ChangeVolume(v);
 
         });
         btnClose.onClick.AddListener(() =>
         {
             Hide();
             
         });
         UpdateSlider(); 
       GameMgr.Instance.OnUnLocalPauseGameUI += GameMgrOnUnLocalPauseGameUI;
       btnInteract.onClick.AddListener((() =>
       {
           RebindingBinding(GameInput.Binding.Interact);
       }));
       btnPause.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.Pause);

       });
       btnInteractAlt.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.InteractAlt);

       });
       btnMoveDown.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.MonveDown);

       });
       btnMoveLeft.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.MonveLeft);

       });
       btnMoveRight.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.MonveRigth);

       });
       btnMoveUp.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.MonveUp);

       });
       btnGamePad_InteractAlt.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.GamePad_InteractAlt);

       });
       btnGamePad_Interact.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.GamePad_InteractAlt);

       });  
       btnGamePad_Pause.onClick.AddListener(() =>
       {
           RebindingBinding(GameInput.Binding.GamePad_Pause);
       });
       btnRestor.onClick.AddListener(() =>
       {
           RestorbindingBinding();
       });
         Hide();
         RebindingUIHide();
    }
    

    private void GameMgrOnUnLocalPauseGameUI(object sender, EventArgs e)
    {
        Hide();
    }


    private void UpdateSlider()
    {
        sdrSound .value= SoundManager.Instance.GetVolume();
        sdrMusic.value = MusicMgr.Instance.GetVolume();
        txtInteract.text = GameInput.Instance.GetBinding(GameInput.Binding.Interact);
        txtPause.text = GameInput.Instance.GetBinding(GameInput.Binding.Pause);
        txtInteractAlt.text = GameInput.Instance.GetBinding(GameInput.Binding.InteractAlt);
        txtMoveDown.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveDown);
        txtMoveLeft.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveLeft);
        txtMoveUp.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveUp);
        txtMoveRight.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveRigth);
        txtGame_PadInteract.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_Interact);
        txtGame_PadInteractAlt.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_InteractAlt);
        txtGame_PadPause.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_Pause);
    }

    void RebindingUIShow()
    {
        rebindingUI.gameObject.SetActive(true);
        
    }

    void RebindingUIHide()
    {
        rebindingUI.gameObject.SetActive(false);

    }
    public void Show(Action action)
    {
        this.action = action;
        gameObject.SetActive(true);
        btnGamePad_Interact.Select();
        UpdateSlider();

    }

    private void Hide()
    {
        gameObject.SetActive(false);
        action?.Invoke();
    }

    private void RebindingBinding(GameInput.Binding binding)
    {
        RebindingUIShow();
        GameInput.Instance.RebindingBinding(binding, () =>
        {
            RebindingUIHide();
            UpdateSlider();
        });
    }

    private void RestorbindingBinding()
    {
        RebindingUIShow();
        GameInput.Instance.RestoreBlinding(() =>
        {
            RebindingUIHide();
            UpdateSlider();
        });
    }
}
