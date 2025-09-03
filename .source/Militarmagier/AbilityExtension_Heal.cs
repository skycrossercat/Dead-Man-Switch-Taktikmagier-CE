using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class AbilityExtension_Heal : AbilityExtension_AbilityMod
    {
        public FloatRange tendQualityRange;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                Pawn pawn = target.Pawn;
                if (pawn != null)
                {
                    int num = 0;
                    List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                    for (int num2 = hediffs.Count - 1; num2 >= 0; num2--)
                    {
                        if ((hediffs[num2] is Hediff_Injury || hediffs[num2] is Hediff_MissingPart) && hediffs[num2].TendableNow())
                        {
                            hediffs[num2].Tended(tendQualityRange.RandomInRange, tendQualityRange.TrueMax, 1);
                            num++;
                        }
                    }
                    if (num > 0)
                    {
                        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(num), 3.65f);
                    }
                    FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, Vector3.zero, 1.5f);
                }
            }
        }
    }
}