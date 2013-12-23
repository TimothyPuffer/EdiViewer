using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public class PropertyProvider
    {
        private List<PropertyProviderInfo> _myPropertyInfo = new List<PropertyProviderInfo>();
        public void AddPropertyInfo(IEnumerable<FileLine> lines, string propertyName, string id, int position, EdiDataType ediType)
        {
            _myPropertyInfo.Add(new PropertyProviderInfo() { Lines = lines, PropertyName = propertyName, Id = id, Position = position, EdiType = ediType });
        }

        public void AddPropertyInfo(IEnumerable<FileLine> lines, string propertyName, string id, string refPosition, int position, EdiDataType ediType)
        {
            _myPropertyInfo.Add(new PropertyProviderInfo() { Lines = lines, PropertyName = propertyName, Id = id, Position = position, RefPosition = refPosition, EdiType = ediType });
        }



        public IEnumerable<PropertyProviderInfo> PropertyProviderInfos
        {
            get { return _myPropertyInfo.Select(p => p); }
        }
    }

    public class PropertyProviderInfo
    {
        public IEnumerable<FileLine> Lines { get; set; }
        public string PropertyName { get; set; }
        public string Id { get; set; }
        public string RefPosition { get; set; }
        public int Position { get; set; }
        public EdiDataType EdiType { get; set; }
        public string Value
        {
            get
            {
                var temp = from l in Lines
                           where l.Id == Id && (RefPosition == null || l.Text[1] == RefPosition)
                           select l;

                if (!temp.Any())
                    return string.Empty;

                if (temp.First().Text.Length < Position)
                    return string.Empty;

                return temp.First().Text[Position];
            }
        }
    }
}
