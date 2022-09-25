﻿using IotaWalletNet.Application.AccountContext.Commands.SendAmount;
using IotaWalletNet.Domain.Options;

namespace IotaWalletNet.Domain.Common.Interfaces
{
    public interface IWallet
    {
        ClientOptionsBuilder ConfigureClientOptions();
        SecretManagerOptionsBuilder ConfigureSecretManagerOptions();
        WalletOptionsBuilder ConfigureWalletOptions();
        void Connect();
        Task<string> CreateAccountAsync(string username);
        Task<string> GetAccountAsync(string username);
        Task<string> GetBalanceAsync(string username);
        Task<string> GetNewMnemonicAsync();
        WalletOptions GetWalletOptions();
        Task<string> SendAmountAsync(string username, AddressesWithAmountAndTransactionOptions addressesWithAmount);
        void SendMessage(string message);

        Task<string> SendMessageAsync(string message);
        Task<string> StoreMnemonicAsync(string mnemonic);
        Task<string> VerifyMnemonicAsync(string mnemonic);
    }
}