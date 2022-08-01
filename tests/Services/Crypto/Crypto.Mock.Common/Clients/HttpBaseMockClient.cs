using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Mock.Common.Common
{
    public abstract class HttpBaseMockClient
    {
        protected readonly bool _isAuthorized;
        protected readonly bool _throwTimeOutException;

        public HttpBaseMockClient(bool isAuthorized, bool throwTimeOutException)
        {
            _isAuthorized = isAuthorized;
            _throwTimeOutException = throwTimeOutException;
        }

        protected virtual async Task Wait(int miliseconds)
        {
            if(miliseconds <= 0)
            {
                return;
            }

            await Task.Delay(miliseconds);
        }
    }
}
