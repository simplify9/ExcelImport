using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public class TypeOf<T>
    {
        public static PropertyPath Property<TProp>(Expression<Func<T,TProp>> propertySelector)
        {
            return PropertyPath.FromExpression(propertySelector.Body);
        }
    }
}
