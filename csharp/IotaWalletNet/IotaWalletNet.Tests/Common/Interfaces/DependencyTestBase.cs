﻿using IotaWalletNet.Application.Common.Extensions;
using IotaWalletNet.Application.Common.Interfaces;
using IotaWalletNet.Domain.Common.Models.Coin;
using Microsoft.Extensions.DependencyInjection;

namespace IotaWalletNet.Tests.Common.Interfaces
{
    public class DependencyTestBase : IDisposable
    {
        protected IServiceScope _serviceScope;
        protected const String STRONGHOLD_PATH = "./stronghold";
        protected const string DATABASE_PATH = "./walletdb";
        protected const string DEFAULT_MNEMONIC = "sail symbol venture people general equal sight pencil slight muscle sausage faculty retreat decorate library all humor metal place mandate cake door disease dwarf";
        protected const string DEFAFULT_API_URL = "https://api.testnet.shimmer.network";

        public DependencyTestBase()
        {

            StrongholdCleanup();

            DatabaseCleanup();

            //Register all of the dependencies into a collection of services
            IServiceCollection services = new ServiceCollection().AddIotaWalletServices();

            //Install services to service provider which is used for dependency injection
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //Use serviceprovider to create a scope, which disposes of all services at end of scope
            _serviceScope = serviceProvider.CreateScope();
        }

        public static IWallet CreateFullWallet(IWallet wallet, string nodeUrl = DEFAFULT_API_URL)
        {
            return wallet
                        .ConfigureWalletOptions()
                            .SetCoinType(TypeOfCoin.Shimmer)
                            .SetStoragePath(DATABASE_PATH)
                            .ThenBuild()
                        .ConfigureClientOptions()
                            .AddNodeUrl(nodeUrl)
                            .IsFallbackToLocalPow()
                            .IsLocalPow()
                            .ThenBuild()
                        .ConfigureSecretManagerOptions()
                            .SetPassword("password")
                            .SetSnapshotPath(STRONGHOLD_PATH)
                            .ThenBuild()
                        .ThenInitialize();
        }

        public static IWallet CreateOfflineFullWallet(IWallet wallet, string nodeUrl = DEFAFULT_API_URL)
        {
            return wallet
                        .ConfigureWalletOptions()
                            .SetCoinType(TypeOfCoin.Shimmer)
                            .SetStoragePath(DATABASE_PATH)
                            .ThenBuild()
                        .ConfigureClientOptions()
                            .IsFallbackToLocalPow()
                            .IsLocalPow()
                            .ThenBuild()
                        .ConfigureSecretManagerOptions()
                            .SetPassword("password")
                            .SetSnapshotPath(STRONGHOLD_PATH)
                            .ThenBuild()
                        .ThenInitialize();
        }


        public void StrongholdCleanup(string path = STRONGHOLD_PATH)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public void DatabaseCleanup(string path = DATABASE_PATH)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public void Dispose()
        {
            _serviceScope.Dispose();

            //Force garbage collection
            GC.Collect();

            //Give enough time for services to close
            Thread.Sleep(100);

            StrongholdCleanup();
            DatabaseCleanup();

        }
    }
}
