namespace GridMap
{
    public static class GridMapRules
    {
        public static int SetCellRule(RuleData rule)
        {
            return rule.From switch
            {
                0 => DefaultRule(rule),
                >= GridMapValues.Fire5Seconds and <= GridMapValues.Fire25Seconds => FireRule(rule),
                GridMapValues.Water or 
                    >= GridMapValues.ThunderWater5Seconds and <= GridMapValues.ThunderWater30Seconds => WaterRule(rule),
                GridMapValues.Steam => SteamRule(rule),
                GridMapValues.IceFloor => IceRule(rule),
                _ => DefaultRule(rule)
            };
        }

        public static int Every5SecondsRule(int value)
        {
            if (value <= 1) return 0;
            
            if (value <= GridMapValues.Fire25Seconds)
                return value - 1;

            if (value == GridMapValues.ThunderWater5Seconds)
                return GridMapValues.Water;
            if(value is >= GridMapValues.ThunderWater10Seconds and <= GridMapValues.ThunderWater30Seconds)
                return value - 1;

            return value;
        }

        public static int DefaultRule(RuleData rule)
        {
            return rule.To switch
            {
                GridMapValues.QuickThunder => GridMapValues.None,
                GridMapValues.Thunder5Seconds => GridMapValues.Fire5Seconds,
                GridMapValues.Thunder10Seconds => GridMapValues.Fire10Seconds,
                GridMapValues.Thunder20Seconds => GridMapValues.Fire20Seconds,
                GridMapValues.QuickFire => 0,
                _ => rule.To
            };
        }

        public static int WaterRule(RuleData rule)
        {
            if (rule.To is >= GridMapValues.Thunder5Seconds and <= GridMapValues.Thunder20Seconds or
                GridMapValues.QuickThunder)
            {
                rule.Map.DelayedCall(() =>
                {
                    var rects = rule.Map.PaintConnectedAreaByPredicate(
                        rule.Indexes.x, rule.Indexes.y, 16, GridMapValues.ThunderWater25Seconds,
                        b => b is GridMapValues.Water);
                    
                    if (rects.Count == 0)
                        return;
                    
                    rule.Map.DelayedCall(() => rule.Map.SetCellsAsync(rects));
                });

                return GridMapValues.ThunderWater25Seconds;
            }

            if (rule.To is >= GridMapValues.Fire5Seconds and <= GridMapValues.Fire25Seconds or
                GridMapValues.QuickFire)
                return GridMapValues.Steam;

            if (rule.To is GridMapValues.IceFloor)
            {
                rule.Map.DelayedCall(() =>
                {
                    var rects = rule.Map.PaintConnectedAreaByPredicate(
                        rule.Indexes.x, rule.Indexes.y, 4, GridMapValues.IceFloor,
                        b => b is GridMapValues.Water or
                            >= GridMapValues.ThunderWater5Seconds and <= GridMapValues.ThunderWater30Seconds);

                    if (rects.Count == 0)
                        return;
                    
                    rule.Map.DelayedCall(() => rule.Map.SetCellsAsync(rects));
                });

                return GridMapValues.IceFloor;
            }
            
            return DefaultRule(rule);
        }

        public static int FireRule(RuleData rule)
        {
            if (rule.To == GridMapValues.IceFloor)
                return 0;
            return DefaultRule(rule);
        }

        public static int SteamRule(RuleData rule)
        {
            if (rule.To is GridMapValues.IceFloor)
                return GridMapValues.Water;
            if(rule.To is >= GridMapValues.Thunder5Seconds and <= GridMapValues.Thunder20Seconds)
                return GridMapValues.Water;
            
            return DefaultRule(rule);
        }

        public static int IceRule(RuleData rule)
        {
            if (rule.To is >= GridMapValues.Fire5Seconds and <= GridMapValues.Fire25Seconds or
                GridMapValues.QuickFire)
                return GridMapValues.Water;
            return DefaultRule(rule);
        }
    }
}