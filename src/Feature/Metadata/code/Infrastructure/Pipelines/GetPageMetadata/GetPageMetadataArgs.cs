namespace Sitecore.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Metadata.Models;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class GetPageMetadataArgs : Sitecore.Pipelines.PipelineArgs
    {
        public GetPageMetadataArgs(IMetadata metadata, Item item)
        {
            this.Metadata = metadata;
            this.Item = item;
        }
        private GetPageMetadataArgs(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        public IMetadata Metadata { get; }
        public Item Item { get; }
    }
}