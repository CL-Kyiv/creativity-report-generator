using System;
using System.Collections.Generic;
using System.Text;

namespace CreativityReportGenerator.Services.Abstractions
{
    public interface IBitbucketAccountService
    {
        List<SharpBucket.V2.Pocos.Commit> GetRepositories();
    }
}
