﻿using NBi.Core.ResultSet.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.DateTime
{
    class DateTimeEqual : AbstractPredicateReference
    {
        public DateTimeEqual(object reference) : base(reference)
        { }

        public override bool Apply(object x)
        {
            var cpr = new DateTimeComparer();
            return cpr.Compare(x, Reference).AreEqual;
        }

        public override string ToString()
        {
            return $"is equal to {Reference}";
        }
    }
}
