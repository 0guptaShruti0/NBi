﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer
{
    class KeyMatchingRowsAnalyzer : BaseRowsAnalyzer
    {
        protected override string Sentence
        {
            get { return "Rows with a matching key and not duplicated"; }
        }
        

        protected override List<CompareHelper> ExtractRows(Dictionary<KeyCollection, CompareHelper> x, Dictionary<KeyCollection, CompareHelper> y)
        {
            List<CompareHelper> rows;
            {
                var keys = x.Keys.Intersect(y.Keys);
                rows = new List<CompareHelper>(keys.Count());
                foreach (var i in keys)
                    rows.Add(x[i]);
            }
            return rows;
        }
    }
}
