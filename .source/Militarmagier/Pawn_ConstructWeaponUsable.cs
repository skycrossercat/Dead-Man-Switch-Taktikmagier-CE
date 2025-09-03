using RimWorld;
using Verse;
using VanillaPsycastsExpanded.Technomancer;
using Fortified;
using Verse.AI;

namespace Militarmagier
{
    public class Pawn_ConstructWeaponUsable : Pawn_Construct, IWeaponUsable
    {
        void IWeaponUsable.Equip(ThingWithComps equipment)
        {
            equipment.SetForbidden(false);
            jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Equip, equipment), JobTag.DraftedOrder);
        }

        void IWeaponUsable.Wear(ThingWithComps apparel)
        {
            apparel.SetForbidden(false);
            this.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wear, apparel), JobTag.DraftedOrder);
        }
    }
}