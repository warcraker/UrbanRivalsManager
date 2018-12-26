﻿using System;

namespace UrbanRivalsCore.Model.Cards.Skills.Prefixes
{
    public class DefaultPrefix : Prefix
    {
        public override bool isMatch(string text)
        {
            throw new InvalidOperationException();
        }
        public override string removePrefixFromText(string text)
        {
            throw new InvalidOperationException();
        }
        public override string ToString()
        {
            return "";
        }
    }
}
