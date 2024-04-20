using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound
{
    public class SourcePoolElement : MonoBehaviour
    {
        #region Fields

        private AudioSource _source;
        private Transform _followTarget;
        private float _volume = 1;
        private Vector2 _hearDistance = new Vector2(3, 500);
        private float _fadeOutTime;
        private float _fadeInTime;
        private bool _loop;
        private bool _stopping;
        private bool _changingTrack;
        private bool _isPlaylist;
        private Queue<AudioClip> _playlist = new Queue<AudioClip>();
        private float _playingTimeForNextSong;
        private float _playlistDuration;
        private AudioClip _cachedClipOnPause;
        
        private Coroutine _lerpVolumeCor;
        private Coroutine _fadeInOnChangeTrackCor;

        private SourceState _currentState = SourceState.Stopped;
        
        private enum SourceState
        {
            Playing,
            Paused,
            Pausing,
            FadingIn,
            ChangingTrack,
            Stopping,
            Stopped
        }

        #endregion

        #region Fields Callbacks

        private Action onPlay;
        private Action onComplete;
        private Action onLoopCycleComplete;
        private Action onNextTrackStart;
        private Action onPause;
        private Action onPauseComplete;
        private Action onResume;

        #endregion

        #region Properties
        internal bool Using { get; private set; }
        internal bool Playing => _source.isPlaying;
        internal bool Paused { get; private set; }
        internal string Id { get; private set; }
        internal float PlayingTime { get; private set; }
        internal float CurrentLoopCycleTime => _source.time;
        internal int CompletedLoopCycles { get; private set; }
        internal int ReproducedTracks { get; private set; }
        internal float CurrentClipDuration => _source.clip.length;
        internal AudioClip CurrentClip => _source.clip;
        internal AudioClip NextPlaylistClip => _playlist.Peek();

        #endregion

        private void Update ()
        {
            if (!Using) return;

            if (!_loop)
            {
                if (!_isPlaylist) HandleSoundStop();
                else HandlePlaylistStop();
            }

            if (Playing)
            {
                if (!_isPlaylist) HandleSoundPlaying();
                else HandlePlaylistPlaying();
            }

            if (_followTarget == null) return;
            
            transform.position = _followTarget.position;
        }

        #region Internal Methods

        internal SourcePoolElement Init (AudioSource source)
        {
            _source = source;
            return this;
        }

        internal SourcePoolElement MarkAsPlaylist ()
        {
            _isPlaylist = true;
            return this;
        }
        
        internal SourcePoolElement SetVolume (float volume, Vector2 hearDistance, float lerpTime = 0)
        {
            _volume = volume;
            _hearDistance = hearDistance;
            _source.minDistance = hearDistance.x;
            _source.maxDistance = hearDistance.y;
            
            if (_currentState == SourceState.Paused || _currentState == SourceState.Stopping ||
                _currentState == SourceState.ChangingTrack)
            {
                Debug.LogWarning("There's a volume fade out taking place at this moment, " +
                                 "so volume won't change right now, but on the next fade in it will go " +
                                 $"up until the new volume of {volume}");
                return this;
            }

            if (lerpTime <= 0) _source.volume = volume;
            else
            {
                _lerpVolumeCor = StartCoroutine(LerpVolume(volume, lerpTime));
            }
            return this;
        }

        internal SourcePoolElement SetPitch (float pitch)
        {
            _source.pitch = pitch;
            return this;
        }

        internal SourcePoolElement SetClip (AudioClip audioClip)
        {
            _source.clip = audioClip;
            return this;
        }
        
        internal SourcePoolElement SetPlaylist (Queue<AudioClip> playlist)
        {
            _playlist = new Queue<AudioClip>(playlist);
            _playlistDuration = 0;
            foreach (AudioClip clip in playlist) _playlistDuration += clip != null ? clip.length : 0;
            return this;
        }
        
        internal void AddToPlaylist (AudioClip addedClip)
        {
            _playlistDuration += addedClip.length;
            _playlist.Enqueue(addedClip);
        }

        internal SourcePoolElement SetId (string id)
        {
            Id = id;
            return this;
        }

        internal SourcePoolElement SetSpatialSound (bool activate)
        {
            _source.spatialBlend = activate ? 1 : 0;
            return this;
        }
        
        internal SourcePoolElement SetPosition (Vector3 position)
        {
            transform.position = position;
            return this;
        }
        
        internal SourcePoolElement SetFollowTarget (Transform followTarget)
        {
            _followTarget = followTarget;
            return this;
        }
        
        internal SourcePoolElement SetFadeIn (float fadeInTime)
        {
            _fadeInTime = fadeInTime;
            return this;
        }
        
        internal SourcePoolElement SetFadeOut (float fadeOutTime)
        {
            _fadeOutTime = fadeOutTime;
            return this;
        }
        
        internal SourcePoolElement SetLoop (bool loop)
        {
            _loop = loop;
            _source.loop = loop;
            return this;
        }

        internal SourcePoolElement SetOutput (AudioMixerGroup output)
        {
            _source.outputAudioMixerGroup = output;
            return this;
        }

        internal SourcePoolElement OnPlay (Action onPlayCallback)
        {
            onPlay = onPlayCallback;
            return this;
        }
        
        internal SourcePoolElement OnComplete (Action onCompleteCallback)
        {
            onComplete = onCompleteCallback;
            return this;
        }
        
        internal SourcePoolElement OnLoopCycleComplete (Action onLoopCycleCompleteCallback)
        {
            onLoopCycleComplete = onLoopCycleCompleteCallback;
            return this;
        }
        
        internal SourcePoolElement OnNextTrackStart (Action onNextTrackStartCallback)
        {
            onNextTrackStart = onNextTrackStartCallback;
            return this;
        }
        
        internal SourcePoolElement OnPause (Action onPauseCallback)
        {
            onPause = onPauseCallback;
            return this;
        }
        
        internal SourcePoolElement OnPauseComplete (Action onPauseCompleteCallback)
        {
            onPauseComplete = onPauseCompleteCallback;
            return this;
        }
        
        internal SourcePoolElement OnResume (Action onResumeCallback)
        {
            onResume = onResumeCallback;
            return this;
        }
        
        internal void Play (float fadeInTime = 0)
        {
            if (_source.clip == null)
            {
                Debug.LogError("No audio clip found, make sure you have initialized it in a method (not in the declaration)");
                return;
            }
            
            Using = true;
            Paused = false;
            PlayingTime = 0;
            CompletedLoopCycles = 0;
            
            onPlay?.Invoke();
            
            _source.Play();
            ChangeState(SourceState.Playing);
            enabled = true;
            
            if (fadeInTime > 0)
            {
                ChangeState(SourceState.FadingIn);
                _source.volume = 0;
                _lerpVolumeCor = StartCoroutine(LerpVolume(_volume, fadeInTime,
                    (() => ChangeState(SourceState.Playing))));
            }
        }
        
        internal void PlayPlaylist (float fadeInTime)
        {
            bool validClips = true;
            for (int i = 0; i < _playlist.Count; i++)
            {
                if (_playlist.ToArray()[i] != null) continue;
                validClips = false;
            }
            if (!validClips)
            {
                Debug.LogError("There are invalid audio clips in playlist, make sure you have " +
                                 "initialized it in a method (not in the declaration)");
                return;
            }
            
            Using = true;
            Paused = false;
            PlayingTime = 0;
            ReproducedTracks = 0;
            CompletedLoopCycles = 0;
            _playingTimeForNextSong = 0;
            if (_loop) _source.loop = false;
            _changingTrack = false;

            PlayNextSong();
            enabled = true;
            
            if (fadeInTime > 0)
            {
                ChangeState(SourceState.FadingIn);
                _source.volume = 0;
                _lerpVolumeCor = StartCoroutine(LerpVolume(_volume, fadeInTime,
                    (() => ChangeState(SourceState.Playing))));
            }
        }

        internal void Pause (float fadeOutTime = 0)
        {
            if (!Using) return;
            if (Paused) return;
            
            Paused = true;
            _cachedClipOnPause = CurrentClip;
            
            onPause?.Invoke();

            void CompletePause ()
            {
                onPauseComplete?.Invoke();
                _source.Pause();
                ChangeState(SourceState.Paused);
            }

            if (_changingTrack)
            {
                CompletePause();
                return;
            }
            
            if (fadeOutTime > 0)
            {
                if (_currentState == SourceState.FadingIn)
                {
                    StopCoroutine(_fadeInOnChangeTrackCor);
                    _fadeInOnChangeTrackCor = null;
                }
                StopLerpCoroutine();
                ChangeState(SourceState.Pausing);
                _lerpVolumeCor = StartCoroutine(LerpVolume(0, fadeOutTime, CompletePause, true));
                return;
            }
            CompletePause();
        }
        
        internal void Resume (float fadeInTime = 0)
        {
            if (!Paused) return;

            onResume?.Invoke();
            
            Paused = false;
            _source.UnPause();
            ChangeState(SourceState.Playing);

            if (_changingTrack) return;

            if (fadeInTime > 0)
            {
                StopLerpCoroutine();
                ChangeState(SourceState.FadingIn);
                _lerpVolumeCor = StartCoroutine(LerpVolume(_volume, fadeInTime,
                    (() => ChangeState(SourceState.Playing))));
            }
        }
        
        internal void Stop (float fadeOutTime = 0, Action onStop = null)
        {
            if (fadeOutTime > 0)
            {
                _stopping = true;
                ChangeState(SourceState.Stopping);
                _lerpVolumeCor = StartCoroutine(LerpVolume(0, fadeOutTime, () => Stop(0, onStop)));
                return;
            }
            
            onComplete?.Invoke();
            onStop?.Invoke();
            
            _source.Stop();
            ChangeState(SourceState.Stopped);
            _source.clip = null;
            
            _followTarget = null;
            
            onComplete = null;
            onLoopCycleComplete = null;
            onPause = null;
            onPauseComplete = null;
            onResume = null;

            Id = null;

            _changingTrack = false;
            _stopping = false;
            Using = false;
            enabled = false;
        }

        #endregion

        #region Private Methods

        private void HandleSoundPlaying ()
        {
            PlayingTime += Time.deltaTime;

            if (!_loop) return;
            if (PlayingTime > CurrentClipDuration * CompletedLoopCycles + 1) return;
            
            CompletedLoopCycles++;
                
            onLoopCycleComplete?.Invoke();
        }

        private void HandlePlaylistPlaying ()
        {
            PlayingTime += Time.deltaTime;
            
            void CompleteLoopCycle ()
            {
                CompletedLoopCycles++;
                
                onLoopCycleComplete?.Invoke();
            }

            if (_loop)
            {
                if (CurrentLoopCycleTime >= _playlistDuration - _fadeOutTime)
                {
                    CompleteLoopCycle();
                }
            }

            if (PlayingTime >= _playingTimeForNextSong - _fadeOutTime && !_changingTrack)
            {
                _changingTrack = true;
                ChangeState(SourceState.ChangingTrack);
                if (_fadeOutTime > 0) 
                    _lerpVolumeCor = StartCoroutine(LerpVolume(0, _fadeOutTime, () => PlayNextSong()));
                else PlayNextSong();
            }
        }

        private void HandleSoundStop ()
        {
            if (_stopping) return;
            
            if (_fadeOutTime > 0)
            {
                if (PlayingTime >= CurrentClipDuration - 0.05f - _fadeOutTime)
                {
                    Stop(_fadeOutTime);
                }
            }

            if (!Playing && !Paused)
            {
                Stop();
            }
        }

        private void HandlePlaylistStop ()
        {
            if (_playlist.Count > 0) return;
            if (_stopping) return;
            
            if (_fadeOutTime > 0)
            {
                if (CurrentLoopCycleTime >= CurrentClipDuration - 0.05f - _fadeOutTime)
                {
                    Stop(_fadeOutTime);
                }
            }

            if (!Playing && !Paused)
            {
                Stop();
            }
        }

        private void PlayNextSong (bool firstTrack = false)
        {
            if (_playlist.Count == 0) return;
            
            _source.clip = _playlist.Dequeue();
            if (_loop) _playlist.Enqueue(_source.clip);
            _playingTimeForNextSong += CurrentClipDuration;
            if (!firstTrack)
            {
                ReproducedTracks++;
                onNextTrackStart?.Invoke();
            }
            _changingTrack = false;
            
            if (Paused) return;
            
            _source.Play();
            if (_fadeInTime > 0)
            {
                ChangeState(SourceState.FadingIn);
                _fadeInOnChangeTrackCor = StartCoroutine(LerpVolume(_volume, _fadeInTime, 
                    () => ChangeState(SourceState.Playing)));
            }
            else
            {
                ChangeState(SourceState.Playing);
                _source.volume = _volume;
            }
        }

        private void StopLerpCoroutine ()
        {
            if (_lerpVolumeCor == null) return;
            StopCoroutine(_lerpVolumeCor);
            _lerpVolumeCor = null;
        }
        
        private void ChangeState (SourceState newState)
        {
            _currentState = newState;
        }

        #endregion

        #region Private Methods (Coroutines)

        private IEnumerator LerpVolume (float newVolume, float lerpTime, Action onFinishLerp = null, bool ignorePause = false)
        {
            float currentVolume = _source.volume;
            for (float t = 0.0f; t < lerpTime; t += Time.deltaTime)
            {
                if (!ignorePause)
                {
                    while (Paused)
                    {
                        yield return null;
                    }
                }

                _source.volume = Mathf.Lerp(currentVolume, newVolume, t / lerpTime);
                yield return null;
            }
            _source.volume = newVolume;

            onFinishLerp?.Invoke();
            
            _lerpVolumeCor = null;
        }

        #endregion
    }
}