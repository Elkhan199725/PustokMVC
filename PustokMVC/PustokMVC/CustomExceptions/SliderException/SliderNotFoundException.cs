﻿namespace PustokMVC.CustomExceptions.SliderException
{

    public class SliderNotFoundException : Exception
    {
        public SliderNotFoundException()
        {
        }

        public SliderNotFoundException(string? message) : base(message)
        {
        }
    }
}
