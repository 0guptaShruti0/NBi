﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer
{
    abstract class BaseRowsAnalyzer : IRowsAnalyzer
    {
        protected abstract string Sentence { get; }

        public List<CompareHelper> Retrieve(Dictionary<KeyCollection,CompareHelper> x, Dictionary<KeyCollection, CompareHelper> y)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var rows = ExtractRows(x, y);
            stopWatch.Stop();
            Trace.WriteLineIf(
                NBiTraceSwitch.TraceInfo, 
                string.Format(
                    "{0}: {1}  [{2}]",
                    Sentence,
                    rows.Count(), 
                    stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")
                    )
                );

            return rows;
        }

        protected abstract List<CompareHelper>  ExtractRows(Dictionary<KeyCollection, CompareHelper> x, Dictionary<KeyCollection, CompareHelper> y);
        
    }
}
