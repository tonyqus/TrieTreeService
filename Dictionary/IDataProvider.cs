using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluePrint.Dictionary
{
    public enum DataProviderType
    { 
        TxtFile,
        MongoDB,
        PanguDict,
    }

    public interface IDataProvider
    {
        List<IDataNode> Load();
    }
}
