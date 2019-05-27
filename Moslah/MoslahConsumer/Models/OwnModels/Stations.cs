using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moslah.Models.OwnModels
{
    public class Stations
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        [ForeignKey("City")]
        public int cityID { get; set; }
        public virtual City City { set; get; }
        public string Name { get; set; }
        public string zone { get; set; }

        

    }
}