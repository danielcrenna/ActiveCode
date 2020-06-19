using System.Collections.Generic;

namespace Demo.Views.Shared.Components.TableComponent
{
	public class TableModel
	{
		public IList<string> Headers { get; set; } = new List<string>();
		public IList<TableRow> Rows { get; set; } = new List<TableRow>();
	}
}