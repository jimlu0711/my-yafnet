﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="ServiceStack, Inc.">
//   Copyright (c) ServiceStack, Inc. All Rights Reserved.
// </copyright>
// <summary>
//   Fork for YetAnotherForum.NET, Licensed under the Apache License, Version 2.0
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

using ServiceStack.OrmLite.Base.Text.Common;

namespace ServiceStack.OrmLite.Base.Text;

/// <summary>
/// A fast, standards-based, serialization-issue free DateTime serializer.
/// </summary>
public static class DateTimeExtensions
{
    public const long UnixEpoch = 621355968000000000L;
    private readonly static DateTime UnixEpochDateTimeUtc = new(UnixEpoch, DateTimeKind.Utc);
    private readonly static DateTime UnixEpochDateTimeUnspecified = new(UnixEpoch, DateTimeKind.Unspecified);
    private readonly static DateTime MinDateTimeUtc = new(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime FromUnixTime(this int unixTime)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
    }

    public static long ToUnixTimeMs(this DateTime dateTime)
    {
        var universal = ToDateTimeSinceUnixEpoch(dateTime);
        return (long)universal.TotalMilliseconds;
    }

    public static long ToUnixTime(this DateTime dateTime)
    {
        return (dateTime.ToDateTimeSinceUnixEpoch().Ticks) / TimeSpan.TicksPerSecond;
    }

    private static TimeSpan ToDateTimeSinceUnixEpoch(this DateTime dateTime)
    {
        var dtUtc = dateTime;
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dtUtc = dateTime.Kind == DateTimeKind.Unspecified && dateTime > DateTime.MinValue && dateTime < DateTime.MaxValue
                        ? DateTime.SpecifyKind(dateTime.Subtract(DateTimeSerializer.LocalTimeZone.GetUtcOffset(dateTime)), DateTimeKind.Utc)
                        : dateTime.ToStableUniversalTime();
        }

        var universal = dtUtc.Subtract(UnixEpochDateTimeUtc);
        return universal;
    }

    public static long ToUnixTimeMs(this long ticks)
    {
        return (ticks - UnixEpoch) / TimeSpan.TicksPerMillisecond;
    }

    public static DateTime FromUnixTimeMs(this long msSince1970)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
    }

    public static DateTime FromUnixTimeMs(this long msSince1970, TimeSpan offset)
    {
        return DateTime.SpecifyKind(UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset, DateTimeKind.Local);
    }

    public static DateTime RoundToSecond(this DateTime dateTime)
    {
        return new DateTime((dateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond, dateTime.Kind);
    }

    public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
    {
        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static bool IsEqualToTheSecond(this DateTime dateTime, DateTime otherDateTime)
    {
        return dateTime.ToStableUniversalTime().RoundToSecond().Equals(otherDateTime.ToStableUniversalTime().RoundToSecond());
    }

    public static string ToTimeOffsetString(this TimeSpan offset, string seperator = "")
    {
        var hours = Math.Abs(offset.Hours).ToString(CultureInfo.InvariantCulture);
        var minutes = Math.Abs(offset.Minutes).ToString(CultureInfo.InvariantCulture);
        return (offset < TimeSpan.Zero ? "-" : "+")
               + (hours.Length == 1 ? "0" + hours : hours)
               + seperator
               + (minutes.Length == 1 ? "0" + minutes : minutes);
    }

    public static TimeSpan FromTimeOffsetString(this string offsetString)
    {
        if (!offsetString.Contains(":"))
            offsetString = offsetString.Insert(offsetString.Length - 2, ":");

        offsetString = offsetString.TrimStart('+');

        return TimeSpan.Parse(offsetString);
    }

    public static DateTime ToStableUniversalTime(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
            return dateTime;
        if (dateTime == DateTime.MinValue)
            return MinDateTimeUtc;

        return PclExport.Instance.ToStableUniversalTime(dateTime);
    }
}