﻿using System;

namespace Computator.NET.UI
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    internal class NameAttribute : Attribute
    {
        public NameAttribute(string str)
        {
            Name = str;
        }

        public string Name { get; set; }
    }
}