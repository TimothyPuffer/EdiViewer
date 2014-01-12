using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public class RawLineCollection : IEnumerable<IEnumerable<RawLineValue>>
    {
        private List<Tuple<EdiFileLine, IEnumerable<Tuple<int, RawLineValue>>>> _internal = new List<Tuple<EdiFileLine, IEnumerable<Tuple<int, RawLineValue>>>>();

        public RawLineCollection(IEnumerable<EdiFileLine> lines)
        {
            _internal = (from l in lines
                         select new Tuple<EdiFileLine, IEnumerable<Tuple<int, RawLineValue>>>(l, ConvertEdiFileLine(l))).ToList();
        }

        public void ClearSelected()
        {
            var temp = from l in _internal
                       from l1 in l.Item2
                       select l1.Item2;

            foreach (var t in temp)
                t.Highlight = FileLineHighLight.None;
        }

        public void SetSelected(EdiFileLine line, int position)
        {
            var temp = from l in _internal
                       from l1 in l.Item2
                       where l.Item1 == line && l1.Item1 == position
                       select l1.Item2;

            foreach (var t in temp)
                t.Highlight = FileLineHighLight.Yellow;
        }

        private IEnumerable<Tuple<int,RawLineValue>> ConvertEdiFileLine(EdiFileLine line)
        {
            List<Tuple<int, RawLineValue>> ret = new List<Tuple<int, RawLineValue>>();
            bool first = true;
            int position = 0;

            foreach (string v in line.Text)
            {
                if (!first)
                    ret.Add(new Tuple<int, RawLineValue>(-1, new RawLineValue(line.Separator)));

                ret.Add(new Tuple<int, RawLineValue>(position, new RawLineValue(v)));

                position = position + 1;
                first = false;
            }

            return ret;
        }

        public IEnumerator<IEnumerable<RawLineValue>> GetEnumerator()
        {
            return (from i in _internal
                    select i.Item2.Select(s => s.Item2)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class RawLineValue : INotifyPropertyChanged
    {
        public RawLineValue(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
        FileLineHighLight _highlight;
        public FileLineHighLight Highlight
        {
            get { return _highlight; }
            set
            {
                if (_highlight != value)
                {
                    _highlight = value;
                    OnPropertyChanged("Highlight");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}
