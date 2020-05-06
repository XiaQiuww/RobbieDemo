using UnityEngine;


namespace Robbie
{
	///<summary>
	/// 门后的过关触发器
	///</summary>
	public class WinZone : MonoBehaviour
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))            
                GameManager.Instance.PlayerWon();            
        }
        /*--------------------------------*/
    }
}