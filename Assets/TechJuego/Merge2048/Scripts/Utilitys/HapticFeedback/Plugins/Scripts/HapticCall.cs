using UnityEngine;
namespace TechJuego.FruitSliceMerge.HapticFeedback
{
    public class HapticCall
    {
        static AndroidHaptic androidHaptic;
        public HapticCall()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                androidHaptic = new AndroidHaptic();
            }
        }
        public static void HeavyHaptic()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
        iOSHaptic.PerformUIImpactFeedbackStyleHeavy();
#endif
#if UNITY_ANDROID

                androidHaptic.PerformUIImpactFeedbackStyleHeavy();

#endif
            }
        }
        public static void MediumHaptic()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUIImpactFeedbackStyleMedium();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUIImpactFeedbackStyleMedium();
#endif
            }
        }
        public static void LightHaptic()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUIImpactFeedbackStyleLight();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUIImpactFeedbackStyleLight();
#endif
            }
        }
        public static void RigidHaptic()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUIImpactFeedbackStyleRigid();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUIImpactFeedbackStyleRigid();
#endif
            }
        }
        public static void SoftHaptic()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUIImpactFeedbackStyleSoft();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUIImpactFeedbackStyleSoft();
#endif
            }
        }
        public static void PerformSuccessFeedback()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUINotificationFeedbackTypeSuccess();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUINotificationFeedbackTypeSuccess();
#endif
            }
        }
        public static void PerformErrorFeedback()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUINotificationFeedbackTypeError();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUINotificationFeedbackTypeError();
#endif
            }
        }
        public static void PerformWarningFeedback()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
#if UNITY_IOS
            iOSHaptic.PerformUINotificationFeedbackTypeWarning();
#endif
#if UNITY_ANDROID
                androidHaptic.PerformUINotificationFeedbackTypeWarning();
#endif
            }
        }
    }
}