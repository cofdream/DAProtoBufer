using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAGoogleProto
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new DAGoogleProtoConfigData();

            string path = "E:/Git/DAProtoBufer/GoogleProto/Assets/GoogleProto/DAProtoConfigPath.txt";
            Util.Serizlization(config, path);

            var config2 = Util.Deserizlization<DAGoogleProtoConfigData>(path);

            Console.WriteLine(1);
        }
    }
}
