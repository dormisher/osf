namespace osf.web.Services
{
	public static class Extensions
	{
        public static string Truncate(this string s, int maxLength, bool elipsis = false)
        {
			if (s.Length <= maxLength)
			{
				return s;
			}
			else
			{
				string sub = s.Substring(0, maxLength);

				if (elipsis)
				{
					sub = sub.Trim() + "...";
				}

				return sub;
			}
        }
	}
}