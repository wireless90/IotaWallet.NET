﻿using FluentAssertions;
using IotaWalletNet.Application.AccountContext.Commands.GenerateAddresses;
using IotaWalletNet.Application.AccountContext.Queries.GetBalance;
using IotaWalletNet.Application.AccountContext.Queries.GetOutputs;
using IotaWalletNet.Application.Common.Interfaces;
using IotaWalletNet.Domain.Common.Models.Address;
using IotaWalletNet.Tests.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IotaWalletNet.Tests.AccountContext.Queries.GetOutputs
{
    [Collection("Sequential")]
    public class GetOutputsTests : DependencyTestBase, IDisposable
    {
        [Fact]
        public async Task AccountShouldBeAbleToGetOutputsAfterSyncing()
        {
            IWallet wallet = _serviceScope.ServiceProvider.GetRequiredService<IWallet>();

            wallet = await CreateFullWalletAsync(wallet);

            (_, IAccount? account) = await wallet.CreateAccountAsync("cookiemonster");

            GenerateAddressesResponse generateAddressesResponse
                = await account!.GenerateAddressesAsync();

            string address = generateAddressesResponse.Payload?.First()?.Address!;

            await account.RequestFromFaucetAsync(address, DEFAULT_FAUCET_URL);

            await Task.Delay(TimeSpan.FromSeconds(SLEEP_DURATION_SECONDS_FAUCET));

            await account.SyncAccountAsync();
            GetBalanceResponse getBalanceResponse = await account.GetBalanceAsync();

            //This will create  basic outputs
            await account.SendAmountAsync(
                new AddressesWithAmountAndTransactionOptions()
                .AddAddressAndAmount(ANOTHER_WALLET_ADDRESS, "1000000")
                .AddAddressAndAmount(ANOTHER_WALLET_ADDRESS, "1000000")
                );

            await Task.Delay(TimeSpan.FromSeconds(SLEEP_DURATION_SECONDS_TRANSACTION));
            await account.SyncAccountAsync();
            getBalanceResponse = await account.GetBalanceAsync();
            GetOutputsResponse response = await account.GetOutputsAsync();

            response.Should().NotBeNull();
            response.IsSuccess().Should().BeTrue();
            response.Payload!.Count.Should().BeGreaterThanOrEqualTo(2);
        }
    }
}
