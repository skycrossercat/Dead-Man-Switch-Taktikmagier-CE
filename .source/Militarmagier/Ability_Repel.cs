using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class Ability_Repel : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            float seconds = GetDurationForPawn().TicksToSeconds() * 2;
            float radius = GetRadiusForPawn();
            IntVec3 origin = pawn.Position;
            AbilityExtension_Explosion modExtension = def.GetModExtension<AbilityExtension_Explosion>();
            if (modExtension != null)
            {
                GenExplosion.DoExplosion(origin,
                    pawn.Map,
                    modExtension.explosionRadius,
                    modExtension.explosionDamageDef,
                    pawn,
                    modExtension.explosionDamageAmount,
                    modExtension.explosionArmorPenetration, 
                    modExtension.explosionSound, 
                    null, 
                    null, 
                    null,
                    modExtension.postExplosionSpawnThingDef, 
                    modExtension.postExplosionSpawnChance, 
                    modExtension.postExplosionSpawnThingCount, 
                    modExtension.postExplosionGasType,
                    modExtension.postExplosionGasRadiusOverride,
                    modExtension.postExplosionGasAmount, 
                    modExtension.applyDamageToExplosionCellsNeighbors, 
                    modExtension.preExplosionSpawnThingDef, 
                    modExtension.preExplosionSpawnChance,
                    modExtension.preExplosionSpawnThingCount,
                    modExtension.chanceToStartFire,
                    modExtension.damageFalloff,
                    modExtension.explosionDirection,
                    modExtension.casterImmune ? new List<Thing> { pawn } : null);
            }
            foreach (GlobalTargetInfo globalTargetInfo in targets)
            {
                if (globalTargetInfo.Thing is Pawn pawnTarget)
                {
                    Repel(pawnTarget, origin, radius);
                    pawnTarget.TakeDamage(new DamageInfo(DamageDefOf.Stun, seconds));
                }
            }
        }

        public void Repel(Pawn target, IntVec3 origin, float radius)
        {
            IntVec3 targetPos = target.Position;
            IntVec3 direction = ((targetPos - origin).ToVector3().normalized * radius).ToIntVec3();
            IntVec3 repelDestination = origin + direction;
            List<IntVec3> path = GenSight.PointsOnLineOfSight(origin, repelDestination).ToList();
            IntVec3 end = targetPos;
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].InBounds(pawn.Map) && !path[i].Filled(pawn.Map))
                {
                    end = path[i];
                }
                else
                {
                    break;
                }
            }
            target.Position = end;
            target.Notify_Teleported(true, false);
        }
    }
}