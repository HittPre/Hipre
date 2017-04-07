using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using System.Linq;

namespace PreLeBlanc
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using Color = System.Drawing.Color;
    internal class DamageIndicator
    {
        private const int BarWidth = 106;
        private const int LineThickness = 10;

        public delegate float DamageToUnitDelegate(AIHeroClient hero);
        private static DamageToUnitDelegate DamageToUnit { get; set; }

        private static Vector2 BarOffset = new Vector2(0, 0);

        private static Color _drawingColor;
        public static Color DrawingColor
        {
            get { return _drawingColor; }
            set { _drawingColor = Color.FromArgb(170, value); }
        }

        public static CheckBox HealthbarEnabled { get; set; }
        public static CheckBox PercentEnabled { get; set; }

        public static void Initialize(DamageToUnitDelegate damageToUnit)
        {
           
            DamageToUnit = damageToUnit;

            DrawingColor = Color.Crimson;
            Menu menu = LBMenu.Menu.AddSubMenu("Damage Indicator");
            HealthbarEnabled = menu.Add("eoqmaluco", new CheckBox("Draw damage on healthbar"));
            PercentEnabled = menu.Add("solaaaaado", new CheckBox("Draw damage % text"));

            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
         
            if (HealthbarEnabled.CurrentValue || PercentEnabled.CurrentValue)
            {
                foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsHPBarRendered))
                {
            
                    var damage = DamageToUnit(unit);

              
                    if (damage <= 0)
                    {
                        continue;
                    }

                    if (HealthbarEnabled.CurrentValue)
                    {
                        
                        var damagePercentage = ((unit.TotalShieldHealth() - damage) > 0 ? (unit.TotalShieldHealth() - damage) : 0) /
                                               (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var currentHealthPercentage = unit.TotalShieldHealth() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);

                        var startPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + damagePercentage * BarWidth), (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        var endPoint = new Vector2((int)(unit.HPBarPosition.X + BarOffset.X + currentHealthPercentage * BarWidth) + 1, (int)(unit.HPBarPosition.Y + BarOffset.Y) - 5);
                        
                        Drawing.DrawLine(startPoint, endPoint, LineThickness, DrawingColor);
                    }

                    if (PercentEnabled.CurrentValue)
                    {

                        Drawing.DrawText(new Vector2(unit.HPBarPosition.X, unit.HPBarPosition.Y + 20), Color.MediumVioletRed, string.Concat(Math.Ceiling((damage / unit.TotalShieldHealth()) * 100), "%"), 10);
                    }
                }
            }
        }
    }
}
