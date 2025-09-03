using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace Militarmagier
{
    public class Ability_Flash : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            LocalTargetInfo target = (LocalTargetInfo)targets[0];
            List<IntVec3> list = AffectedCells(target);
            Map map = pawn.Map;
            float seconds = GetDurationForPawn().TicksToSeconds() * 2;
            for (int i = 0; i < list.Count; i++)
            {
                FleckMaker.ThrowMicroSparks(list[i].ToVector3Shifted(), map);
                foreach (Pawn stunPawn in list[i].GetThingList(map).OfType<Pawn>())
                {
                    stunPawn.TakeDamage(new DamageInfo(DamageDefOf.Stun, seconds));
                }
            }
        }

        public readonly List<IntVec3> tmpCells = new List<IntVec3>();

        public override void DrawHighlight(LocalTargetInfo target)
        {
            float rangeForPawn = GetRangeForPawn();
            GenDraw.DrawRadiusRing(pawn.Position, rangeForPawn, def.rangeRingColor);
            GenDraw.DrawFieldEdges(AffectedCells(target));
        }

        public List<IntVec3> AffectedCells(LocalTargetInfo target)
        {
            tmpCells.Clear();
            float range = GetRangeForPawn();
            float radius = GetRadiusForPawn();
            Vector3 vector = pawn.Position.ToVector3Shifted().Yto0();
            IntVec3 intVec = target.Cell.ClampInsideMap(pawn.Map);
            if (pawn.Position == intVec)
            {
                return tmpCells;
            }
            float lengthHorizontal = (intVec - pawn.Position).LengthHorizontal;
            float num = (intVec.x - pawn.Position.x) / lengthHorizontal;
            float num2 = (intVec.z - pawn.Position.z) / lengthHorizontal;
            intVec.x = Mathf.RoundToInt(pawn.Position.x + num * range);
            intVec.z = Mathf.RoundToInt(pawn.Position.z + num2 * range);
            float target2 = Vector3.SignedAngle(intVec.ToVector3Shifted().Yto0() - vector, Vector3.right, Vector3.up);
            float num3 = radius / 2f;
            float num4 = Mathf.Sqrt(Mathf.Pow((intVec - pawn.Position).LengthHorizontal, 2f) + Mathf.Pow(num3, 2f));
            float num5 = 57.29578f * Mathf.Asin(num3 / num4);
            int num6 = GenRadial.NumCellsInRadius(range);
            for (int i = 0; i < num6; i++)
            {
                IntVec3 intVec2 = pawn.Position + GenRadial.RadialPattern[i];
                if (CanUseCell(intVec2) && Mathf.Abs(Mathf.DeltaAngle(Vector3.SignedAngle(intVec2.ToVector3Shifted().Yto0() - vector, Vector3.right, Vector3.up), target2)) <= num5)
                {
                    tmpCells.Add(intVec2);
                }
            }
            List<IntVec3> list = GenSight.BresenhamCellsBetween(pawn.Position, intVec);
            for (int j = 0; j < list.Count; j++)
            {
                IntVec3 intVec3 = list[j];
                if (!tmpCells.Contains(intVec3) && CanUseCell(intVec3))
                {
                    tmpCells.Add(intVec3);
                }
            }
            return tmpCells;
            bool CanUseCell(IntVec3 c)
            {
                if (!c.InBounds(pawn.Map))
                {
                    return false;
                }
                if (c == pawn.Position)
                {
                    return false;
                }
                if (!c.InHorDistOf(pawn.Position, range))
                {
                    return false;
                }
                return verb.TryFindShootLineFromTo(pawn.Position, c, out _);
            }
        }
    }
}