using System;
using DG.Tweening;
using RDG;
using UnityEngine;

public static class VibrationManager
{
    public static bool IsVibrateOn = true;

    public static void Vibrate(float second, float delay = 0)
    {
        if(!IsVibrateOn) return;
        if (delay > 0)
        {
            DOVirtual.DelayedCall(delay,()=> Vibration.Vibrate(TimeSpan.FromSeconds(second).Milliseconds));
            return;
        }
        Vibration.Vibrate(TimeSpan.FromSeconds(second).Milliseconds);
    }

    public static void StopVibrate()
    {
        Vibration.Cancel();
    }
}
