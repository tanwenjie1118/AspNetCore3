using Hal.Core.ElasticSearch.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Core.ElasticSearch
{
    public interface IESRepository
    {
        bool PostData(ESInformation eSInformation);
    }
}
