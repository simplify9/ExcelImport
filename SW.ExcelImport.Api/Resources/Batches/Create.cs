using System.Diagnostics;
using FluentValidation;
using SW.PrimitiveTypes;
using SW.ExcelImport.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;

namespace SW.ExcelImport.Api.Resources.Batches
{
    public class Create : ICommandHandler<CreateBatch>
    {
        readonly ICloudFilesService cloudFilesService;
        
        public Create(ICloudFilesService cloudFilesService)
        {
            this.cloudFilesService = cloudFilesService;
        }
        public async Task<object> Handle(CreateBatch request)
        {
            

            
            return null;
        }
    }
}

