using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthereumAPI.Models
{
    public interface iGateKeeper
    {

        string GetBalance(string senderAddress);

        string Transfer(string receiverAddress, string amount, string senderAddress, string privateKey);

        string CreateAccount();
    }
}
