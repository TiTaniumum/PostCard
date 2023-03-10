using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCard
{
    public class Singleton
    {

        private static ColorBuffer instanceCB;
        
        private Singleton() { 
            instanceCB = new ColorBuffer();
        }

        public static ColorBuffer GetColorBuffer()
        {
            if(instanceCB == null) new Singleton();
            return instanceCB;
        }
       
    }
}
