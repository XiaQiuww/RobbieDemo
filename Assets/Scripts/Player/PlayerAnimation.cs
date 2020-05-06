using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 角色动画
	///</summary>
	public class PlayerAnimation : MonoBehaviour
	{
        //动画脚本挂在robbie的子物体 Robbie body上

        //先获取父物体的playerMovement组件
        private PlayerMovement playerMovement;        
        private Rigidbody2D rb;

        private Animator animator;

        private int groundID;
        private int hangingID;
        private int crouchID;
        private int speedID;
        private int fallID;


        /*-------------生命周期------------*/
        private void Start()
        {
            animator = GetComponent<Animator>();

            playerMovement = GetComponentInParent<PlayerMovement>();
            rb = GetComponentInParent<Rigidbody2D>();

            groundID = Animator.StringToHash("isOnGround");
            hangingID = Animator.StringToHash("isHanging");
            crouchID = Animator.StringToHash("isCrouching");
            speedID = Animator.StringToHash("speed");
            fallID = Animator.StringToHash("verticalVelocity");            
        }

        private void Update()
        {
            //根据主角的状态更改状态机的状态
            animator.SetFloat(speedID, Mathf.Abs(playerMovement.xVelocity));
            animator.SetBool(groundID, playerMovement.isOnGround);
            animator.SetBool(hangingID, playerMovement.isHanging);
            animator.SetBool(crouchID, playerMovement.isCrouch);
            animator.SetFloat(fallID, rb.velocity.y);
        }
        /*------------End-生命周期------------*/


        /// <summary>
        /// running动画事件调用脚步声的函数
        /// </summary>
        public void StepAudio()
        {
            AudioManager.Instance.PlayerFootstepAudio();
        }

        /// <summary>
        /// 下蹲行走动画事件调用脚步声的函数
        /// </summary>
        public void CrouchStepAudio()
        {
            AudioManager.Instance.PlayerCrouchFootstepAudio();
        }
    /*-----------------------------------------------------*/
    }
}