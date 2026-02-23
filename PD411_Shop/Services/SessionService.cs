
using System.Text.Json;

namespace PD411_Shop.Services
{
    public static class SessionService
    {
        public static string Key { get; } = "qquryewfjsdfsafjwrhgipqeogjwheifsd";

        public static void Set<T>(this ISession session, T value)
        {
            session.SetString(Key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session)
        {
            var value = session.GetString(Key);
            if (value == null)
            {
                return default;
            }

            T? result = JsonSerializer.Deserialize<T>(value);
            return result;
        }
    }
}
