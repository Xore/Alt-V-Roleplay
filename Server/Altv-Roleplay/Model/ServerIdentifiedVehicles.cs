using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Altv_Roleplay.Factories;
using Altv_Roleplay.Handler;
using Altv_Roleplay.models;
using Altv_Roleplay.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Altv_Roleplay.Model
{
    class ServerIdentifiedVehicles
    {
            
        public static List<Server_IdentifiedVehicles> ServerIdentifiedVehicles_ = new List<Server_IdentifiedVehicles>();

        public static string getEntryByName(string firstname, string lastname)
        {
            string returnPlate = "None";
            if (firstname == "None" || lastname == "None") { return returnPlate; }

            try
            {
                Server_IdentifiedVehicles tmp1 = ServerIdentifiedVehicles_.FirstOrDefault(x => x.lastname == lastname && x.lastname == lastname);
                returnPlate = tmp1.plate;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                returnPlate = "None";
            }

            return returnPlate;
        }
        public static string getOwnerByPlate(string plate)
        {
            string returnOwner = "None";
            if (plate == "None") { return returnOwner; }

            try
            {
                Server_IdentifiedVehicles tmp1 = ServerIdentifiedVehicles_.FirstOrDefault(x => x.plate == plate);
                returnOwner = $"{tmp1.firstname} {tmp1.lastname}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                returnOwner = "None";
            }

            return returnOwner;
        }


        public static void addEntry(string plate, string firstname, string lastname)
        {
            try
            {
                var identifiedVehicleData = new Server_IdentifiedVehicles
                {
                    plate = plate,
                    firstname = firstname,
                    lastname = lastname
                };
                ServerIdentifiedVehicles_.Add(identifiedVehicleData);

                using (gtaContext db = new gtaContext())
                {
                    db.Server_IdentifiedVehicles.Add(identifiedVehicleData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }


    }
}

