using UnityEngine;

namespace Robbie
{
    /// <summary>
    /// 砖块下落触发器
    /// </summary>
    public class FallingBlock : MonoBehaviour
    {
        public FallingBlockCollision block;

        private Animator anim;
        private BoxCollider2D boxCol;
        private AudioSource audioSource;
        private int playerLayer;
        private int fallParamID;


        /*-------------生命周期------------*/
        private void Start()
        {
            anim = GetComponent<Animator>();
            boxCol = GetComponent<BoxCollider2D>();
            audioSource = GetComponent<AudioSource>();

            playerLayer = LayerMask.NameToLayer("Player");
            fallParamID = Animator.StringToHash("Activate");
        }
        /*------------END-生命周期------------*/


        /// <summary>
        /// 砖块下落动画事件调用此函数
        /// </summary>
        public void Fall()
        {
            block.Fall();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != playerLayer)
                return;

            boxCol.enabled = false;
            audioSource.Play();
            anim.SetTrigger(fallParamID);
        }
        /*--------------------------------*/
    }
}
