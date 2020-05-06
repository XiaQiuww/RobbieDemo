using UnityEngine;

namespace Robbie
{
    /// <summary>
    /// 自动伸缩尖刺陷阱
    /// </summary>
    public class AutoSpikes : MonoBehaviour
    {
        public float activeDuration = 2f; //陷阱持续时间

        private Animator anim;
        private AudioSource audioSource;
        private int activeParamID;
        private float deactivationTime;   //陷阱下降时间
        private bool playerInRange;       //主角在触发区
        private bool trapActive;          //陷阱是否激活
        private int playerLayer;


        /*-------------生命周期------------*/
        private void Start()
        {
            activeParamID = Animator.StringToHash("Active");

            playerLayer = LayerMask.NameToLayer("Player");

            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            //  陷阱已激活     主角不在触发区     已到达陷阱下降时间
            if (trapActive && !playerInRange && Time.time >= deactivationTime)
            {
                trapActive = false;
                anim.SetBool(activeParamID, false);
            }
        }
        /*------------END-生命周期------------*/


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == playerLayer)
            {
                playerInRange = true;
                trapActive = true;
                anim.SetBool(activeParamID, true);
                audioSource.Play();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == playerLayer)
            {
                playerInRange = false;
                deactivationTime = Time.time + activeDuration;
            }
        }
        /*--------------------------------*/
    }
}
