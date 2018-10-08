using Barcamp.Bot.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Barcamp.Bot.Database
{
    public class Barcamp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Titel { get; set; }
        public CostCategory Cost { get; set; }

        public string Description { get; set; }
        public string Link { get; set; }
        public string Hashtag { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public LocationTime Time { get; internal set; }

        public override string ToString() => Titel + " " + Time;

        public enum CostCategory
        {
            /// <summary>
            /// Not specified
            /// </summary>
            None,
            /// <summary>
            /// Free Barcamp
            /// </summary>
            Free,
            /// <summary>
            /// Cost contribution up to 50 Euro
            /// </summary>
            Post,
            /// <summary>
            /// Costs from 50 Euro
            /// </summary>
            Duty
        }
    }
}
