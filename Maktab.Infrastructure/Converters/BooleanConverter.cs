using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maktab.Infrastructure.Converters
{
     public class BoolConverter : JsonConverter<bool>
     {
          private static readonly UTF8Encoding encoder = new(false);
          public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
          {
               // if we already have a bool that is true, return immediately
               if (reader.TokenType == JsonTokenType.True)
               {
                    return true;
               }

               return false;
          }

          public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
          {
               throw new NotImplementedException();
          }
     }
}
