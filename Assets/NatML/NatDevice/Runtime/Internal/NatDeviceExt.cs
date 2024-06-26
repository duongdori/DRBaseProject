/* 
*   NatDevice
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatSuite.Devices.Internal {

    using System;
    using System.Runtime.InteropServices;

    public static class NatDeviceExt { // NatDeviceExt.h

        #region --Enumerations--
        public enum PermissionType : int {
            AudioDevice     = 1,
            CameraDevice    = 2
        }
        #endregion


        #region --Delegates--
        public delegate void PermissionResultHandler (IntPtr context, PermissionStatus result);
        #endregion


        #region --Permissions--
        [DllImport(NatDevice.Assembly, EntryPoint = @"NDCheckPermissions")]
        public static extern PermissionStatus CheckPermissions (PermissionType type);

        [DllImport(NatDevice.Assembly, EntryPoint = @"NDRequestPermissions")]
        public static extern void RequestPermissions (
            PermissionType type,
            PermissionResultHandler handler,
            IntPtr context
        );
        #endregion


        #region --PixelBufferOutput--
        [DllImport(NatDevice.Assembly, EntryPoint = @"NDPixelBufferOutputConvert")]
        public static unsafe extern void Convert (
            IntPtr cameraImage,
            int orientation,
            void* tempBuffer,
            void* destBuffer,
            out int outWidth,
            out int outHeight
        );
        #endregion


        #region --AudioSession--
        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport(NatDevice.Assembly, EntryPoint = @"NDConfigureAudioSession")]
        public static extern void ConfigureAudioSession ();
        #else
        public static void ConfigureAudioSession () { }
        #endif
        #endregion
    }
}