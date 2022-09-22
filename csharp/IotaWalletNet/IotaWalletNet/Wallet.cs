﻿using IotaWalletNet.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.InteropServices;
using System.Text;

namespace IotaWalletNet
{
    public class Wallet
    {
        //[DllImport("bindings", EntryPoint = "create_wallet_manager", CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr CreateWalletManager(string password, string nodeUrl, UInt32 coinType);

        //[DllImport("bindings", EntryPoint = "create_account", CallingConvention = CallingConvention.Cdecl)]
        //private static extern void CreateAccount(IntPtr accountManagerHandle, string accountName);

        //[DllImport("bindings", EntryPoint = "get_usernames", CallingConvention = CallingConvention.Cdecl)]
        //private static extern string GetUsernames(IntPtr accountManagerHandle);

        //[DllImport("bindings", EntryPoint = "get_account", CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr GetAccount(IntPtr accountManagerHandle, string username);

        [DllImport("bindings", EntryPoint = "iota_initialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr InitializeIotaWallet(string managerOptions, [MarshalAs(UnmanagedType.LPStr)] StringBuilder errorBuffer, int errorBufferSize);

        private IntPtr _walletHandle;
        private StringBuilder _errorBuffer;

        public Wallet()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            ManagerOptions managerOptions = new ManagerOptions();

            

            string managerOptionsSerialized = JsonConvert.SerializeObject(managerOptions);
            //if (managerOptionsSerialized.Contains("clientOptions"))
            //    managerOptionsSerialized = managerOptionsSerialized.Replace("clientOptions", "ClientOptions");
            
            _errorBuffer = new StringBuilder(1024);

            _walletHandle = Wallet.InitializeIotaWallet(managerOptionsSerialized, _errorBuffer, 1024);
        }


    }
}
