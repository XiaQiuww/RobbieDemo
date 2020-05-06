using UnityEngine;

namespace Robbie
{
    /// <summary>
    /// 砖块下落
    /// </summary>
    public class FallingBlockCollision : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        private BoxCollider2D boxCol;
        private AudioSource audioSource;
        private LayerMask groundMask;
        private int groundLayer;        


        /*-------------生命周期------------*/
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            boxCol = GetComponent<BoxCollider2D>();
            audioSource = GetComponent<AudioSource>();

            groundMask = LayerMask.GetMask("Ground"); 
            groundLayer = LayerMask.NameToLayer("Ground");                                                            
        }
        /*------------END-生命周期------------*/


        /// <summary>
        /// 砖块下落
        /// </summary>
        public void Fall()
        {
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
            gameObject.layer = LayerMask.NameToLayer("Traps");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != groundLayer)
                return;

            Vector3 pos = rigidBody.position;
            RaycastHit2D hit;

            hit = Physics2D.Raycast(pos, Vector2.down, 1f, groundMask);
            pos.y = hit.point.y + 0.5f;
            transform.position = pos;

            boxCol.usedByComposite = true; //与Tile的collider连接为一体
            Destroy(rigidBody);

            audioSource.Play();
        }
        /*--------------------------------*/
    }
}
