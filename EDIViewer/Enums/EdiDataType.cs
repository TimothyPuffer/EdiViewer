using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDIViewer
{
    public enum EdiDataType
    {
        Numeric = 1,
        Decimal = 2,
        Identifier = 3,
        String = 4,
        Date = 5,
        Time = 6,
        Binary = 7
    }

    public enum EdiFindStatus
    {
        HasValue = 1,
        NoLine = 2,
        NoPosition = 3,
        Blank = 4

    }

    public enum FileLineHighLight
    {
        None = 0,
        Yellow = 1
    }
}
