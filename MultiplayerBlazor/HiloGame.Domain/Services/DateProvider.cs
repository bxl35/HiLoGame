using System;
using System.Collections.Generic;
using System.Text;

namespace HiloGame.Domain.Services
{
    public class DateProvider : IDateProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
