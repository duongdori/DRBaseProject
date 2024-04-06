using System.Collections;
using UnityEngine;
using UnityEngine.Android;

#if !UNITY_EDITOR
using NatSuite.Devices;
#endif

public class FlashlightController : MonoSingleton<FlashlightController>
{
#if !UNITY_EDITOR
    private CameraDevice device;
#endif

    public void InitFlash()
    {
#if !UNITY_EDITOR
        //if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        //{
        //    yield return null;
        //}

        // Create a device query to find camera devices
        var filter = MediaDeviceCriteria.CameraDevice;
        var query = new MediaDeviceQuery(filter);
        device = query.current as CameraDevice;
#endif
    }

    public void TurnOnFlashlight()
    {        
#if !UNITY_EDITOR
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera)) return;
        if (device == null) return;        
        device.torchEnabled = true;
#endif
    }

    public void TurnOffFlashlight()
    {
#if !UNITY_EDITOR
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera)) return;
        if (device == null) return;        
        device.torchEnabled = false;
#endif
    }

    private void OnDestroy()
    {        
#if !UNITY_EDITOR
        TurnOffFlashlight();
#endif
    }
}