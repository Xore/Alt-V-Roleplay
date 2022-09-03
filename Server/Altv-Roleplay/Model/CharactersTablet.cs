using AltV.Net;
using AltV.Net.Data;
using Altv_Roleplay.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altv_Roleplay.Model
{
    class CharactersTablet
    {
        public static List<Characters_Tablet_Apps> CharactersTabletApps_ = new List<Characters_Tablet_Apps>();
        public static List<Server_Tablet_Apps> ServerTabletAppsData_ = new List<Server_Tablet_Apps>();
        public static List<Server_Tablet_Events> ServerTabletEventsData_ = new List<Server_Tablet_Events>();
        public static List<Server_Tablet_Notes> ServerTabletNotesData_ = new List<Server_Tablet_Notes>();

        public static void CreateCharacterTabletAppEntry(int charId, bool weather, bool news, bool banking, bool lifeinvader, bool vehicles, bool events, bool company, bool notices)
        {
            if (charId == 0) return;
            var appData = new Characters_Tablet_Apps
            {
                charId = charId,
                weather = weather,
                news = news,
                banking = banking,
                lifeinvader = lifeinvader,
                vehicles = vehicles,
                events = events,
                company = company,
                notices = notices
            };

            try
            {
                CharactersTabletApps_.Add(appData);
                using (gtaContext db = new gtaContext())
                {
                    db.Characters_Tablet_Apps.Add(appData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static bool ExistCharacterTabletAppEntry(int charId)
        {
            var tabletData = CharactersTabletApps_.FirstOrDefault(x => x.charId == charId);
            if (tabletData != null) return true;
            return false;
        }

        public static void CreateServerTabletEvent(int charId, string title, string callNumber, string eventDate, string eventTime, string location, string eventType, string info)
        {
            try
            {
                if (charId == 0) return;
                var eventData = new Server_Tablet_Events
                {
                    charId = charId,
                    title = title,
                    ownerName = Characters.GetCharacterName(charId),
                    callnumber = callNumber,
                    location = location,
                    eventtype = eventType,
                    date = eventDate,
                    time = eventTime,
                    info = info,
                    created = DateTime.Now
                };

                ServerTabletEventsData_.Add(eventData);
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Tablet_Events.Add(eventData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void CreateServerTabletNote(int charId, string title, string text, string color)
        {
            try
            {
                if (charId == 0 || title == "" || text == "" || color == "") return;
                var noteData = new Server_Tablet_Notes
                {
                    charId = charId,
                    color = color,
                    title = title,
                    text = text,
                    created = DateTime.Now
                };

                ServerTabletNotesData_.Add(noteData);
                using (gtaContext db = new gtaContext())
                {
                    db.Server_Tablet_Notes.Add(noteData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void DeleteServerTabletNote(int charId, int noteId)
        {
            try
            {
                if (charId == 0 || noteId == 0) return;
                var noteData = ServerTabletNotesData_.FirstOrDefault(x => x.id == noteId && x.charId == charId);
                if (noteData != null)
                {
                    ServerTabletNotesData_.Remove(noteData);
                    using (gtaContext db = new gtaContext())
                    {
                        db.Server_Tablet_Notes.Remove(noteData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static int GetServerTabletAppPrice(string appName)
        {
            try
            {
                if (appName == "") return 999999999;
                var tabletData = ServerTabletAppsData_.FirstOrDefault(x => x.appName == appName);
                if (tabletData != null)
                {
                    return tabletData.appPrice;
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return 999999999;
        }

        public static bool HasCharacterTabletApp(int charId, string AppName)
        {
            try
            {
                if (charId == 0 || AppName == "") return false;
                var tabletData = CharactersTabletApps_.FirstOrDefault(x => x.charId == charId);
                if (tabletData != null)
                {
                    switch (AppName)
                    {
                        case "weather": return tabletData.weather;
                        case "news": return tabletData.weather;
                        case "banking": return tabletData.banking;
                        case "lifeinvader": return tabletData.lifeinvader;
                        case "vehicles": return tabletData.vehicles;
                        case "events": return tabletData.events;
                        case "company": return tabletData.company;
                        case "notices": return tabletData.notices;
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return false;
        }

        public static void ChangeCharacterTabletAppInstallState(int charId, string appName, bool isInstalled)
        {
            try
            {
                if (charId == 0 || appName == "") return;
                var tabletData = CharactersTabletApps_.FirstOrDefault(x => x.charId == charId);
                if (tabletData != null)
                {
                    switch (appName)
                    {
                        case "weather": tabletData.weather = isInstalled; break;
                        case "news": tabletData.news = isInstalled; break;
                        case "banking": tabletData.banking = isInstalled; break;
                        case "lifeinvader": tabletData.lifeinvader = isInstalled; break;
                        case "vehicles": tabletData.vehicles = isInstalled; break;
                        case "events": tabletData.events = isInstalled; break;
                        case "company": tabletData.company = isInstalled; break;
                        case "notices": tabletData.notices = isInstalled; break;
                    }

                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Tablet_Apps.Update(tabletData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static void SetCharacterTabletApps(int charId, bool weather, bool news, bool banking, bool lifeinvader, bool vehicles, bool events, bool company, bool notices)
        {
            try
            {
                if (charId == 0) return;
                var tabletData = CharactersTabletApps_.FirstOrDefault(x => x.charId == charId);
                if (tabletData != null)
                {
                    tabletData.weather = weather;
                    tabletData.news = news;
                    tabletData.banking = banking;
                    tabletData.lifeinvader = lifeinvader;
                    tabletData.vehicles = vehicles;
                    tabletData.events = events;
                    tabletData.company = company;
                    tabletData.notices = notices;

                    using (gtaContext db = new gtaContext())
                    {
                        db.Characters_Tablet_Apps.Update(tabletData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
        }

        public static string GetCharacterTabletApps(int charId)
        {
            if (charId == 0) return "";

            var items = CharactersTabletApps_.Where(x => x.charId == charId).Select(x => new
            {
                x.weather,
                x.news,
                x.banking,
                x.lifeinvader,
                x.vehicles,
                x.events,
                x.notices,
                factionmanager = ServerFactions.IsCharacterInAnyFaction(charId) && (ServerFactions.GetCharacterFactionRank(charId) >= ServerFactions.GetFactionMaxRankCount(ServerFactions.GetCharacterFactionId(charId)) - 2),
                gangmanager = ServerGangs.IsCharacterInAnyGang(charId) && (ServerGangs.GetCharacterGangRank(charId) >= ServerGangs.GetGangMaxRankCount(ServerGangs.GetCharacterGangId(charId)) - 2),
                lspdapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 2,
                fbiapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 12,
                lsfdapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 3,
                aclsapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 4,
                taxiapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 16,
                justiceapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 1,
                bennysapp = ServerFactions.IsCharacterInAnyFaction(charId) && ServerFactions.IsCharacterInFactionDuty(charId) && ServerFactions.GetCharacterFactionId(charId) == 14,
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetCharacterTabletBankingAppOwnerInfo(int charId)
        {
            if (charId == 0) return "";
            var items = CharactersBank.CharactersBank_.Where(x => x.charId == charId && x.mainAccount == true).Select(x => new
            {
                charname = Characters.GetCharacterName(charId),
                banknumber = x.accountNumber,
                bankmoney = x.money,
                banksubdivision = x.createZone,
            }).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetServerTabletEvents()
        {
            var items = ServerTabletEventsData_.Where(x => DateTime.Now.Subtract(Convert.ToDateTime(x.created)).TotalHours < 168).Select(x => new
            {
                x.id,
                x.title,
                owner = x.ownerName,
                x.callnumber,
                x.location,
                x.eventtype,
                x.date,
                x.time,
                x.info,
            }).OrderByDescending(x => x.id).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetCharacterTabletNotes(int charId)
        {
            if (charId == 0) return "";
            var items = ServerTabletNotesData_.Where(x => x.charId == charId).Select(x => new
            {
                x.id,
                x.color,
                x.title,
                x.text,
            }).OrderByDescending(x => x.id).ToList();

            return JsonConvert.SerializeObject(items);
        }

        public static string GetCharacterTabletVehicles(int charId)
        {
            if (charId == 0) return "";
            var items = ServerVehicles.ServerVehicles_.Where(x => x.charid == charId).Select(x => new
            {
                x.id,
                name = ServerVehicles.GetVehicleNameOnHash(x.hash),
                x.plate,
                lastgarage = ServerGarages.GetGarageName(x.garageId),
                parkstate = x.isInGarage,
                hasgps = true,
                posX = GetCharacterTabletVehiclePosition(x.id).X,
                posY = GetCharacterTabletVehiclePosition(x.id).Y,
            }).ToList();
            return JsonConvert.SerializeObject(items);
        }

        public static Position GetCharacterTabletVehiclePosition(int vehId)
        {
            try
            {
                if (vehId == 0) return new Position(0, 0, 0);
                var vehicle = ServerVehicles.ServerVehicles_.FirstOrDefault(x => x.id == vehId);
                if (vehicle != null)
                {
                    if (vehicle.isInGarage) { return ServerGarages.GetGarageSlotPosition(vehicle.garageId, 1); }
                    else { return new Position(vehicle.posX, vehicle.posY, vehicle.posZ); }
                }
            }
            catch (Exception e)
            {
                Alt.Log($"{e}");
            }
            return new Position(0, 0, 0);
        }
    }
}
