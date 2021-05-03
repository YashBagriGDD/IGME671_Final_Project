using System;
using System.Collections;
using Programmer;
using Sound;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour {
        [SerializeField] private NecessityController[] programmers;
        [SerializeField] private Text timeDisplayer;
        [SerializeField] private float hours;
        [SerializeField] private float actualMinutes;
        [SerializeField] private PanelsSlide gameOverPanel;
        [SerializeField] private PanelsSlide winPanel;
        [SerializeField] private AudioClip mainGameAudio;
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private Interference interference;

        private float _timeMultiplier;
        private float _minutesRemaining;
        private bool _gameOver;

        /* Variables for Audio
        public static GameController Instance;
        private static FMOD.Studio.EventInstance MenuMusic;
        private static FMOD.Studio.EventInstance GameMusic;
        private static FMOD.Studio.EventInstance GameOverMusic;
        private static FMOD.Studio.EventInstance GameAmbience;
        private static FMOD.Studio.EventInstance WhiteNoise;
        private static FMOD.Studio.EventInstance ACNoise;
        */

        private void Awake() {
            _minutesRemaining = hours * 60;
            _timeMultiplier = _minutesRemaining / actualMinutes;
            gameOverPanel.onFinishSlides.AddListener(GoToMenu);
            winPanel.onFinishSlides.AddListener(interference.ShowInterference);
            interference.OnFinishInterferation += GoToMenu;
            foreach (var programmer in programmers) {
                programmer.OnMaxStressLevel += GameOver;
            }

            /*
            //Attach Events to fmod instances
            MenuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Menu");
            GameMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Game_Song");
            GameOverMusic = FMODUnity.RuntimeManager.CreateInstance("event:/BGM/Game_Over");
            GameAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/Game_Ambience");
            WhiteNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/White_Noise");
            ACNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Problem/AC");
            */
        }

        private void Start() {
            //AudioManager.Instance.ChangeClip(mainGameAudio);
        }

        private void Update() {
            if (_gameOver) return;

            float x = 0;
            foreach (var programmer in programmers) {
                if (programmer.StressLevel > x)
                    x = programmer.StressLevel;
            }
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Excitement", x/100);

            var hours = (int)(_minutesRemaining / 60);
            var minutes = (int)(_minutesRemaining % 60);
            timeDisplayer.text = $"{hours}h {minutes}m";
            actualMinutes -= Time.deltaTime / 60;
            _minutesRemaining = _timeMultiplier * actualMinutes;
            if (actualMinutes <= 0) Win();
        }

        private void Win() {
            if (_gameOver) return;
            LevelManager.Instance.NextLevel();
            _gameOver = true;
            winPanel.StartSlides();
            AudioManager.Instance.SoundEffectsMuted = true;
            
            AudioManager.Instance.StopGameMusic();
        }

        private void GameOver() {
            if (_gameOver) return;
            _gameOver = true;

            AudioManager.Instance.StopGameMusic();
            AudioManager.Instance.StartGameOverMusic();

            gameOverPanel.StartSlides();
            AudioManager.Instance.FadeOutClip();
            AudioManager.Instance.PlaySoundWithFade(gameOverSound, 0.5f);
            AudioManager.Instance.SoundEffectsMuted = true;

            
        }


        private void GoToMenu() {
            AudioManager.Instance.SoundEffectsMuted = false;
            AudioManager.Instance.StopGameMusic();
            SceneChanger.Instance.ChangeScene(0);
        }

        /*
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
        */
    }
}