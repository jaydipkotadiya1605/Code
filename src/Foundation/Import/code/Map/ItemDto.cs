using Sitecore.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sitecore.Foundation.Import.Map
{
    [DebuggerDisplay("Name={Name} Children={Children.Count}")]
    public class ItemDto
    {
        public string Name { get; set; }
        public ID TemplateId { get; set; }
        public ID RootId { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public ItemDto Parent { get; set; }
        public ID ParentRootId { get; set; }
        public List<ItemDto> Children { get; set; }
        public string DisplayName { get; set; }

        public ItemDto(string name)
        {
            Name = name;
            Fields = new Dictionary<string, string>();
            Children = new List<ItemDto>();
        }
    }
}