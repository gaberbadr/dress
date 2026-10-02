using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string MainImage { get; set; } = string.Empty;
        public List<string> AdditionalImages { get; set; } = new List<string>();

        public bool IsAvailable { get; set; } = true;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<string> Sizes { get; set; } = new List<string>();
        
        public List<ProductFeature> Features { get; set; } = new List<ProductFeature>();
    }
}
