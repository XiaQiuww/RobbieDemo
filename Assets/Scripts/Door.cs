using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 门
	///</summary>
	public class Door : MonoBehaviour
	{
        private Animator anim;
        private int openID;


        /*-------------生命周期------------*/
        private void Start()
        {
            anim = GetComponent<Animator>();
            openID = Animator.StringToHash("Open");
            GameManager.Instance.RegisterDoor(this);
        }
        /*------------End-生命周期------------*/



        /// <summary>
        /// 开门
        /// </summary>
        public void Open()
        {
            anim.SetTrigger(openID);

            AudioManager.Instance.PlayDoorOpenAudio();
        }
    /*----------------------------------------------*/
    }
}