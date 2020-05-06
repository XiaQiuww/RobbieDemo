using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robbie
{
	///<summary>
	/// 对象池管理
	///</summary>
	public class PoolManager : MonoBehaviour
	{
        private static PoolManager _instance;

        public static PoolManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public GameObject deathPosPre;

        private int maxDeathPosNum; //池子容量
        public int MaxDeathPosNum
        {
            get
            {
                return maxDeathPosNum;
            }
        }

        public List<GameObject> deathPosList;


        /*-------------生命周期------------*/
        private void Awake()
        {
            if(_instance !=null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            maxDeathPosNum = 5;
            deathPosList = new List<GameObject>(maxDeathPosNum);

            DontDestroyOnLoad(this);
        }

        /*------------END-生命周期------------*/


        /// <summary>
        /// 预热
        /// </summary>
        public void Preload()
        {
            for (int i = 0; i < maxDeathPosNum; i++)
            {
                GameObject newGameObject = Instantiate(deathPosPre);
                newGameObject.SetActive(false);
                deathPosList.Add(newGameObject);
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="rotation">旋转角度</param>
        /// <returns>GameObject</returns>
        public GameObject GetObject(Vector3 position, Quaternion rotation)
        {
            if(deathPosList.Count == 0)
            {
                GameObject newGameObject = Instantiate(deathPosPre, position, rotation);
                GameManager.Instance.RegisterDeathPos(newGameObject);
                return newGameObject;
            }

            GameObject nextGameObject = deathPosList[0];
            nextGameObject.SetActive(true);
            nextGameObject.transform.position = position;
            nextGameObject.transform.rotation = rotation;

            deathPosList.RemoveAt(0);
            GameManager.Instance.RegisterDeathPos(nextGameObject);
            return nextGameObject;
        }

        /// <summary>
        /// 将对象放回池中
        /// </summary>
        /// <param name="go"></param>
        public void PutObject(GameObject go)
        {
            if (deathPosList.Count >= maxDeathPosNum)            
                Destroy(go);                                            
            else
            {
                go.SetActive(false);
                deathPosList.Add(go);                
            }
        }

    /*-----------------------------------------*/
    }
}