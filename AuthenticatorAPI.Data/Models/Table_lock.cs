
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Models
{
    public class Table_lock
    {
        [Key]
        public int Fld_Lock_ID { get; set; }

        [Required]
        [StringLength(150)]
        public string Fld_Lock_Serial { get; set; }

        public int? Fld_LockBadaneh_ID { get; set; }

        [Required]
        [StringLength(150)]
        public string Fld_Lock_Model { get; set; }

        public string Fld_Lock_SharhL1 { get; set; }

        public string Fld_Lock_SharhL2 { get; set; }

        public string Fld_Lock_SharhL3 { get; set; }

        public int Fld_Lock_KindLock { get; set; }

        public int Fld_Lock_KindSystem { get; set; }

        public bool Fld_Lock_Tick { get; set; }

        public bool Fld_Lock_Remote { get; set; }

        public bool Fld_Lock_LocalDataBase { get; set; }

        public int? Fld_Moshtarian_ID { get; set; }

        public int? Fld_Moshtarian_IDNamayandeh { get; set; }

        public DateTime? Fld_Lock_MoshtarianDate { get; set; }

        public string Fld_Lock_ImportantDescriptionL1 { get; set; }

        public string Fld_Lock_ImportantDescriptionL2 { get; set; }

        public string Fld_Lock_ImportantDescriptionL3 { get; set; }

        public bool? Fld_Lock_BlackList { get; set; } = false;

        public string Fld_Lock_BlackListSharhL1 { get; set; }

        public string Fld_Lock_BlackListSharhL2 { get; set; }

        public string Fld_Lock_BlackListSharhL3 { get; set; }

        public int? Fld_Person_ID { get; set; }

        public bool Fld_Pub_Default { get; set; }

        public DateTime Fld_Pub_LogDateTime { get; set; }

        public int Fld_User_Code_In { get; set; }

        public int Fld_Shoab_Code_In { get; set; }

        [Required]
        [StringLength(255)]
        public string Fld_Pub_ComputerNameOrIP_In { get; set; }

        public int? Fld_Lock_Amani { get; set; }

        public int? Fld_Person_IDSaler { get; set; }

        public int? Fld_HokmKar_ID { get; set; }

        public bool? Fld_Namayandeh_Tick { get; set; }

        public int? Fld_Lock_UserCount { get; set; }

        public string Fld_Lock_PartitionString { get; set; }

        public int? Fld_Product_Id { get; set; }

        public bool? Fld_Lock_BlackListNotUsed { get; set; }

        public bool? Fld_Lock_Tasvye { get; set; }

        public int? Fld_Movaghat_Count { get; set; }

        [StringLength(50)]
        public string Fld_Code_Moshtari { get; set; }

        public string Fld_CPUid { get; set; }

        public int? Fld_Status_Work { get; set; }

        [StringLength(700)]
        public string Fld_Status_Work_Desc { get; set; }

        public bool? IsDeactive { get; set; }
    }
}
