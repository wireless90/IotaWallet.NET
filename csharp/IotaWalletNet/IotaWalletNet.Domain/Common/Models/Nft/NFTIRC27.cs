﻿namespace IotaWalletNet.Domain.Common.Models.Nft
{
    public class NFTIRC27
    {
        public string Standard { get; } = "IRC27";

        public string Version { get; } = "v1.0";

        /// <summary>
        /// Mime type
        /// </summary>
        public string Type { get; set; }
        public string NameOfNft { get; }
        public string Uri { get; }

        public string? CollectionName { get; set; }

        public string? IssuerName { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// address to royalties mapping. Its not encouraged to have total royalties exceed 50%
        /// </summary>
        public Dictionary<string, decimal> Royalties { get; set; } = new Dictionary<string, decimal>();

        /// <summary>
        /// Attributes for the item, which will show up on dApps like NFT Marketplaces
        /// </summary>
        public List<NFTIRC27Attribute> Attributes { get; set; } = new List<NFTIRC27Attribute>();

        /// <summary>
        /// Attributes for the item, which need not be used publicly
        /// </summary>
        public List<NFTIRC27Attribute> InternalAttributes { get; set; } = new List<NFTIRC27Attribute>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mimeType">Use KnownMimeTypes class to help</param>
        public NFTIRC27(string mimeType, string nameOfNft, string uri)
        {
            Type = mimeType;
            NameOfNft = nameOfNft;
            Uri = uri;
        }

        public NFTIRC27 SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public NFTIRC27 SetCollectionName(string collectionName)
        {
            CollectionName = collectionName;
            return this;
        }

        public NFTIRC27 SetIssuerName(string issuerName)
        {
            IssuerName = issuerName;
            return this;
        }

        public NFTIRC27 AddRoyalty(string address, decimal royaltyPercentage)
        {
            Royalties[address] = royaltyPercentage;

            return this;
        }

        public NFTIRC27 AddAttribute(string traitType, string value)
        {
            Attributes.Add(new NFTIRC27Attribute(traitType, value));
            return this;
        }

        public NFTIRC27 AddInternalAttribute(string traitType, string value)
        {
            InternalAttributes.Add(new NFTIRC27Attribute(traitType, value));
            return this;
        }
    }
}
