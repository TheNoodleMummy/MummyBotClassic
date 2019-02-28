//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Mummybot
//{
//    public sealed class TabularData
//    {
//        private IList<int> _widths;
//        private IList<string> _columns;
//        private readonly IList<IEnumerable<string>> _rows;

//        public TabularData()
//        {
//            _widths = new List<int>();
//            _columns = new List<string>();
//            _rows = new List<IEnumerable<string>>();
//        }

//        public TabularData SetColumns(params string[] columns)
//        {
//            _widths = columns.Select(x => x.Length + 2).ToList();
//            _columns = columns;

//            return this;
//        }

//        public TabularData AddRow(params object[] row)
//        {
//            if (row.Length != _columns.Count)
//                throw new ArgumentOutOfRangeException(nameof(row), "The row's width must be equal to the amount of columns.");

//            _rows.Add(row.Select(x => x.ToString()));

//            for (var i = 0; i < row.Length; i++)
//            {
//                var width = row[i].ToString().Length + 2;
//                if (width > _widths[i])
//                    _widths[i] = width;
//            }

//            return this;
//        }

//        public TabularData AddSeparator()
//        {
//            _rows.Add(null);
//            return this;
//        }

//        public TabularData AddRows(IEnumerable<object[]> rows)
//        {
//            foreach (var row in rows)
//                AddRow(row);

//            return this;
//        }

//        public string Render()
//        {
//            if (_columns.Count == 0)
//                throw new InvalidOperationException("You must set at least one column before rendering the table.");

//            if (_rows.Count == 0 || _rows.All(x => x is null))
//                throw new InvalidOperationException("You must set at least one row before rendering the table.");

//            var separator = $"+{string.Join('+', _widths.Select(x => new string('-', x)))}+";
//            var toDraw = new List<string> { separator, GetEntry(_columns), separator };
//            toDraw.AddRange(_rows.Select(row => row is null ? separator : GetEntry(row)));
//            toDraw.Add(separator);
//            return string.Join('\n', toDraw);
//        }

//        private string GetEntry(IEnumerable<string> text)
//            => $"|{string.Join('|', text.Select((x, i) => x.PadCenter(_widths[i])))}|";

//        public override string ToString()
//            => Render();
//    }
//}