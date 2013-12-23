using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public class ClaimPaymentInformation : ParentSegment<LeafSegment>
    {
        public ClaimPaymentInformation(HeaderNumber parent)
        {
            Parent = parent;
        }
        public HeaderNumber Parent { get; private set; }

        public LeafSegment AddNewSegment()
        {
            LeafSegment t = new LeafSegment();
            this._headers.Add(t);
            return t;
        }

        public IEnumerable<PropertyProviderInfo> PropertyProviderInfos
        {
            get
            {
                PropertyProvider pp = new PropertyProvider();

                pp.AddPropertyInfo(this.Lines, "PatientControlNumber", "CLP", 1, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "ClaimStatusCode", "CLP", 2, EdiDataType.Identifier);
                pp.AddPropertyInfo(this.Lines, "ClaimCharge", "CLP", 3, EdiDataType.Decimal);
                pp.AddPropertyInfo(this.Lines, "ClaimPayment", "CLP", 4, EdiDataType.Decimal);
                pp.AddPropertyInfo(this.Lines, "ClaimICN", "CLP", 7, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "PatientLastName", "NM1", "QC", 3, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "PatientFirstName", "NM1", "QC", 4, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "PatientMiddleName", "NM1", "QC", 5, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "PatientIdCode", "NM1", "QC", 9, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "MemberIdNum", "REF", "1W", 2, EdiDataType.String);
                pp.AddPropertyInfo(this.Lines, "FromDate", "DTM", "232", 2, EdiDataType.Date);
                pp.AddPropertyInfo(this.Lines, "ToDate", "DTM", "233", 2, EdiDataType.Date);

                return pp.PropertyProviderInfos;
            }
        }

        public IEnumerable<FileLine> ShowLines
        {
            get
            {
                return this.Lines;
            }
        }

    }
}
