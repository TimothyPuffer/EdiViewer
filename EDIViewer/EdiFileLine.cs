using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public class EdiFileLine
    {
        public EdiFileLine(int lineNumber, char[] separator, string rawText)
        {
            LineNumber = lineNumber;
            _separator = separator;
            RawText = rawText;
        }
        char[] _separator = null;
        public char[] Separator
        {
            get
            {
                return _separator.ToArray();
            }
        }
        public int LineNumber { get; private set; }
        public string Id
        {
            get
            {
                return Text[0];
            }
        }
        public string[] Text
        {
            get
            {
                return RawText.Split(_separator, StringSplitOptions.None);
            }
        }
        public string RawText { get; private set; }
    }

    public class EdiFileLineShower
    {
        public EdiFileLineShower(IEnumerable<EdiFileLine> lines)
        {

        }
    }
}
