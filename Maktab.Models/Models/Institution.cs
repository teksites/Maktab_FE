using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Models.Models
{
     public class Institution
     {
          public Guid Id { get; set; }
          public string Name { get; set; }
          public string Description { get; set; }
          public string Type { get; set; }

     }
}
