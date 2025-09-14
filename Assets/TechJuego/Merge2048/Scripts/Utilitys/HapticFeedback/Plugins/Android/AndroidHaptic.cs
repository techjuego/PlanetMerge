using UnityEngine;
namespace TechJuego.FruitSliceMerge.HapticFeedback
{
    public class HapticInitialize
    {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                AndroidHaptic.LoadPluing();
            }
        }
    }
    public class AndroidHaptic
    {
        static AndroidJavaClass unityclass;
        static AndroidJavaObject unityActivity;
        static AndroidJavaObject _PluginInstance;

        public static void LoadPluing()
        {
            ItializePlugin("com.techjuego.hapticfeedback.HapticInstance");
        }
        private static void ItializePlugin(string pluginName)
        {
            unityclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityclass.GetStatic<AndroidJavaObject>("currentActivity");
            _PluginInstance = new AndroidJavaObject(pluginName);
            if (_PluginInstance != null)
            {
                Debug.Log("Done");
            }
            _PluginInstance.CallStatic("receiveUnityActivity", unityActivity);
        }

        public  void PerformUIImpactFeedbackStyleHeavy()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUIImpactFeedbackStyleHeavy");
            }
        }

        public  void PerformUIImpactFeedbackStyleMedium()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUIImpactFeedbackStyleMedium");
            }
        }

        public  void PerformUIImpactFeedbackStyleLight()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUIImpactFeedbackStyleLight");
            }
        }

        public  void PerformUIImpactFeedbackStyleRigid()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUIImpactFeedbackStyleRigid");
            }
        }

        public  void PerformUIImpactFeedbackStyleSoft()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUIImpactFeedbackStyleSoft");
            }
        }

        public  void PerformUINotificationFeedbackTypeSuccess()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUINotificationFeedbackTypeSuccess");
            }
        }

        public  void PerformUINotificationFeedbackTypeError()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUINotificationFeedbackTypeError");
            }
        }
        public  void PerformUINotificationFeedbackTypeWarning()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUINotificationFeedbackTypeWarning");
            }
        }
        public  void PerformUISelectionFeedbackGenerator()
        {
            if (_PluginInstance != null)
            {
                _PluginInstance.Call("PerformUISelectionFeedbackGenerator");
            }
        }
    }
}