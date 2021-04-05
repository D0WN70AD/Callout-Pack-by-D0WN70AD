using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;

namespace CalloutPack
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
        }

        public override void Finally()
        {

        }

        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            if (OnDuty)
            {
                RegisterCallouts();
            }
        }

        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.StolenVehicle));
            Functions.RegisterCallout(typeof(Callouts.Fight));
            Functions.RegisterCallout(typeof(Callouts.GangAttack));
            Functions.RegisterCallout(typeof(Callouts.DriveBy));
            Functions.RegisterCallout(typeof(Callouts.WantedCriminalInCar));
            Functions.RegisterCallout(typeof(Callouts.HighSpeedCarChase));
            Functions.RegisterCallout(typeof(Callouts.DrugDealerSpotted));
            Functions.RegisterCallout(typeof(Callouts.WantedCriminalOnMotorcycle));
        }
    }
}