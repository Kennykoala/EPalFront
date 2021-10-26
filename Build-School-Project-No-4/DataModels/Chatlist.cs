namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chatlist")]
    public partial class Chatlist
    {
        public int ChatlistId { get; set; }

        public int SenderId { get; set; }

        public int? ReceiverId { get; set; }

        public int MessageTypeId { get; set; }

        public string MessageContent { get; set; }

        public DateTime? SendOrReceiveDateTime { get; set; }

        public virtual MessageType MessageType { get; set; }

        public virtual Members Members { get; set; }

        public virtual Members Members1 { get; set; }
    }
}
