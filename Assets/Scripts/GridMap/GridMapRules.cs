namespace GridMap
{
    public static class GridMapRules
    {
        public static int SetCellRule(GridMap map, int fromRule, int toRule)
        {
            return fromRule switch
            {
                0 => DefaultRule(map, toRule),
                >= GridMapValues.Fire5Seconds and <= GridMapValues.Fire25Seconds => FireRule(map, toRule),
                GridMapValues.Water => WaterRule(map, toRule),
                GridMapValues.Steam => SteamRule(map, toRule),
                GridMapValues.IceFloor => IceRule(map, toRule),
                _ => DefaultRule(map, toRule)
            };
        }

        public static int Every5SecondsRule(int value)
        {
            if (value <= 1) return 0;
            
            if (value <= GridMapValues.Fire25Seconds)
                return value - 1;

            if (value == GridMapValues.ThunderWater5Seconds)
                return GridMapValues.Water;
            if(value <= GridMapValues.ThunderWater30Seconds)
                return value - 1;

            return value;
        }

        public static int DefaultRule(GridMap map, int value)
        {
            return value switch
            {
                GridMapValues.QuickThunder => GridMapValues.None,
                GridMapValues.Thunder5Seconds => GridMapValues.Fire5Seconds,
                GridMapValues.Thunder10Seconds => GridMapValues.Fire10Seconds,
                GridMapValues.Thunder20Seconds => GridMapValues.Fire20Seconds,
                _ => value
            };
        }

        public static int WaterRule(GridMap map, int value)
        {
            return DefaultRule(map, value);
        }

        public static int FireRule(GridMap map, int value)
        {
            if (value == GridMapValues.IceFloor)
                return 0;
            return DefaultRule(map, value);
        }

        public static int SteamRule(GridMap map, int value)
        {
            return DefaultRule(map, value);
        }

        public static int IceRule(GridMap map, int value)
        {
            return DefaultRule(map, value);
        }
    }
}