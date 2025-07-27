using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.MVVM.Model
{
     class RoomDetailModel
    {
        public required int Id { get; set; }
        public required string RoomName { get; set; }
        public required string RoomType { get; set; }
        public required string Status { get; set; }
        public required string TenantName { get; set; }
        public required string StayPeriod { get; set; }
        public required string RentPaidStatus { get; set; }
        public required string RoomImage { get; set; }
        public required string Lightcolor { get; set; }
        public required string Darkcolor { get; set; }


    }
}
