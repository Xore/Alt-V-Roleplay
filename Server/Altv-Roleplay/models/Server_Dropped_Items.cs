using AltV.Net.Data;
using Altv_Roleplay.EntityStreamer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Altv_Roleplay.models
{
    public partial class Server_Dropped_Items
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string itemName { get; set; }
        public int itemAmount { get; set; }
        public int dimension { get; set; }
        public Position pos { get; set; }
        public DateTime droppedTimestamp { get; set; }

        [NotMapped]
        public PlayerLabel textLabel { get; set; } = null;

        [NotMapped]
        public EntityStreamer.Prop prop { get; set; } = null;
    }
}
