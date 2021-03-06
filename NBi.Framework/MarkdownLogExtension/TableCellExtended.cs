﻿using MarkdownLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.MarkdownLogExtension
{
    class TableCellExtended : ITableCellExtended
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value ?? ""; }
        }

        public int RequiredWidth
        {
            get { return GetEncodedText().Length; }
        }

        public string BuildCodeFormattedString(TableCellRenderSpecificationExtended spec)
        {
            var alignment = spec.Alignment;
            var maximumWidth = spec.MaximumWidth;
            var encodedText = GetEncodedText();
            return encodedText.Align(alignment, maximumWidth);
        }

        public string BuildCodeFormattedString(TableCellRenderSpecification spec)
        {
            var alignment = spec.Alignment;
            var maximumWidth = spec.MaximumWidth;
            var encodedText = GetEncodedText();
            return encodedText.Align(alignment, maximumWidth);
        }

        private string GetEncodedText()
        {
            return _text.Trim().EscapeCSharpString();
        }

        
    }
}
