using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Version = System.Version;

namespace PreLeBlanc
{
    class PreHep
    {
        public static bool CastCheckbox(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }
        public static int CastSlider(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }

        public static GameObject Clone = null;

        public static AIHeroClient myHero { get { return Player.Instance; } }
    }
}
