using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface ISettingsRepository
    {
        void Update(Settings settings);
        Settings Get();
    }
}
