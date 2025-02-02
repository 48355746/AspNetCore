// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.AspNetCore.Razor.Language.Legacy
{
    [DebuggerDisplay("{" + nameof(DebuggerToString) + "(),nq}")]
    internal class LocationTagged<TValue> : IFormattable
    {
        public LocationTagged(TValue value, int absoluteIndex, int lineIndex, int characterIndex)
            : this (value, new SourceLocation(absoluteIndex, lineIndex, characterIndex))
        {
        }

        public LocationTagged(TValue value, SourceLocation location)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Location = location;
            Value = value;
        }

        public SourceLocation Location { get; }

        public TValue Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as LocationTagged<TValue>;
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Equals(other.Location, Location) &&
                Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "P";
            }
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }
            switch (format.ToUpperInvariant())
            {
                case "F":
                    return string.Format(formatProvider, "{0}@{1}", Value, Location);
                default:
                    return Value.ToString();
            }
        }

        public static implicit operator TValue(LocationTagged<TValue> value)
        {
            return value == null ? default(TValue) : value.Value;
        }

        private string DebuggerToString()
        {
            return $@"({Location})""{Value}""";
        }
    }
}
