using System;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound
{
    public class Music
    {
        #region Fields

        private SourcePoolElement _sourcePoolElement;

        private float _volume = 1;
        private Vector2 _hearDistance = new Vector2(3, 500);
        private float _pitch = 1;
        private Vector2 _pitchRange = new Vector2(0.85f, 1.15f);
        private string _id;
        private Vector3 _position = Vector3.zero;
        private Transform _followTarget;
        private bool _loop;
        private bool _spatialSound;
        private float _fadeOutTime;
        private bool _randomClip = true;
        private bool _forgetSourcePoolOnStop;
        private AudioClip _clip;
        private AudioMixerGroup _output;
        private string _cachedSoundTag;

        #endregion

        #region Fields (Callbacks)

        private Action onPlay;
        private Action onComplete;
        private Action onLoopCycleComplete;
        private Action onPause;
        private Action onPauseComplete;
        private Action onResume;

        #endregion

        #region Properties

        /// <summary>It's true when it's being used. When it's paused, it's true as well</summary>
        public bool Using => _sourcePoolElement != null;
        /// <summary>It's true when audio is playing.</summary>
        public bool Playing => Using && _sourcePoolElement.Playing;
        /// <summary>It's true when audio paused (it ignore the fade out time).</summary>
        public bool Paused => Using && _sourcePoolElement.Paused;
        /// <summary>Volume level between [0,1].</summary>
        public float Volume => _volume;
        /// <summary>Total time in seconds that it have been playing.</summary>
        public float PlayingTime => Using ? _sourcePoolElement.PlayingTime : 0;
        /// <summary>Reproduced time in seconds of current loop cycle.</summary>
        public float CurrentLoopCycleTime => Using ? _sourcePoolElement.CurrentLoopCycleTime : 0;
        /// <summary>Times it has looped.</summary>
        public int CompletedLoopCycles => Using ? _sourcePoolElement.CompletedLoopCycles : 0;
        /// <summary>Duration in seconds of matched clip.</summary>
        public float ClipDuration => _clip != null ? _clip.length : 0;
        /// <summary>Matched clip.</summary>
        public AudioClip Clip => _clip;

        #endregion

        #region Public Methods

        /// <summary>
        /// Create new Music object given a Track.
        /// </summary>
        /// <param name="track">Music track you've created before on Audio Creator window</param>
        public Music (Track track)
        {
            SetClip(track.ToString());
        }
        
        /// <summary>
        /// Create new Music object given a tag.
        /// </summary>
        /// <param name="tag">The tag you've used to create the sound on Audio Creator</param>
        public Music (string tag)
        {
            SetClip(tag);
        }
        
        /// <summary>
        /// Store volume parameters BEFORE play music.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        public Music SetVolume (float volume)
        {
            _volume = volume;
            return this;
        }
        
        /// <summary>
        /// Set volume parameters BEFORE play music.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        /// <param name="hearDistance">Distance range to hear music</param>
        public Music SetVolume (float volume, Vector2 hearDistance)
        {
            _volume = volume;
            _hearDistance = hearDistance;
            return this;
        }
        
        /// <summary>
        /// Change volume while music is reproducing.
        /// </summary>
        /// <param name="newVolume">New volume: min 0, Max 1</param>
        /// <param name="lerpTime">Time to lerp current to new volume</param>
        public void ChangeVolume (float newVolume, float lerpTime = 0)
        {
            if (Mathf.Approximately(_volume, newVolume)) return;
            
            _volume = newVolume;
            
            if (!Using) return;
            
            _sourcePoolElement.SetVolume(newVolume, _hearDistance, lerpTime);
        }
        
        /// <summary>
        /// Set given pitch. Make your music sound different :)
        /// </summary>
        public Music SetPitch (float pitch)
        {
            _pitch = pitch;
            return this;
        }

        /// <summary>
        /// Set an id to identify this music on AudioManager static methods.
        /// </summary>
        public Music SetId (string id)
        {
            _id = id;
            return this;
        }
        
        /// <summary>
        /// Make your music loops for infinite time. If you need to stop it, use Stop() method.
        /// </summary>
        public Music SetLoop (bool loop)
        {
            _loop = loop;
            return this;
        }
        
        /// <summary>
        /// Change the AudioClip of this Music.
        /// </summary>
        /// <param name="tag">The tag you've used to save the music track on Audio Creator window</param>
        public Music SetClip (string tag)
        {
            _cachedSoundTag = tag;
            _clip = AudioManager.GetTrack(tag);
            return this;
        }
        
        /// <summary>
        /// Set a new track BEFORE play it.
        /// </summary>
        /// <param name="track">Music track you've created before on Audio Creator window</param>
        public Music SetClip (Track track)
        {
            SetClip(track.ToString());
            return this;
        }
        
        /// <summary>
        /// Make the music clip change with each new Play().
        /// A random clip from those you have added together in the Audio Creator will be played.
        /// </summary>
        /// <param name="random">Use random clip</param>
        public Music SetRandomClip (bool random)
        {
            _randomClip = random;
            return this;
        }

        /// <summary>
        /// Set the position of the sound emitter.
        /// </summary>
        public Music SetPosition (Vector3 position)
        {
            _position = position;
            return this;
        }
        
        /// <summary>
        /// Set a target to follow. Audio source will update its position every frame.
        /// </summary>
        /// <param name="followTarget">Transform to follow</param>
        public Music SetFollowTarget (Transform followTarget)
        {
            _followTarget = followTarget;
            return this;
        }

        /// <summary>
        /// Set spatial sound.
        /// </summary>
        /// <param name="true">Your sound will be 3D</param>
        /// <param name="false">Your sound will be global / 2D</param>
        /// <param name="activate"></param>
        public Music SetSpatialSound (bool activate = true)
        {
            _spatialSound = activate;
            return this;
        }
        
        /// <summary>
        /// Set fade out duration. It'll be used when music ends.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public Music SetFadeOut (float fadeOutTime)
        {
            _fadeOutTime = fadeOutTime;
            return this;
        }
        
        /// <summary>
        /// Set the audio output to manage the volume using the Audio Mixers.
        /// </summary>
        /// <param name="output">Output you've created before inside Master AudioMixer
        /// (Remember reload the outputs database on Output Manager Window)</param>
        public Music SetOutput (Output output)
        {
            _output = AudioManager.GetOutput(output);
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music start playing.
        /// </summary>
        /// <param name="onPlayCallback">Method will be invoked</param>
        public Music OnPlay (Action onPlayCallback)
        {
            onPlay = onPlayCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music complete.
        /// If "loop" is active, it'll be called when you Stop the music manually.
        /// </summary>
        /// <param name="onCompleteCallback">Method will be invoked</param>
        public Music OnComplete (Action onCompleteCallback)
        {
            onComplete = onCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on loop cycle complete.
        /// You need to set loop on true to use it.
        /// </summary>
        /// <param name="onLoopCycleCompleteCallback">Method will be invoked</param>
        public Music OnLoopCycleComplete (Action onLoopCycleCompleteCallback)
        {
            onLoopCycleComplete = onLoopCycleCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music pause.
        /// It will ignore the fade out time.
        /// </summary>
        /// <param name="onPauseCallback">Method will be invoked</param>
        public Music OnPause (Action onPauseCallback)
        {
            onPause = onPauseCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music pause and fade out ends.
        /// </summary>
        /// <param name="onPauseCompleteCallback">Method will be invoked</param>
        public Music OnPauseComplete (Action onPauseCompleteCallback)
        {
            onPauseComplete = onPauseCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on resume/unpause music.
        /// </summary>
        /// <param name="onResumeCallback">Method will be invoked</param>
        public Music OnResume (Action onResumeCallback)
        {
            onResume = onResumeCallback;
            return this;
        }

        /// <summary>
        /// Reproduce music.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Play (float fadeInTime = 0)
        {
            if (Using && Playing)
            {
                Stop();
                _forgetSourcePoolOnStop = true;
            }

            if (_randomClip) SetClip(_cachedSoundTag);
            
            _sourcePoolElement = AudioManager.GetSource();
            _sourcePoolElement
                .SetVolume(_volume, _hearDistance)
                .SetPitch(_pitch)
                .SetLoop(_loop)
                .SetClip(_clip)
                .SetPosition(_position)
                .SetFollowTarget(_followTarget)
                .SetSpatialSound(_spatialSound)
                .SetFadeOut(_fadeOutTime)
                .SetId(_id)
                .SetOutput(_output)
                .OnPlay(onPlay)
                .OnComplete(onComplete)
                .OnLoopCycleComplete(onLoopCycleComplete)
                .OnPause(onPause)
                .OnPauseComplete(onPauseComplete)
                .OnResume(onResume)
                .Play(fadeInTime);
        }

        /// <summary>
        /// Pause music.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before pause</param>
        public void Pause (float fadeOutTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Pause(fadeOutTime);
        }

        /// <summary>
        /// Resume/Unpause music.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Resume (float fadeInTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Resume(fadeInTime);
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before stop</param>
        public void Stop (float fadeOutTime = 0)
        {
            if (!Using) return;
            
            if (_forgetSourcePoolOnStop)
            {
                _sourcePoolElement.Stop(fadeOutTime);
                _sourcePoolElement = null;
                _forgetSourcePoolOnStop = false;
                return;
            }
            _sourcePoolElement.Stop(fadeOutTime, () => _sourcePoolElement = null);
        }

        #endregion
    }
}