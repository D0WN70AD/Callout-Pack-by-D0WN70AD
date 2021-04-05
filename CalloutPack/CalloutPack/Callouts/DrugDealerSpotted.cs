using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack.Callouts
{
    [CalloutInfo("DrugDealerSpotted", CalloutProbability.Low)]
    class DrugDealerSpotted : Callout
    {
        private Ped Suspect;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Drug Dealer Spotted On Foot"; CalloutPosition = SpawnPoint;

            Functions.PlayScannerAudioUsingPosition("", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();

        }

        public override bool OnCalloutAccepted()
        {

            Suspect = new Ped();
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsFriendly = true;

            Game.DisplayNotification("Try to arrest the Drug Dealer!");

            Suspect.Inventory.GiveNewWeapon("Weapon_SMG", -1, true);
            Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
            return base.OnCalloutAccepted();



        }

        public override void Process()
        {
            base.Process();



            if (Suspect.IsDead || Suspect.IsCuffed)
            {
                End();
            }

        }

        public override void End()
        {
            Functions.PlayScannerAudioUsingPosition("WE_ARE_CODE_4", SpawnPoint);
            base.End();


            Suspect.Dismiss();
            SuspectBlip.Delete();

        }
    }
}
