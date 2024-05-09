using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RepairServiceWeb
{
    public class AutorizationOptions // Класс для настроек авторизации
    {
        public const string ISSUER = "Server"; // Издатель токена
        
        public const string AUDIENCE = "Client"; // Аудитория токена
        
        const string KEY = "cegthctrhtnysqrk.x100ghwtynjd,tpjgfcyjcnb"; // Секретный ключ для подписи токена

        /// <summary>
        /// Метод для получения симметричного ключа безопасности из секретного ключа
        /// </summary>
        /// <returns>Симметричный ключ безопасности</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
