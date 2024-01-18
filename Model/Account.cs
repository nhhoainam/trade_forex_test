using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandleMessage.Model;

[Table("Accounts")]
public class Account
{
    [Key]
    public int id { get; set; }
    public int? username { get; set; }
    public string? accountName { get; set; }
    public string? password { get; set; }
    public string? oldPassword { get; set; }
    public string? serverBroker { get; set; }
    public string? host { get; set; }
    public int? port { get; set; }
    public string? typeAcc { get; set; }
    public string? proxyUsername { get; set; }
    public string? proxyPassword { get; set; }
    public string? proxyHost { get; set; }
    public int? proxyPort { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset updatedAt { get; set; }
    public DateTimeOffset? deletedAt { get; set; }
    public User User { get; set; }
    public int IdUser { get; set; }
}