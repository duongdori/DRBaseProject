using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DR.DRSound
{
    public class Playlist
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
        private float _fadeInTime;
        private bool _forgetSourcePoolOnStop;
        private Queue<AudioClip> _playlist = new();
        private AudioMixerGroup _output;

        #endregion

        #region Fields (Callbacks)

        private Action onPlay;
        private Action onComplete;
        private Action onLoopCycleComplete;
        private Action onNextTrackStart;
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
        /// <summary>Duration in seconds of current playing clip.</summary>
        public float CurrentClipDuration => Using ? _sourcePoolElement.CurrentClipDuration : 0;
        /// <summary>Total duration in seconds of entire playlist.</summary>
        public float PlayListDuration
        {
            get
            {
                float playlistDuration = 0;
                foreach (AudioClip clip in _playlist) playlistDuration += clip.length;
                return playlistDuration;
            }
        }
        /// <summary>Reproduced tracks in this playlist.</summary>
        public float ReproducedTracks => Using ? _sourcePoolElement.ReproducedTracks : 0;
        /// <summary>Current clip that is playing.</summary>
        public AudioClip CurrentPlaylistClip => _sourcePoolElement.CurrentClip;
        /// <summary>The next clip of current playlist</summary>
        public AudioClip NextPlaylistClip => _sourcePoolElement.NextPlaylistClip;

        #endregion

        #region Public Methods

        /// <summary>
        /// Create new Playlist object given a Tracks array.
        /// </summary>
        /// <param name="playlistTracks">Track array with all music tracks that you want to reproduce in order</param>
        public Playlist (Track[] playlistTracks)
        {
            SetPlaylist(playlistTracks);
        }
        
        /// <summary>
        /// Create new Playlist object given a tags array.
        /// </summary>
        /// <param name="playlistTags">A music tracks tags array that you want to reproduce in order</param>
        public Playlist (string[] playlistTags)
        {
            SetPlaylist(playlistTags);
        }

        /// <summary>
        /// Store volume parameters BEFORE play playlist.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        public Playlist SetVolume (float volume)
        {
            _volume = volume;
            return this;
        }
        
        /// <summary>
        /// Store volume parameters BEFORE play playlist.
        /// </summary>
        /// <param name="volume">Volume: min 0, Max 1</param>
        /// <param name="hearDistance">min and Max distance to hear music</param>
        public Playlist SetVolume (float volume, Vector2 hearDistance)
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
        public Playlist SetPitch (float pitch)
        {
            _pitch = pitch;
            return this;
        }

        /// <summary>
        /// Set an id to identify this music on AudioManager static methods.
        /// </summary>
        public Playlist SetId (string id)
        {
            _id = id;
            return this;
        }
        
        /// <summary>
        /// Make your playlist loops for infinite time. If you need to stop it, use Stop() method.
        /// </summary>
        public Playlist SetLoop (bool loop)
        {
            _loop = loop;
            return this;
        }

        /// <summary>
        /// Set a new playlist BEFORE play it.
        /// </summary>
        /// <param name="playlistTags">A music tracks tags array in order</param>
        public Playlist SetPlaylist (string[] playlistTags)
        {
            _playlist.Clear();
            foreach (string tag in playlistTags)
                _playlist.Enqueue(AudioManager.GetTrack(tag));
            return this;
        }
        
        /// <summary>
        /// Set a new playlist BEFORE play it.
        /// </summary>
        /// <param name="playlistTracks">A music tracks array in order</param>
        public Playlist SetPlaylist (Track[] playlistTracks)
        {
            _playlist.Clear();
            foreach (Track track in playlistTracks)
                _playlist.Enqueue(AudioManager.GetTrack(track.ToString()));
            return this;
        }
        
        /// <summary>
        /// Enqueue a new track to the existing playlist.
        /// </summary>
        /// <param name="addedTrackTag">The new track's tag you want to add at the end of the playlist</param>
        public void AddToPlaylist (string addedTrackTag)
        {
            _playlist.Enqueue(AudioManager.GetTrack(addedTrackTag));
            _sourcePoolElement.AddToPlaylist(AudioManager.GetTrack(addedTrackTag));
        }
        
        /// <summary>
        /// Enqueue a new track to the existing playlist.
        /// </summary>
        /// <param name="addedTrack">The new track you want to add at the end of the playlist</param>
        public void AddToPlaylist (Track addedTrack)
        {
            _playlist.Enqueue(AudioManager.GetTrack(addedTrack.ToString()));
            _sourcePoolElement.AddToPlaylist(AudioManager.GetTrack(addedTrack.ToString()));
        }

        /// <summary>
        /// Set the position of the sound emitter.
        /// </summary>
        public Playlist SetPosition (Vector3 position)
        {
            _position = position;
            return this;
        }
        
        /// <summary>
        /// Set a target to follow. Audio source will update its position every frame.
        /// </summary>
        /// <param name="followTarget">Transform to follow. Null to follow Main Camera transform.</param>
        public Playlist SetFollowTarget (Transform followTarget)
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
        public Playlist SetSpatialSound (bool activate = true)
        {
            _spatialSound = activate;
            return this;
        }
        
        /// <summary>
        /// Set fade out duration for all tracks.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last</param>
        public Playlist SetFadeOut (float fadeOutTime)
        {
            _fadeOutTime = fadeOutTime;
            return this;
        }
        
        /// <summary>
        /// Set fade in duration for all tracks.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public Playlist SetFadeIn (float fadeInTime)
        {
            _fadeInTime = fadeInTime;
            return this;
        }
        
        /// <summary>
        /// Set the audio output to manage the volume using the Audio Mixers.
        /// </summary>
        /// <param name="output">Output you've created before inside Master AudioMixer
        /// (Remember reload the outputs database on Output Manager Window)</param>
        public Playlist SetOutput (Output output)
        {
            _output = AudioManager.GetOutput(output);
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on playlist starts.
        /// </summary>
        /// <param name="onPlayCallback">Method will be invoked</param>
        public Playlist OnPlay (Action onPlayCallback)
        {
            onPlay = onPlayCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on playlist complete.
        /// If "loop" is active, it'll be called when you Stop the playlist manually.
        /// </summary>
        /// <param name="onCompleteCallback">Method will be invoked</param>
        public Playlist OnComplete (Action onCompleteCallback)
        {
            onComplete = onCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on playlist loop cycle complete.
        /// You need to set loop on true to use it.
        /// </summary>
        /// <param name="onLoopCycleCompleteCallback">Method will be invoked</param>
        public Playlist OnLoopCycleComplete (Action onLoopCycleCompleteCallback)
        {
            onLoopCycleComplete = onLoopCycleCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on finish track and start the next one.
        /// </summary>
        /// <param name="onNextTrackStartCallback">Method will be invoked</param>
        public Playlist OnNextTrackStart (Action onNextTrackStartCallback)
        {
            onNextTrackStart = onNextTrackStartCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on playlist pause.
        /// It will ignore the fade out time.
        /// </summary>
        /// <param name="onPauseCallback">Method will be invoked</param>
        public Playlist OnPause (Action onPauseCallback)
        {
            onPause = onPauseCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on playlist pause and fade out ends.
        /// </summary>
        /// <param name="onPauseCompleteCallback">Method will be invoked</param>
        public Playlist OnPauseComplete (Action onPauseCompleteCallback)
        {
            onPauseComplete = onPauseCompleteCallback;
            return this;
        }
        
        /// <summary>
        /// Define a callback that will be invoked on resume/unpause playlist.
        /// </summary>
        /// <param name="onResumeCallback">Method will be invoked</param>
        public Playlist OnResume (Action onResumeCallback)
        {
            onResume = onResumeCallback;
            return this;
        }

        /// <summary>
        /// Reproduce playlist.
        /// </summary>
        public void Play ()
        {
            if (Using && Playing)
            {
                Stop();
                _forgetSourcePoolOnStop = true;
            }

            _sourcePoolElement = AudioManager.GetSource();
            _sourcePoolElement
                .MarkAsPlaylist()
                .SetVolume(_volume, _hearDistance)
                .SetPitch(_pitch)
                .SetLoop(_loop)
                .SetPlaylist(_playlist)
                .SetPosition(_position)
                .SetFollowTarget(_followTarget)
                .SetSpatialSound(_spatialSound)
                .SetFadeIn(_fadeInTime)
                .SetFadeOut(_fadeOutTime)
                .SetId(_id)
                .SetOutput(_output)
                .OnPlay(onPlay)
                .OnComplete(onComplete)
                .OnLoopCycleComplete(onLoopCycleComplete)
                .OnNextTrackStart(onNextTrackStart)
                .OnPause(onPause)
                .OnPauseComplete(onPauseComplete)
                .OnResume(onResume)
                .PlayPlaylist(_fadeInTime);
        }

        /// <summary>
        /// Pause playlist.
        /// </summary>
        /// <param name="fadeOutTime">Seconds that fade out will last before pause</param>
        public void Pause (float fadeOutTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Pause(fadeOutTime);
        }

        /// <summary>
        /// Resume/Unpause playlist.
        /// </summary>
        /// <param name="fadeInTime">Seconds that fade in will last</param>
        public void Resume (float fadeInTime = 0)
        {
            if (!Using) return;
            
            _sourcePoolElement.Resume(fadeInTime);
        }

        /// <summary>
        /// Stop playlist.
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