﻿using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class ObjectArrayResultSetService : IResultSetService
    {
        private readonly object[] objects;

        public ObjectArrayResultSetService(object[] objects)
        {
            this.objects = objects;
        }

        public virtual ResultSet Execute()
        {
            var rows = new List<IRow>();
            foreach (var obj in objects)
            {
                var items = obj as List<object>;
                var row = new Row();
                foreach (var item in items)
                {
                    var cell = new Cell();
                    cell.Value = item.ToString();
                    row.Cells.Add(cell);
                }
                rows.Add(row);
            }

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }

        private class Row : IRow
        {
            private readonly IList<ICell> cells = new List<ICell>();

            public IList<ICell> Cells
            {
                get { return cells; }
            }
        }

        private class Cell : ICell
        {
            public string Value { get; set; }
            public string ColumnName { get; set; }
        }
    }
}