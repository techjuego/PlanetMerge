using UnityEngine;
namespace TechJuego.FruitSliceMerge
{
    public class DebugUtility
    {
        public static string TextWithColor(object _text, string _hexColor)
        {
            return "<color=" + (!_hexColor.Contains("#") ? "#" : "") + _hexColor + ">" + _text + "</color>";
        }
        public static string TextWithColor(object _text, Color _color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(_color) + ">" + _text + "</color>";
        }
    }

}