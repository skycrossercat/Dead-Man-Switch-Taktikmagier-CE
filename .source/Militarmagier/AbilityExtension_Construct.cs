using RimWorld;
using RimWorld.Planet;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;
using VanillaPsycastsExpanded;

namespace Militarmagier
{
    public class AbilityExtension_Construct : AbilityExtension_AbilityMod
    {
        public ThingDef constructDef;
        public ThingDef costDef;
        public float heat;
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            Pawn pawn = ability.pawn;
            Hediff_Focus focus = (Hediff_Focus)pawn.health.hediffSet.GetFirstHediffOfDef(MilitarmagierDefOf.DMS_PsycastFocus);
            if (focus == null)
            {
                focus = (Hediff_Focus)pawn.health.AddHediff(MilitarmagierDefOf.DMS_PsycastFocus);
            }
            foreach (GlobalTargetInfo globalTargetInfo in targets)
            {
                ThingDef stuff = null;
                if (constructDef.MadeFromStuff)
                {
                    stuff = GenStuff.DefaultStuffFor(constructDef);
                }
                Thing thing = ThingMaker.MakeThing(constructDef, stuff);
                thing.SetFactionDirect(pawn.Faction);
                thing.TryGetComp<CompBreakLink>().Pawn = pawn;
                focus.AddHeatGiver(thing, heat);
                Thing cost = globalTargetInfo.Thing;
                GenSpawn.Spawn(thing, cost.Position, cost.Map);
                cost.SplitOff(1);
            }
        }

        public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
            Thing thing = target.Thing;
            if (thing != null && thing.def == costDef)
            {
                return true;
            }
            return false;
        }

        public override bool CanApplyOn(LocalTargetInfo target, Ability ability, bool throwMessages = false)
        {
            return ValidateTarget(target, ability, throwMessages);
        }
    }
}