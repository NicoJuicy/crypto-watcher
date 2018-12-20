﻿using System;
using System.Linq.Expressions;
using CryptoWatcher.Domain.Models;

namespace CryptoWatcher.Domain.Expressions
{
    public static class LineExpression
    {
        public static Expression<Func<Line, bool>> ObsoleteLine()
        {
            return x => x.Time < DateTime.Now.AddDays(-7);
        }
    }
}