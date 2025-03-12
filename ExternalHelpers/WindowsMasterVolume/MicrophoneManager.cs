using System;
using System.Runtime.InteropServices;

namespace PowerControl.Helpers.WindowsMasterVolume
{
    public static class MicrophoneManager
    {
        #region Microphone Volume Manipulation

        /// <summary>
        /// Gets the current microphone volume in scalar values (percentage)
        /// </summary>
        /// <returns>-1 in case of an error, if successful the value will be between 0 and 100</returns>
        public static int GetMicrophoneVolume()
        {
            IAudioEndpointVolume? micVol = null;
            try
            {
                micVol = GetMicrophoneVolumeObject();
                if (micVol != null)
                {
                    float volumeLevel;
                    micVol.GetMasterVolumeLevelScalar(out volumeLevel);
                    return (int)(volumeLevel * 100);
                }

                return -1;
            }
            finally
            {
                if (micVol != null)
                    Marshal.ReleaseComObject(micVol);
            }
        }

        /// <summary>
        /// Sets the microphone volume to a specific level
        /// </summary>
        /// <param name="newLevel">Value between 0 and 100 indicating the desired scalar value of the volume</param>
        public static void SetMicrophoneVolume(int newLevel)
        {
            IAudioEndpointVolume? micVol = null;
            try
            {
                micVol = GetMicrophoneVolumeObject();
                if (micVol == null)
                    return;

                micVol.SetMasterVolumeLevelScalar((float)newLevel / 100.0f, Guid.Empty);
            }
            finally
            {
                if (micVol != null)
                    Marshal.ReleaseComObject(micVol);
            }
        }

        /// <summary>
        /// Gets the mute state of the microphone.
        /// </summary>
        /// <returns>false if not muted, true if volume is muted</returns>
        public static bool GetMicrophoneMute()
        {
            IAudioEndpointVolume? micVol = null;
            try
            {
                micVol = GetMicrophoneVolumeObject();
                if (micVol == null)
                    return false;

                bool isMuted;
                micVol.GetMute(out isMuted);
                return isMuted;
            }
            finally
            {
                if (micVol != null)
                    Marshal.ReleaseComObject(micVol);
            }
        }

        /// <summary>
        /// Mute or unmute the microphone
        /// </summary>
        /// <param name="isMuted">true to mute the microphone, false to unmute</param>
        public static void SetMicrophoneMute(bool isMuted)
        {
            IAudioEndpointVolume? micVol = null;
            try
            {
                micVol = GetMicrophoneVolumeObject();
                if (micVol == null)
                    return;

                micVol.SetMute(isMuted, Guid.Empty);
            }
            finally
            {
                if (micVol != null)
                    Marshal.ReleaseComObject(micVol);
            }
        }

        /// <summary>
        /// Switches between the microphone mute states depending on the current state
        /// </summary>
        /// <returns>the current mute state, true if the volume was muted, false if unmuted</returns>
        public static bool ToggleMicrophoneMute()
        {
            IAudioEndpointVolume? micVol = null;
            try
            {
                micVol = GetMicrophoneVolumeObject();
                if (micVol == null)
                    return false;

                bool isMuted;
                micVol.GetMute(out isMuted);
                micVol.SetMute(!isMuted, Guid.Empty);

                return !isMuted;
            }
            finally
            {
                if (micVol != null)
                    Marshal.ReleaseComObject(micVol);
            }
        }

        private static IAudioEndpointVolume? GetMicrophoneVolumeObject()
        {
            IMMDeviceEnumerator? deviceEnumerator = null;
            IMMDevice? microphone = null;
            try
            {
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia, out microphone);

                Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
                object? o = null;
                microphone?.Activate(ref IID_IAudioEndpointVolume, 0, IntPtr.Zero, out o);
                IAudioEndpointVolume? micVol = o != null ? (IAudioEndpointVolume)o : null;

                return micVol;
            }
            finally
            {
                if (microphone != null) Marshal.ReleaseComObject(microphone);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #endregion

        #region Microphone Connection Check

        /// <summary>
        /// Checks if a microphone is connected to the system.
        /// </summary>
        /// <returns>True if a microphone is connected, false otherwise.</returns>
        public static bool IsMicrophoneConnected()
        {
            IMMDeviceEnumerator? deviceEnumerator = null;
            IMMDevice? microphone = null;
            try
            {
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());

                // Попытка получить устройство захвата звука (микрофон)
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia, out microphone);

                // Если устройство найдено, микрофон подключен
                return microphone != null;
            }
            catch (COMException)
            {
                // Если устройство не найдено, будет выброшено исключение
                return false;
            }
            finally
            {
                if (microphone != null) Marshal.ReleaseComObject(microphone);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #endregion
    }
}
