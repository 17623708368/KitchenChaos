using System;
using UnityEngine;

 
    public class SelectedCounterVisual : MonoBehaviour
    {
       [SerializeField] private BaseCounter clearCount;
      [SerializeField]  private GameObject []kit;
     
        private void Start()
        {
            if (PlayerObject.LocalInstance!=null)
            {
                 PlayerObject.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
         
              PlayerObject.OnAnyPlayerSpwand += PlayerObject_OnAnyPlayerSpwand;
        }

        private void PlayerObject_OnAnyPlayerSpwand(object sender, EventArgs e)
        {
            if (PlayerObject.LocalInstance!=null)
            {
                PlayerObject.LocalInstance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
            }
        }

        private void Player_OnSelectedCounterChanged(object sender, PlayerObject.OnSelectedCounterChangedEventAgr e)
        {
            if (e.selectCounter==clearCount)
            {
               
                Show();
            }
            else
            { Hide();
                
            }
        }

        void Show()
        {
            foreach (GameObject item in kit)
            {
                item.SetActive(true);
            }
        }

        void Hide()
        { foreach (GameObject item in kit)
            {
                item.SetActive(false);
            }
        }
    }
 