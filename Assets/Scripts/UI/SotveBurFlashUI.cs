 
    using UnityEngine;

    public class SotveBurFlashUI:MonoBehaviour
    {
        [SerializeField]private StoveCounter stoveCounter;
        private Animator animator;
        private const string ISFLASH = "isFlash";
        private bool show;
 
        private void Start()
        {
            stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;
            animator = GetComponent<Animator>();
        }

        private void stoveCounter_OnProgressChanged(object sender, StoveCounter.OnProgressChangedArgs e)
        {
              float burnShowProgressAount = 0.5f;
              show = stoveCounter.IsFried()&&e.progressNomel >= burnShowProgressAount;
              animator.SetBool(ISFLASH,show);
        }

      
        
    }
 