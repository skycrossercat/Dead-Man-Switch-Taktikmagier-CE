using RimWorld.Planet;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class AbilityExtension_Destroy : AbilityExtension_AbilityMod
    {
        public int destroyPoints = 300;
        public float undestroyFactor = 0.5f;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            foreach (GlobalTargetInfo target in targets)
            {
                Building building = (Building)target.Thing;
                if (building != null)
                {
                    if (building.def.destroyable)
                    {
                        if (building.HitPoints <= destroyPoints)
                        {
                            building.Destroy();
                        }
                        else
                        {
                            building.HitPoints = (int)(building.HitPoints * undestroyFactor);
                        }
                    }
                }
            }
        }
    }
}