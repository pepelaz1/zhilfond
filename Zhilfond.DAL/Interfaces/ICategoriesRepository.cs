using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface ICategoriesRepository
    {
        IEnumerable<Category> GetAll();
        void Add(Category item);
        void Update(int id, Category item);
        void Delete(int id);
    }
}
