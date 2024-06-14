using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI txtkeyMoveUp;
    [SerializeField]private TextMeshProUGUI txtkeyMovedown;
    [SerializeField]private TextMeshProUGUI txtkeyMoveLeft;
    [SerializeField]private TextMeshProUGUI txtkeyMoveRight;
    [SerializeField]private TextMeshProUGUI txtkeyInteract;
    [SerializeField]private TextMeshProUGUI txtkeyAlt;
    [SerializeField]private TextMeshProUGUI txtkeyPause;
    [SerializeField]private TextMeshProUGUI txtkeyGemaPadMove;
    [SerializeField]private TextMeshProUGUI txtkeyGamePadInteract;
    [SerializeField]private TextMeshProUGUI txtkeyGamePadAlt;
    [SerializeField]private TextMeshProUGUI txtkeyGamePadPause;

    private void Start()
    {
         Show();
         GameMgr.Instance.OnLocalPlayerReadyChanged += GameMgr_OnLocalPlayerReadyChanged;
    }

    private void GameMgr_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (GameMgr.Instance.IsLocalPlayerReadey())
        {
            Hide();
        }
    }
        public void UpdateVisual()
    {
        txtkeyMoveUp.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveUp);
        txtkeyMovedown.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveDown);
        txtkeyMoveLeft.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveLeft);
        txtkeyMoveRight.text = GameInput.Instance.GetBinding(GameInput.Binding.MonveRigth);
        txtkeyInteract.text = GameInput.Instance.GetBinding(GameInput.Binding.Interact);
        txtkeyAlt.text = GameInput.Instance.GetBinding(GameInput.Binding.InteractAlt);
        txtkeyPause.text = GameInput.Instance.GetBinding(GameInput.Binding.Pause); 
        txtkeyGamePadAlt.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_InteractAlt);
        txtkeyGamePadInteract.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_Interact);
        txtkeyGamePadPause.text = GameInput.Instance.GetBinding(GameInput.Binding.GamePad_Pause);
    }

    void Show()
    {
        gameObject.SetActive(true);
        UpdateVisual();
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
