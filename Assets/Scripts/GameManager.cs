using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Robbie
{
	///<summary>
	/// 游戏主控制器
	///</summary>
	public class GameManager : MonoBehaviour
	{
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 加载场景的UI动画
        /// </summary>
        private SceneFader fader;

        /// <summary>
        /// 已拾起宝珠数量
        /// </summary>
        public List<Orb> orbs;

        private Door lockedDoor; //门

        public int deathNum;     //死亡数

        private float gameTime;  //游玩时间

        private bool gameIsOver; //是否游戏结束

        public List<GameObject> deathPosList; //死亡残影list


        /*-------------生命周期------------*/
        private void Awake()
        {
            Screen.SetResolution(1920, 1080, true);

            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            orbs = new List<Orb>();            

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            deathPosList = new List<GameObject>();
            PoolManager.Instance.Preload();
        }

        private void Update()
        {
            if (gameIsOver)
                return;

            gameTime += Time.deltaTime;
            UIManager.Instance.UpdateTimeUI(gameTime);            
        }
        /*------------End-生命周期------------*/


        /*----注册----*/
        /// <summary>
        /// 注册 门(获取Door组件的引用
        /// </summary>
        /// <param name="door">门</param>
        public void RegisterDoor(Door door)
        {
            _instance.lockedDoor = door;
        }

        /// <summary>
        /// 注册 宝珠(获取Orb组件的引用
        /// </summary>
        /// <param name="orb"></param>
        public void RegisterOrb(Orb orb)
        {
            if (_instance == null)
                return;

            if (!_instance.orbs.Contains(orb))          
                _instance.orbs.Add(orb);

            UIManager.Instance.UpdateOrbUI(_instance.orbs.Count);
        }

        /// <summary>
        /// 注册 加载场景的UI动画(获取SceneFader组件的引用
        /// </summary>
        /// <param name="fader">加载场景的UI动画</param>
        public void RegisterSceneFader(SceneFader fader)
        {
            _instance.fader = fader;
        }

        /// <summary>
        /// 注册死亡残影
        /// </summary>
        /// <param name="obj">残影的obj</param>
        public void RegisterDeathPos(GameObject obj)
        {
            deathPosList.Add(obj);
        }
        /*---ENd-注册----*/


        /// <summary>
        /// 主角拾起宝珠(未拾起宝珠数减一
        /// </summary>
        /// <param name="orb">Orb</param>
        public void PlayerGrabbedOrb(Orb orb)
        {
            if(!_instance.orbs.Contains(orb))
                return; 

            _instance.orbs.Remove(orb);

            if (_instance.orbs.Count == 0)
                _instance.lockedDoor.Open();

            UIManager.Instance.UpdateOrbUI(_instance.orbs.Count);
        }

        /// <summary>
        /// 主角死亡
        /// </summary>
        public void PlayerDied()
        {
            AudioManager.Instance.PlayerDeathAudio(); //播放音效    

            _instance.fader.FadeOut();
            _instance.deathNum++;
            PlayerPrefs.SetInt("DeathNum", deathNum);
            UIManager.Instance.UpdateDeathUI(PlayerPrefs.GetInt("DeathNum"));

            _instance.Invoke("RestartScene", 1.5f);
            AudioManager.Instance.Invoke("PlayStartLevelAudio", 1.5f);
        }

        /// <summary>
        /// 重新加载场景(在PlayDied函数中延时调用
        /// </summary>
        private void RestartScene()
        {
            _instance.orbs.Clear();            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// 主角过关
        /// </summary>
        public void PlayerWon()
        {
            if (SceneManager.GetActiveScene().name == "S03")
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
                AudioManager.Instance.PlayerWonAudio();

                _instance.gameIsOver = true;
                UIManager.Instance.DisplayGameOver(); //显示Game Over
                Invoke("QuitGame", 3f);                
            }
            else
            {
                int deathPosListCount = deathPosList.Count;
                
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
                print(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name));
                AudioManager.Instance.PlayerWonAudio();

                for (int i = 0; i < deathPosListCount; i++)
                {
                    PoolManager.Instance.PutObject(deathPosList[i]);
                }

                deathPosList.Clear();

                UIManager.Instance.LoadNextLevel();
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <returns></returns>
        public bool GameOver()
        {
            return _instance.gameIsOver;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

    /*--------------------------------------------------*/
    }
}