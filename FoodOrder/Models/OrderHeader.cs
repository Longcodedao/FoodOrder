using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Models
{
    public enum shoppingOptions
    {
        Basic,
        Standard,
        Premium
    };
    public class OrderHeader
    {
       

        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]  
        public DateTime OrderDate { get; set; }
        public DateTime TimeofPick { get; set; }
        public DateTime DateOfPick { get; set; }

        public int NumItems { get; set; } = 0;
        public double OrderTotal { get; set; } = 0;
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public string AddressReceive { get; set; }
        public int PackingMoney { get; set; }
        public shoppingOptions ShopOptions { get; set; }

        public int BillTotal { get; set; }

        public string ConfirmByAdmin { get; set; } = "NOT CONFIRM";

        public bool FinishPopup { get; set; } = false;

    }
}
