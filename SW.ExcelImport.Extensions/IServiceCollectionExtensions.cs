using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SW.ExcelImport;
using SW.ExcelImport.Services;

namespace SW.ExcelImport.IServiceCollectionExtensions
{
    public static class Extensions
    {
        public static IServiceCollection AddExcelImport(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSheetReader();
            serviceCollection.AddTransient<ExcelFileTypedParseToJsonLoader>();
            serviceCollection.AddScoped<ExcelRepo>();
            
            serviceCollection.AddTransient<ExcelRowTypeParser>();
            serviceCollection.AddTransient<ExcelRowTypeValidatorOnAnnotations>();
            serviceCollection.AddTransient<ExcelService>();
            
            serviceCollection.AddTransient<IExcelQueryable, ExcelQueryable>();
            return serviceCollection;
        }
        
        public static IServiceCollection AddSheetReader(this IServiceCollection serviceCollection)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            serviceCollection.AddScoped<IExcelReader, ExcelFileReader>();
            serviceCollection.AddScoped(typeof(SheetReader<>));
            serviceCollection.AddTransient<ExcelSheetOnTypeValidator>();
            
            return serviceCollection;
        }
        
    }
}
