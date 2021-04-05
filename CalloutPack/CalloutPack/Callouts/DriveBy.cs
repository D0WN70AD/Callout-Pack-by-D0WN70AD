using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack.Callouts
{
    [CalloutInfo("GangDriveBy", CalloutProbability.Low)]
    class DriveBy : Callout
    {
        private Ped Suspect;
        private Ped Suspect2;
        private Vehicle SuspectVehicle;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private bool PursuitCreated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(500f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Gang Drive-by"; CalloutPosition = SpawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            SuspectVehicle = new Vehicle("Vigero", SpawnPoint); SuspectVehicle.IsPersistent = true;

            Suspect = SuspectVehicle.CreateRandomDriver(); Suspect.IsPersistent = true; Suspect.BlockPermanentEvents = true;
            Suspect2 = new Ped(SpawnPoint);
            Suspect2.IsPersistent = true; Suspect2.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip(); SuspectBlip.IsFriendly = false;
            Suspect2.WarpIntoVehicle(SuspectVehicle, 0);
            Suspect2.Inventory.GiveNewWeapon("Weapon_SMG", -1, true);
            Suspect2.Tasks.AimWeaponAt(Game.LocalPlayer.Character, -1);
            Suspect2.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);
            Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency); return base.OnCalloutAccepted();
            

        }

        public override void Process()
        {
            base.Process();
            if (!PursuitCreated && Game.LocalPlayer.Character.DistanceTo(Suspect.Position) < 30f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                PursuitCreated = true;
            }

            if (PursuitCreated && !Functions.IsPursuitStillRunning(Pursuit))
            {
                End();
            }
        }

        public override void End()
        {
            Functions.PlayScannerAudioUsingPosition("WE_ARE_CODE_4", SpawnPoint);
            base.End();
            if (Suspect.Exists())
            {
                Suspect.Dismiss();
            }
            if (SuspectVehicle.Exists())
            {
                SuspectVehicle.Dismiss();
            }
            if (SuspectBlip.Exists())
            {
                SuspectBlip.Delete();
            }
        }
    }
}
