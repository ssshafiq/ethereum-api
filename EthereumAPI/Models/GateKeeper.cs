using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.StandardTokenEIP20.ContractDefinition;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Web;

namespace EthereumAPI.Models
{

    public class Gatekeeper : iGateKeeper
    {

        public string GetBalance(string senderAddress)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://api-ropsten.etherscan.io/api?module=contract&action=getabi&address=0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79").Result;

            var responseObj = response.Content.ReadAsStringAsync().Result;

            var contractABI = JsonConvert.DeserializeObject<ContractABI>(responseObj);

            if (!string.IsNullOrEmpty(contractABI.ToString()))
            {
                var web3 = new Web3("https://ropsten.infura.io/TJGN6GCM6J591N7DH5DK5QMC9UXN2NF4Z8");
                var MyContract = web3.Eth.GetContract(contractABI.result, "0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79");
                var contractbalance = MyContract.GetFunction("balanceOf");
                var result = contractbalance.CallAsync<dynamic>(senderAddress).Result;
                return result;
            }

            return "";

        }
        public string Transfer(string receiverAddress, string amount, string senderAddress = "0x7e2E7d9c5917D9399101054Ec69f5Ed19f256b19", string privateKey = "C1C2798F8D13B9FB9A016A4F01C999D8525655580C0FCBB4EDD7F2A10F4D9DDF")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //var res = CreateAccount();


            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://api-ropsten.etherscan.io/api?module=contract&action=getabi&address=0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79").Result;

            var responseObj = response.Content.ReadAsStringAsync().Result;

            var contractABI = JsonConvert.DeserializeObject<ContractABI>(responseObj);




            if (!string.IsNullOrEmpty(contractABI.ToString()))
            {
                var web3 = new Web3(new Account(privateKey), "https://ropsten.infura.io/TJGN6GCM6J591N7DH5DK5QMC9UXN2NF4Z8");
                //var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, "12410101", 60).Result;

                var transactionMessage = new TransferFunction()
                {
                    FromAddress = senderAddress,
                    To = receiverAddress,
                    Value = BigInteger.Multiply(BigInteger.Parse(amount), BigInteger.Parse("1000000000000000000")),
                    //Set our own price
                    GasPrice = Web3.Convert.ToWei(25, UnitConversion.EthUnit.Gwei)

                };

                var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();

                var estimate = transferHandler.EstimateGasAsync("0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79", transactionMessage).Result;
                transactionMessage.Gas = estimate.Value;

                var result = transferHandler.SendRequestAsync("0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79", transactionMessage).Result;

                //var MyContract = web3.Eth.GetContract(contractABI.result, "0xa989ccbb59e60ad9ed8f4ba39f03ae69a2c33e79");
                //var contractbalance = MyContract.GetFunction("transfer");
                //var result = contractbalance.SendTransactionAsync(senderAddress, receiverAddress, amount).Result;
                return result;
            }

            return "";


        }

        public string CreateAccount()
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var newPrivateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Account(newPrivateKey);

            var newSenderAddress = account.Address;
            var password = "password";

            var newAccount = new ManagedAccount(newSenderAddress, password);
            var web3 = new Web3(account);


            return "success";
        }
    }

    public class ContractABI
    {
        public string result { get; set; }
    }
}