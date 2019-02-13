namespace Sitecore.Foundation.Assets.Models
{
    using Sitecore.Caching;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal class AssetRequirementList : ICacheable, IEnumerable<Asset>
    {
        private readonly List<Asset> _items = new List<Asset>();

        public bool Cacheable { get; set; }

        public bool Immutable => true;

        event DataLengthChangedDelegate ICacheable.DataLengthChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public AssetRequirementList()
        {
            this.Cacheable = true;
        }

        public long GetDataLength()
        {
            return this._items.Sum(x => x.GetDataLength());
        }

        public IEnumerator<Asset> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Asset requirement)
        {
            this._items.Add(requirement);
        }
    }
}