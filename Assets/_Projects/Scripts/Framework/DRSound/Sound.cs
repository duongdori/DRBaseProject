using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace DR.DRSound
{
    public class Sound
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
        private bool _spatialSound = true;
        private float _fadeOutTime;
        private bool _randomClip = true;
        private bool _forgetSourcePoolOnStop;
        private AudioClip _clip;
        private AudioMixerGroup _output;
        private string _cachedSoundTag;

        #endregion

        #region Fields Callbacks

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
        /// Create new Sound object given a clip.
        /// </summary>
        /// <param name="sfx">Sound you've created before on Audio Creator window</param>
        public Sound (SFX sfx)
        {
            SetClip(sfx.ToString());
        }
        
        /// <summary>
        /// Create new Sound object given a tag.
        /// </summary>
        /// <param name="tag">The tag you've used to create the sound on Audio Creator window</param>
        public Sound (string tag)
        {
            SetClip(tag);
        }

        /// <summary>
        /// Store volume parameters BEFORE play sound.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        public Sound SetVolume (float volume)
        {
            _volume = volume;
            return this;
        }
        
        /// <summary>
        /// Store volume parameters BEFORE play sound.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        /// <param name="hearDistance">Distance range to hear sound</param>
        public Sound SetVolume (float volume, Vector2 hearDistance)
        {
            _volume = volume;
            _hearDistance = hearDistance;
            return this;
        }

        /// <summary>
        /// Change volume while sound is reproducing.
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
        /// Set given pitch. Make your sounds sound different :)
        /// </summary>
        public Sound SetPitch (float pitch)
        {
            _pitch = pitch;
            return this;
        }
        
        /// <summary>
        /// Set my recommended random pitch. Range is (0.85, 1.15). It's useful to avoid sounds be repetitive.
        /// </summary>
        public Sound SetRandomPitch ()
        {
            _pitch = Random.Range(0.85f, 1.15f);
            return this;
        }
        
        /// <summary>
        /// Set random pitch between given range. It's useful to avoid sounds be repetitive.
        /// </summary>
        /// <param name="pitchRange">Pitch range (min, Max)</param>
        public Sound SetRandomPitch (Vector2 pitchRange)
        {
            _pitch = Random.Range(pitchRange.x, pitchRange.y);
            return this;
        }

        /// <summary>
        /// Set an id to identify this sound on AudioManager static methods.
        /// </summary>
        public Sound SetId (string id)
        {
            _id = id;
            return this;
        }

        /// <summary>
        /// Make your sound loops for infinite time. If you need to stop it, use Stop() method.
        /// </summary>
        public Sound SetLoop (bool loop)
        {
            _loop = loop;
            return this;
        }
        
        /// <summary>
        /// Change the AudioClip of this Sound BEFORE play it.
        /// </summary>
        /// <param name="tag">The tag you've used to create the sound on Audio Creator</param>
        public Sound SetClip (string tag)
        {
            _cachedSoundTag = tag;
            _clip = AudioManager.GetSFX(tag);
            return this;
        }
        
        /// <summary>
        /// Change the AudioClip of this Sound BEFORE play it.
        /// </summary>
        /// <param name="sfx">Sound you've created before on Audio Creator</param>
        public Sound SetClip (SFX sfx)
        {
            SetClip(sfx.ToString());
            return this;
        }
        
        /// <summary>
        /// Make the sound clip change with each new Play().
        /// It'll choose a random sound from those you have added with the same tag in the Audio Creator.
        /// </summary>
        /// <param name="random">Use random clip</param>
        public Sound SetRandomClip (bool random)
        {
            _randomClip = random;
            return this;
        }
        
        /// <summary>
        /// Set the position of the sound emitter.
        /// </summary>
        public Sound SetPosition (Vector3 position)
        {
            _position = position;
            return this;
        }
        
        /// <summary>
        /// Set a target to follow. Audio source will update its position every frame.
        /// </summary>
        /// <param name="followTarget">Transform to follow</param>
        public Sound SetFollowTarget (Transform followTarget)
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
        public Sound SetSpatialSound (bool activate = true)
        {
            _spatialSound = activate;
            return this;
        }
        
        /// <summary>
        /// Set fade out duration. It'll be used when sound ends.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public Sound SetFadeOut (float fadeOutTime)
        {
            _fadeOutTime = fadeOutTime;
            return this;
        }
        
        /// <summary>
        /// Set the audio output to manage the volume using the Audio Mixers.
        /// </summary>
        /// <param name="output">Output you've created before inside Master AudioMixer
        /// (Remember reload the outputs database on Output Manager Window)</param>
        public Sound SetOutput (Output output)
        {
            _output = AudioManager.GetOutput(output);
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on sound start playing.
        /// </summary>
        /// <param name="onPlayCallback">Method will be invoked</param>
        public Sound OnPlay (Action onPlayCallback)
        {
            onPlay = onPlayCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on sound complete.
        /// If "loop" is active, it'll be called when you Stop the sound manually.
        /// </summary>
        /// <param name="onCompleteCallback">Method will be invoked</param>
        public Sound OnComplete (Action onCompleteCallback)
        {
            onComplete = onCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on loop cycle complete.
        /// You need to set loop on true to use it.
        /// </summary>
        /// <param name="onLoopCycleCompleteCallback">Method will be invoked</param>
        public Sound OnLoopCycleComplete (Action onLoopCycleCompleteCallback)
        {
            onLoopCycleComplete = onLoopCycleCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on sound pause.
        /// It will ignore the fade out time.
        /// </summary>
        /// <param name="onPauseCallback">Method will be invoked</param>
        public Sound OnPause (Action onPauseCallback)
        {
            onPause = onPauseCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on sound pause and fade out ends.
        /// </summary>
        /// <param name="onPauseCompleteCallback">Method will be invoked</param>
        public Sound OnPauseComplete (Action onPauseCompleteCallback)
        {
            onPauseComplete = onPauseCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on resume/unpause sound.
        /// </summary>
        /// <param name="onResumeCallback">Method will be invoked</param>
        public Sound OnResume (Action onResumeCallback)
        {
            this.onResume = onResumeCallback;
            return this;
        }

        /// <summary>
        /// Reproduce sound.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Play (float fadeInTime = 0)
        {
            if (Using && Playing && _loop)
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
        /// Pause sound.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before pause</param>
        public void Pause (float fadeOutTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Pause(fadeOutTime);
        }

        /// <summary>
        /// Resume/Unpause sound.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Resume (float fadeInTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Resume(fadeInTime);
        }

        /// <summary>
        /// Stop sound.
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

