
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace HomeLibrary.Infrastructure
{
    public class InvitationTokenProvider
    {
        private IDataProtector Protector { get; set; }
        public TimeSpan TokenLifeSpan {get;set;} = TimeSpan.FromDays(1);

         public InvitationTokenProvider(IDataProtectionProvider dataProtectionProvider, string Name = "DataProtectorTokenProvider")
         {
            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            Protector = dataProtectionProvider.CreateProtector(Name); 
         }

         public string Generate(string email)
         {             
            var ms = new MemoryStream();
            using (var writer = ms.CreateWriter())
            {
                writer.Write(DateTimeOffset.UtcNow);
                writer.Write(email);
               
            }
            var protectedBytes = Protector.Protect(ms.ToArray());

            return Convert.ToBase64String(protectedBytes);
        }
    

        public bool Validate(string token, string email)
        {
                try
                {
                    var unprotectedData = Protector.Unprotect(Convert.FromBase64String(token));
                    var ms = new MemoryStream(unprotectedData);
                    using (var reader = ms.CreateReader())
                    {
                        var creationTime = reader.ReadDateTimeOffset();
                        var expirationTime = creationTime + TokenLifeSpan;
                        if (expirationTime < DateTimeOffset.UtcNow)
                        {
                            return false;
                        }

                        var readEmail = reader.ReadString();
                        var actualEmail = email;

                        if (readEmail != actualEmail)
                        {
                            return false;
                        }

                        return true;
                    }
                }          
                catch
                {   
                }
                return false;
        }
    }


    internal static class StreamExtensions
    {
        internal static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public static BinaryReader CreateReader(this Stream stream)
        {
            return new BinaryReader(stream, DefaultEncoding, true);
        }

        public static BinaryWriter CreateWriter(this Stream stream)
        {
            return new BinaryWriter(stream, DefaultEncoding, true);
        }

        public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
        {
            return new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
        }

        public static void Write(this BinaryWriter writer, DateTimeOffset value)
        {
            writer.Write(value.UtcTicks);
        }
    }

}