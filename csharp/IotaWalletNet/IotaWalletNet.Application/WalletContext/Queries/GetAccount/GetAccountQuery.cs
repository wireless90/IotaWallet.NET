﻿using IotaWalletNet.Application.Common.Interfaces;
using MediatR;

namespace IotaWalletNet.Application.WalletContext.Queries.GetAccount
{
    public class GetAccountQuery : IRequest<string>
    {
        public GetAccountQuery(IWallet wallet, string username)
        {
            Wallet = wallet;
            Username = username;
        }

        public IWallet Wallet { get; }
        public string Username { get; }
    }
}
