﻿using System.Collections.Generic;

namespace osf.web.Models
{
	public class HomeModel
	{
        public List<LatestEventModel> LatestEvents { get; set; }

		public QuoteModel Quote { get; set; }

        public string BrochuresUrl { get; set; }
	}
}