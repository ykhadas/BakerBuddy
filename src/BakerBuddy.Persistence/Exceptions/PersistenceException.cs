﻿namespace BakerBuddy.Persistence.Exceptions
{
    public class PersistenceException : Exception
    {
        public PersistenceException(string message)
            : base(message)
        {
        }
    }
}
