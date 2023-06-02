using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierApplication.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public string ToStringProperty
        {
            get
            {
                return $"{OrderDate.ToString("dd.MM.yyyy HH:mm")} {Address} - {CustomerName}";
            }
        }
    }
}
