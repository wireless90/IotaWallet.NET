﻿using IotaWalletNet.Domain.Common.Interfaces;

namespace IotaWalletNet.Domain.Options
{
    public class ClientOptions
    {
        public List<string> Nodes { get; set; } = new List<string>();

        public bool LocalPow { get; set; } = true;

        public bool FallbackToLocalPow { get; set; } = true;

        public bool Offline { get; set; } = false;

    }

    public class ClientOptionsBuilder
    {
        private IWallet _wallet;
        private ClientOptions _clientOptions;

        public ClientOptionsBuilder(IWallet wallet)
        {
            _wallet = wallet;
            _clientOptions = _wallet.GetWalletOptions().ClientConfigOptions;
        }

        public ClientOptionsBuilder AddNodeUrl(string nodeUrl)
        {
            _clientOptions.Nodes.Add(nodeUrl);
            return this;
        }

        public ClientOptionsBuilder IsLocalPow(bool isLocalPow = true)
        {
            _clientOptions.LocalPow = isLocalPow;
            return this;
        }

        public ClientOptionsBuilder IsFallbackToLocalPow(bool isFallbackToLocalPow = true)
        {
            _clientOptions.FallbackToLocalPow = isFallbackToLocalPow;
            return this;
        }

        public ClientOptionsBuilder IsOffline(bool isOffline = true)
        {
            _clientOptions.Offline = isOffline;
            return this;
        }

        public IWallet ThenBuild()
        {
            //To trigger json re-construction
            _wallet.GetWalletOptions().ClientConfigOptions = _clientOptions;
            return _wallet;
        }
    }
}