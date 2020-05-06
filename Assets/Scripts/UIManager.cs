using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Robbie
{
	///<summary>
	/// UI管理器
	///</summary>
	public class UIManager : MonoBehaviour
	{
        private static UIManager _instance;

        public static UIManager Instance
        {
            get
            {
                return _instance;
            }
        }

        //Start Panel
        private GameObject startPanel;

        //Game Panel
        public TextMeshProUGUI gameOverText; //屏幕中央显示 Game Over 字体
        private GameObject gamePanel;
        private TextMeshProUGUI orbText;
        private TextMeshProUGUI timeText;
        private TextMeshProUGUI deathText;

        //Pause Panel
        private GameObject pausePanel;
        private Slider volumeSlider;
        public AudioMixer audioMixer;

        //Load Panel
        private GameObject loadPanel;
        private Slider loadingSlider;
        private Text loadingText;

        //select panel
        private GameObject selectPanel;
        private List<CheckSceneLock> checkSceneLocksList;


        /*-------------生命周期------------*/
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            //get loading panel
            startPanel = transform.Find("Start Panel").gameObject;
            loadPanel = transform.Find("Load Panel").gameObject;
            loadingSlider = transform.Find("Load Panel").GetComponentInChildren<Slider>();
            loadingText = transform.Find("Load Panel").GetComponentInChildren<Text>();

            //get game panel
            orbText = transform.Find("GamePanel").Find("Orb UI").GetComponentInChildren<TextMeshProUGUI>();
            timeText = transform.Find("GamePanel").Find("Time UI").GetComponentInChildren<TextMeshProUGUI>();
            deathText = transform.Find("GamePanel").Find("Deaths UI").GetComponentInChildren<TextMeshProUGUI>();

            //get pause panel
            pausePanel = transform.Find("PausePanel").gameObject;
            volumeSlider = pausePanel.transform.GetComponentInChildren<Slider>();

            //get select panel
            selectPanel = transform.Find("Select Panel").gameObject;
            checkSceneLocksList = new List<CheckSceneLock>(4);
            selectPanel.transform.GetComponentsInChildren<CheckSceneLock>(checkSceneLocksList);
            

            //get game panel
            gamePanel = transform.Find("GamePanel").gameObject;
        }
        /*------------End-生命周期------------*/


        /*------Game Panel-------*/
        /// <summary>
        /// 更新宝珠数量UI
        /// </summary>
        /// <param name="orbCount">宝珠数量</param>
        public void UpdateOrbUI(int orbCount)
        {
            _instance.orbText.text = orbCount.ToString();
        }

        /// <summary>
        /// 更新死亡数UI
        /// </summary>
        /// <param name="deathNum"></param>
        public void UpdateDeathUI(int deathNum)
        {
            _instance.deathText.text = deathNum.ToString();
        }

        /// <summary>
        /// 更新游戏时间UI
        /// </summary>
        /// <param name="time"></param>
        public void UpdateTimeUI(float time)
        {
            int minutes = (int)(time / 60);
            float seconds = time % 60;

            _instance.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        /// <summary>
        /// 显示游戏结束
        /// </summary>
        public void DisplayGameOver()
        {
            _instance.gameOverText.enabled = true;
        }

        /// <summary>
        /// 显示游戏面板
        /// </summary>
        public void ShowGamePanel()
        {
            gamePanel.SetActive(true);            
        }

        /// <summary>
        /// 隐藏游戏面板
        /// </summary>
        public void HideGamePanel()
        {
            gamePanel.SetActive(false);
        }
        /*-----END-Game Panel-------*/


        /*----Pause Panel----*/
        /// <summary>
        /// 退出游戏Button
        /// </summary>
        public void QuitGameButton()
        {
            Application.Quit();
        }

        /// <summary>
        /// 窗口化Button    
        /// </summary>
        public void NoFullResolutionButton()
        {
            Screen.SetResolution(1280, 1024, false);
        }

        /// <summary>
        /// 全屏Button
        /// </summary>
        public void FullResolutionButton()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        /// <summary>
        /// 游戏暂停，显示设置面板Button
        /// </summary>
        public void PauseGameAndShowPanelButton()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        /// <summary>
        /// 隐藏pause面板
        /// </summary>
        public void HidePausePanel()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        /// <summary>
        /// 继续游戏 隐藏PausePanel button
        /// </summary>
        public void ContinueGameButton()
        {            
            Time.timeScale = 1f;
            HidePausePanel();
        }

        /// <summary>
        /// slider更改游戏声音大小
        /// </summary>
        public void SetVolume()
        {
            audioMixer.SetFloat("MainVolume", volumeSlider.value);
        }

        /// <summary>
        /// 返回主界面button
        /// </summary>
        public void BackStartScene()
        {
            GameManager.Instance.orbs.Clear();
            SceneManager.LoadScene("S00");
            ShowStartPanel();
            HidePausePanel();
            HideGamePanel();            
        }
        /*---END-Pause Panel----*/


        /*----开始面板----*/
        public void ShowStartPanel()
        {
            startPanel.SetActive(true);
        }

        public void HideStartPanel()
        {
            startPanel.SetActive(false);
        }

        public void NewGameButton()
        {            
            PlayerPrefs.SetInt("S01", 0);
            PlayerPrefs.SetInt("S02", 0);
            PlayerPrefs.SetInt("S03", 0);
            LoadNextLevel();
        }
        /*---END-开始面板----*/


        /*----加载面板----*/
        public void ShowLoadPanel()
        {
            loadPanel.SetActive(true);
        }

        public void HideLoadPanel()
        {
            loadPanel.SetActive(false);
        }

        public void DeletePlayerPrefs()
        {
            PlayerPrefs.SetInt("S01", 0);
            PlayerPrefs.SetInt("S02", 0);
            PlayerPrefs.SetInt("S03", 0);
            PlayerPrefs.SetInt("S04", 0);
        }
        /*---END-加载面板----*/


        /*----选关面板----*/
        /// <summary>
        /// 显示选关面板
        /// </summary>
        public void ShowSelectPanel()
        {
            selectPanel.SetActive(true);
            for (int i = 0; i < checkSceneLocksList.Count; i++)
            {
                checkSceneLocksList[i].CheckScene();
            }
        }

        /// <summary>
        /// 隐藏选关面板 button
        /// </summary>
        public void HideSelectPanel()
        {
            selectPanel.SetActive(false);            
        }
        /*---END-选关面板----*/


        /// <summary>
        /// 加载下一关
        /// </summary>
        public void LoadNextLevel()
        {
            HideStartPanel();
            HideGamePanel();
            StartCoroutine(LoadNextLevel_IEnumerator());            
        }

        /// <summary>
        /// 迭代器：加载下一关
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadNextLevel_IEnumerator()
        {
            ShowLoadPanel();

            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

            operation.allowSceneActivation = false; //场景加载完 不 直接跳转

            while (!operation.isDone)
            {
                loadingSlider.value = operation.progress;
                loadingText.text = loadingSlider.value * 100 + "%";

                if (operation.progress >= 0.9f)
                {
                    loadingSlider.value = 1.0f;
                    loadingText.text = "输入任意按键继续";

                    if (Input.anyKeyDown)
                    {
                        operation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }

            HideLoadPanel();
            ShowGamePanel();
            AudioManager.Instance.PlayStartLevelAudio();
        }

        /// <summary>
        /// 加载用户选择的关卡
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadSelectLevel(string sceneName)
        {
            HideSelectPanel();
            HideStartPanel();

            StartCoroutine(LoadSelectLevel_IEnumerator(sceneName));            
        }

        /// <summary>
        /// 迭代器：加载用户选择的关卡
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadSelectLevel_IEnumerator(string sceneName)
        {
            ShowLoadPanel();

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false; //场景加载完 不 直接跳转

            while (!operation.isDone)
            {
                loadingSlider.value = operation.progress;
                loadingText.text = loadingSlider.value * 100 + "%";

                if (operation.progress >= 0.9f)
                {
                    loadingSlider.value = 1.0f;
                    loadingText.text = "输入任意按键继续";

                    if (Input.anyKeyDown)
                    {
                        operation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }

            HideLoadPanel();
            ShowGamePanel();
            AudioManager.Instance.PlayStartLevelAudio();
        }
        /*--------------------------------*/
    }
}