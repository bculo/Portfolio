using Crypto.Application.Interfaces.Services;
using HashidsNet;

namespace Crypto.Infrastructure.Services
{
    public class IdentifierHasher : IIdentiferHasher
    {
        private readonly Hashids _hasher;

        public IdentifierHasher(Hashids hasher)
        {
            _hasher = hasher;
        }

        public long Decode(string value)
        {
            return _hasher.DecodeLong(value).First();
        }

        public string Encode(long value)
        {
            return _hasher.EncodeLong(value);
        }
    }
}
