using UnityEngine;

namespace Robbie
{
    ///<summary>
    /// 加载场景的UI动画
    ///</summary>
    public class SceneFader : MonoBehaviour
	{
        private Animator anim;
        private int faderID;


        /*-------------生命周期------------*/
        private void Start()
        {
            anim = GetComponent<Animator>();

            faderID = Animator.StringToHash("Fade");

            GameManager.Instance.RegisterSceneFader(this);
        }
        /*------------END-生命周期------------*/


        /// <summary>
        /// 播放加载场景的UI动画
        /// </summary>
        public void FadeOut()
        {
            anim.SetTrigger(faderID);
        }
    /*------------------------------------------------*/
    }
}