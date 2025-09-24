namespace GridMap
{
    public static class GridMapRules
    {
        public static int SetCellRule(int fromRule, int toRule)
        {
            return toRule switch
            {
                GridMapValues.QuickThunder => GridMapValues.None,
                GridMapValues.Thunder5Seconds => GridMapValues.Fire5Seconds,
                GridMapValues.Thunder10Seconds => GridMapValues.Fire10Seconds,
                GridMapValues.Thunder20Seconds => GridMapValues.Fire20Seconds,
                _ => GridMapValues.None
            };
        }
    }
}