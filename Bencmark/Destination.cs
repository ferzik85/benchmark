using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreakAwayConsole
{
  public class Destination
  {
    [Index(IsUnique = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid DestinationId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Description { get; set; }  

  }
}