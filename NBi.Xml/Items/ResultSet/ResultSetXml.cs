﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.ResultSet
{
    public class ResultSetXml : ConnectionItemXml
    {
        [XmlElement("row")]
        public List<RowXml> _rows { get; set; }

        public IList<IRow> Rows
        {
            get { return _rows.Cast<IRow>().ToList(); }
        }

        [XmlAttribute("file")]
        public string File { get; set; }

        public string GetFile()
        {
            var file = string.Empty;
            if (Path.IsPathRooted(File))
                file = File;
            else
                file = Settings.BasePath + File;

            return file;
        }

        public ResultSetXml()
        {
            _rows = new List<RowXml>();
        } 

    }
}
