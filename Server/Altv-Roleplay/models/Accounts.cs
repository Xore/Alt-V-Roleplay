using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Altv_Roleplay.models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Accounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int playerid { get; set; }
        public string playerName { get; set; }
        public ulong  hardwareId { get; set; }
        public string ip { get; set; }
        public int Online { get; set; } //CharakterID mit welchem der Spieler eingeloggt ist - 0 = offline.
        public bool whitelisted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ban { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string banReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adminLevel { get; set; }

    }
}
