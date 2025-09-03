using RimWorld;
using RimWorld.Planet;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class Ability_Swap : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            Pawn target = targets[0].Pawn;
            if (target != null)
            {
                IntVec3 pos = targets[0].Cell;
                IntVec3 ori = pawn.Position;
                Map map = pawn.Map;
                SkipUtility.SkipTo(pawn, pos, map);
                target.Position = ori;
                target.Notify_Teleported(true, false);
            }
        }
    }
}