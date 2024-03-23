using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RepairServiceWeb.Controllers
{
    public class AutorizationOptions
    {
        public const string ISSUER = "Server";
        public const string AUDIENCE = "Client";
        const string KEY = "cegthctrhtnysqrk.x100ghwtynjd,tpjgfcyjcnb";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
