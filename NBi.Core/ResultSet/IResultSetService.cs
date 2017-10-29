﻿using System;
using System.Data;

namespace NBi.Core.ResultSet
{
    public interface IResultSetService
    {
        ResultSet Execute();

        int TransformationCount { get; }
    }
}