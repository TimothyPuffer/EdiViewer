using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public class PropertyProvider
    {
        private List<PropertyProviderInfo> _myPropertyInfo = new List<PropertyProviderInfo>();
        public void AddPropertyInfo(IEnumerable<EdiFileLine> lines, string propertyName, string id, int position, EdiDataType ediType)
        {
            _myPropertyInfo.Add(new PropertyProviderInfo() { Lines = lines, PropertyName = propertyName, Id = id, Position = position, EdiType = ediType });
        }

        public void AddPropertyInfo(IEnumerable<EdiFileLine> lines, string propertyName, string id, string refPosition, int position, EdiDataType ediType)
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
        private static string EMPTY_VALUE = string.Empty;
        public IEnumerable<EdiFileLine> Lines { get; set; }
        public string PropertyName { get; set; }
        public string Id { get; set; }
        public string RefPosition { get; set; }
        public int Position { get; set; }
        public EdiDataType EdiType { get; set; }
        public EdiMonad ValueMonad
        {
            get
            {
                var temp = from l in Lines
                           where l.Id == Id && (RefPosition == null || l.Text[1] == RefPosition)
                           select l;

                if (!temp.Any())
                    return new EdiMonad(EMPTY_VALUE, EdiFindStatus.NoLine, EdiType, null);

                if (temp.First().Text.Length < Position)
                    return new EdiMonad(EMPTY_VALUE, EdiFindStatus.NoPosition, EdiType, null);

                var tLine = temp.First();
                var tValue = tLine.Text[Position];

                if (string.IsNullOrEmpty(tValue))
                    return new EdiMonad(EMPTY_VALUE, EdiFindStatus.Blank, EdiType, tLine);
                else
                    return new EdiMonad(tValue, EdiFindStatus.HasValue, EdiType, tLine);
            }
        }
    }

    public class EdiMonad
    {
        public EdiMonad(string value,EdiFindStatus status, EdiDataType datatype,EdiFileLine line)
        {
            Value = value;
            FindStatus = status;
            DataType = datatype;
            Line = line;

        }
        public string Value { get; private set; }
        public EdiFindStatus FindStatus { get; private set; }
        public EdiDataType DataType { get; private set; }
        public EdiFileLine Line { get; private set; }
    }
}
