using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EDIViewer
{
    public class EdiFileLine
    {
        public EdiFileLine(int lineNumber, string separator, string rawText)
        {
            LineNumber = lineNumber;
            Separator = separator;
            RawText = rawText;
        }
        public string Separator { get; private set; }
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
                return RawText.Split(new string[] { Separator }, StringSplitOptions.None);
            }
        }

        public string RawText { get; private set; }
    }

}
