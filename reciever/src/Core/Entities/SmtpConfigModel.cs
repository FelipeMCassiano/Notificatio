namespace reciever.Core.Entities;
public class SmtpConfigModel
{
    public string? host { get; set; }
    public int port { get; set; }
    public string? userName { get; set; }
    public string? password { get; set; }
    public bool enableSsl { get; set; }
}
