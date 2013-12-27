using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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

        public string[] TextWithSeparator
        {
            get
            {
                return Zip(Text, Separator.First().ToString()).ToArray();
            }
        }

        private IEnumerable<T> Zip<T>(IEnumerable<T> ttt,T tt)
        {
            bool notFirst = false;
            foreach(T tttt in ttt)
            {
                if (notFirst)
                {
                    yield return tt;
                }
                notFirst = true;
                yield return tttt;
            }
        }
        public string RawText { get; private set; }
    }

    public class EdiFileLineShower
    {
        IEnumerable<EdiFileLine> _lines;
        public EdiFileLineShower(IEnumerable<EdiFileLine> lines)
        {
            _lines = lines;
        }

        private class InternalSelectedString :ISelectedString, INotifyPropertyChanged
        {

            public InternalSelectedString(string value)
            {
                Value = value;
            }


            public event PropertyChangedEventHandler PropertyChanged;

            public string Value
            {
                get;
                private set;
            }

            bool _isSelected;
            public bool IsSelected
            {
                get
                {
                    return _isSelected;
                }
                set
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }

            private void RaisePropertyChanged(string caller)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(caller));
                }
            }
        }
    }

    public interface ISelectedString
    {
        string Value { get; }
        bool IsSelected { get; set; }
    }
}
