using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IPrivateKeysRepository
    {
        PrivateKey Add(PrivateKey item);
        PrivateKey Get();
    }
}
