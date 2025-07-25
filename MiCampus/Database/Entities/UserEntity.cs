﻿using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace MiCampus.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("no_account")]
        public int NoAccount { get; set; }

        [Column("id_campus")]
        public string CampusId { get; set; }

        [ForeignKey("CampusId")]
        public CampusEntity Campus { get; set; }
    }
}
