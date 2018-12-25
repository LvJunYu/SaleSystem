namespace Sale
{
    public static class SaleTools
    {
        public static int SafeParse(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }

            return int.Parse(content);
        }
    }
}