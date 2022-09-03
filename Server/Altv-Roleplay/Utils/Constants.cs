using AltV.Net.Data;

namespace Altv_Roleplay.Utils
{
    public static class Constants
    {
        public static class DatabaseConfig
        {
            //------------------------------------------------------------//
            //Live Server
            //------------------------------------------------------------///
            public static string Host = "localhost";
            public static string User = "altv";
            public static string Password = "fm.I7fUM+QpCSU!x#S";
            public static string Port = "3306";
            public static string Database = "altv";
            //------------------------------------------------------------//
            //Dev Server
            //------------------------------------------------------------//
            //public static string Host = "localhost";
            //public static string User = "altv_dev";
            //public static string Password = ".n$xHbP*qbgt,HGS2I";
            //public static string Port = "3306";
            //public static string Database = "altvdev";
            //------------------------------------------------------------//
            //------------------------------------------------------------//
            //Dev Server
            //------------------------------------------------------------//
            //public static string Host = "localhost";
            //public static string User = "root";
            //public static string Password = "root";
            //public static string Port = "3306";
            //public static string Database = "altv";
            //------------------------------------------------------------//
        }

        public static class Positions
        {
            public static readonly Position Empty = new Position(0, 0, 0);
            //TOWNHALL
            public static readonly Position IdentityCardApply = new Position((float)-550.114, (float)-190.167, (float)37.000);
            public static readonly Position TownhallHouseSelector = new Position((float)-530.018, (float)-191.196, (float)37.210);
            //VEH-LIC-SELL-KEY
            public static readonly Position VehicleLicensing_Position = new Position((float)240.567, (float)-1379.380, (float)34.728);
            public static readonly Position VehicleLicensing_VehPosition = new Position((float)224.281, (float)-1379.907, (float)31.493);
            public static readonly Position LicensePoint = new Position((float)337.358, (float)-1562.650, (float)30.290);
            //
            public static readonly Position VehicleKey_Position = new Position((float)167.301, (float)-1804.04, (float)28.313);
            public static readonly Position VehicleKey_VehPosition = new Position((float)174.23737, (float)-1808.9011, (float)28.302612);
            //
            public static readonly Position VehicleSell_Position = new Position((float)-323.287, (float)-1534.140, (float)28.712);
            public static readonly Position VehicleSell_VehPosition = new Position((float)-325.595, (float)-1530.092, (float)28.527);
            //ACLS
            public static readonly Position AutoClubLosSantos_StoreVehPosition = new Position((float)400.4967, (float)-1632.4088, (float)29.279907);
            //TUNING
            public static readonly Position BennyPosition = new Position((float)-211.58241, (float)-1323.5077, (float)30.880615);
            public static readonly Position WP_Position_01 = new Position((float)810.567, (float)-967.17365, (float)26.095215);
            public static readonly Position WP_Position_03 = new Position((float)-1161.0593, (float)-1695.0198, (float)4.4432373);
            //SPAWN
            public static readonly Position SpawnPos_Airport = new Position((float)-1042.1143, (float)-2745.0857, (float)21.343628);
            public static readonly Rotation SpawnRot_Airport = new Rotation(0, 0, (float)0.44526514);
            //MINIJOBS
            public static readonly Position Minijob_Elektrolieferent_StartPos = new Position((float)677.644, (float)73.99121, (float)83.11499);
            public static readonly Position Minijob_Elektrolieferant_VehOutPos = new Position((float)694.11426, (float)51.375824, (float)83.5531);
            public static readonly Rotation Minijob_Elektrolieferant_VehOutRot = new Rotation((float)-0.015625, (float)0.0625, (float)-2.078125);
            //
            public static readonly Position Minijob_Müllmann_StartPos = new Position((float)-617.0723266601562, (float)-1622.7850341796875, (float)33.010528564453125);
            public static readonly Position Minijob_Müllmann_VehOutPos = new Position((float)-591.8637, (float)-1586.2814, (float)25.977295);
            public static readonly Rotation Minijob_Müllmann_VehOutRot = new Rotation(0, 0, (float)1.453125);
            //
            public static readonly Position Minijob_Busdriver_StartPos = new Position((float)454.12713623046875, (float)-600.075927734375, (float)28.578372955322266);
            public static readonly Position Minijob_Busdriver_VehOutPos = new Position((float)466.33847, (float)-579.0725, (float)27.729614);
            public static readonly Rotation Minijob_Busdriver_VehOutRot = new Rotation(0, 0, (float)3.046875);
            //CARRENTAL
            //CAYO
            public static readonly Position CarRental_StartPos = new Position((float)4917.2573, (float)-5227.1343, (float)2.5054932);
            public static readonly Position CarRental_VehOutPos = new Position((float)4918.0615, (float)-5235.297, (float)2.5054932);
            public static readonly Rotation CarRental_VehOutRot = new Rotation(0, 0, (float)38.686);
            //HOTEL
            public static readonly Position Hotel_Apartment_ExitPos = new Position((float)266.08685302734375, (float)-1007.5635986328125, (float)-101.00853729248047);
            public static readonly Position Hotel_Apartment_StoragePos = new Position((float)265.9728698730469, (float)-999.4517211914062, (float)-99.00858306884766);
            //CLOTHES
            public static readonly Position Clothes_Police = new Position((float)461.57803, (float)-999.75824, (float)29.678345);
            public static readonly Position Clothes_FBI = new Position((float)144.9245, (float)-761.5264, (float)241.4813);
            public static readonly Position Clothes_Medic = new Position((float)-438.1978, (float)-307.64835, (float)33.907715);
            public static readonly Position Clothes_ACLS = new Position((float)-203.1956, (float)-1328.1362, (float)33.89087);
            public static readonly Position Clothes_Bennys = new Position((float)-213.389, (float)-1332.0396, (float)22.129639);
            //DIV
            public static readonly Position ProcessTest = new Position((float)-252.05, (float)-971.736, (float)31.21);
            public static readonly Position Arrest_Position = new Position(1690.7076f, 2576.611f, 46.910645f);
            //Waschstraßen
            public static readonly Position Waschstrasse = new Position((float)24.487913, (float)-1391.9868, (float)29.330444);
            public static readonly Position Waschstrasse2 = new Position((float)-699.9033, (float)-935.9077, (float)19.001465);
            // Lagerhallen Positions
            public static readonly Position storage_ExitPosition = new Position(1087.556f, -3099.468f, -39.01245f);
            public static readonly Position storage_InvPosition = new Position(1095.5472f, -3098.3604f, -39.01245f);
            public static readonly Position storage_LSPDInvPosition = new Position(485.4066f, -995.68353f, 30.678345f);
            // Labor Positions
            public static readonly Position methLabor_ExitPosition = new Position(997.2f, -3200.7166f, -36.400757f);
            public static readonly Position methLabor_InvPosition = new Position(1005.7319f, -3200.3076f, -38.523804f);
            public static readonly Position weedLabor_ExitPosition = new Position(1065.9956f, -3183.4812f, -39.164062f);
            public static readonly Position weedLabor_InvPosition = new Position(1039.2263f, -3205.3845f, -38.16992f);
            // Dynasty 8 Positions
            public static readonly Position dynasty8_pedPositionShop = new Position(-703.041f, 266.189f, 82.131f);
            public static readonly float dynasty8_pedRotationShop = 29.940f;
            public static readonly Position dynasty8_positionShop = new Position(-704.4132f, 267.863f, 82.331f);
            public static readonly Position dynasty8_pedPositionStorage = new Position(-706.5231f, 264.567f, 82.131f);
            public static readonly float dynasty8_pedRotationStorage = 25.817f;
            public static readonly Position dynasty8_positionStorage = new Position(-707.894f, 266.3209f, 82.131f);
        }
    }
}
