using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 宝珠
	///</summary>
	public class Orb : MonoBehaviour
	{
        public GameObject explosionVFXPrefab; //拾起宝珠后的特效


        /*-------------生命周期------------*/
        private void Start()
        {
            GameManager.Instance.RegisterOrb(this);
        }
        /*------------END-生命周期------------*/


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Instantiate(explosionVFXPrefab, transform.position, transform.rotation); //生成特效

                AudioManager.Instance.PlayerOrbAudio(); 

                GameManager.Instance.PlayerGrabbedOrb(this); //宝珠数减1

                this.gameObject.SetActive(false); 
            }
        }
    /*--------------------------------------------------*/
    }
}