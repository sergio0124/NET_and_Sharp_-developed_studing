using System.Collections.Generic;
namespace LawFirmFileImplement.Models
{
    /// <summary>
    /// Изделие, изготавливаемое в магазине
    /// </summary>
    public class Document
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, int> DocumentBlanks { get; set; }
    }
}