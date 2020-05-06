using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Robbie
{
	///<summary>
	/// 检查关卡是否通关
	///</summary>
	public class CheckSceneLock : MonoBehaviour
	{
        private Image buttonImage;
        private Text buttonText;
        private Button button;


        /*-------------生命周期------------*/
        private void Awake()
        {
            buttonImage = transform.GetComponent<Image>();
            buttonText = transform.GetComponentInChildren<Text>();
            button = transform.GetComponent<Button>();
        }
        /*------------End-生命周期------------*/


        /// <summary>
        /// 检查游戏是否通关
        /// </summary>
        public void CheckScene()
        {
            if(PlayerPrefs.GetInt(buttonText.text) == 0)
            {
                button.enabled = false;
                buttonImage.color = Color.red;                
            }
            else
            {
                button.enabled = true;
                buttonImage.color = Color.white;
            }
        }
    /*--------------------------------------------------------*/
    }
}