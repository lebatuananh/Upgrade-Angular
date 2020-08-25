using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel.CMS
{
	public class Pager
	{
		public string ActionScript { get; set; }

		public int CurrentPage { get; set; }

		public int PageSize { get; set; }

		public int TotalItem { get; set; }

		public int TotalPage
		{
			get
			{
				if (this.TotalItem % this.PageSize != 0)
				{
					return (this.TotalItem / this.PageSize) + 1;
				}
				else
				{
					return this.TotalItem / this.PageSize;
				}
			}
		}

		public int Previous
		{
			get
			{
				return this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
			}
		}

		public int Next
		{
			get
			{
				return this.CurrentPage == this.TotalPage ? this.TotalPage : this.CurrentPage + 1;
			}
		}

		public Pager()
		{
			this.ActionScript = "void";
			this.CurrentPage = 1;
			this.TotalItem = 1;
			this.PageSize = 25;
		}
	}
}
