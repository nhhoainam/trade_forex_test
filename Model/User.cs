using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HandleMessage.Model;

[Table("Users")]
public class User
{
    [Key]
    public int id { get; set; }
    public string? email { get; set; }
    public string? password { get; set; }
    public Single? point { get; set; }
    public long? chatIdLogin { get; set; }
    public string? roles { get; set; }
    public bool Lock { get; set; }
    public DateTimeOffset? createdAt { get; set; }
    public DateTimeOffset? updatedAt { get; set; }
    public DateTimeOffset? deletedAt { get; set; }
    public string? addressPayment { get; set; }
    [JsonIgnore]
    public ICollection<Account> accounts { get; set; }
}