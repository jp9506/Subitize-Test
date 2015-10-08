using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
namespace Subitize_Test
{
    [DataContract] public class User
    {
        [DataMember] public string AuthCode { get; set; }
        [DataMember] public string Gender { get; set; }
        [DataMember] public string Age { get; set; }
        [DataMember] public Test[] TestResults { get; set; }
    }
    [DataContract] public class Settings
    {
        [DataMember] public int MaxTests { get; set; }
        [DataMember] public int TimeEst { get; set; }
    }
    [DataContract] public class Test
    {
        private Random rand = null;
        [DataMember] public int ID { get; set; }
        [DataMember] public int MaxArraySize { get; set; }
        [DataMember] public int ArraysPerSize { get; set; }
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
        public void GenerateArrays()
        {
            rand = new Random();
            ImageArrays.Clear();
            List<ImageArray> l = new List<ImageArray>();
            for (int i = 0; i < MaxArraySize; i++)
            {
                for (int j = 0; j < ArraysPerSize; j++)
                {
                    l.Add(new ImageArray()
                    {
                        ImagesDisplayed = i,
                        UserInput = 0
                    });
                }
            }
            ImageArrays.AddRange(l.OrderBy(x => rand.Next()).ToArray());
        }
    }
    [DataContract] public class ImageArray
    {
        [DataMember] public int ImagesDisplayed { get; set; }
        [DataMember] public int UserInput { get; set; }
    }
}
