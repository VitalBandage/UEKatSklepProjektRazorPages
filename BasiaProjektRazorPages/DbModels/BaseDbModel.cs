﻿using System.Reflection;

namespace BasiaProjektRazorPages.DbModels
{
    public class BaseDbModel
    {
        public override bool Equals(object? other)
        {
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                var val1 = propInfo.GetValue(this);
                var val2 = other.GetType().GetProperty(propInfo.Name).GetValue(other);
                if (val1 == null && val2 == null)
                    continue;
                if (val1 == null && val2 != null)
                    return false;
                if (val1.Equals(val2) == false)
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}