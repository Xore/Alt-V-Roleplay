using AltV.Net.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Altv_Roleplay.models
{
    public partial class Server_Shops
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string name { get; set; }
        public int owner { get; set; }
        public int bank { get; set; }
        public int price { get; set; }
        public Position pedPos { get; set; }
        public Position shopPos { get; set; }
        public Position managePos { get; set; }
        public string pedModel { get; set; }
        public float pedRot { get; set; }
        public int blipColor { get; set; }
        public int blipSprite { get; set; }
        public bool isBlipVisible { get; set; }
        public int type { get; set; } // 0 = normal, 1 = Staatsfraktion (ohne Manage), 2 = Badfraktion (ohne Manage)
        public int faction { get; set; }
        public bool isOnlySelling { get; set; }
        public string neededLicense { get; set; }
        public int closed { get; set; }
        public int stateClosed { get; set; }

        [NotMapped]
        public bool isRobbedNow { get; set; } = false;
    }
}
