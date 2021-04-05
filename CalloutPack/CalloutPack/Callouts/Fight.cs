using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack.Callouts
{

    [CalloutInfo("Fight", CalloutProbability.High)]
    class Fight : Callout
    {
        private Ped Suspect1;
        private Ped Suspect2;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip1;
        private Blip SuspectBlip2;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Fight"; CalloutPosition = SpawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect1 = new Ped(SpawnPoint, 31f);
            Suspect2 = new Ped(SpawnPoint, 30f);
            Suspect1.IsPersistent = true;
            Suspect2.IsPersistent = true;

            Suspect2.IsPersistent = true;
            Suspect1.IsPersistent = true;
            Suspect1.BlockPermanentEvents = true;
            Suspect2.BlockPermanentEvents = true;

            SuspectBlip1 = Suspect1.AttachBlip();
            SuspectBlip1.IsFriendly = false;
            SuspectBlip2 = Suspect2.AttachBlip();
            SuspectBlip2.IsFriendly = false;

            Suspect1.Health = 300;
            Suspect2.Health = 300;

            Suspect1.Tasks.FightAgainst(Suspect2);
            Suspect2.Tasks.FightAgainst(Suspect1);

            return base.OnCalloutAccepted();
           

        }

        public override void Process()
        {
            base.Process();

            

           

            if (Suspect1.IsDead || Suspect1.IsCuffed)
            {
                Suspect1IsClear();
            }

            if (Suspect2.IsDead || Suspect2.IsCuffed)
            {
                Suspect2IsClear();
            }

        }

        public void Suspect1IsClear()
        {
            if (Suspect2.IsCuffed || Suspect2.IsDead)
            {
                End();
            }
        }

        public void Suspect2IsClear()
        {
            if (Suspect1.IsCuffed || Suspect1.IsDead)
            {
                End();
            }
        }
        
        public override void End()
        {
            Functions.PlayScannerAudioUsingPosition("WE_ARE_CODE_4", SpawnPoint);
            base.End();

            Suspect1.Dismiss();
            Suspect2.Dismiss();
            
            SuspectBlip1.Delete();
            SuspectBlip2.Delete();

        }

    }
}
