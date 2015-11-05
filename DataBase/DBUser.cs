using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [Table("users")]
    public class DBUser
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public string Code { get; set; }

        public bool IsDead { get; set; }

        public int ShipPresetId { get; set; }
        
        public string CellTypesString { get; set; }
        public int[] CellTypes
        {
            get
            {
                string[] types = CellTypesString.Split(',');
                int[] result = new int[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    result[i] = int.Parse(types[i]);
                }
                return result;
            }
            set
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < value.Length; i++)
                {
                    builder.Append(value[i].ToString());
                    builder.Append(",");
                }
                builder.Remove(builder.Length - 1, 1);//Remove last ","
                CellTypesString = builder.ToString();
            }
        }
    }
}
