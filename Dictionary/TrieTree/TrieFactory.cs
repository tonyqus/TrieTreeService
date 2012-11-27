using BluePrint.Dictionary;

namespace BluePrint.NLPCore
{
    public class TrieFactory
    {
        public static TrieTree LoadFromDataProvider(IDataProvider provider)
        {
            TrieTree tt = TrieTree.GetInstance();
            tt.Load(provider);
            return tt;
        }
    }
}
