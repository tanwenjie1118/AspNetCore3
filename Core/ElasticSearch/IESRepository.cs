using Hal.Core.ElasticSearch.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hal.Core.ElasticSearch
{
    public interface IESRepository
    {
        bool CreateIndex<T>(string indexName) where T : class;
        bool PostData(ESInformation eSInformation);
        void PostBatchData(List<ESInformation> list);
    }
}
