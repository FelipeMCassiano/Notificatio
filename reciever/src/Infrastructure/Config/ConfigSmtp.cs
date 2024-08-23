namespace reciever.Infrastructure.Config;
public class SmtpConfig
{
    public string? host { get; set; }
    public int port { get; set; }
    public string? passowrd { get; set; }
    public bool enableSsl { get; set; }

}
