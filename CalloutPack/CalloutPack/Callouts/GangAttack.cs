using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack.Callouts
{

    [CalloutInfo("GangAttack", CalloutProbability.Medium)]
    class GangAttack : Callout
    {
        private Ped Suspect1;
        private Ped Suspect2;
        private Ped Suspect3;
        private Ped Suspect4;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip1;
        private Blip SuspectBlip2;
        private Blip SuspectBlip3;
        private Blip SuspectBlip4;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));

            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f); AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Gang Attack"; CalloutPosition = SpawnPoint;

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Suspect1 = new Ped(SpawnPoint, 31f);
            Suspect2 = new Ped(SpawnPoint, 30f);
            Suspect3 = new Ped(SpawnPoint, 32f);
            Suspect4 = new Ped(SpawnPoint, 33f);
            Suspect1.IsPersistent = true;
            Suspect2.IsPersistent = true;
            Suspect3.IsPersistent = true;
            Suspect4.IsPersistent = true;

            Suspect1.BlockPermanentEvents = true;
            Suspect2.BlockPermanentEvents = true;
            Suspect3.BlockPermanentEvents = true;
            Suspect4.BlockPermanentEvents = true;

            SuspectBlip1 = Suspect1.AttachBlip();
            SuspectBlip1.IsFriendly = false;
            SuspectBlip2 = Suspect2.AttachBlip();
            SuspectBlip2.IsFriendly = false;
            SuspectBlip3 = Suspect3.AttachBlip();
            SuspectBlip3.IsFriendly = false;
            SuspectBlip4 = Suspect4.AttachBlip();
            SuspectBlip4.IsFriendly = false;

            Suspect1.Health = 300;
            Suspect2.Health = 300;
            Suspect3.Health = 300;
            Suspect4.Health = 300;

            Suspect1.Inventory.GiveNewWeapon("Weapon_SMG", -1, true);
            Suspect2.Inventory.GiveNewWeapon("Weapon_Pistol", -1, true);
            Suspect3.Inventory.GiveNewWeapon("Weapon_PUMPSHOTGUN", -1, true);
            Suspect4.Inventory.GiveNewWeapon("Weapon_CARBINERIFLE", -1, true);

           
            Suspect1.Tasks.FightAgainst(Game.LocalPlayer.Character);
            Suspect2.Tasks.FightAgainst(Game.LocalPlayer.Character);
            Suspect3.Tasks.FightAgainst(Game.LocalPlayer.Character);
            Suspect4.Tasks.FightAgainst(Game.LocalPlayer.Character);

            Game.DisplayNotification("You need to kill them!");

            return base.OnCalloutAccepted();


        }

        public override void Process()
        {
            base.Process();




            if (Suspect1.IsDead && Suspect2.IsDead && Suspect3.IsDead && Suspect4.IsDead)
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
            Suspect3.Dismiss();
            Suspect4.Dismiss();


            SuspectBlip1.Delete();
            SuspectBlip2.Delete();
            SuspectBlip3.Delete();
            SuspectBlip4.Delete();

        }

    }
}
