using RimWorld.Planet;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;
using VanillaPsycastsExpanded;
using VanillaPsycastsExpanded.Technomancer;
namespace Militarmagier
{
    public class AbilityExtension_ConstructPawn : AbilityExtension_AbilityMod
    {
        public PawnKindDef constructDef;
        public ThingDef costDef;
        public float heat;
        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            Pawn pawn = ability.pawn;
            Hediff_Focus focus = (Hediff_Focus)pawn.health.hediffSet.GetFirstHediffOfDef(MilitarmagierDefOf.DMS_PsycastFocus);
            focus ??= (Hediff_Focus)pawn.health.AddHediff(MilitarmagierDefOf.DMS_PsycastFocus);
            foreach (GlobalTargetInfo globalTargetInfo in targets)
            {
                Pawn constructed = PawnGenerator.GeneratePawn(constructDef, pawn.Faction);
                constructed.TryGetComp<CompBreakLink>().Pawn = pawn;
                focus.AddHeatGiver(constructed, heat);
                Thing cost = globalTargetInfo.Thing;
                GenSpawn.Spawn(constructed, cost.Position, cost.Map);
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