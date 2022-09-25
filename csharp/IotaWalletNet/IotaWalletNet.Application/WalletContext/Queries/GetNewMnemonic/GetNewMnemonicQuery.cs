﻿using IotaWalletNet.Domain.Common.Interfaces;
using MediatR;

namespace IotaWalletNet.Application.WalletContext.Queries.GetNewMnemonic
{
    public class GetNewMnemonicQuery : IRequest<string>
    {
        public GetNewMnemonicQuery(IWallet wallet)
        {
            Wallet = wallet;
        }

        public IWallet Wallet { get; }
    }
}