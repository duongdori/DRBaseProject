using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound
{
    public class DynamicMusic
    {
        #region Fields

        private SourcePoolElement _referenceSourcePoolElement;
        private Dictionary<string, SourcePoolElement> _sourcePoolElementDictionary 
            = new Dictionary<string, SourcePoolElement>();

        private Dictionary<string, float> _volumeDictionary = new Dictionary<string, float>();
        private Dictionary<string, Vector2> _hearDistanceDictionary = new Dictionary<string, Vector2>();
        private Vector2 _defaultHearDistance = new Vector2(3, 500);
        private float _pitch = 1;
        private Vector2 _pitchRange = new Vector2(0.85f, 1.15f);
        private string _id;
        private Vector3 _position = Vector3.zero;
        private Transform _followTarget;
        private bool _loop;
        private bool _spatialSound;
        private float _fadeOutTime;
        private bool _forgetSourcePoolOnStop;
        private AudioClip[] _clips;
        private AudioMixerGroup _output;

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
        public bool Using => _referenceSourcePoolElement != null;
        /// <summary>It's true when audio is playing.</summary>
        public bool Playing => Using && _referenceSourcePoolElement.Playing;
        /// <summary>It's true when audio paused (it ignore the fade out time).</summary>
        public bool Paused => Using && _referenceSourcePoolElement.Paused;
        /// <summary>Total time in seconds that it have been playing.</summary>
        public float PlayingTime => Using ? _referenceSourcePoolElement.PlayingTime : 0;
        /// <summary>Reproduced time in seconds of current loop cycle.</summary>
        public float CurrentLoopCycleTime => Using ? _referenceSourcePoolElement.CurrentLoopCycleTime : 0;
        /// <summary>Times it has looped.</summary>
        public int CompletedLoopCycles => Using ? _referenceSourcePoolElement.CompletedLoopCycles : 0;
        /// <summary>Duration in seconds of matched clip (use the first clip of the array because they should have the same duration).</summary>
        public float ClipDuration => _clips.Length > 0 ? _clips[0].length : 0;
        /// <summary>Matched clip.</summary>
        public AudioClip[] Clips => _clips;

        #endregion

        #region Public Methods

        /// <summary>
        /// Create new Dynamic Music object given a Tracks array.
        /// </summary>
        /// <param name="tracks">Track array with all music tracks that you want to reproduce at the same time</param>
        public DynamicMusic (Track[] tracks)
        {
            _clips = new AudioClip[tracks.Length];
            int i = 0;
            foreach (Track track in tracks)
            {
                _clips[i] = AudioManager.GetTrack(track.ToString());
                _sourcePoolElementDictionary.Add(track.ToString(), null);
                _volumeDictionary.Add(track.ToString(), 0.5f);
                _hearDistanceDictionary.Add(track.ToString(), _defaultHearDistance);
                i++;
            }
        }

        /// <summary>
        /// Create new Dynamic Music object given a tags array.
        /// </summary>
        /// <param name="tag">Track array with all music tracks tags that you want to reproduce at the same time</param>
        /// <param name="tags"></param>
        public DynamicMusic (string[] tags)
        {
            _clips = new AudioClip[tags.Length];
            int i = 0;
            foreach (string tag in tags)
            {
                _clips[i] = AudioManager.GetTrack(tag);
                _sourcePoolElementDictionary.Add(tag, null);
                _volumeDictionary.Add(tag, 0.5f);
                _hearDistanceDictionary.Add(tag, _defaultHearDistance);
                i++;
            }
        }
        
        /// <summary>
        /// Store volume parameters of all tracks BEFORE play Dynamic Music.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        public DynamicMusic SetAllVolumes (float volume)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                _volumeDictionary[tagSourcePair.Key] = volume;
                _hearDistanceDictionary[tagSourcePair.Key] = _defaultHearDistance;
            }
            return this;
        }
        
        /// <summary>
        /// Store volume parameters of all tracks BEFORE play Dynamic Music.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1.</param>
        /// <param name="hearDistance">Distance range to hear music</param>
        public DynamicMusic SetAllVolumes (float volume, Vector2 hearDistance)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                _volumeDictionary[tagSourcePair.Key] = volume;
                _hearDistanceDictionary[tagSourcePair.Key] = hearDistance;
            }
            return this;
        }

        /// <summary>
        /// Store volume parameters of specific track BEFORE play Dynamic Music.
        /// </summary>
        /// /// <param name="track">Track you want modify.</param>
        /// <param name="volume">Volume: min 0, Max 1.</param>
        public DynamicMusic SetTrackVolume (Track track, float volume)
        {
            return SetTrackVolume(track.ToString(), volume);
        }
        
        /// <summary>
        /// Store volume parameters of specific track BEFORE play Dynamic Music.
        /// </summary>
        /// /// <param name="tag">Track you want modify.</param>
        /// <param name="volume">Volume: min 0, Max 1.</param>
        public DynamicMusic SetTrackVolume (string tag, float volume)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                if (!tagSourcePair.Key.Equals(tag)) continue;
                
                _volumeDictionary[tagSourcePair.Key] = volume;
                _hearDistanceDictionary[tagSourcePair.Key] = _defaultHearDistance;
                break;
            }
            return this;
        }
        
        /// <summary>
        /// Set volume parameters BEFORE play music.
        /// </summary>
        /// /// <param name="track">Track you want modify.</param>
        /// <param name="volume">Volume: min 0, Max 1.</param>
        /// <param name="hearDistance">min and Max distance to hear music</param>
        public DynamicMusic SetTrackVolume (Track track, float volume, Vector2 hearDistance)
        {
            return SetTrackVolume(track.ToString(), volume, hearDistance);
        }
        
        /// <summary>
        /// Set volume parameters BEFORE play music.
        /// </summary>
        /// /// <param name="tag">Track you want modify.</param>
        /// <param name="volume">Volume: min 0, Max 1.</param>
        /// <param name="hearDistance">min and Max distance to hear music</param>
        public DynamicMusic SetTrackVolume (string tag, float volume, Vector2 hearDistance)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                if (!tagSourcePair.Key.Equals(tag)) continue;
                
                _volumeDictionary[tagSourcePair.Key] = volume;
                _hearDistanceDictionary[tagSourcePair.Key] = hearDistance;
                break;
            }
            return this;
        }

        /// <summary>
        /// Change all tracks volume while music is reproducing.
        /// </summary>
        /// <param name="newVolume">New volume: min 0, Max 1</param>
        /// <param name="lerpTime">Time to lerp current to new volume</param>
        public void ChangeAllVolumes (float newVolume, float lerpTime = 0)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                if (Mathf.Approximately(_volumeDictionary[tagSourcePair.Key], newVolume)) continue;
                _volumeDictionary[tagSourcePair.Key] = newVolume;
            
                if (!Using) return;
            
                tagSourcePair.Value.SetVolume(newVolume, _hearDistanceDictionary[tagSourcePair.Key], lerpTime);
            }
        }

        /// <summary>
        /// Change volume while music is reproducing.
        /// </summary>
        /// <param name="track">Track you want modify.</param>
        /// <param name="newVolume">New volume: min 0, Max 1.</param>
        /// <param name="lerpTime">Time to lerp current to new volume.</param>
        public void ChangeTrackVolume (Track track, float newVolume, float lerpTime = 0)
        {
            ChangeTrackVolume(track.ToString(), newVolume, lerpTime);
        }

        /// <summary>
        /// Change volume while music is reproducing.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="newVolume">New volume: min 0, Max 1.</param>
        /// <param name="lerpTime">Time to lerp current to new volume.</param>
        public void ChangeTrackVolume (string tag, float newVolume, float lerpTime = 0)
        {
            foreach (var tagSourcePair in _sourcePoolElementDictionary)
            {
                if (!tagSourcePair.Key.Equals(tag)) continue;
                
                if (Mathf.Approximately(_volumeDictionary[tagSourcePair.Key], newVolume)) return;
                _volumeDictionary[tagSourcePair.Key] = newVolume;
            
                if (!Using) return;
            
                tagSourcePair.Value.SetVolume(newVolume, _hearDistanceDictionary[tagSourcePair.Key], lerpTime);
                return;
            }
        }

        /// <summary>
        /// Set given pitch. Make your music sound different :)
        /// </summary>
        public DynamicMusic SetPitch (float pitch)
        {
            _pitch = pitch;
            return this;
        }

        /// <summary>
        /// Set an id to identify this music on AudioManager static methods.
        /// </summary>
        public DynamicMusic SetId (string id)
        {
            _id = id;
            return this;
        }
        
        /// <summary>
        /// Make your music loops for infinite time. If you need to stop it, use Stop() method.
        /// </summary>
        public DynamicMusic SetLoop (bool loop)
        {
            _loop = loop;
            return this;
        }

        /// <summary>
        /// Set the position of the sound emitter.
        /// </summary>
        public DynamicMusic SetPosition (Vector3 position)
        {
            _position = position;
            return this;
        }
        
        /// <summary>
        /// Set a target to follow. Audio source will update its position every frame.
        /// </summary>
        /// <param name="followTarget">Transform to follow. Null to follow Main Camera transform.</param>
        public DynamicMusic SetFollowTarget (Transform followTarget)
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
        public DynamicMusic SetSpatialSound (bool activate = true)
        {
            _spatialSound = activate;
            return this;
        }
        
        /// <summary>
        /// Set fade out duration. It'll be used when sound ends.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public DynamicMusic SetFadeOut (float fadeOutTime)
        {
            _fadeOutTime = fadeOutTime;
            return this;
        }
        
        /// <summary>
        /// Set the audio output to manage the volume using the Audio Mixers.
        /// </summary>
        /// <param name="output">Output you've created before inside Master AudioMixer
        /// (Remember reload the outputs database on Output Manager Window)</param>
        public DynamicMusic SetOutput (Output output)
        {
            _output = AudioManager.GetOutput(output);
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music start playing.
        /// </summary>
        /// <param name="onPlayCallback">Method will be invoked</param>
        public DynamicMusic OnPlay (Action onPlayCallback)
        {
            onPlay = onPlayCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music complete.
        /// If "loop" is active, it'll be called when you Stop the sound manually.
        /// </summary>
        /// <param name="onCompleteCallback">Method will be invoked</param>
        public DynamicMusic OnComplete (Action onCompleteCallback)
        {
            onComplete = onCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on loop cycle complete.
        /// You need to set loop on true to use it.
        /// </summary>
        /// <param name="onLoopCycleCompleteCallback">Method will be invoked</param>
        public DynamicMusic OnLoopCycleComplete (Action onLoopCycleCompleteCallback)
        {
            onLoopCycleComplete = onLoopCycleCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music pause.
        /// It will ignore the fade out time.
        /// </summary>
        /// <param name="onPauseCallback">Method will be invoked</param>
        public DynamicMusic OnPause (Action onPauseCallback)
        {
            onPause = onPauseCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on music pause and fade out ends.
        /// </summary>
        /// <param name="onPauseCompleteCallback">Method will be invoked</param>
        public DynamicMusic OnPauseComplete (Action onPauseCompleteCallback)
        {
            onPauseComplete = onPauseCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on resume/unpause music.
        /// </summary>
        /// <param name="onResumeCallback">Method will be invoked</param>
        public DynamicMusic OnResume (Action onResumeCallback)
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
            
            for (int i = 0; i < _sourcePoolElementDictionary.Count; i++)
            {
                string tag = _sourcePoolElementDictionary.ElementAt(i).Key;
                
                SourcePoolElement newSourcePoolElement = AudioManager.GetSource();
                _sourcePoolElementDictionary[tag] = newSourcePoolElement;
                if (i == 0)
                {
                    _referenceSourcePoolElement = newSourcePoolElement;
                    _referenceSourcePoolElement
                        .SetVolume(_volumeDictionary[tag], _hearDistanceDictionary[tag])
                        .SetPitch(_pitch)
                        .SetLoop(_loop)
                        .SetClip(_clips[i])
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
                    continue;
                }
                
                newSourcePoolElement
                    .SetVolume(_volumeDictionary[tag], _hearDistanceDictionary[tag])
                    .SetPitch(_pitch)
                    .SetLoop(_loop)
                    .SetClip(_clips[i])
                    .SetPosition(_position)
                    .SetFollowTarget(_followTarget)
                    .SetSpatialSound(_spatialSound)
                    .SetFadeOut(_fadeOutTime)
                    .SetId(_id)
                    .SetOutput(_output)
                    .Play(fadeInTime);
            }
        }

        /// <summary>
        /// Pause music.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before pause</param>
        public void Pause (float fadeOutTime = 0)
        {
            if (!Using) return;
            
            foreach (var source in _sourcePoolElementDictionary.Values)
            {
                source.Pause(fadeOutTime);
            }
        }

        /// <summary>
        /// Resume/Unpause music.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Resume (float fadeInTime = 0)
        {
            if (!Using) return;
            
            foreach (var source in _sourcePoolElementDictionary.Values)
            {
                source.Resume(fadeInTime);
            }
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before stop</param>
        public void Stop (float fadeOutTime = 0)
        {
            if (!Using) return;

            for (int i = 0; i < _sourcePoolElementDictionary.Count; i++)
            {
                string tag = _sourcePoolElementDictionary.ElementAt(i).Key;

                if (_forgetSourcePoolOnStop)
                {
                    _sourcePoolElementDictionary[tag].Stop(fadeOutTime, () =>
                    {
                        _sourcePoolElementDictionary[tag] = null;
                        _referenceSourcePoolElement = null;
                    });
                    continue;
                }
                _sourcePoolElementDictionary[tag].Stop(fadeOutTime, () => _sourcePoolElementDictionary[tag] = null);
            }
            _referenceSourcePoolElement = null;
            _forgetSourcePoolOnStop = false;
        }

        #endregion
    }
}