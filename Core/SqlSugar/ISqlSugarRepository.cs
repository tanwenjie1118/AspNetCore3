using System;
using System.Collections.Generic;
using System.Text;

namespace Core.SqlSugar
{
    public interface ISqlSugarRepository
    {
        List<T> GetList<T>();
    }
}
