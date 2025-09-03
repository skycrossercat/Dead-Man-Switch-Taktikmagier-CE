using RimWorld;
using RimWorld.Planet;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class AbilityExtension_Calm : AbilityExtension_AbilityMod
    {
        public ThoughtDef thought;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                Pawn pawn = target.Pawn;
                if (pawn != null)
                {
                    Need_Rest rest = pawn.needs.rest;
                    if (rest != null)
                    {
                        rest.CurLevel = rest.MaxLevel;
                    }
                    pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
                }
            }
        }
    }
}