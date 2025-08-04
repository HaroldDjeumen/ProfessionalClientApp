using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.MVVM.Model
{
    internal class WandEFolderModel
    {
        public required int Id { get; set; }
        public required string PropertyName { get; set; }
        public required string Roomname { get; set; }
        public required string RoomClick { get; set; }
        public required string WaterType { get; set; }
        public required string ElecType { get; set; }
    }
}
