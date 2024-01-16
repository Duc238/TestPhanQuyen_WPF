using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPhanQuyen_WPF.Model
{
    public class DataProvider
    {
        private static DataProvider _Instance;
        public TestEntities DB;

        public static DataProvider Instance
        {
            get { if (_Instance == null) _Instance = new DataProvider(); return DataProvider._Instance; }
            private set { DataProvider._Instance = value; }
        }
        public DataProvider()
        {
            DB = new TestEntities();
        }
    }
}
