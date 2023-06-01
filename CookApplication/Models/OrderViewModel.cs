using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookApplication.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Products { get; set; }
    }
}
