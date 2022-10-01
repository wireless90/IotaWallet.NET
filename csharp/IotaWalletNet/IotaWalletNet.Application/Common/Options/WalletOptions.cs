﻿using IotaWalletNet.Application.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static IotaWalletNet.Application.Common.Options.WalletOptions;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace IotaWalletNet.Application.Common.Options
{
    public class WalletOptionsBuilder
    {
        private readonly IWallet _wallet;
        private readonly WalletOptions _walletOptions;
        public WalletOptionsBuilder(IWallet wallet)
        {
            _wallet = wallet;
            _walletOptions = _wallet.GetWalletOptions();
        }

        public WalletOptionsBuilder SetStoragePath(string storagePath)
        {
            _walletOptions.StoragePath = storagePath;
            return this;
        }

        public WalletOptionsBuilder SetCoinType(TypeOfCoin coinType)
        {
            _walletOptions.CoinType = (int)coinType;
            return this;
        }

        public IWallet ThenBuild() => _wallet;
    }

    public class WalletOptions
    {
        private SecretManagerOptions _secretManagerOptions;
        private ClientOptions _clientConfigOptions = new ClientOptions();

        public string StoragePath { get; set; } = "./walletdb";

        public int CoinType { get; set; } = (int)TypeOfCoin.Shimmer;

        public enum TypeOfCoin : int
        {
            Iota = 4218,
            Shimmer = 4219,
        }

        public WalletOptions()
        {
            SecretManager = new SecretManagerOptions();
            ClientConfigOptions = new ClientOptions();
        }

        public SecretManagerOptions SecretManager { get; set; }

        #region ClientOptions


        [JsonProperty(PropertyName = "clientOptions")]
        public ClientOptions ClientConfigOptions{ get; set; }

        #endregion
    }
}
