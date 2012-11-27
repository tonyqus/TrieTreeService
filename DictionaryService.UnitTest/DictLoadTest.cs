using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BluePrint.Dictionary;
using BluePrint.Dictionary.Providers;

namespace DictionaryService.UnitTest
{
    [TestClass]
    public class DictLoadTest
    {
        internal const string UnitTestProjectFolder = @"..\..\..\DictionaryService.UnitTest";

        [TestMethod]
        public void TestLoadPanguDict()
        {
            var mockPanguProviderSetting = new Mock<IDataProviderSetting>();
            mockPanguProviderSetting.Setup(s => s.Uri).Returns(UnitTestProjectFolder + @"\Data\panguDict.dct");
            mockPanguProviderSetting.Setup(s => s.ProviderType).Returns("PanguDictProvider");

            var panguProvider = new PanguDictProvider(mockPanguProviderSetting.Object);
            var dataList = panguProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.AreEqual(146259, dataList.Count);
        }
        [TestMethod]
        public void TestLoadMongoDBDict_placeNames()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns("mongodb://localhost:28001");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("MongoDBDataProvider");
            mockProviderSetting.Setup(s => s.DBName).Returns("nameResearch");
            mockProviderSetting.Setup(s => s.CollectionName).Returns("placeNames");
            //mockProviderSetting.Setup(s => s.EntityType).Returns("text");
            var dataProvider = new MongoDBDataProvider(mockProviderSetting.Object);
            var dataList = dataProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.IsTrue(dataList.Count>8000);
        }

        [TestMethod]
        public void TestLoadMongoDBDict_CityNames()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns("mongodb://localhost:28001");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("MongoDBDataProvider");
            mockProviderSetting.Setup(s => s.DBName).Returns("nameResearch");
            mockProviderSetting.Setup(s => s.CollectionName).Returns("cityNames");
            mockProviderSetting.Setup(s => s.EntityType).Returns("BluePrint.Dictionary.Entities.CityEntity,BluePrint.Dictionary");
            var dataProvider = new MongoDBDataProvider(mockProviderSetting.Object);
            var dataList = dataProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.IsTrue(dataList.Count >400);
            Assert.IsFalse(string.IsNullOrEmpty(dataList[0].Word));
        }

        [TestMethod]
        public void TestLoadMongoDBDict_HumanNames()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns("mongodb://localhost:28001");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("MongoDBDataProvider");
            mockProviderSetting.Setup(s => s.DBName).Returns("nameResearch");
            mockProviderSetting.Setup(s => s.CollectionName).Returns("fullNames");
            mockProviderSetting.Setup(s => s.EntityType).Returns("BluePrint.Dictionary.Entities.NameEntity,BluePrint.Dictionary");
            var dataProvider = new MongoDBDataProvider(mockProviderSetting.Object);
            var dataList = dataProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.IsTrue(dataList.Count > 900000);
            Assert.IsFalse(string.IsNullOrEmpty(dataList[0].Word));
        }
        [TestMethod]
        public void TestLoadMongoDBDict_Surnames()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns("mongodb://localhost:28001");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("MongoDBDataProvider");
            mockProviderSetting.Setup(s => s.DBName).Returns("nameResearch");
            mockProviderSetting.Setup(s => s.CollectionName).Returns("surnames");
            mockProviderSetting.Setup(s => s.EntityType).Returns("BluePrint.Dictionary.Entities.NameEntity,BluePrint.Dictionary");
            var dataProvider = new MongoDBDataProvider(mockProviderSetting.Object);
            var dataList = dataProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.IsTrue(dataList.Count > 500);
            Assert.IsFalse(string.IsNullOrEmpty(dataList[0].Word));
        }
        [TestMethod]
        public void TestLoadTxtDict()
        {
            var mockProviderSetting = new Mock<IDataProviderSetting>();
            mockProviderSetting.Setup(s => s.Uri).Returns(UnitTestProjectFolder + @"\Data\textdict.txt");
            mockProviderSetting.Setup(s => s.ProviderType).Returns("TxtFileProvider");
            var dataProvider = new TxtFileProvider(mockProviderSetting.Object);
            var dataList = dataProvider.Load();
            Assert.IsNotNull(dataList);
            Assert.IsTrue(dataList.Count > 5);
            Assert.IsFalse(string.IsNullOrEmpty(dataList[0].Word));
        }
    }
}
