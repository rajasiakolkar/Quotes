using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuotesAPI.Models
{
    public class Quote
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        public string Type { get; set; }

        public DateTime CreatedOn { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
    }
}
