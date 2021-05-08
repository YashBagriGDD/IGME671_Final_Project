using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Utils;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager: MonoBehaviour
    {
        [SerializeField] private int audioSourceQuantity;
        [SerializeField] private AudioSourcePooleable audioSourcePrefab;
        [SerializeField] private AudioClip mainMusic;
        [SerializeField] private float fadeTime = 2f;
        [SerializeField] private int sceneNum;

        public static AudioManager Instance;
        public bool Muted { get; private set; }
        public bool SoundEffectsMuted { get; set; }

        private AudioSource _audioSource;
        private ObjectPooler<AudioSourcePooleable> _pooler;

        //Variables for Audio
        //public static GameController Instance;
        private static FMOD.Studio.EventInstance MenuMusic;
        private static FMOD.Studio.EventInstance GameMusic;
        private static FMOD.Studio.EventInstance GameOverMusic;
        private static FMOD.Studio.EventInstance GameAmbience;
        private static FMOD.Studio.EventInstance WhiteNoise;
        private static FMOD.Studio.EventInstance ACNoise;

        private static FMOD.Studio.Bus musicBus;
        private static FMOD.Studio.Bus sfxBus;
        private static FMOD.Studio.Bus interactBus;
        private static FMOD.Studio.Bus ambienceBus;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            
            PoolAudioSources();

            //Attach Events to fmod instances
            MenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Menu");
            GameMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Game_Song");
            GameOverMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Game_Over");
            GameAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Game_Ambience");
            WhiteNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/White_Noise");
            ACNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Problem/AC");

            musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
            sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
            interactBus = FMODUnity.RuntimeManager.GetBus("bus:/Interactables"); 
            ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");

        }

    private void Start()
        {
            //_audioSource.clip = mainMusic;
            //_audioSource.loop = true;
            //_audioSource.Play();

            if (sceneNum == 0) StartMenuMusic();
            else if (sceneNum == 1) StartGameMusic();
        }

        public void PlaySound(AudioClip clip, float volume = 1)
        {
            //if (Muted || SoundEffectsMuted) return;
            //var audioSource = _pooler.GetNextObject();
            //audioSource.SetClip(clip);
            //audioSource.SetVolume(volume);
            //audioSource.StartClip();
        }

        public void PlaySoundWithFade(AudioClip clip, float volume)
        {
            //if (Muted || SoundEffectsMuted) return;
            //var audioSource = _pooler.GetNextObject();
            //audioSource.SetClip(clip);
            //StartCoroutine(AudioFades.FadeIn(audioSource.AudioSource, fadeTime, volume));
        } 
        
        public void PlaySound(AudioClip clip)
        {
            //PlaySound(clip, 1);
        }

        public void Mute()
        {
            //_audioSource.Pause();
            musicBus.setMute(true);
            ambienceBus.setMute(true);
            interactBus.setMute(true);
            Muted = true;
        }

        public void UnMute()
        {
            //_audioSource.Play();
            musicBus.setMute(false);
            ambienceBus.setMute(false);
            interactBus.setMute(true);
            Muted = false;
        }
        
        public void ChangeClip(AudioClip clip)
        {
            //_audioSource.clip = clip;
            //mainMusic = clip;
            //if(!Muted) _audioSource.Play();
        }

        public void FadeOutClip()
        {
            //StartCoroutine(AudioFades.FadeOut(_audioSource, fadeTime));
        }

        public void FadeOutClip(float velocity)
        {
            //StartCoroutine(AudioFades.FadeOut(_audioSource, velocity));
        }

        public void PoolAudioSources()
        {
            //_pooler = new ObjectPooler<AudioSourcePooleable>();
            //_pooler.InstantiateObjects(audioSourceQuantity, audioSourcePrefab, "Audio Sources");
        }

        //FMOD play events
        public void StartMenuMusic() => MenuMusic.start();
        public void StopMenuMusic() => MenuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        public void StartGameMusic() {
            GameMusic.start();
            GameAmbience.start();
            WhiteNoise.start();
            ACNoise.start();
        }

        public void StopGameMusic() {
            GameMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GameAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            WhiteNoise.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ACNoise.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void StartGameOverMusic() => GameOverMusic.start();
    }
}