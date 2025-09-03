using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Militarmagier
{
    public class Hediff_Focus : HediffWithComps
    {
        public HediffStage curStage;
        public Dictionary<Thing, float> heatGiver = new();
        public List<Thing> list1 = new();
        public List<float> list2 = new();
        public override HediffStage CurStage
        {
            get
            {
                if (curStage == null) RecacheCurStage();
                return curStage;
            }
        }

        public override string Description => base.Description + ShowCost();
        public string ShowCost()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("");
            for (int i = 0; i < heatGiver.Count; i++)
            {
                stringBuilder.AppendLine(heatGiver.Keys.ElementAt(i).Label + ": " + heatGiver.Values.ElementAt(i));
            }
            return stringBuilder.ToString();
        }

        public void AddHeatGiver(Thing thing, float heat)
        {
            heatGiver[thing] = heat;
            RecacheCurStage();
        }
        public void RecacheCurStage()
        {
            StatModifier heat = new()
            {
                stat = MilitarmagierDefOf.VPE_PsychicEntropyMinimum,
                value = 0
            };
            for (int i = 0; i < heatGiver.Count; i++)
            {
                heat.value += heatGiver.Values.ElementAt(i);
            }
            curStage = new()
            {
                statOffsets = new()
                {
                    heat
                }
            };
            if (pawn != null && pawn.Spawned)
            {
                pawn.health.Notify_HediffChanged(this);
            }
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            for (int i = 0; i < heatGiver.Count; i++)
            {
                heatGiver.Keys.ElementAt(i).Kill();
            }
            pawn.health.RemoveHediff(this);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref heatGiver, "heatGiver", LookMode.Reference, LookMode.Value, ref list1, ref list2);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                RecacheCurStage();
            }
        }
    }
}