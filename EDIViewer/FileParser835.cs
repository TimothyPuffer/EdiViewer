using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDIViewer
{
    public class FileInfo : ParentSegment<FileSegment>
    {
        private FileInfo() { }
        public static FileInfo Load(IEnumerable<FileLine> lines)
        {
            FileInfo lastFileInfo = new FileInfo();

            FileSegment lastFileSegment = null;
            Header lastHeader = null;
            HeaderNumber lastHeaderNumber = null;
            bool firstN1Segment = true;
            ClaimPaymentInformation lastClaimPaymentInformation = null;

            Action<FileLine> currentSegment = null;
            foreach (var l in lines)
            {
                switch (l.Id)
                {
                    case "ISA":
                        lastFileSegment = lastFileInfo.AddNewSegment();
                        currentSegment = lastFileSegment.AddActionSegment();
                        firstN1Segment = true;
                        break;
                    case "ST":
                        lastHeader = lastFileSegment.AddNewSegment();
                        currentSegment = lastHeader.AddActionSegment();
                        firstN1Segment = true;
                        break;
                    case "N1":
                        if (firstN1Segment)
                        {
                            currentSegment = lastHeader.PayerSegment.AddActionSegment();
                            firstN1Segment = false;
                        }
                        else
                            currentSegment = lastHeader.PayeeSegment.AddActionSegment();
                        break;
                    case "LX":
                        lastHeaderNumber = lastHeader.AddNewSegment();
                        currentSegment = lastHeaderNumber.AddActionSegment();
                        break;
                    case "CLP":
                        lastClaimPaymentInformation = lastHeaderNumber.AddNewSegment();
                        currentSegment = lastClaimPaymentInformation.AddActionSegment();
                        break;
                    case "SVC":
                        var svc = lastClaimPaymentInformation.AddNewSegment();
                        currentSegment = svc.AddActionSegment();
                        break;
                    case "SE":
                    case "GE":
                    case "IEA":
                    case "PLB":
                        currentSegment = lastFileSegment.EndSegment.AddActionSegment();
                        break;

                }

                currentSegment(l);
            }

            return lastFileInfo;
        }

        public FileSegment AddNewSegment()
        {
            FileSegment t = new FileSegment(this);
            this._headers.Add(t);
            return t;
        }

        public IEnumerable<ClaimPaymentInformation> ClaimPaymentInformations
        {
            get
            {
                return from f in this.Segments
                       from h in f.Segments
                       from ha in h.Segments
                       from cpi in ha.Segments
                       select cpi;
            }
        }
    }

    public class FileSegment : ParentSegment<Header>
    {
        public FileSegment(FileInfo parent)
        {
            Parent = parent;
            EndSegment = new LeafSegment();
        }
        public FileInfo Parent { get; private set; }
        public LeafSegment EndSegment { get; private set; }

        public Header AddNewSegment()
        {
            Header t = new Header(this);
            this._headers.Add(t);
            return t;
        }
    }

    public class Header : ParentSegment<HeaderNumber>
    {
        public Header(FileSegment parent)
        {
            Parent = parent;
            PayerSegment = new LeafSegment();
            PayeeSegment = new LeafSegment();
        }
        public FileSegment Parent { get; private set; }
        public LeafSegment PayerSegment { get; private set; }
        public LeafSegment PayeeSegment { get; private set; }

        public HeaderNumber AddNewSegment()
        {
            HeaderNumber t = new HeaderNumber(this);
            this._headers.Add(t);
            return t;
        }
    }

    public class HeaderNumber : ParentSegment<ClaimPaymentInformation>
    {
        public HeaderNumber(Header parent)
        {
            Parent = parent;
        }
        public Header Parent { get; private set; }

        public ClaimPaymentInformation AddNewSegment()
        {
            ClaimPaymentInformation t = new ClaimPaymentInformation(this);
            this._headers.Add(t);
            return t;
        }

    }

    public class LeafSegment
    {
        private List<FileLine> _segments = new List<FileLine>();

        public IEnumerable<FileLine> Lines
        {
            get
            {
                return from s in _segments
                       select s;
            }
        }

        public Action<FileLine> AddActionSegment()
        {
            return x => this._segments.Add(x);
        }
    }

    public class ParentSegment<T> : LeafSegment where T : LeafSegment
    {
        protected List<T> _headers = new List<T>();

        public IEnumerable<T> Segments
        {
            get
            {
                return from s in _headers
                       select s;
            }
        }
    }

    public class FileLine
    {
        public FileLine(int lineNumber, string id, string[] line,string rawText)
        {
            LineNumber = lineNumber;
            Id = id;
            Text = line;
            RawText = rawText;
        }
        public int LineNumber { get; private set; }
        public string Id { get; private set; }
        public string[] Text { get; private set; }
        public string RawText { get; private set; }
    }

    public class ValueSegment
    {
        public ValueSegment(string value, int position, FileLine line,EdiDataType dataType)
        {
            Value = value;
            Position = position;
            Line = line;
            DateType = dataType;
        }

        public string Value { get; private set; }
        public int Position { get; private set; }
        public FileLine Line { get; private set; }
        public EdiDataType DateType { get; private set; }
    }
}
