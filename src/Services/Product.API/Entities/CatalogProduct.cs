using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains;

namespace Product.API.Entities;

public class CatalogProduct :EntityAuditBase<long>
{
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string No{ get; set; }
    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string Name{ get; set; }
    [Column(TypeName = "nvarchar(250)")]
    public string Summary{ get; set; }
    public string Description{ get; set; }
    public decimal Price{ get; set; }
  //  public int StockQuantity{ get; set; }
}