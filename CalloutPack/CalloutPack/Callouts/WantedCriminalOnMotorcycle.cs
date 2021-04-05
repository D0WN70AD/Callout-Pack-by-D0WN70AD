using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack.Callouts
{
    [CalloutInfo("Wanted Criminal Spotted On Motorcycle", CalloutProbability.Medium)]
    class WantedCriminalOnMotorcycle : Callout
    {
        private Ped Suspect;
        private Vehicle SuspectVehicle;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Wanted Criminal In Car"; CalloutPosition = SpawnPoint;

            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();

        }

        public override bool OnCalloutAccepted()
        {
            SuspectVehicle = new Vehicle("Bati", SpawnPoint);
            SuspectVehicle.IsPersistent = true;

            Suspect = SuspectVehicle.CreateRandomDriver();
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsFriendly = true;

            Game.DisplayNotification("Try to arrest the criminal!");

            Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);
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
            SuspectVehicle.Dismiss();
            SuspectBlip.Delete();

        }

    }
}