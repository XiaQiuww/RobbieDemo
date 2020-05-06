using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 主角死亡机制
	///</summary>
	public class PlayerHealth : MonoBehaviour
	{
        /// <summary>
        /// 死亡特效预制件
        /// </summary>
        public GameObject deathVFXPrefab;

        /// <summary>
        /// 尖刺layer
        /// </summary>
        private int trapsLayer;

        private bool isDeath; //防止触发多次死亡

        /*-------------生命周期------------*/
        private void Start()
        {
            isDeath = false;
            trapsLayer = LayerMask.NameToLayer("Traps");
        }
        /*------------END-生命周期------------*/

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == trapsLayer && !isDeath)
            {
                isDeath = true;

                Instantiate(deathVFXPrefab, transform.position, transform.rotation);     //在主角死亡处生成 死亡特效(一团烟雾
                PoolManager.Instance.GetObject(transform.position, transform.rotation);  //在主角死亡处生成 残影

                gameObject.SetActive(false);                                             //隐藏Robbie                            

                GameManager.Instance.PlayerDied();
            }
        }
    /**************************************************************/
    }
}