using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shared.Data;

[Index(nameof(Date), IsDescending = new[] { true })]
public class Hashes
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? RandomStringSha1 { get; set; }
}
