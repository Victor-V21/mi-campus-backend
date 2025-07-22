using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("chats")]
    public class ChatEntity : BaseEntity
    {
        [Column("id_emisor")]
        public string EmisorId { get; set; }

        [ForeignKey("EmisorId")]
        public UserEntity Emisor { get; set; }

        [Column("id_receptor")]
        public string ReceptorId { get; set; }

        [ForeignKey("ReceptorId")]
        public UserEntity Receptor { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("date_send")]
        public DateTime DateSend { get; set; }

        [Column("date_modify")]
        public DateTime DateModify { get; set; }

        [Column("received")]
        public bool Received { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }
    }
}
