﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

namespace MaddeningJinx
{
    public static class LaneClear
    {
        public static Menu Menu
        {
            get { return MenuManager.GetSubMenu("LaneClear"); }
        }

        public static bool IsActive
        {
            get { return ModeManager.IsLaneClear; }
        }

        public static IEnumerable<Obj_AI_Minion> Minions
        {
            get
            {
                return Orbwalker.LaneClearMinionsList.Where(m => m.IsInFishBonesRange()).Concat(LastHit.Minions);
            }
        }

        public static IEnumerable<Obj_AI_Base> AttackableUnits
        {
            get { return Minions.Concat(Combo.HeroesInFishBonesRange); }
        }

        private static bool CanAutoAttack(this Champion.BestResult result)
        {
            return (Orbwalker.LastHitMinion == null && !Orbwalker.ShouldWait) ||
                   LastHit.Minions.Contains(result.Target);
            //(Orbwalker.AlmostLasthittableMinion == null || !tuple.Item1.Intersect(Orbwalker.AlmostLasthittableMinions).Any());
        }

        public static void Execute()
        {
            if (MenuManager.Menu.GetCheckBoxValue("Farming.Q"))
            {
                if (!Combo.CanUseQ || (MyTargetSelector.Target.IsInEnemyTurret() && Util.MyHero.IsInEnemyTurret()))
                {
                    Champion.DisableFishBones();
                    return;
                }
                var t = LastHit.AttackableUnits.GetBestFishBonesTarget();
                if (t.List.Count > 1)
                {
                    Champion.EnableFishBones(t.Target);
                }
                else
                {
                    t = AttackableUnits.GetBestFishBonesTarget();
                    if (t.List.Count > 2 && t.CanAutoAttack())
                    {
                        Champion.EnableFishBones(t.Target);
                    }
                    else
                    {
                        Champion.DisableFishBones();
                    }
                }
            }
            else
            {
                Orbwalker.ForcedTarget = null;
            }
        }
    }
}