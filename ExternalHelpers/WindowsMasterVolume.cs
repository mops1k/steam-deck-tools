using PowerControl.Helpers.WindowsMasterVolume;

namespace ExternalHelpers
{
    /// <summary>
    /// Enum для выбора типа мультимедийного устройства (микрофон или звук).
    /// </summary>
    public enum MultimediaDeviceType
    {
        Microphone,
        Speaker
    }

    /// <summary>
    /// Фасад для управления мультимедийными устройствами (микрофоном или звуком).
    /// </summary>
    public static class WindowsMasterVolume
    {
        /// <summary>
        /// Получает текущую громкость микрофона.
        /// </summary>
        /// <returns>Громкость в процентах (от 0 до 100) или -1 в случае ошибки.</returns>
        public static int GetVolume()
        {
            return MicrophoneManager.GetMicrophoneVolume();
        }
        
        /// <summary>
        /// Получает текущую громкость звука.
        /// </summary>
        /// <returns>Громкость в процентах (от 0 до 100) или -1 в случае ошибки.</returns>
        public static int GetVolume(double roundValue)
        {
            return AudioManager.GetMasterVolume(roundValue);
        }

        /// <summary>
        /// Устанавливает громкость устройства.
        /// </summary>
        /// <param name="deviceType">Тип устройства (микрофон или звук).</param>
        /// <param name="volumeLevel">Уровень громкости (от 0 до 100).</param>
        public static void SetVolume(MultimediaDeviceType deviceType, int volumeLevel)
        {
            if (deviceType == MultimediaDeviceType.Microphone)
                MicrophoneManager.SetMicrophoneVolume(volumeLevel);
            else
                AudioManager.SetMasterVolume(volumeLevel);
        }

        /// <summary>
        /// Получает состояние mute устройства.
        /// </summary>
        /// <param name="deviceType">Тип устройства (микрофон или звук).</param>
        /// <returns>True, если устройство muted, иначе false.</returns>
        public static bool GetMute(MultimediaDeviceType deviceType)
        {
            return deviceType == MultimediaDeviceType.Microphone
                ? MicrophoneManager.GetMicrophoneMute()
                : AudioManager.GetMasterVolumeMute();
        }

        /// <summary>
        /// Устанавливает состояние mute устройства.
        /// </summary>
        /// <param name="deviceType">Тип устройства (микрофон или звук).</param>
        /// <param name="isMuted">True, чтобы mute устройство, false, чтобы unmute.</param>
        public static void SetMute(MultimediaDeviceType deviceType, bool isMuted)
        {
            if (deviceType == MultimediaDeviceType.Microphone)
                MicrophoneManager.SetMicrophoneMute(isMuted);
            else
                AudioManager.SetMasterVolumeMute(isMuted);
        }

        /// <summary>
        /// Переключает состояние mute устройства.
        /// </summary>
        /// <param name="deviceType">Тип устройства (микрофон или звук).</param>
        /// <returns>Новое состояние mute (true, если устройство muted, иначе false).</returns>
        public static bool ToggleMute(MultimediaDeviceType deviceType)
        {
            return deviceType == MultimediaDeviceType.Microphone
                ? MicrophoneManager.ToggleMicrophoneMute()
                : AudioManager.ToggleMasterVolumeMute();
        }

        /// <summary>
        /// Проверяет, подключено ли устройство.
        /// </summary>
        /// <param name="deviceType">Тип устройства (микрофон или звук).</param>
        /// <returns>True, если устройство подключено, иначе false.</returns>
        public static bool IsDeviceConnected(MultimediaDeviceType deviceType)
        {
            return deviceType == MultimediaDeviceType.Microphone
                ? MicrophoneManager.IsMicrophoneConnected()
                : true; // Предполагаем, что звуковое устройство всегда подключено
        }
    }
}