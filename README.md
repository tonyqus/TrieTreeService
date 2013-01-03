TrieTreeService
===============
This is a windows service which can be used to load many dictionary from different word segment software like PanguDict, IKAnalyzer. Trie tree is really useful in natual language processing,  especially you wanna index ngram results against words. The service will use memory as cache for the tree structure. If your dictionaries are too big, please make sure you have enough memory space to load it. By default, there should be 2GB available for user in each windows service on x86 archiecture.


Chinese Tutorial
================
For now, there is only Chinese tutorial: http://www.cnblogs.com/tonyqus/archive/2012/11/26/trietreeservice.html.
