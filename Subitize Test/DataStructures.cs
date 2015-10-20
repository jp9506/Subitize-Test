using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
namespace Subitize_Test
{
    [DataContract] public class User
    {
        [DataMember] public string AuthCode { get; set; }
        [DataMember] public string Gender { get; set; }
        [DataMember] public string Age { get; set; }
        [DataMember] public Test[] TestResults
        {
            get
            {
                return Tests.Values.ToArray();
            }
            set
            {
                Tests.Clear();
                foreach (Test t in value)
                {
                    Tests.Add(t.ID, t);
                }
            }
        }
        private Dictionary<int, Test> _tests = null;
        public Dictionary<int, Test> Tests
        {
            get
            {
                if (_tests == null) _tests = new Dictionary<int, Test>();
                return _tests;
            }
        }
    }
    [DataContract] public class Test
    {
        private Random rand = null;
        [DataMember] public int ID { get; set; }
        [DataMember] public int TimeEst { get; set; }
        [DataMember] public int MaxArraySize { get; set; }
        [DataMember] public int DelayPeriod { get; set; }
        [DataMember] public ImageArray[] Arrays
        {
            get
            {
                return ImageArrays.ToArray();
            }
            set
            {
                ImageArrays.Clear();
                ImageArrays.AddRange(value);
            }
        }
        [DataMember] public SubTest[] TestParts
        {
            get
            {
                return SubTests.ToArray();
            }
            set
            {
                SubTests.Clear();
                SubTests.AddRange(value);
            }
        }

        private List<ImageArray> _arrays = null;
        public List<ImageArray> ImageArrays
        {
            get
            {
                if (_arrays == null)
                    _arrays = new List<ImageArray>();
                return _arrays;
            }
        }
        private List<SubTest> _subtests = null;
        public List<SubTest> SubTests
        {
            get
            {
                if (_subtests == null)
                    _subtests = new List<SubTest>();
                return _subtests;
            }
        }
        public void GenerateArrays()
        {
            rand = new Random();
            ImageArrays.Clear();
            List<ImageArray> l = new List<ImageArray>();
            for (int j = 0; j < MaxArraySize; j++)
            {
                foreach (SubTest test in SubTests)
                {
                    l.Add(new ImageArray()
                    {
                        ImagesDisplayed = j,
                        UserInput = -1,
                        ImageFile = test.ImageFile
                    });
                }
            }
            ImageArrays.AddRange(l.OrderBy(x => rand.Next()).ToArray());
            int indx = 0;
            ImageArrays.ForEach((x) => { x.Index = indx; indx++; });
        }
    }
    [DataContract] public class SubTest
    {
        [DataMember] public int TestID { get; set; }
        [DataMember] public string ImageFile { get; set; }
    }
    [DataContract] public class ImageArray
    {
        [DataMember] public int Index { get; set; }
        [DataMember] public int ImagesDisplayed { get; set; }
        [DataMember] public int UserInput { get; set; }
        [DataMember] public string ImageFile { get; set; }
    }
}
