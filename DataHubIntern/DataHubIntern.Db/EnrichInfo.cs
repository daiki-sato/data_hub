using System.ComponentModel.DataAnnotations;

namespace DataHubIntern.Db
{
  public class EnrichInfo
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Soc { get; set; }

    [MaxLength(100)]
    public string MainIndustrialClassName { get; set; }

    public int LatestSalesAccountingTermSalesGreaterThan { get; set; }

    public int LatestSalesAccountingTermSalesLessThan { get; set; }
  }
}
