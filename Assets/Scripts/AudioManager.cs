using UnityEngine;
using UnityEngine.Audio;

namespace Robbie
{
	///<summary>
	/// 声音管理器
	///</summary>
	public class AudioManager : MonoBehaviour
	{
        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                return _instance;
            }
        }

        [Header("环境声音")]
        public AudioClip ambientClip;       //环境音乐
        public AudioClip musicClip;         //背景音乐

        [Header("FX音效")]
        public AudioClip deathFXClip;       //死亡后归还宝珠的音效
        public AudioClip orbFXClip;         //拾起宝珠的音效
        public AudioClip doorFXClip;        //门开启音效
        public AudioClip startLevelClip;    //游戏开始的音效
        public AudioClip winClip;           //过关音效

        [Header("Robbie音效")]
        public AudioClip[] walkStepClips;   //主角走路时的音效，随机播放4种之1
        public AudioClip[] crouchStepClips; //下蹲时走路音效，随机播放4种之1
        public AudioClip jumpClip;          //跳跃音效
        public AudioClip deathClip;         //死亡音效

        [Header("Robbie说话的声音")]
        public AudioClip jumpVoiceClip;     //跳跃时Robbie的发声
        public AudioClip deathVoiceClip;    //死亡时Robbie的发声
        public AudioClip orbVoiceClip;      //Robbie拾起宝珠的发声

        //AudioSource 音源
        private AudioSource ambientSource;  //环境音源
        private AudioSource musicSource;    //背景音乐 音源
        private AudioSource fxSource;       //特殊音源(如门开启
        private AudioSource playerSource;   //Robbie音效 音源
        private AudioSource voiceSource;    //Robbie说话 音源

        //group
        public AudioMixerGroup ambientGroup, musicGroup, FXGroup, playerGroup, voiceGroup;


        /*-------------生命周期------------*/
        private void Awake()
        {
            if(_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            DontDestroyOnLoad(gameObject); //切换场景，此对象不被销毁

            ambientSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            fxSource = gameObject.AddComponent<AudioSource>();
            playerSource = gameObject.AddComponent<AudioSource>();
            voiceSource = gameObject.AddComponent<AudioSource>();

            ambientSource.outputAudioMixerGroup = ambientGroup;
            musicSource.outputAudioMixerGroup = musicGroup;
            fxSource.outputAudioMixerGroup = FXGroup;
            playerSource.outputAudioMixerGroup = playerGroup;
            voiceSource.outputAudioMixerGroup = voiceGroup;            
        }
        /*------------END-生命周期------------*/


        /// <summary>
        /// 播放游戏开始音效，背景音乐，环境音效
        /// </summary>
        public void PlayStartLevelAudio()
        {
            _instance.ambientSource.clip = _instance.ambientClip;
            _instance.ambientSource.loop = true; //循环播放
            _instance.ambientSource.Play();

            _instance.musicSource.clip = _instance.musicClip;
            _instance.musicSource.loop = true; //循环播放
            _instance.musicSource.Play();

            _instance.fxSource.clip = _instance.startLevelClip;
            _instance.fxSource.Play();            
        }

        /// <summary>
        /// 播放过关音效
        /// </summary>
        public void PlayerWonAudio()
        {
            _instance.fxSource.clip = _instance.winClip;
            _instance.fxSource.Play();
            _instance.playerSource.Stop();
        }

        /// <summary>
        /// 播放开门的音效
        /// </summary>
        public void PlayDoorOpenAudio()
        {
            _instance.fxSource.clip = _instance.doorFXClip;
            _instance.fxSource.PlayDelayed(1f);
        }

        /// <summary>
        /// 播放Robbie走路时的音效
        /// </summary>
        public void PlayerFootstepAudio()
        {
            int index = Random.Range(0, _instance.walkStepClips.Length);

            _instance.playerSource.clip = _instance.walkStepClips[index];
            _instance.playerSource.Play();
        }

        /// <summary>
        /// 播放Robbie下蹲走路时的音效
        /// </summary>
        public void PlayerCrouchFootstepAudio()
        {
            int index = Random.Range(0, _instance.crouchStepClips.Length); //随机4选1走路音效

            _instance.playerSource.clip = _instance.crouchStepClips[index];
            _instance.playerSource.Play();
        }

        /// <summary>
        /// Robbie跳跃时播放跳跃音效和跳跃人声
        /// </summary>
        public void PlayerJumpAudio()
        {
            _instance.playerSource.clip = _instance.jumpClip;
            _instance.playerSource.Play();

            _instance.voiceSource.clip = _instance.jumpVoiceClip;
            _instance.voiceSource.Play();
        }

        /// <summary>
        /// 播放robbie死亡音效，死亡人声，死亡特殊音效
        /// </summary>
        public void PlayerDeathAudio()
        {
            _instance.playerSource.clip = _instance.deathClip;
            _instance.playerSource.Play();

            _instance.voiceSource.clip = _instance.deathVoiceClip;
            _instance.voiceSource.Play();

            _instance.fxSource.clip = _instance.deathFXClip;
            _instance.fxSource.Play();
        }

        /// <summary>
        /// 播放拾起宝珠的音效
        /// </summary>
        public void PlayerOrbAudio()
        {
            _instance.fxSource.clip = _instance.orbFXClip;
            _instance.fxSource.Play();

            _instance.voiceSource.clip = _instance.orbVoiceClip;
            _instance.voiceSource.Play();
        }        
    /*--------------------------------------------------------*/
    }
}