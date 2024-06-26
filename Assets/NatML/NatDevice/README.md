# NatDevice API
NatDevice is a cross-platform media device API for iOS, Android, macOS, and Windows. NatDevice provides powerful abstractions for hardware cameras and microphones through a concise .NET API. Features include:

+ Stream the camera preview and microphone audio into Unity with very little latency.
+ Support for high resolution camera previews, at full HD and higher on devices that support it.
+ Support for specifying microphone sample rate and channel count.
+ Full feature camera control, with the ability to set flash, focus, exposure, white balance, torch, zoom, and orientation.
+ Support for microphones with echo cancellation for karaoke and voice call apps.
+ NatML support, for building high performance machine learning and computer vision apps.
+ NatCorder support, for recording the camera and/or the microphone.
+ OpenCV support, for using computer vision algorithms on the camera preview.
+ VR support, compatible with GearVR and Google Cardboard.

## Discovering Devices
NatDevice exposes instances of the `IMediaDevice` interface, each of which abstracts a media device. Every media device allows for streaming either a camera preview or microphone audio. In order to discover available devices, you will use the `MediaDeviceQuery` class. The device query is responsible for finding available devices:
```csharp
// Create a device query to search for available devices
var query = new MediaDeviceQuery();
// Now we can get the currently selected device
IMediaDevice device = query.current;
```

## Using Audio Devices
Audio devices are exposed with the `AudioDevice` class. The `StartRunning` method starts streaming audio sample buffers to a provided delegate:
```csharp
// Get a microphone
var query = new MediaDeviceQuery(MediaDeviceCriteria.AudioDevice);
var device = query.current as AudioDevice;
// Start recording
device.StartRecording(OnAudioBuffer);
Debug.Log($"Started recording with format: {device.sampleRate}Hz, {device.channelCount} channels");
```

The sample buffers are always floating point linear PCM, interleaved by channel:
```csharp
void OnAudioBuffer (AudioBuffer audioBuffer) {
    // `audioBuffer.sampleBuffer` is linear PCM, interleaved by channel
}
```

> Note that on iOS, you must enable `Prepare iOS for Recording` in Player Settings before building.

## Using Camera Devices
Camera devices are exposed with the `CameraDevice` class. The `StartRunning` method starts streaming the camera preview to a provided delegate:
```csharp
// Get a camera
var query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
var device = query.current as CameraDevice;
// Start the preview
device.StartRunning(OnCameraImage);
```

The camera images contain a pixel buffer along with a selection of EXIF metadata for the frame. The pixel `format` of the camera image is platform-dependent, and must be considered when using the pixel buffer.

## Streaming to Device Outputs
Device outputs are lightweight, modular components that convert camera images or audio buffers from media devices into other types of data. Device outputs allow for building more complex applications on top of camera and audio devices. NatDevice provides a few outputs:

- `PixelBufferOutput`. This is a camera device output that converts camera images into `RGBA8888` pixel buffers.
- `TextureOutput`. This is a camera device output that streams camera images into a `Texture2D` object.
- `AudioClipOutput`. This is an audio device output that accumulates audio buffers into an `AudioClip` object.

Refer to the [online docs](https://docs.natml.ai/natdevice) for more information on working with device outputs.

## Managing Permissions
On some platforms, it is necessary to request permissions from the user before attempting to start media devices. For this, NatDevice provides the `MediaDeviceQuery.RequestPermissions` method:
```csharp
// Request camera permissions
bool cameraPermissionGranted = await MediaDeviceQuery.RequestPermissions<CameraDevice>();
// Or request microphone permissions
bool microphonePermissionGranted = await MediaDeviceQuery.RequestPermissions<AudioDevice>();
```

> Note that on iOS, you will need to include the `NSCameraUsageDescription` and/or `NSMicrophoneUsageDescription` keys in your Xcode project's `Info.plist` file. You can alternatively specify these description strings in Player Settings.

___

## Requirements
- Unity 2020.3+
- Android API Level 24+
- iOS 13+
- macOS 10.15+ (Apple Silicon and Intel)
- Windows 10 64-bit

## Resources
- [NatDevice Documentation](https://docs.natml.ai/natdevice)
- [NatML community on Discord](https://discord.gg/y5vwgXkz2f)
- [NatML on GitHub](https://github.com/natmlx)
- [NatDevice on Unity Forums](https://forum.unity.com/threads/natdevice-media-device-api.374690)
- [Simplifying Media Devices](https://blog.natml.ai/natdevice-simplifying-media-devices-619fc97c74)
- [Email Support](mailto:hi@natml.ai)

Thank you very much!