﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

namespace MaddeningJinx
{
    public static class Harass
    {
        public static Menu Menu
        {
            get { return MenuManager.GetSubMenu("Harass"); }
        }

        public static bool IsActive
        {
            get { return ModeManager.IsHarass; }
        }
        public static IEnumerable<Obj_AI_Base> AttackableUnits
        {
            get { return LastHit.Minions.Concat(Combo.HeroesInFishBonesRange); }
        }
        private static bool CanAutoAttack(this Champion.BestResult result)
        {
            return LastHit.Minions.Contains(result.Target) /* && result.List.Intersect(Combo.HeroesInFishBonesRange).Any()*/;
        }
        public static void Execute()
        {
            if (MyTargetSelector.Target != null && MyTargetSelector.Target.IsInEnemyTurret() && Util.MyHero.IsInEnemyTurret())
            {
                Champion.DisableFishBones();
                return;
            }
            var t = AttackableUnits.GetBestFishBonesTarget();
            if (t.List.Count > 1 && t.CanAutoAttack())
            {
                Champion.EnableFishBones(t.Target);
            }
            else
            {
                if (Orbwalker.LastHitMinion == null && !Orbwalker.ShouldWait && MyTargetSelector.Target != null && MyTargetSelector.Target.Distance(Util.MousePos, true) <= MyTargetSelector.Target.Distance(Util.MyHero, true) && Util.MyHero.IsInRange(MyTargetSelector.Target, MyTargetSelector.AaRange) && Combo.CanUseQ)
                {
                    Champion.EnableFishBones(MyTargetSelector.FishBonesTarget);
                }
                else
                {
                    Champion.DisableFishBones();
                }
            }
        }
    }
}
