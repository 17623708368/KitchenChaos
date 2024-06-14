using System;
using UnityEngine;
 
    public class FollowTransform : MonoBehaviour
    {
        private Transform targetTransform;

        private void Update()
        {
            if (targetTransform==null)
           return;
               
   
            this.transform .position= targetTransform.transform.position;
            this.transform .rotation= targetTransform.transform.rotation;
        }

        public void SetTargetTranform(Transform transform)
        {
            targetTransform = transform;
        }
        
    }
 