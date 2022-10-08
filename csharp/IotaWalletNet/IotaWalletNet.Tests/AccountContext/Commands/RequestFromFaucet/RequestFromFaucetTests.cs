﻿using FluentAssertions;
using IotaWalletNet.Application.AccountContext.Commands.GenerateAddresses;
using IotaWalletNet.Application.AccountContext.Queries.GetBalance;
using IotaWalletNet.Application.Common.Interfaces;
using IotaWalletNet.Domain.Common.Models.Address;
using IotaWalletNet.Tests.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IotaWalletNet.Tests.AccountContext.Commands.RequestFromFaucet
{

    [Collection("Sequential")]
    public class RequestFromFaucetTests : DependencyTestBase
    {
        [Fact]
        public async Task AccountShouldBeAbleToGetTokensFromFaucet()
        {
            IWallet wallet = _serviceScope.ServiceProvider.GetRequiredService<IWallet>();

            wallet = CreateFullWallet(wallet);

            await wallet.StoreMnemonicAsync(DEFAULT_MNEMONIC);

            (_, IAccount? account) = await wallet.CreateAccountAsync("cookiemonster");

            GenerateAddressesResponse generateAddressesResponse
                = await account!.GenerateAddressesAsync();


            await account.SyncAccountAsync();
            GetBalanceResponse getBalanceResponse = await account.GetBalanceAsync();
            long totalBalance = long.Parse(getBalanceResponse.Payload!.BaseCoin.Total);

            long thousandShimmer = 1000000 * 1000;
            if (totalBalance >= thousandShimmer)
            {
                //Lets send our tokens to some other address, else we cant ask from faucet
                string receiverAddress = "rms1qz8wf6jrchvsfmcnsfhlf6s53x3u85y0j4hvwth9a5ff3xhrxtmvvyc9ae7";
                var transactionOptions = new AddressesWithAmountAndTransactionOptions().AddAddressAndAmount(receiverAddress, totalBalance.ToString());
                await account.SendAmountAsync(transactionOptions);

                Thread.Sleep(30 * 1000);//Let's wait for it to be confirmed
                await account.SyncAccountAsync();

                getBalanceResponse = await account.GetBalanceAsync();
                totalBalance = long.Parse(getBalanceResponse.Payload!.BaseCoin.Total);

                if (totalBalance >= thousandShimmer)
                    throw new Exception("Tried sending out shimmer in order to obtain new ones from faucet, however, sending out shimmer failed.");
            }

            string address = generateAddressesResponse.Payload?.First()?.Address!;

            await account.RequestFromFaucetAsync(address, DEFAULT_FAUCET_URL);

            Thread.Sleep(30 * 1000);

            await account.SyncAccountAsync();
            getBalanceResponse = await account.GetBalanceAsync();

            long newBalance = long.Parse(getBalanceResponse.Payload!.BaseCoin.Total);

            newBalance.Should().BeGreaterThan(totalBalance);
        }
    }
}