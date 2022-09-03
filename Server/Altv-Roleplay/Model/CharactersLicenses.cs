using AltV.Net;
using Altv_Roleplay.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altv_Roleplay.Model
{
    class CharactersLicenses
    {
        public static List<Characters_Licenses> CharactersLicenses_ = new List<Characters_Licenses>();
        public static List<Server_Licenses> ServerLicenses_ = new List<Server_Licenses>();

        public static void CreateCharacterLicensesEntry(int charId, bool pkw, bool lkw, bool bike, bool boat, bool fly, bool helicopter, bool passengertransport, bool weaponlicense)
        {
            if (charId <= 0) return;
            var licenseData = new Characters_Licenses
            {
                charId = charId,
                PKW = pkw,
                LKW = lkw,
                Bike = bike,
                Boat = boat,
                Fly = fly,
                Helicopter = helicopter,
                PassengerTransport = passengertransport,
                weaponlicense = weaponlicense
            };

            try
            {
                CharactersLicenses_.Add(licenseData);
                using (gtaContext db = new gtaContext())
                {
                    db.Characters_Licenses.Add(licenseData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static bool ExistCharacterLicenseEntry(int charId)
        {
            var licData = CharactersLicenses_.FirstOrDefault(x => x.charId == charId);
            if (licData != null) return true;
            return false;
        }

        public static bool HasCharacterLicense(int charId, string lic)
        {
            try
            {
                if (charId <= 0) return false;
                var licData = CharactersLicenses_.FirstOrDefault(x => x.charId == charId);
                if (licData != null)
                {
                    switch (lic)
                    {
                        case "pkw": return licData.PKW;
                        case "lkw": return licData.LKW;
                        case "bike": return licData.Bike;
                        case "boat": return licData.Boat;
                        case "fly": return licData.Fly;
                        case "helicopter": return licData.Helicopter;
                        case "passengertransport": return licData.PassengerTransport;
                        case "weaponlicense": return licData.weaponlicense;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static string GetFullLicenseName(string licShort)
        {
            try
            {
                if (licShort == "") return "None";
                var licData = ServerLicenses_.FirstOrDefault(x => x.licCut == licShort);
                if (licData != null)
                {
                    return licData.licName;
                }
                if (licShort == "weaponlicense") return "Waffen-Schein";
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return "None";
        }

        public static int GetLicensePrice(string licShort)
        {
            try
            {
                if (licShort == "") return 0;
                var licData = ServerLicenses_.FirstOrDefault(x => x.licCut == licShort);
                if (licData != null)
                {
                    return licData.licPrice;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 0;
        }

        public static void SetCharacterLicense(int charId, string lic, bool valid)
        {
            try
            {
                if (charId <= 0 || lic == "") return;
                var licData = CharactersLicenses_.FirstOrDefault(x => x.charId == charId);
                if (licData != null)
                {
                    switch (lic)
                    {
                        case "pkw": 
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.PKW = valid;
                            break;
                        case "lkw":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.LKW = valid; 
                            break;
                        case "bike":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.Bike = valid; 
                            break;
                        case "boat":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.Boat = valid; 
                            break;
                        case "fly":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.Fly = valid; 
                            break;
                        case "helicopter":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.Helicopter = valid; 
                            break;
                        case "passengertransport":
                            if (CharactersInventory.ExistCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", "brieftasche"))
                            {
                                // nothing
                            }
                            else
                            {
                                CharactersInventory.AddCharacterItem(charId, $"Fuehrerschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            }
                            licData.PassengerTransport = valid; 
                            break;
                        case "weaponlicense": 
                            licData.weaponlicense = valid;
                            CharactersInventory.AddCharacterItem(charId, $"Waffenschein {Characters.GetCharacterName(charId)}", 1, "brieftasche");
                            break;
                    }

                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Licenses.Update(licData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static string GetCharacterLicenses(int charId)
        {
            if (charId <= 0) return "";

            var items = CharactersLicenses_.Where(x => x.charId == charId).Select(x => new
            {
                x.charId,
                x.PKW,
                pkwName = "C",
                PKWPrice = GetLicensePrice("pkw"),
                x.LKW,
                lkwName = "B",
                LKWPrice = GetLicensePrice("lkw"),
                x.Bike,
                bikeName = "M",
                BikePrice = GetLicensePrice("bike"),
                x.Boat,
                boatName = "BL",
                BoatPrice = GetLicensePrice("boat"),
                x.Fly,
                flyName = "FL",
                FlyPrice = GetLicensePrice("fly"),
                x.Helicopter,
                heliName = "HL",
                HelicopterPrice = GetLicensePrice("helicopter"),
                x.PassengerTransport,
                passengerTransportName = "C-CDL",
                PassengerTransportPrice = GetLicensePrice("passengertransport"),
                x.weaponlicense,
                weaponlicenseName = "Waffen-Schein",
                weaponlicensePrice = GetLicensePrice("weaponlicense"),
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetDriverLicenses(int charId)
        {
            try
            {
                if (charId <= 0) return "[]";
                var items = CharactersLicenses_.Where(x => x.charId == charId).Select(x => new
                {
                    x.charId,
                    charname = Characters.GetCharacterName(charId),
                    birthdate = Characters.GetCharacterBirthdate(charId),
                    x.PKW,
                    pkwName = "C",
                    x.LKW,
                    lkwName = "B",
                    x.Bike,
                    bikeName = "M",
                    x.Boat,
                    boatName = "BL",
                    x.Fly,
                    flyName = "FL",
                    x.Helicopter,
                    heliName = "HL",
                    x.PassengerTransport,
                    passengerTransportName = "C-CDL",

                }).ToList();
                return JsonConvert.SerializeObject(items);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return "[]";
            }
        }

        public static string GetWeaponLicense(int charId)
        {
            try
            {
                if (charId <= 0) return "[]";
                var items = CharactersLicenses_.Where(x => x.charId == charId).Select(x => new
                {
                    x.charId,
                    charname = Characters.GetCharacterName(charId),
                    birthdate = Characters.GetCharacterBirthdate(charId),
                    x.weaponlicense,
                    weaponlicenseName = "Waffen-Schein",

                }).ToList();
                return JsonConvert.SerializeObject(items);
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
                return "[]";
            }
        }

        public static bool ExistServerLicense(string licShort)
        {
            try
            {
                if (licShort == "") return false;
                var licData = ServerLicenses_.FirstOrDefault(x => x.licCut == licShort);
                if (licData != null) return true;
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }
    }
}
