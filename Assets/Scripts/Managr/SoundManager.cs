using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
  [SerializeField]  private AudioClipRefsSO audioClipRefsSo;
  [SerializeField] private AudioSource audioSource;
  private static SoundManager instance;
  private const string PLAYERPREFS_SOUND="Playerprefs_Sound";
  public static SoundManager Instance => instance;
  private bool isOnPlaySizze=false;
  private GameObject soundGameObject;
  //声音大小
  private float volume;

  private void Awake()
  {
      instance = this;
      
  }

  private void Start()
  {
      volume = PlayerPrefs.GetFloat(PLAYERPREFS_SOUND, 1);
      //播放上菜失败得音效
    DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
    //播放上菜成功得音效
    DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    //播放切菜音效
    CuttingCounter.OnCuttingPlaySound += CuttingCounter_OnCuttingPlaySound;
    //
   // PlayerObject.Instance.OnPickedSomething += PlayerObject_OnPickedSomething;
    BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
    TrashCounter.OnAnyObjectTranshed += TrashCounter_OnAnyObjectTranshed;
  }

  private void TrashCounter_OnAnyObjectTranshed(object sender, EventArgs e)
  {
      TrashCounter trashCounter=sender as TrashCounter;
      PlaySound(audioClipRefsSo.trash[Random.Range(0,audioClipRefsSo.trash.Length)],trashCounter.transform.position);
  }

  private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
  {
       BaseCounter baseCounter=sender as BaseCounter;
       PlaySound(audioClipRefsSo.drop[Random.Range(0,audioClipRefsSo.drop.Length)],baseCounter.transform.position);
  }

  private void PlayerObject_OnPickedSomething(object sender, EventArgs e)
  {
      if (PlayerObject.LocalInstance!=null)
      {
          PlaySound(audioClipRefsSo.pickup[Random.Range(0,audioClipRefsSo.pickup.Length)],PlayerObject.LocalInstance.transform.position);
      }
      else
      {
          PlayerObject.OnAnyPlayerSpwand += PlayerObject_OnAnyPlayerSpwand;

      }
  }

  private void PlayerObject_OnAnyPlayerSpwand(object sender, EventArgs e)
  {
      if (PlayerObject.LocalInstance!=null)
      {
          PlaySound(audioClipRefsSo.pickup[Random.Range(0,audioClipRefsSo.pickup.Length)],PlayerObject.LocalInstance.transform.position);
      }
  }

  private void CuttingCounter_OnCuttingPlaySound(object sender, EventArgs e)
  {
      CuttingCounter cuttingCounter=   sender as CuttingCounter;
      PlaySound(audioClipRefsSo.chop[Random.Range(0,audioClipRefsSo.chop.Length)],cuttingCounter.transform.position,1);
  }

  private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
  {
      PlaySound(audioClipRefsSo.deliverySuccess,DeliveryManager.Instance.transform.position);
   
  }

  private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
  {
      PlaySound(audioClipRefsSo.deliveryfail,Camera.main.transform.position,0.5f);
  }

  private void PlaySound(AudioClip[] audioClip, Vector3 point, float volume = 1)
  {
   PlaySound(audioClip[Random.Range(0,audioClip.Length)],point,volume);
  }

  private void PlaySound(AudioClip audioClip, Vector3 point, float volumeMutile = 1)
  {
      AudioSource.PlayClipAtPoint(audioClip,point,volume);
  }

  public void PlayFootstepSound(Vector3 point,float volumeMutile)
  {
      AudioSource.PlayClipAtPoint(audioClipRefsSo.footstep[Random.Range(0,audioClipRefsSo.footstep.Length)],point,volume);
  }
  public void PlayCountdownSound( )
  {
      AudioSource.PlayClipAtPoint(audioClipRefsSo.warning[0  ],Camera.main.transform.position,volume);
  }
  public void PlaywarniSound( )
  {
      AudioSource.PlayClipAtPoint(audioClipRefsSo.warning[1],Camera.main.transform.position,volume);
  }

  public void SetSoundVolume(float volume)
  {
      this.volume = volume;
      PlayerPrefs.SetFloat(PLAYERPREFS_SOUND,volume);
      PlayerPrefs.Save();
  }

  public float GetVolume()
  {
      return volume;
  }
}
