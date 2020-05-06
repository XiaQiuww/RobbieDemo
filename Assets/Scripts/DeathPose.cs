using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 主角死亡时在死亡位置留下残影
	///</summary>
	public class DeathPose : MonoBehaviour
	{
        /*-------------生命周期------------*/
        private void Awake()
        {            
            DontDestroyOnLoad(this);
        }
        /*------------End-生命周期------------*/
    }
}