using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using TypeKitchen;

namespace Demo.Views.Shared.Components.TableComponent
{
	public class TableModel
	{
		public IList<string> Headers { get; set; } = new List<string>();
		public IList<TableRow> Rows { get; set; } = new List<TableRow>();
	}

	public class TableRow
	{
		public List<string> Cells { get; set; } = new List<string>();
	}

	public class TableComponent : ViewComponent
	{
		public TableComponent()
		{
			
		}

		public IViewComponentResult Invoke(IEnumerable<object> models)
		{
			var table = new TableModel();

			var accessor = ReadAccessor.Create(models.First(), AccessorMemberTypes.Properties, AccessorMemberScope.Public, out var members);

			foreach (var member in members)
			{
				if (member.CanRead)
				{
					table.Headers.Add(member.Name);
				}
			}

			foreach (var model in models)
			{
				var row = new TableRow();

				foreach (var member in members)
				{
					if (member.CanRead)
					{
						if (accessor.TryGetValue(model, member.Name, out var cell))
							row.Cells.Add(cell?.ToString() ?? "");
					}
				}

				table.Rows.Add(row);
			}

			return View(table);
		}
	}
}
